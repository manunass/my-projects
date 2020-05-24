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

public class NewReservationController {

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
    ChoiceBox<String> numberOfRooms;
    @FXML
    ChoiceBox<String> bedTypes;
    @FXML
    Button showRoomsButton;
    @FXML
    Hyperlink cancelHyperlink;

    Alert a = new Alert(Alert.AlertType.CONFIRMATION);
    ButtonType b1 = new ButtonType("Confirmed");
    ButtonType b2 = new ButtonType("Unconfirmed");

    Alert a2 = new Alert(Alert.AlertType.CONFIRMATION);
    ButtonType b3 = new ButtonType("OK");
    ButtonType b4 = new ButtonType("Cancel");

    Alert a3 = new Alert(Alert.AlertType.CONFIRMATION);
    ButtonType b5 = new ButtonType("Done");

    Logic l = null;
    Scene available = null;
    Scene mainmenu = null;



    public void initialize() {


        ObservableList<String> list1 = FXCollections.observableArrayList();
        list1.addAll("1", "2","3","4","5","6","7");
        numberOfRooms.setItems(list1);

        ObservableList<String> list2 = FXCollections.observableArrayList();
        list2.addAll("Single", "Double","Family");
        bedTypes.setItems(list2);

        ObservableList<String> list3= FXCollections.observableArrayList();
        list3.addAll("Regular", "Deluxe");
        roomTypes.setItems(list3);

        a.setHeaderText("Reservation Status Confirmation");
        a.setContentText("Do you want your order to be Confirmed or Unconfirmed? (You can edit/cancel an Unconfirmed reservation at no cost, while cancelling a Confirmed reservation adds a $30 penalty to your account)");
        a.getButtonTypes().setAll(b1,b2);

        a2.setContentText("You are reserving for an Arrival that's in less than 4 days. Your reservation will be automatically Confirmed");
        a2.getButtonTypes().setAll(b3,b4);

        a3.getButtonTypes().setAll(b5);
        message.setVisible(false);

    }

    public void confirm() throws IOException {
        if (arrival.getValue() == null || departure.getValue() == null || numberOfRooms.getSelectionModel().isEmpty() || roomTypes.getSelectionModel().isEmpty() || bedTypes.getSelectionModel().isEmpty()) {
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

                            l.outToServer.writeBytes("Reserve\n");
                            l.outToServer.writeBytes(numberOfRooms.getValue() + "\n");
                            l.outToServer.writeBytes(roomTypes.getValue() + "\n");
                            l.outToServer.writeBytes(bedTypes.getValue() + "\n");
                            l.outToServer.writeBytes(arrival.getValue().toString() + "\n");
                            l.outToServer.writeBytes(departure.getValue().toString() + "\n");
                            if (smoking.isSelected()) {
                                l.outToServer.writeBytes("1" + "\n");
                            } else {
                                l.outToServer.writeBytes("0" + "\n");
                            }


                            if (!early) {
                                Optional<ButtonType> result = a.showAndWait();
                                if (result.get() == b1) {
                                    confOrnot = b1.getText();
                                } else {
                                    confOrnot = b2.getText();
                                }
                            }
                            l.outToServer.writeBytes(confOrnot + "\n");


                            int number = Integer.parseInt(l.inFromServer.readLine());
                            if (number == -1) {
                                message.setVisible(true);
                                message.setTextFill(Color.RED);
                                message.setText("Reservation wasn't made!");
                            } else {
                                //System.out.println(number);
                                for (int i = 0; i < number; i++) {
                                    int res = Integer.parseInt(l.inFromServer.readLine());
                                    //System.out.println(res);
                                }

                                message.setVisible(true);
                                message.setTextFill(Color.BLACK);
                                message.setText("Reservation made"); //just for test now
                            }
                        } else {
                            message.setVisible(true);
                            message.setTextFill(Color.RED);
                            message.setText("Reservation wasn't made!");
                        }

                    }


                }
            }
        }
    }

    public void listRooms() throws IOException {
        if (arrival.getValue() == null || departure.getValue() == null) {
            message.setVisible(true);
            message.setTextFill(Color.RED);
            message.setText("No blank dates are allowed!");
        }
        else {
            LocalDate checkin = arrival.getValue();
            LocalDate checkout = departure.getValue();
            if (checkin.isAfter(checkout)) {
                message.setVisible(true);
                message.setTextFill(Color.RED);
                message.setText("Arrival must be before Departure!");
            }
            else {
                if (checkin.isBefore(LocalDate.now())) {
                    message.setVisible(true);
                    message.setTextFill(Color.RED);
                    message.setText("You can't make a reservation before Today!");
                } else {
                    l.checkin = checkin;
                    l.checkout = checkout;
                    a3.setHeaderText("Rooms available between " + checkin + " and " + checkout);
                    FXMLLoader loadlist = new FXMLLoader(getClass().getResource("roomlisting.fxml"));
                    Parent parentlist = loadlist.load();
                    RoomListingController listController = loadlist.getController();
                    listController.l = this.l;
                    listController.refresh();

                    a3.getDialogPane().setContent(parentlist);
                    a3.showAndWait();

                    message.setVisible(false);
                }
            }
        }
    }


    public void cancel() throws IOException {
        arrival.setValue(null);
        departure.setValue(null);
        numberOfRooms.getSelectionModel().clearSelection();
        roomTypes.getSelectionModel().clearSelection();
        bedTypes.getSelectionModel().clearSelection();
        smoking.setSelected(false);
        message.setVisible(false);
        Stage primarystage = (Stage) cancelHyperlink.getScene().getWindow();
        primarystage.setTitle("Main Menu");
        primarystage.setScene(mainmenu);
    }

}

