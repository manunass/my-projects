package sample;

import javafx.fxml.FXMLLoader;
import javafx.geometry.HPos;
import javafx.geometry.VPos;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.layout.GridPane;
import javafx.scene.text.Text;
import javafx.scene.text.TextAlignment;
import javafx.stage.Stage;

import java.io.IOException;
import java.time.LocalDate;
import java.time.temporal.ChronoUnit;
import java.util.Optional;

public class ViewReservationController {
    public ScrollPane new_reservation_scrollpane;
    public GridPane NewRes_reservations_gridpane;
    public ScrollPane past_reservation_scrollpanenew_reservation_scrollpane;
    public GridPane PastRes_reservations_gridpane;

    public Alert a1 = new Alert(Alert.AlertType.CONFIRMATION);
    public ButtonType b1 = new ButtonType("OK");
    public ButtonType b2 = new ButtonType("Cancel");

    public Alert a2 = new Alert(Alert.AlertType.CONFIRMATION);
    public ButtonType b3 = new ButtonType("Done");

    public Logic l = null;
    public Scene mainmenu;
    public Label noPastRes;
    public Label noUpcomingRes;


    public void initialize() {
        a1.setContentText("You are about to cancel a Confirmed reservation. A penalty of 30$ would be added to your account if you continue.");
        a1.getButtonTypes().setAll(b1,b2);

        a2.setHeaderText("Edit Reservation");
        a2.getButtonTypes().setAll(b3);
    }

    public void refresh() throws IOException {
        noUpcomingRes.setVisible(false);
        NewRes_reservations_gridpane.getChildren().clear();
        l.outToServer.writeBytes("Get Reservations\n");
        //current reservations
        int currentReservs = Integer.parseInt(l.inFromServer.readLine());

        if (currentReservs == 0) {
            noUpcomingRes.setVisible(true);
        }
        else {
            for (int i = 0; i< currentReservs;i++) {
                final String checkin = l.inFromServer.readLine();
                final String checkout = l.inFromServer.readLine();
                final int roomnumber = Integer.parseInt(l.inFromServer.readLine());
                final int reservnumber = Integer.parseInt(l.inFromServer.readLine());
                final String status = l.inFromServer.readLine();
                final String smoking = l.inFromServer.readLine();
                final String roomtype = l.inFromServer.readLine();
                final String bedtype = l.inFromServer.readLine();
                int price = Integer.parseInt(l.inFromServer.readLine());
                Text t1 = new Text(checkin);
                Text t2 = new Text(checkout);
                Text t3 = new Text(String.valueOf(roomnumber));
                Text t4 = new Text(String.valueOf(reservnumber));
                int totprice = (int) (price*(ChronoUnit.DAYS.between(LocalDate.parse(checkin),LocalDate.parse(checkout))));
                Text t5 = new Text(String.valueOf(totprice));
                Text t6 = new Text(status);
                Hyperlink cancellink = new Hyperlink("Cancel");
                cancellink.setOnAction(event -> {
                    try {
                        cancelReservation(status,reservnumber);
                    } catch (IOException e) {
                        e.printStackTrace();
                    }
                });

                if (status.equals("Unconfirmed")) {
                    Hyperlink editlink = new Hyperlink("Edit");
                    editlink.setOnAction(event -> {
                        try {
                            editReservation(reservnumber,checkin,checkout,status,smoking,roomtype,bedtype);
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                    });
                    NewRes_reservations_gridpane.add(editlink,6,i);
                    GridPane.setHalignment(editlink,HPos.CENTER);

                }

                NewRes_reservations_gridpane.add(t1,0,i);
                NewRes_reservations_gridpane.add(t2,1,i);
                NewRes_reservations_gridpane.add(t3,2,i);
                NewRes_reservations_gridpane.add(t4,3,i);
                NewRes_reservations_gridpane.add(t5,4,i);
                NewRes_reservations_gridpane.add(t6,5,i);
                NewRes_reservations_gridpane.add(cancellink,7,i);
                GridPane.setHalignment(t1, HPos.CENTER);
                GridPane.setHalignment(t2, HPos.CENTER);
                GridPane.setHalignment(t3, HPos.CENTER);
                GridPane.setHalignment(t4, HPos.CENTER);
                GridPane.setHalignment(t5, HPos.CENTER);
                GridPane.setHalignment(t6, HPos.CENTER);
                GridPane.setHalignment(cancellink, HPos.CENTER);

            }
        }
    }

    public void refreshPast() throws IOException {
        noPastRes.setVisible(false);
        PastRes_reservations_gridpane.getChildren().clear();
        l.outToServer.writeBytes("Get Past Reservations\n");

        int pastReservs = Integer.parseInt(l.inFromServer.readLine());
        if (pastReservs == 0) {
            noPastRes.setVisible(true);
        }
        else {
            for (int i = 0 ; i < pastReservs; i ++ ){
                String checkin = l.inFromServer.readLine();
                String checkout = l.inFromServer.readLine();
                String room = l.inFromServer.readLine();
                String reservid = l.inFromServer.readLine();
                Text t1 = new Text(checkin);
                Text t2 = new Text(checkout);
                Text t3 = new Text(room);
                Text t4 = new Text(reservid);
                PastRes_reservations_gridpane.add(t1,0,i);
                PastRes_reservations_gridpane.add(t2,1,i);
                PastRes_reservations_gridpane.add(t3,2,i);
                PastRes_reservations_gridpane.add(t4,3,i);
                GridPane.setHalignment(t1,HPos.CENTER);
                GridPane.setHalignment(t2,HPos.CENTER);
                GridPane.setHalignment(t3,HPos.CENTER);
                GridPane.setHalignment(t4,HPos.CENTER);
                GridPane.setValignment(t1,VPos.CENTER);
                GridPane.setValignment(t2,VPos.CENTER);
                GridPane.setValignment(t3,VPos.CENTER);
                GridPane.setValignment(t4,VPos.CENTER);
            }
        }
        PastRes_reservations_gridpane.setVgap(8);
    }



    public void cancelReservation(String status, int reservid) throws IOException {
        if (status.equals("Unconfirmed")) {
            l.outToServer.writeBytes("Cancel Reservation\n");
            l.outToServer.writeBytes(reservid+"\n");
            l.outToServer.writeBytes("Don't Increment\n");
        }
        else {
            Optional<ButtonType> result = a1.showAndWait();
            if (result.get() == b1) {
                l.outToServer.writeBytes("Cancel Reservation\n");
                l.outToServer.writeBytes(reservid+"\n");
                l.outToServer.writeBytes("Increment\n");
            }
        }
        refresh();
    }

    public void editReservation(int reservid,String checkin, String checkout, String status,String smoking, String roomtype, String bedtype) throws IOException {
        FXMLLoader loadedit = new FXMLLoader(getClass().getResource("editReservation.fxml"));
        Parent parentedit = loadedit.load();
        editReservationController editController = loadedit.getController();
        editController.l = l;
        editController.reservationNumber = reservid;
        editController.prevcheckin = checkin;
        editController.prevcheckout = checkout;
        editController.prevstatus = status;
        editController.prevsmoking = smoking;
        editController.prevroomtype = roomtype;
        editController.prevbedtype = bedtype;
        editController.refresh();

        a2.setHeaderText("Edit Reservation No. " + reservid);
        a2.getDialogPane().setContent(parentedit);
        a2.showAndWait();
        refresh();
    }

    public void backToMain() {
        Stage primarystage = (Stage) new_reservation_scrollpane.getScene().getWindow();
        primarystage.setTitle("Main Menu");
        primarystage.setScene(mainmenu);
    }


}
