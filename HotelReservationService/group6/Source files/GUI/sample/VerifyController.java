package sample;

import javafx.beans.value.ChangeListener;
import javafx.beans.value.ObservableValue;
import javafx.geometry.Pos;
import javafx.scene.control.Button;
import javafx.scene.control.Label;
import javafx.scene.control.Slider;
import javafx.scene.control.TextField;
import javafx.scene.paint.Color;
import javafx.scene.paint.Paint;
import javafx.scene.text.Text;

import java.io.IOException;

public class VerifyController {
    public Slider sliderid;
    public Button submitButton;
    public Label messageid;


    public Logic l = null;
    public Label numberid;

    public void initialize() {
        sliderid.valueProperty().addListener(new ChangeListener<Number>() {
            @Override
            public void changed(ObservableValue<? extends Number> observable, Number oldValue, Number newValue) {
                numberid.setText(String.valueOf(newValue.intValue()));
            }
        });
    }


    public void refresh() throws IOException {
        l.outToServer.writeBytes("Generate\n");
        int num1 = Integer.parseInt(l.inFromServer.readLine());
        int num2 = Integer.parseInt(l.inFromServer.readLine());
        messageid.setText("What is: " + num1  +" + " + num2 + " = ?");
        messageid.setAlignment(Pos.CENTER);

    }

    public void submit() throws IOException {
        int answer = (int) sliderid.getValue();
        l.outToServer.writeBytes(answer+"\n");
        String checker = l.inFromServer.readLine();
        if (checker.equals("Y")) {
            messageid.setText("Thank you for verifying");
            messageid.setTextFill(Color.DARKGREEN);
            l.verified = true;
        }
        else {
            messageid.setText("Wrong Answer");
            messageid.setTextFill(Color.RED);
        }
        submitButton.setVisible(false);
        sliderid.setVisible(false);
        numberid.setVisible(false);

    }


}

