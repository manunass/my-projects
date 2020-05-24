package sample;

import javafx.fxml.FXML;
import javafx.scene.Scene;
import javafx.event.ActionEvent;
import javafx.scene.control.*;
import javafx.stage.Stage;

import java.io.IOException;

public class WelcomePageController {

    public Scene login;
    public Scene aboutus;
    public Logic l = null;

    public Hyperlink loginHyp;

    public void goToLogin() {
        Stage primaryStage = (Stage) loginHyp.getScene().getWindow();
        primaryStage.setTitle("Login");
        primaryStage.setScene(login);
    }

    public void goToAbout() {
        Stage primaryStage = (Stage) loginHyp.getScene().getWindow();
        primaryStage.setTitle("About Us");
        primaryStage.setScene(aboutus);
    }

    public void exit() {
        Stage primaryStage = (Stage) loginHyp.getScene().getWindow();
        try {
            l.outToServer.writeBytes("EXIT\n");
        } catch (IOException e) {
            primaryStage.close();
            e.printStackTrace();
        }
        primaryStage.close();
    }
}

