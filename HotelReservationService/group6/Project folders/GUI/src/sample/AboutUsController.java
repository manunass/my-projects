package sample;

import javafx.fxml.FXML;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.stage.Stage;

public class AboutUsController {

    public Scene welcome;
    public Hyperlink backHyp;

    public void back() {
        Stage primaryStage = (Stage) backHyp.getScene().getWindow();
        primaryStage.setTitle("Welcome Page");
        primaryStage.setScene(welcome);
    }

}

