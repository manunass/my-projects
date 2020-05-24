package sample;

import javafx.beans.property.SimpleIntegerProperty;
import javafx.beans.property.SimpleStringProperty;
import javafx.collections.FXCollections;
import javafx.collections.ObservableArray;
import javafx.collections.ObservableList;
import javafx.scene.control.TableColumn;
import javafx.scene.control.TableView;
import javafx.scene.control.cell.PropertyValueFactory;

import java.io.IOException;

public class RoomListingController {
    public TableView<Reservation> tableid;
    public TableColumn roomcolumn;
    public TableColumn bedcolumn;
    public TableColumn numbercolumn;
    public Logic l = null;
    public TableColumn pricecolumn;
    ObservableList<Reservation> data = null;


    public void initialize() {
        roomcolumn.setCellValueFactory(new PropertyValueFactory<Reservation,String>("roomType"));
        bedcolumn.setCellValueFactory(new PropertyValueFactory<Reservation,String>("bedType"));
        numbercolumn.setCellValueFactory(new PropertyValueFactory<Reservation,Integer>("number"));
        pricecolumn.setCellValueFactory(new PropertyValueFactory<Reservation,String>("price"));
    }

    public void refresh() throws IOException {
        l.outToServer.writeBytes("List Rooms\n");
        l.outToServer.writeBytes(l.checkin+"\n");
        l.outToServer.writeBytes(l.checkout+"\n");
        Integer n1 = Integer.valueOf(l.inFromServer.readLine());
        Integer n2 = Integer.valueOf(l.inFromServer.readLine());
        Integer n3 = Integer.valueOf(l.inFromServer.readLine());
        Integer n4 = Integer.valueOf(l.inFromServer.readLine());
        Integer n5 = Integer.valueOf(l.inFromServer.readLine());
        Integer n6 = Integer.valueOf(l.inFromServer.readLine());

        data = FXCollections.observableArrayList(new Reservation("Regular","Single",n1,"50 $"),
        new Reservation("Regular","Double",n2,"60 $"),
        new Reservation("Regular","Family",n3,"70 $"),
        new Reservation("Deluxe","Single",n4,"100 $"),
        new Reservation("Deluxe","Double",n5,"120 $"),
        new Reservation("Deluxe","Family",n6,"140 $")
        );

        tableid.setItems(data);
    }


    public class Reservation {
        public SimpleStringProperty roomType;
        public SimpleStringProperty bedType;
        public SimpleIntegerProperty number;
        public SimpleStringProperty price;

        public Reservation(String r, String b, Integer n, String p) {
            roomType = new SimpleStringProperty(r); bedType = new SimpleStringProperty(b); number = new SimpleIntegerProperty(n); price = new SimpleStringProperty(p);
        }

        public String getRoomType() {
            return roomType.get();
        }
        public String getBedType() {
            return bedType.get();
        }
        public Integer getNumber() {
            return number.get();
        }
        public String getPrice() {
            return price.get();
        }
    }

}
