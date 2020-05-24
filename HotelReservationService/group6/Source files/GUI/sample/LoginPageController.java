package sample;

import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.control.*;
import java.io.IOException;
import java.util.ArrayList;

import javafx.scene.control.Label;
import javafx.scene.Scene;
import javafx.scene.paint.Color;
import javafx.stage.Stage;

public class LoginPageController {

    /*public GridPane gridid;
    public GridPane g2 = new GridPane();
    public AnchorPane anchorid;*/
    @FXML
    Label serverMessage;
    @FXML
    Button confirmButton;
    @FXML
    Label loginLabel;
    @FXML
    Label usernameLabel;
    @FXML
    Label passwordLabel;
    @FXML
    TextField usernameField;
    @FXML
    PasswordField passwordField;
    @FXML
    Hyperlink signupHyperlink;

    Logic l = null;
    MainMenuController menuController = null;


    String serverMsg = null;
    Scene signup;
    Scene mainmenu;
    Scene welcome;

    Alert a = new Alert(Alert.AlertType.CONFIRMATION);
    ButtonType b1 = new ButtonType("Done");



    public void initialize() { serverMessage.setText("Connection Established!");
        a.setHeaderText("Human Verification");
        a.getButtonTypes().setAll(b1);
    }


    public void login() throws IOException {
        l.username = usernameField.getText();
        l.password = passwordField.getText();
        l.outToServer.writeBytes("L\n");
        l.outToServer.writeBytes(l.username + "\n");
        l.outToServer.writeBytes(l.password + "\n");
        serverMsg = l.inFromServer.readLine();
        if (serverMsg.equals("Y")) {
            Stage primaryStage = (Stage) signupHyperlink.getScene().getWindow();
            usernameField.clear();
            passwordField.clear();
            serverMessage.setVisible(false);
            menuController.refresh();
            primaryStage.setTitle("Main Menu");
            primaryStage.setScene(mainmenu);
        }
        else {
            serverMessage.setTextFill(Color.RED);
            serverMessage.setText("Username or Password incorrect!");
            serverMessage.setVisible(true);
        }

    }

    public void switchToSignup(ActionEvent action) throws IOException {
        Stage primaryStage = (Stage) signupHyperlink.getScene().getWindow();
        FXMLLoader loadverify = new FXMLLoader(getClass().getResource("verify.fxml"));
        Parent parentverify = loadverify.load();
        VerifyController verifyController = loadverify.getController();
        verifyController.l = this.l;
        verifyController.refresh();
        a.getDialogPane().setContent(parentverify);
        a.showAndWait();

        if (l.verified) {
            usernameField.clear();
            passwordField.clear();
            serverMessage.setVisible(false);
            primaryStage.setTitle("Signup");
            primaryStage.setScene(signup);
        }
        else {
            l.outToServer.writeBytes("0\n"); //in case the client closes the dialogue without submitting, which would leave the server waiting for a readline
            l.inFromServer.readLine();
        }
    }

    public void goToWelcome() {
        usernameField.clear();
        passwordField.clear();
        serverMessage.setVisible(false);
        Stage primaryStage = (Stage) signupHyperlink.getScene().getWindow();
        primaryStage.setTitle("Welcome Page");
        primaryStage.setScene(welcome);
    }


}
