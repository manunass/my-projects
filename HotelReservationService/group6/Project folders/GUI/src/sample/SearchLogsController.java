package sample;

import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.paint.Color;
import javafx.stage.Stage;

import java.io.IOException;
import java.time.LocalDate;

public class SearchLogsController {
    public DatePicker from;
    public DatePicker to;
    public TextField roomid;
    public Label message;

    public Logic l = null;
    public Alert a = new Alert(Alert.AlertType.CONFIRMATION);
    public ButtonType b1 = new ButtonType("Done");
    public LocalDate d1, d2;
    public Label datenote;

    public Scene mainmenu;
    public TextField userid;

    public void initialize() {
        a.setHeaderText("Activity logs");
        a.getButtonTypes().setAll(b1);
    }

    public void refresh() throws IOException {
        l.outToServer.writeBytes("Get Log Dates\n");
        d1 = LocalDate.parse(l.inFromServer.readLine());
        d2 = LocalDate.parse(l.inFromServer.readLine());
        from.setValue(d1);
        to.setValue(d2);
        datenote.setText("Note: Logs available from " + d1 + " to " + d2);
    }

    public void logsByDate() throws IOException {
        LocalDate fromdate = from.getValue();
        LocalDate todate = to.getValue();
        if (!( (d1.isBefore(fromdate) || d1.equals(fromdate)) && (fromdate.isBefore(todate) || fromdate.equals(todate)) && (todate.isBefore(d2) || todate.equals(d2)) )) {
            message.setText("Invalid dates");
            message.setTextFill(Color.RED);
            message.setVisible(true);
        }
        else {
            message.setVisible(false);
            l.outToServer.writeBytes("Get Logs from Dates\n");
            l.outToServer.writeBytes(fromdate.toString() + "\n");
            l.outToServer.writeBytes(todate.toString() + "\n");
            a.setHeaderText("Activity Logs Between " + fromdate + " and " + todate);
            FXMLLoader loadreports = new FXMLLoader(getClass().getResource("reports.fxml"));
            Parent parentreports = loadreports.load();
            ReportsController reportsController = loadreports.getController();
            reportsController.l = this.l;
            reportsController.refresh();

            a.getDialogPane().setContent(parentreports);
            a.showAndWait();
        }
    }

    public void logsByRoom() throws IOException {
        if (!roomid.getText().matches("\\d+")) {
            message.setText("Invalid room");
            message.setTextFill(Color.RED);
            message.setVisible(true);
        }
        else {
            int roomNumber = Integer.parseInt(roomid.getText());
            if (roomNumber < 0 || roomNumber > 120) {
                message.setText("Invalid room");
                message.setTextFill(Color.RED);
                message.setVisible(true);
            }
            else {
                message.setVisible(false);
                l.outToServer.writeBytes("Get Logs from Room\n");
                l.outToServer.writeBytes( roomNumber+ "\n");
                if (roomNumber == 0 ) {
                    a.setHeaderText("Room-less Activity Logs");
                }
                else {
                    a.setHeaderText("Activity Logs for Room " + roomNumber);
                }

                FXMLLoader loadreports = new FXMLLoader(getClass().getResource("reports.fxml"));
                Parent parentreports = loadreports.load();
                ReportsController reportsController = loadreports.getController();
                reportsController.l = this.l;
                reportsController.refresh();

                a.getDialogPane().setContent(parentreports);
                a.showAndWait();
            }
        }
    }

    public void logsByUser() throws IOException {
        if (userid.getText().isEmpty()) {
            message.setText("No blank user!");
            message.setTextFill(Color.RED);
            message.setVisible(true);
        }
        else {
            String user = userid.getText();
            message.setVisible(false);
            l.outToServer.writeBytes("Get Logs from User\n");
            l.outToServer.writeBytes(user + "\n");
            a.setHeaderText("Activity Logs for username " + user);


            FXMLLoader loadreports = new FXMLLoader(getClass().getResource("reports.fxml"));
            Parent parentreports = loadreports.load();
            ReportsController reportsController = loadreports.getController();
            reportsController.l = this.l;
            reportsController.refresh();

            a.getDialogPane().setContent(parentreports);
            a.showAndWait();

        }
    }



    public void backToMain() {
        Stage primarystage = (Stage) from.getScene().getWindow();
        from.setValue(null);
        to.setValue(null);
        roomid.clear();
        userid.clear();
        message.setVisible(false);
        primarystage.setTitle("Main Menu");
        primarystage.setScene(mainmenu);
    }

}
