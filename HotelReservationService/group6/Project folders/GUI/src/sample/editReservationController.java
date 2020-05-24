package sample;

import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.control.ChoiceBox;
import javafx.scene.paint.Color;
import javafx.stage.Stage;

import java.io.IOException;
import java.time.LocalDate;
import java.time.temporal.ChronoUnit;
import java.util.Optional;

public class editReservationController {

    public Label message;
    @FXML
    DatePicker arrival;
    @FXML
    DatePicker departure;
    @FXML
    CheckBox smoking;
    @FXML
    Button confirmReservation;
    @FXML
    ChoiceBox<String> roomTypes;
    @FXML
    ChoiceBox<String> bedTypes;

    Alert a = new Alert(Alert.AlertType.CONFIRMATION);
    ButtonType b1 = new ButtonType("Confirmed");
    ButtonType b2 = new ButtonType("Unconfirmed");

    Alert a2 = new Alert(Alert.AlertType.CONFIRMATION);
    ButtonType b3 = new ButtonType("OK");
    ButtonType b4 = new ButtonType("Cancel");


    Logic l = null;
    public int reservationNumber;
    public String prevcheckin;
    public String prevcheckout;
    public String prevstatus;
    public String prevsmoking;
    public String prevroomtype;
    public String prevbedtype;

    public void initialize() {


        ObservableList<String> list2 = FXCollections.observableArrayList();
        list2.addAll("Single", "Double","Family");
        bedTypes.setItems(list2);

        ObservableList<String> list3= FXCollections.observableArrayList();
        list3.addAll("Regular", "Deluxe");
        roomTypes.setItems(list3);

        a.setContentText("Do you want your order to be Confirmed or Unconfirmed? (You can edit/cancel an Unconfirmed reservation at no cost, while cancelling a Confirmed reservation adds a $30 penalty to your account)");
        a.getButtonTypes().setAll(b1,b2);

        a2.setContentText("You are reserving for an Arrival that's in less than 4 days. Your reservation will be automatically Confirmed");
        a2.getButtonTypes().setAll(b3,b4);

        message.setVisible(false);

    }

    public void confirm() throws IOException {
        if (arrival.getValue() == null || departure.getValue() == null || roomTypes.getSelectionModel().isEmpty() || bedTypes.getSelectionModel().isEmpty()) {
            message.setVisible(true);
            message.setTextFill(Color.RED);
            message.setText("No blank entries are allowed!");
        }
        else {
            LocalDate checkin = arrival.getValue();
            LocalDate checkout = departure.getValue();
            if (!checkin.isBefore(checkout)) {
                message.setVisible(true);
                message.setTextFill(Color.RED);
                message.setText("Arrival must be before Departure!");
            }
            else {
                if (checkin.isBefore(LocalDate.now())) {
                    message.setVisible(true);
                    message.setTextFill(Color.RED);
                    message.setText("You can't make a reservation before Today!");
                }
                else {

                    if (ChronoUnit.DAYS.between(checkin,checkout) >= 30) {
                        message.setVisible(true);
                        message.setTextFill(Color.RED);
                        message.setText("You can't reserve a room for more than 30 days!");
                    }

                    else {

                        boolean early = false;
                        boolean cancel = false;
                        String confOrnot = "Unconfirmed";

                        if (ChronoUnit.DAYS.between(LocalDate.now(), checkin) <= 3) {   //ask if want to continue since order will be confirmed
                            early = true;
                            Optional<ButtonType> result = a2.showAndWait();
                            if (result.get() == b3) {
                                confOrnot = "Confirmed";
                            } else {
                                cancel = true;
                            }
                        }

                        if (!cancel) {
                            l.outToServer.writeBytes("Edit Reservation\n");
                            //first save the reservation
                            l.outToServer.writeBytes(reservationNumber + "\n");
                            l.outToServer.writeBytes(prevcheckin + "\n");
                            l.outToServer.writeBytes(prevcheckout + "\n");
                            l.outToServer.writeBytes(prevstatus + "\n");
                            l.outToServer.writeBytes(prevsmoking + "\n");
                            l.outToServer.writeBytes(prevroomtype + "\n");
                            l.outToServer.writeBytes(prevbedtype + "\n");

                            l.outToServer.writeBytes(arrival.getValue().toString() + "\n");
                            l.outToServer.writeBytes(departure.getValue().toString() + "\n");
                            if (!early) {   //for status of reservation if not early
                                Optional<ButtonType> result = a.showAndWait();
                                if (result.get() == b1) {
                                    confOrnot = b1.getText();
                                } else {
                                    confOrnot = b2.getText();
                                }
                            }
                            l.outToServer.writeBytes(confOrnot + "\n");   //status of reservation

                            if (smoking.isSelected()) {
                                l.outToServer.writeBytes("true" + "\n");
                            } else {
                                l.outToServer.writeBytes("false" + "\n");
                            }
                            l.outToServer.writeBytes(roomTypes.getValue() + "\n");
                            l.outToServer.writeBytes(bedTypes.getValue() + "\n");


                            String checker = l.inFromServer.readLine();
                            if (checker.equals("No")) {
                                message.setVisible(true);
                                message.setTextFill(Color.RED);
                                message.setText("Reservation wasn't edited!");
                                refresh();
                            } else {
                                message.setVisible(true);
                                message.setTextFill(Color.BLACK);
                                message.setText("Reservation edited"); //just for test now
                                confirmReservation.setVisible(false);
                            }
                        } else {
                            message.setVisible(true);
                            message.setTextFill(Color.RED);
                            message.setText("Reservation wasn't edited!");
                            refresh();
                        }

                    }


                }
            }
        }
    }



    public void refresh() {
        arrival.setValue(LocalDate.parse(prevcheckin));
        departure.setValue(LocalDate.parse(prevcheckout));
        roomTypes.getSelectionModel().select(prevroomtype);
        bedTypes.getSelectionModel().select(prevbedtype);
        if (prevsmoking.equals("true")) {
            smoking.setSelected(true);
        }
    }


}

