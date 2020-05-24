package sample;

import javafx.beans.property.SimpleIntegerProperty;
import javafx.beans.property.SimpleStringProperty;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.scene.control.TableColumn;
import javafx.scene.control.TableView;
import javafx.scene.control.cell.PropertyValueFactory;

import java.io.IOException;

public class ReportsController {
    public TableView <Report> tableid;
    public TableColumn datecolumn;
    public TableColumn usercolumn;
    public TableColumn roomcolumn;
    public TableColumn logcolumn;

    public Logic l = null;
    ObservableList<Report> data = null;

    public void initialize() {
        datecolumn.setCellValueFactory(new PropertyValueFactory<Report,String>("date"));
        usercolumn.setCellValueFactory(new PropertyValueFactory<Report,String>("user"));
        roomcolumn.setCellValueFactory(new PropertyValueFactory<Report,String>("roomnum"));
        logcolumn.setCellValueFactory(new PropertyValueFactory<Report,String>("log"));
    }

    public void refresh() throws IOException {
        tableid.getItems().clear();
        data = FXCollections.observableArrayList();
        int count = Integer.parseInt(l.inFromServer.readLine());
        if (count > 0) {
            for (int i = 0 ; i < count; i++) {
                String date = l.inFromServer.readLine();
                String user = l.inFromServer.readLine();
                String roomnum = l.inFromServer.readLine();
                String log = l.inFromServer.readLine();
                if (roomnum.equals("0")) {roomnum = "N/A";}
                data.add(new Report(date,user,roomnum,log));
            }
            tableid.setItems(data);
        }
    }

    public class Report {
        public SimpleStringProperty date;
        public SimpleStringProperty user;
        public SimpleStringProperty roomnum;
        public SimpleStringProperty log;

        public Report(String d, String u, String r, String l) {
            date = new SimpleStringProperty(d); user = new SimpleStringProperty(u); roomnum = new SimpleStringProperty(r); log = new SimpleStringProperty(l);
        }

        public String getDate() {return date.get();}
        public String getUser() {return user.get();}
        public String getRoomnum() {return roomnum.get();}
        public String getLog() {return log.get();}

    }

}
