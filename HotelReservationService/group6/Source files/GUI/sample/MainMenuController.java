package sample;

import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.control.Label;
import javafx.stage.Stage;

import java.io.IOException;

public class MainMenuController {
    public Label welcomeuser_label;
    public Scene newreservation;
    public Scene invoice;
    public Scene profile;
    public Scene login;
    public Scene editreservation;
    public Scene searchlogs;

    public Logic l = null;
    public ViewReservationController reservationController = null;
    public ProfileController profileController = null;
    public InvoiceController invoiceController = null;
    public SearchLogsController searchLogsController = null;
    public Button viewreportsbutton;
    public Label generateid;


    public void initialize() {
        viewreportsbutton.setVisible(false);
        generateid.setVisible(false);
    }

    public void refresh() throws IOException {
        if (l.username.equals("admin")) {
            viewreportsbutton.setVisible(true);
            generateid.setVisible(true);
        }
        l.outToServer.writeBytes("Get Name\n");
        l.firstName = l.inFromServer.readLine();
        l.lastName = l.inFromServer.readLine();
        l.gender = l.inFromServer.readLine();
        String preName = "";
        if (l.gender.equals("Male")) {
            preName = "Mr.";
        }
        else {
            preName = "Ms.";
        }
        l.fullname = preName + " " + l.firstName+" " + l.lastName;
        welcomeuser_label.setText("Welcome " + preName + " " + l.firstName+" " + l.lastName);
    }

    public void gotoNewReservation() {
        Stage primaryStage = (Stage) welcomeuser_label.getScene().getWindow();
        primaryStage.setTitle("New Reservation");
        primaryStage.setScene(newreservation);
    }

    public void gotoInvoice() throws IOException {
        Stage primaryStage = (Stage) welcomeuser_label.getScene().getWindow();
        invoiceController.refresh();
        primaryStage.setTitle("Invoice");
        primaryStage.setScene(invoice);
    }

    public void gotoProfile() throws IOException {
        Stage primaryStage = (Stage) welcomeuser_label.getScene().getWindow();
        profileController.refresh();
        primaryStage.setTitle("Profile");
        primaryStage.setScene(profile);
    }

    public void gotoEditReservation() throws IOException {
        Stage primaryStage = (Stage) welcomeuser_label.getScene().getWindow();
        reservationController.refresh();
        reservationController.refreshPast();
        primaryStage.setTitle("View/Edit Reservations");
        primaryStage.setScene(editreservation);
    }

    public void gotoLogin() throws IOException {
        Stage primaryStage = (Stage) welcomeuser_label.getScene().getWindow();
        l.outToServer.writeBytes("Logout\n");
        viewreportsbutton.setVisible(false);
        generateid.setVisible(false);
        primaryStage.setTitle("Login");
        primaryStage.setScene(login);
    }

    public void gotoSearchLogs() throws IOException {
        Stage primaryStage = (Stage) welcomeuser_label.getScene().getWindow();
        primaryStage.setTitle("Activity Reports");
        searchLogsController.refresh();
        primaryStage.setScene(searchlogs);
    }

}
