package sample;

import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.event.ActionEvent;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.control.Button;
import javafx.scene.control.Label;
import javafx.scene.control.TextField;
import javafx.scene.paint.Color;
import javafx.scene.paint.Paint;
import javafx.stage.Stage;

import java.awt.*;
import java.io.IOException;

public class SignupController {
    public Button doneb;
    public TextField userf;
    public TextField passf;

    public PasswordField confirmPassf;
    public TextField firstf;
    public TextField lastf;
    public ComboBox genderc;
    public Hyperlink backtoLogin;
    public TextField phoneid;

    public Label messagebox;


    Scene login;
    Logic l = null;


   public void initialize(){
       ObservableList<String> list1 = FXCollections.observableArrayList();
       list1.addAll("Male", "Female");
       genderc.setItems(list1);
   }


    public void signupConfirmation(ActionEvent action) throws IOException {

        if (firstf.getText().isEmpty() || lastf.getText().isEmpty() || genderc.getSelectionModel().isEmpty() || confirmPassf.getText().isEmpty() || userf.getText().isEmpty() || passf.getText().isEmpty() || phoneid.getText().isEmpty()) {
            messagebox.setTextFill(Color.RED);
            messagebox.setText("No blank fields are allowed!");
            messagebox.setVisible(true);
        }
        else {
            if (!confirmPassf.getText().equals(passf.getText())) {
                messagebox.setTextFill(Color.RED);
                messagebox.setText("Passwords do not match!");
                messagebox.setVisible(true);
            }

            else {
                if (!l.isPhone(phoneid.getText())) {
                    messagebox.setTextFill(Color.RED);
                    messagebox.setText("Not a phone number!");
                    messagebox.setVisible(true);
                }
                else {

                    l.outToServer.writeBytes("S\n");
                    l.outToServer.writeBytes(firstf.getText() + "\n");                //HANDLE FIRSTNAME
                    l.outToServer.writeBytes(lastf.getText() + "\n");
                    l.outToServer.writeBytes(genderc.getValue() + "\n");
                    l.outToServer.writeBytes(userf.getText() + "\n");
                    l.outToServer.writeBytes(phoneid.getText() + "\n");
                    l.outToServer.writeBytes(passf.getText() + "\n");
                    String checker = l.inFromServer.readLine();

                    if (checker.equals("OK")) {
                        messagebox.setTextFill(Color.BLACK);
                        messagebox.setText("Username created!");
                        messagebox.setVisible(true);
                    } else {
                        if (checker.equals("Duplicate")) {
                            messagebox.setTextFill(Color.RED);
                            messagebox.setText("Username already used!");
                            messagebox.setVisible(true);
                        }
                    }
                }
            }
        }
    }

    public void returnToLogin(ActionEvent actionEvent) {
        userf.clear();
        passf.clear();
        confirmPassf.clear();
        firstf.clear();
        lastf.clear();
        phoneid.clear();
        genderc.getSelectionModel().clearSelection();
        messagebox.setVisible(false);
        l.verified = false;
        Stage primaryStage = (Stage) doneb.getScene().getWindow();
        primaryStage.setTitle("Login");
        primaryStage.setScene(login);

    }
}

