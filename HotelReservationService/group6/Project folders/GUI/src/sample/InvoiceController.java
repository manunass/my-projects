package sample;

import javafx.beans.property.SimpleIntegerProperty;
import javafx.beans.property.SimpleStringProperty;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.scene.Scene;
import javafx.scene.control.Label;
import javafx.scene.control.TableColumn;
import javafx.scene.control.TableView;
import javafx.scene.control.cell.PropertyValueFactory;
import javafx.stage.Stage;

import java.io.IOException;

public class InvoiceController {



    public Logic l = null;
    public Label invoicetext;
    public TableView <Row> tableid;
    public TableColumn itemcolumn;
    public TableColumn qtycolumn;
    public TableColumn unitpcolumn;
    public TableColumn amountcolumn;
    public Label totalid;
    ObservableList<Row> data = null;

    public Scene mainmenu;

    public void initialize() {
        itemcolumn.setCellValueFactory(new PropertyValueFactory<Row,String>("item"));
        qtycolumn.setCellValueFactory(new PropertyValueFactory<Row,Integer>("qty"));
        unitpcolumn.setCellValueFactory(new PropertyValueFactory<Row,String>("price"));
        amountcolumn.setCellValueFactory(new PropertyValueFactory<Row,String>("amount"));
    }

    public void refresh() throws IOException {
        tableid.getItems().clear();
        boolean edit = false;
        data = FXCollections.observableArrayList();
        invoicetext.setText("Invoice for " + l.fullname);
        int total = 0;
        l.outToServer.writeBytes("Get Invoice\n");
        int n1 = Integer.parseInt(l.inFromServer.readLine());
        int n2 = Integer.parseInt(l.inFromServer.readLine());
        int n3 = Integer.parseInt(l.inFromServer.readLine());
        int n4 = Integer.parseInt(l.inFromServer.readLine());
        int n5 = Integer.parseInt(l.inFromServer.readLine());
        int n6 = Integer.parseInt(l.inFromServer.readLine());
        int penalties = Integer.parseInt(l.inFromServer.readLine());

        if (n1>0) {
            edit = true;
            int price = 50;
            int a = n1*price;
            data.add(new Row("Stay in Regular room with Single bed",Integer.valueOf(n1), price +" $", a +" $"));
            total = total + a;
        }
        if (n2>0) {
            edit = true;
            int price = 60;
            int a = n2*price;
            data.add(new Row("Stay in Regular room with Double bed",Integer.valueOf(n2), price +" $", a +" $"));
            total = total + a;
        }
        if (n3>0) {
            edit = true;
            int price = 70;
            int a = n3*price;
            data.add(new Row("Stay in Regular room with Family bed",Integer.valueOf(n3), price +" $", a +" $"));
            total = total + a;
        }
        if (n4>0) {
            edit = true;
            int price = 100;
            int a = n4*price;
            data.add(new Row("Stay in Deluxe room with Single bed",Integer.valueOf(n4), price +" $", a +" $"));
            total = total + a;
        }
        if (n5>0) {
            edit = true;
            int price = 120;
            int a = n5*price;
            data.add(new Row("Stay in Deluxe room with Double bed",Integer.valueOf(n5), price +" $", a +" $"));
            total = total + a;
        }
        if (n6>0) {
            edit = true;
            int price = 140;
            int a = n6*price;
            data.add(new Row("Stay in Deluxe room with Family bed",Integer.valueOf(n6), price +" $", a +" $"));
            total = total + a;
        }
        if (penalties>0) {
            edit = true;
            int price = 30;
            int a = penalties*price;
            data.add(new Row("Penalty",Integer.valueOf(penalties), price +" $", a +" $"));
            total = total + a;
        }

        if (edit) {
            tableid.setItems(data);
            totalid.setText(total+" $");
        }

    }

    public class Row {
        public SimpleStringProperty item;
        public SimpleIntegerProperty qty;
        public SimpleStringProperty price;
        public SimpleStringProperty amount;

        public Row(String i, Integer q, String p, String a) {
            item = new SimpleStringProperty(i); qty = new SimpleIntegerProperty(q); price = new SimpleStringProperty(p); amount = new SimpleStringProperty(a);
        }

        public String getItem() {return item.get();}
        public Integer getQty() {return qty.get();}
        public String getPrice() {return price.get();}
        public String getAmount() {return amount.get();}

    }

    public void backToMain() {
        Stage primarystage = (Stage) tableid.getScene().getWindow();
        primarystage.setTitle("Main Menu");
        primarystage.setScene(mainmenu);
    }
}




