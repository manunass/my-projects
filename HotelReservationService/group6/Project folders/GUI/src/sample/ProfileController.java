package sample;

import javafx.scene.Scene;
import javafx.scene.control.Label;
import javafx.scene.control.PasswordField;
import javafx.scene.control.TextField;
import javafx.scene.paint.Color;
import javafx.stage.Stage;

import java.io.IOException;

public class ProfileController {
    public TextField first_name_field;
    public TextField last_name_field;
    public PasswordField old_password_field;
    public PasswordField new_password_field;
    public PasswordField confirm_password_field;
    public TextField mobile_nb_field;
    public Label userid;
    public Label genderid;
    public Label message;

    public Logic l = null;
    public Scene mainmenu;
    public MainMenuController menuController;

    public void confirm() throws IOException {
        if (first_name_field.getText().isEmpty() || last_name_field.getText().isEmpty() || mobile_nb_field.getText().isEmpty()) {
            message.setText("Blank User data entries!");
            message.setTextFill(Color.RED);
            message.setVisible(true);
        }
        else {
            if (!l.isPhone(mobile_nb_field.getText())) {
                message.setTextFill(Color.RED);
                message.setText("Not a phone number!");
                message.setVisible(true);
            }
            else {
                boolean passChange = false;
                boolean proceed = true;
                if (!(old_password_field.getText().isEmpty() || new_password_field.getText().isEmpty() || confirm_password_field.getText().isEmpty())) {
                    passChange = true;
                    l.outToServer.writeBytes("Check Password\n");
                    l.outToServer.writeBytes(old_password_field.getText()+"\n");
                    String checker = l.inFromServer.readLine();
                    if (checker.equals("No")) {
                        message.setText("Wrong Password!");
                        message.setTextFill(Color.RED);
                        message.setVisible(true);
                        proceed = false;
                    }
                    else {
                        if (!new_password_field.getText().equals(confirm_password_field.getText())) {
                            message.setText("New Passwords don't match!");
                            message.setTextFill(Color.RED);
                            message.setVisible(true);
                            proceed = false;
                        }
                    }
                }

                if (proceed) {
                    l.outToServer.writeBytes("Edit Profile\n");
                    l.outToServer.writeBytes(first_name_field.getText()+"\n");
                    l.outToServer.writeBytes(last_name_field.getText()+"\n");
                    l.outToServer.writeBytes(mobile_nb_field.getText()+"\n");

                    if (passChange) {
                        l.outToServer.writeBytes("Edit Password\n");
                        l.outToServer.writeBytes(confirm_password_field.getText()+"\n");

                        message.setText("Profile and Password were changed");
                        message.setTextFill(Color.BLACK);
                        message.setVisible(true);
                    }
                    else {
                        message.setText("Profile was changed");
                        message.setTextFill(Color.BLACK);
                        message.setVisible(true);
                    }
                }

            }

        }

        refresh();
    }


    public void refresh() throws IOException {
        l.outToServer.writeBytes("Get Profile\n");
        userid.setText(l.inFromServer.readLine());
        first_name_field.setText(l.inFromServer.readLine());
        last_name_field.setText(l.inFromServer.readLine());
        genderid.setText(l.inFromServer.readLine());
        mobile_nb_field.setText(l.inFromServer.readLine());
    }

    public void backToMain() throws IOException {
        Stage primaryStage = (Stage) userid.getScene().getWindow();
        menuController.refresh();
        new_password_field.clear();
        confirm_password_field.clear();
        old_password_field.clear();
        primaryStage.setTitle("Main Menu");
        primaryStage.setScene(mainmenu);
    }

}
