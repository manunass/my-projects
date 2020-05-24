package sample;
import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.*;
import java.time.LocalDate;


public class Logic {
    String username = null;
    String password = null;
    String firstName, lastName, gender = null;
    String hostname = "localhost";
    public Socket clientSocket = null;
    public DataOutputStream outToServer = null;
    public BufferedReader inFromServer = null;
    LocalDate checkin, checkout = null;
    Boolean verified = false;
    String fullname = null;

    //Need to keep track of Controllers, so that when we switch to

    public Logic() {
        try {
            clientSocket = new Socket(hostname, 1927);
            outToServer = new DataOutputStream(clientSocket.getOutputStream());
            inFromServer = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));

        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public boolean isPhone(String phone) {
        for (int i = 0 ; i< phone.length();i++) {
            char c = phone.charAt(i);
            if (!((c>= '0' && c<='9') || c == ' ' || c == '+')) {
                return false;
            }
        }
        return true;
    }


}


