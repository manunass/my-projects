import java.io.*;
import java.net.*;
import java.sql.*;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.temporal.ChronoUnit;
import java.util.ArrayList;
import java.util.Random;

class ServerGUI {
    private ServerSocket welcomeSocket;

    final private String url = "jdbc:mysql://localhost:3306/sys";


    public void accept() throws IOException {
        welcomeSocket = new ServerSocket(1927);

        System.out.print("Server on \n");

        while (true) {

            Socket connectionSocket = welcomeSocket.accept();
            System.out.println("accepted connection");
            ServerThread st = new ServerThread(connectionSocket);
            new Thread(st).start();

        }
    }

    public static void main(String[] args) throws Exception {
        ServerGUI server = new ServerGUI();
        server.accept();
    }

    class ServerThread implements Runnable {

        Socket connectionSocket;
        BufferedReader inFromClient;
        DataOutputStream outToClient;

        Connection connect = null;
        PreparedStatement preparedStatement = null;
        ResultSet resultSet = null;
        String username = null;
        String pass = null;
        String firstName, lastName, gender = null;
        Boolean isLogged = false;

        public ServerThread(Socket socket) {
            connectionSocket = socket;
        }

        public void databconnect() throws Exception {
            try {
                connect = DriverManager.getConnection(url,"root","login");
                System.out.println("mySQL database is connected");
            }
            catch (Exception e) {
                throw e;
            }
        }

        public void generateCode() throws IOException {
            Random gen = new Random();
            int num1 = gen.nextInt(15) + 1;
            int num2 = gen.nextInt(15) + 1;
            int answer = num1 + num2;
            outToClient.writeBytes(num1 + "\n");
            outToClient.writeBytes(num2+ "\n");
            int checker = Integer.parseInt(inFromClient.readLine());
            System.out.println(checker);
            if (checker == answer) {
                outToClient.writeBytes("Y\n");
            }
            else {
                outToClient.writeBytes("N\n");
            }
        }

        public boolean finduser(String user) throws SQLException {
            preparedStatement = connect.prepareStatement("SELECT user FROM usernames_pass where user = ? ");
            preparedStatement.setString(1,user);
            resultSet = preparedStatement.executeQuery();
            boolean ret = resultSet.next();
            return ret;
        }

        public boolean checkonline(String user) throws SQLException {
            preparedStatement = connect.prepareStatement("SELECT isLoggedIn FROM usernames_pass where user = ? ");
            preparedStatement.setString(1,user);
            resultSet = preparedStatement.executeQuery();
            resultSet.next();
            return resultSet.getBoolean("isLoggedIn");
        }

        public boolean findp(String user, String password) throws SQLException {
            preparedStatement = connect.prepareStatement("SELECT password FROM usernames_pass where user = ? and password = ?");
            preparedStatement.setString(1,user);
            preparedStatement.setString(2,password);
            resultSet = preparedStatement.executeQuery();
            boolean ret = resultSet.next();
            System.out.println(ret);
            return ret;
        }

        public void getName() throws SQLException, IOException {
            String query = "SELECT firstName,lastName,gender FROM profiles WHERE user = ?";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setString(1,username);
            resultSet = preparedStatement.executeQuery();
            resultSet.next();
            outToClient.writeBytes(resultSet.getString(1)+"\n");
            outToClient.writeBytes(resultSet.getString(2)+"\n");
            outToClient.writeBytes(resultSet.getString(3)+"\n");
        }

        public void getProfile() throws SQLException, IOException {
            String query = "SELECT firstName,lastName,gender,phone FROM profiles WHERE user = ?";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setString(1,username);
            resultSet = preparedStatement.executeQuery();
            resultSet.next();
            outToClient.writeBytes(username+"\n");
            outToClient.writeBytes(resultSet.getString(1)+"\n");
            outToClient.writeBytes(resultSet.getString(2)+"\n");
            outToClient.writeBytes(resultSet.getString(3)+"\n");
            outToClient.writeBytes(resultSet.getString(4)+"\n");
        }

        public void editProfile(String fname, String lname, String phone) throws SQLException {
            String query = "UPDATE profiles SET firstName = ?, lastName = ?, phone = ? WHERE user = ?";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setString(1,fname);
            preparedStatement.setString(2,lname);
            preparedStatement.setString(3,phone);
            preparedStatement.setString(4,username);
            preparedStatement.execute();
            insertlog(0,"Profile was edited");
        }

        public void editPass(String pass) throws SQLException {
            String query = "UPDATE usernames_pass SET password = ? WHERE user = ?";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setString(1,pass);
            preparedStatement.setString(2,username);
            preparedStatement.execute();
            insertlog(0,"Password was edited");
        }


        public void insertup(String user, String Password, String first, String last, String gender, String phone) throws SQLException {
            String query = "INSERT INTO usernames_pass (user,password)" + " values (?,?)";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setString(1,user);
            preparedStatement.setString(2,Password);
            preparedStatement.execute();
            query = "INSERT INTO profiles (user,firstName,gender,lastName,phone)" + " values (?,?,?,?,?)";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setString(1,user);
            preparedStatement.setString(2,first);
            preparedStatement.setString(3,gender);
            preparedStatement.setString(4,last);
            preparedStatement.setString(5,phone);
            preparedStatement.execute();
            query = "INSERT INTO invoices (user)" + " values (?)";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setString(1,user);
            preparedStatement.execute();
        }



        public int reserve(String type, String bed, Date checkin, Date checkout, boolean smoking) throws SQLException {
            String query = "SELECT roomNumber FROM rooms WHERE roomNumber NOT IN (SELECT roomNumber FROM reservations WHERE (? < checkout AND ? > checkin)) AND type = ? AND bedType = ?";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setDate(1, checkin);
            preparedStatement.setDate(2, checkout);
            preparedStatement.setString(3,type);
            preparedStatement.setString(4,bed);
            resultSet = preparedStatement.executeQuery();

            if (!resultSet.next()) {
                return -1;      //no room was found
            }
            String number = resultSet.getString(1);


            query = "INSERT INTO reservations (checkIn,checkOut,roomNumber,smoking,status,user)" + " values (?,?,?,?,?,?)";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setDate(1,checkin);
            preparedStatement.setDate(2,checkout);
            preparedStatement.setString(3,number);
            if (smoking) {
                preparedStatement.setString(4,"1");
            }
            else {
                preparedStatement.setString(4,"0");
            }
            preparedStatement.setString(5,"Unconfirmed");
            preparedStatement.setString(6,username);
            preparedStatement.execute();

            query = "SELECT LAST_INSERT_ID()";
            preparedStatement = connect.prepareStatement(query);
            resultSet = preparedStatement.executeQuery();
            resultSet.next();
            return resultSet.getInt(1);
        }

        public int reserveWithID(String type, String bed, Date checkin, Date checkout, boolean smoking,int id, String status) throws SQLException {
            String query = "SELECT roomNumber FROM rooms WHERE roomNumber NOT IN (SELECT roomNumber FROM reservations WHERE (? < checkout AND ? > checkin)) AND type = ? AND bedType = ?";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setDate(1, checkin);
            preparedStatement.setDate(2, checkout);
            preparedStatement.setString(3,type);
            preparedStatement.setString(4,bed);
            resultSet = preparedStatement.executeQuery();

            if (!resultSet.next()) {
                return -1;      //no room was found
            }
            String number = resultSet.getString(1);


            query = "INSERT INTO reservations (checkIn,checkOut,roomNumber,smoking,status,user,reservNumber)" + " values (?,?,?,?,?,?,?)";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setDate(1,checkin);
            preparedStatement.setDate(2,checkout);
            preparedStatement.setString(3,number);
            if (smoking) {
                preparedStatement.setString(4,"1");
            }
            else {
                preparedStatement.setString(4,"0");
            }
            preparedStatement.setString(5,status);
            preparedStatement.setString(6,username);
            preparedStatement.setInt(7,id);
            preparedStatement.execute();

            query = "SELECT LAST_INSERT_ID()";
            preparedStatement = connect.prepareStatement(query);
            resultSet = preparedStatement.executeQuery();
            resultSet.next();
            insertlog(getRoomNumFromReserv(id),"Reservation " + id + " was edited");
            return resultSet.getInt(1);
        }



        public void deleteReservation(int reservationid) throws SQLException {
            String query = "DELETE FROM reservations WHERE reservNumber = ?";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setInt(1,reservationid);
            preparedStatement.execute();
        }

        public void confirmReservation(int reservationid) throws SQLException {
            String query = "UPDATE reservations SET status = ? WHERE reservNumber = ?";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setString(1,"Confirmed");
            preparedStatement.setInt(2,reservationid);
            preparedStatement.execute();
            insertlog(getRoomNumFromReserv(reservationid),"Reservation " + reservationid + " was confirmed");
        }

        public int getRoomNumFromReserv(int reservationid) throws SQLException{
            String query = "SELECT roomNumber FROM reservations WHERE reservNumber = ?";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setInt(1,reservationid);
            resultSet = preparedStatement.executeQuery();
            if (!resultSet.next()) {
                return -1;
            }
            else {
                return resultSet.getInt(1);
            }
        }

        public int getPenalties(String user) throws SQLException{
            String query = "SELECT penalties FROM invoices WHERE user = ?";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setString(1,user);
            resultSet = preparedStatement.executeQuery();
            resultSet.next();
            return resultSet.getInt(1);
        }

        public void incrementPenalties(String user, int roomnum) throws SQLException {
            String query = "UPDATE invoices SET penalties = penalties + 1 WHERE user = ?";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setString(1,user);
            preparedStatement.execute();
            insertlog(roomnum,"Penalties increased to " + getPenalties(user));
        }

        public void multipleReserve(int num, String type, String bed, Date checkin, Date checkout, boolean smoking) throws SQLException, IOException {
            int i = 0;
            boolean error = false;
            ArrayList <Integer> reservs = new ArrayList<Integer>();
            while (i<num && !error) {
                int check = reserve(type, bed, checkin, checkout, smoking);
                if (check == -1) {
                    error = true;
                }
                else {
                    reservs.add(check);
                }
                i++;
            }
            int size = reservs.size();
            if (error) {
                for (int j = 0; j < size; j++) {
                    deleteReservation(reservs.get(j));
                }
                outToClient.writeBytes("-1" + "\n");
            }
            else {
                outToClient.writeBytes(size + "\n");
                for (int j = 0; j < size; j ++) {
                    int reservid = reservs.get(j);
                    int roomnum = getRoomNumFromReserv(reservid);
                    insertlog(roomnum,"Reservation " + reservid + " was made");
                    outToClient.writeBytes(reservid + "\n" );
                }
                String checker = inFromClient.readLine();
                if (checker.equals("Confirmed")) {
                    for (int j = 0; j < size; j++) {
                        confirmReservation(reservs.get(j));
                    }
                }

            }
        }

        public void getCurrentReservations() throws SQLException, IOException {
            String query = "SELECT checkIn,checkOut,roomNumber,reservNumber,status,smoking FROM reservations WHERE user = ? AND checkin >= CURDATE()";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setString(1, username);
            resultSet = preparedStatement.executeQuery();
            ResultSet temp = resultSet;
            int counter = 0;
            while (temp.next()) {
                counter++;
            }
            if (counter == 0) {
                outToClient.writeBytes("0\n");
            }
            else {
                outToClient.writeBytes(counter+"\n");
                resultSet.beforeFirst();
                while (resultSet.next()) {
                    outToClient.writeBytes(resultSet.getDate(1)+"\n");
                    outToClient.writeBytes(resultSet.getDate(2)+"\n");
                    int room = resultSet.getInt(3);
                    outToClient.writeBytes(room+"\n");
                    outToClient.writeBytes(resultSet.getInt(4)+"\n");
                    outToClient.writeBytes(resultSet.getString(5)+"\n");
                    outToClient.writeBytes(resultSet.getBoolean(6)+"\n");
                    getRoomDetails(room);
                }
            }
        }

        public void getPastReservations() throws SQLException, IOException {
            String query = "SELECT checkIn,checkOut,roomNumber,reservNumber FROM reservations WHERE user = ? AND checkin < CURDATE()";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setString(1, username);
            resultSet = preparedStatement.executeQuery();
            ResultSet temp = resultSet;
            int counter = 0;
            while (temp.next()) {
                counter++;
            }
            if (counter == 0) {
                outToClient.writeBytes("0\n");
            }
            else {
                outToClient.writeBytes(counter+"\n");
                resultSet.beforeFirst();
                while (resultSet.next()) {
                    outToClient.writeBytes(resultSet.getDate(1)+"\n");
                    outToClient.writeBytes(resultSet.getDate(2)+"\n");
                    int room = resultSet.getInt(3);
                    outToClient.writeBytes(room+"\n");
                    outToClient.writeBytes(resultSet.getInt(4)+"\n");
                }
            }
        }

        public void getRoomDetails(int roomnumber) throws SQLException, IOException {
            String query = "SELECT type,bedType,price FROM rooms WHERE roomNumber = ?";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setInt(1, roomnumber);
            ResultSet r = preparedStatement.executeQuery();
            r.next();
            outToClient.writeBytes(r.getString(1)+"\n");
            outToClient.writeBytes(r.getString(2)+"\n");
            outToClient.writeBytes(r.getInt(3)+"\n");
        }


        public void findRooms(Date checkin, Date checkout) throws SQLException, IOException {
            String query = "SELECT roomNumber FROM rooms WHERE roomNumber NOT IN (SELECT roomNumber FROM reservations WHERE (? < checkout AND ? > checkin)) AND roomNumber<=20";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setDate(1, checkin);
            preparedStatement.setDate(2, checkout);
            resultSet = preparedStatement.executeQuery();
            int count = 0;
            while (resultSet.next()) {count++;}
            outToClient.writeBytes(count + "\n");    //regular single


            query = "SELECT roomNumber FROM rooms WHERE roomNumber NOT IN (SELECT roomNumber FROM reservations WHERE (? < checkout AND ? > checkin)) AND roomNumber>=21 AND roomNumber<=40 ";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setDate(1, checkin);
            preparedStatement.setDate(2, checkout);
            resultSet = preparedStatement.executeQuery();
            count = 0;
            while (resultSet.next()) {count++;}
            outToClient.writeBytes(count + "\n");    //regular double


            query = "SELECT roomNumber FROM rooms WHERE roomNumber NOT IN (SELECT roomNumber FROM reservations WHERE (? < checkout AND ? > checkin)) AND roomNumber>=41 AND roomNumber<=60 ";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setDate(1, checkin);
            preparedStatement.setDate(2, checkout);
            resultSet = preparedStatement.executeQuery();
            count = 0;
            while (resultSet.next()) {count++;}
            outToClient.writeBytes(count + "\n");    //regular family


            query = "SELECT roomNumber FROM rooms WHERE roomNumber NOT IN (SELECT roomNumber FROM reservations WHERE (? < checkout AND ? > checkin)) AND roomNumber>=61 AND roomNumber<=80 ";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setDate(1, checkin);
            preparedStatement.setDate(2, checkout);
            resultSet = preparedStatement.executeQuery();
            count = 0;
            while (resultSet.next()) {count++;}
            outToClient.writeBytes(count + "\n");    //deluxe single


            query = "SELECT roomNumber FROM rooms WHERE roomNumber NOT IN (SELECT roomNumber FROM reservations WHERE (? < checkout AND ? > checkin)) AND roomNumber>=81 AND roomNumber<=100 ";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setDate(1, checkin);
            preparedStatement.setDate(2, checkout);
            resultSet = preparedStatement.executeQuery();
            count = 0;
            while (resultSet.next()) {count++;}
            outToClient.writeBytes(count + "\n");    //deluxe double


            query = "SELECT roomNumber FROM rooms WHERE roomNumber NOT IN (SELECT roomNumber FROM reservations WHERE (? < checkout AND ? > checkin)) AND roomNumber>=101 AND roomNumber<=120 ";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setDate(1, checkin);
            preparedStatement.setDate(2, checkout);
            resultSet = preparedStatement.executeQuery();
            count = 0;
            while (resultSet.next()) {count++;}
            outToClient.writeBytes(count + "\n");    //deluxe family

        }


        public void getInvoice() throws SQLException, IOException {
            int n1,n2,n3,n4,n5,n6, penalties;
            n1=n2=n3=n4=n5=n6 = 0;

            String query = "SELECT checkIn,checkOut,roomNumber FROM reservations WHERE user = ?";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setString(1,username);
            resultSet = preparedStatement.executeQuery();
            while (resultSet.next()) {
                String in = resultSet.getDate(1).toString();
                String out = resultSet.getDate(2).toString();
                int roomnumber = resultSet.getInt(3);
                int nights = (int) ChronoUnit.DAYS.between(LocalDate.parse(in),LocalDate.parse(out));
                query = "SELECT price FROM rooms WHERE roomNumber = ?";
                preparedStatement = connect.prepareStatement(query);
                preparedStatement.setInt(1, roomnumber);
                ResultSet r = preparedStatement.executeQuery();
                r.next();
                int price = r.getInt(1);
                if (price == 50) {n1 = n1 + nights;}
                if (price == 60) {n2 = n2 + nights;}
                if (price == 70) {n3 = n3 + nights;}
                if (price == 100) {n4 = n4 + nights;}
                if (price == 120) {n5 = n5 + nights;}
                if (price == 140) {n6 = n6 + nights;}
            }
            penalties = getPenalties(username);
            outToClient.writeBytes(n1+"\n");
            outToClient.writeBytes(n2+"\n");
            outToClient.writeBytes(n3+"\n");
            outToClient.writeBytes(n4+"\n");
            outToClient.writeBytes(n5+"\n");
            outToClient.writeBytes(n6+"\n");
            outToClient.writeBytes(penalties+"\n");
        }


        public void terminate() throws IOException, SQLException {
            System.out.println("closing socket");
            if (isLogged) {
                insertlog(0, "Username '"+ username + "' logged out");
                setLogin(false);
            }
            inFromClient.close();
            outToClient.close();
            connectionSocket.close();
        }

        public void insertlog(int room, String log) throws SQLException {
            String query = "INSERT INTO reports (date,user,roomNumber,entry)" + " values (NOW(1),?,?,?)";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setString(1,username);
            preparedStatement.setInt(2,room);
            preparedStatement.setString(3,log);
            preparedStatement.execute();
        }

        public void getLogDates() throws SQLException, IOException {
            String query = "SELECT date FROM reports";
            preparedStatement = connect.prepareStatement(query);
            resultSet = preparedStatement.executeQuery();
            if (!resultSet.next()) {
                outToClient.writeBytes(LocalDate.now()+"/n");
                outToClient.writeBytes(LocalDate.now()+"/n");
            }
            else {
                outToClient.writeBytes(resultSet.getDate(1) + "\n");
                resultSet.last();
                outToClient.writeBytes(resultSet.getDate(1) + "\n");
            }
        }

        public void getLogFromDates(Date first, Date last) throws SQLException, IOException {
            String query = "SELECT date,user,roomNumber,entry FROM reports WHERE date BETWEEN ? AND ?";
            String d1 = first + " 00:00:00";
            String d2 = last + " 23:59:59";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setString(1,d1);
            preparedStatement.setString(2,d2);
            resultSet = preparedStatement.executeQuery();
            int count = 0;
            while (resultSet.next()) {count++;}
            resultSet.beforeFirst();
            resultSet.next();
            outToClient.writeBytes(count+"\n");
            for (int i = 0; i < count; i ++) {
                outToClient.writeBytes(resultSet.getTimestamp(1)+"\n");
                outToClient.writeBytes(resultSet.getString(2) + "\n");
                outToClient.writeBytes(resultSet.getInt(3)+"\n");
                outToClient.writeBytes(resultSet.getString(4)+"\n");
                resultSet.next();
            }
        }

        public void getLogFromRoom(int roomid) throws SQLException, IOException {
            String query = "SELECT date,user,roomNumber,entry FROM reports WHERE roomNumber = ?";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setInt(1,roomid);
            resultSet = preparedStatement.executeQuery();
            int count = 0;
            while (resultSet.next()) {count++;}
            resultSet.beforeFirst();
            resultSet.next();
            outToClient.writeBytes(count+"\n");
            for (int i = 0; i < count; i ++) {
                outToClient.writeBytes(resultSet.getTimestamp(1)+"\n");
                outToClient.writeBytes(resultSet.getString(2) + "\n");
                outToClient.writeBytes(resultSet.getInt(3)+"\n");
                outToClient.writeBytes(resultSet.getString(4)+"\n");
                resultSet.next();
            }
        }

        public void getLogFromUser(String user) throws SQLException, IOException {
            String query = "SELECT date,user,roomNumber,entry FROM reports WHERE user = ?";
            preparedStatement = connect.prepareStatement(query);
            preparedStatement.setString(1,user);
            resultSet = preparedStatement.executeQuery();
            int count = 0;
            while (resultSet.next()) {count++;}
            resultSet.beforeFirst();
            resultSet.next();
            outToClient.writeBytes(count+"\n");
            for (int i = 0; i < count; i ++) {
                outToClient.writeBytes(resultSet.getTimestamp(1)+"\n");
                outToClient.writeBytes(resultSet.getString(2) + "\n");
                outToClient.writeBytes(resultSet.getInt(3)+"\n");
                outToClient.writeBytes(resultSet.getString(4)+"\n");
                resultSet.next();
            }
        }

        public void setLogin(boolean b) throws SQLException {
            String query = "UPDATE usernames_pass SET isLoggedIn = ? WHERE user = ?";
            preparedStatement = connect.prepareStatement(query);
            if (b) {
                preparedStatement.setString(1,"1");
            }
            else {
                preparedStatement.setString(1,"0");
            }
            preparedStatement.setString(2,username);
            preparedStatement.execute();
        }


        public void run() {
            System.out.println("Running");
            try {
                databconnect();
            }
            catch (Exception e) {
                System.out.print(e);
            }

            try {
                inFromClient = new BufferedReader(new InputStreamReader(connectionSocket.getInputStream()));
                outToClient = new DataOutputStream(connectionSocket.getOutputStream());


                /*System.out.println(getPenalties("test3"));
                incrementPenalties("test3",0);
                System.out.println(getPenalties("test3"));*/

                /*Date d1 = Date.valueOf("2019-04-21");
                Date d2 = Date.valueOf("2019-04-24");
                Date d3 = Date.valueOf("2019-04-25");
                Date d4 = Date.valueOf("2019-04-26");
                multipleReserve(1,"Regular","Single",d2,d3,false);
                multipleReserve(5,"Regular","Single",d1,d2,false);
                multipleReserve(4,"Regular","Single",d3,d4,false);
                confirmReservation(1);
                confirmReservation(4);*/



                /*for (int i = 0; i < 20; i++) {                    //THIS IS THE ROOMS TABLE INITIALIZATION
                    String query = "INSERT INTO rooms (bedType,price,type)" + " values (?,?,?)";
                    preparedStatement = connect.prepareStatement(query);
                    preparedStatement.setString(1,"Single");
                    preparedStatement.setString(2,"50");
                    preparedStatement.setString(3,"Regular");
                    preparedStatement.execute();
                }
                for (int i = 0; i < 20; i++) {
                    String query = "INSERT INTO rooms (bedType,price,type)" + " values (?,?,?)";
                    preparedStatement = connect.prepareStatement(query);
                    preparedStatement.setString(1,"Double");
                    preparedStatement.setString(2,"60");
                    preparedStatement.setString(3,"Regular");
                    preparedStatement.execute();
                }
                for (int i = 0; i < 20; i++) {
                    String query = "INSERT INTO rooms (bedType,price,type)" + " values (?,?,?)";
                    preparedStatement = connect.prepareStatement(query);
                    preparedStatement.setString(1,"Family");
                    preparedStatement.setString(2,"70");
                    preparedStatement.setString(3,"Regular");
                    preparedStatement.execute();
                }
                for (int i = 0; i < 20; i++) {
                    String query = "INSERT INTO rooms (bedType,price,type)" + " values (?,?,?)";
                    preparedStatement = connect.prepareStatement(query);
                    preparedStatement.setString(1,"Single");
                    preparedStatement.setString(2,"100");
                    preparedStatement.setString(3,"Deluxe");
                    preparedStatement.execute();
                }
                for (int i = 0; i < 20; i++) {
                    String query = "INSERT INTO rooms (bedType,price,type)" + " values (?,?,?)";
                    preparedStatement = connect.prepareStatement(query);
                    preparedStatement.setString(1,"Double");
                    preparedStatement.setString(2,"120");
                    preparedStatement.setString(3,"Deluxe");
                    preparedStatement.execute();
                }
                for (int i = 0; i < 20; i++) {
                    String query = "INSERT INTO rooms (bedType,price,type)" + " values (?,?,?)";
                    preparedStatement = connect.prepareStatement(query);
                    preparedStatement.setString(1,"Family");
                    preparedStatement.setString(2,"140");
                    preparedStatement.setString(3,"Deluxe");
                    preparedStatement.execute();
                }*/


                String clientSentence = "";
                clientSentence = inFromClient.readLine();
                System.out.println("OK");
                while (!clientSentence.equals("EXIT")) {

                    if (clientSentence.equals("0")) {       //used in case the client closes the human verification dialogue without submitting, which would leave the server waiting for a readline
                        System.out.println("EchoBack");
                        outToClient.writeBytes("EchoBack\n");
                    }

                    if (clientSentence.equals("Echo")) {
                        System.out.println("EchoBack");
                        outToClient.writeBytes("EchoBack\n");
                    }

                    if (clientSentence.equals("Generate")) {
                        generateCode();
                    }

                    if (clientSentence.equals("Get Name")) {
                        getName();
                    }

                    if (clientSentence.equals("Check Password")) {
                        String password = inFromClient.readLine();
                        boolean check = findp(username,password);
                        if (check) {
                            outToClient.writeBytes("Yes\n");
                        }
                        else {
                            outToClient.writeBytes("No\n");
                        }
                    }

                    if (clientSentence.equals("Get Profile")) {
                        getProfile();
                    }

                    if (clientSentence.equals("Get Invoice")) {
                        getInvoice();
                    }

                    if (clientSentence.equals("Edit Profile")) {
                        String fname = inFromClient.readLine();
                        String lname = inFromClient.readLine();
                        String phone = inFromClient.readLine();
                        editProfile(fname,lname,phone);
                    }

                    if (clientSentence.equals("Edit Password")) {
                        String pass = inFromClient.readLine();
                        editPass(pass);
                    }

                    if (clientSentence.equals("List Rooms")) {
                        Date checkin = Date.valueOf(inFromClient.readLine());
                        Date checkout = Date.valueOf(inFromClient.readLine());
                        findRooms(checkin,checkout);
                    }

                    if (clientSentence.equals("Logout")) {
                        setLogin(false);
                        insertlog(0, "Username '"+ username + "' logged out");
                        isLogged = false;
                    }

                    if (clientSentence.equals("S")) {
                        firstName = inFromClient.readLine();
                        lastName  = inFromClient.readLine();
                        gender = inFromClient.readLine();
                        username = inFromClient.readLine();
                        String phone = inFromClient.readLine();
                        boolean check = finduser(username);
                        pass = inFromClient.readLine();

                            if (check) {
                                outToClient.writeBytes("Duplicate\n");
                            } else {
                                insertup(username, pass, firstName, lastName, gender,phone);
                                outToClient.writeBytes("OK\n");
                                insertlog(0, "Username '"+ username + "' has been created");
                            }

                    }

                    if (clientSentence.equals("L")) {
                        username = inFromClient.readLine();
                        pass = inFromClient.readLine();
                        boolean test = findp(username, pass);
                        if (test && !checkonline(username)) {
                            outToClient.writeBytes("Y\n");
                            isLogged = true;
                            setLogin(true);
                            insertlog(0, "Username '"+ username + "' logged in");
                        } else {
                            outToClient.writeBytes("N\n");
                        }
                    }

                    if (clientSentence.equals("Reserve")) {
                        int num = Integer.parseInt(inFromClient.readLine());
                        String type = inFromClient.readLine();
                        String bed = inFromClient.readLine();
                        Date checkin = Date.valueOf(inFromClient.readLine());
                        Date checkout = Date.valueOf(inFromClient.readLine());
                        boolean smoking = Boolean.parseBoolean(inFromClient.readLine());
                        multipleReserve(num,type,bed,checkin,checkout,smoking);

                    }

                    if (clientSentence.equals("Cancel Reservation")) {
                        int reservid = Integer.parseInt(inFromClient.readLine());
                        int roomnumber = getRoomNumFromReserv(reservid);
                        deleteReservation(reservid);
                        insertlog(roomnumber,"Reservation " + reservid + " was cancelled");
                        String checker = inFromClient.readLine();
                        if (checker.equals("Increment")) {
                            incrementPenalties(username,roomnumber);
                        }
                    }
                    
                    if (clientSentence.equals("Edit Reservation")) {
                        int resesrvationNumber = Integer.parseInt(inFromClient.readLine());
                        Date prevcheckin = Date.valueOf(inFromClient.readLine());
                        Date prevcheckout = Date.valueOf(inFromClient.readLine());
                        String prevstatus = inFromClient.readLine();
                        Boolean prevsmoking = Boolean.valueOf(inFromClient.readLine());
                        String prevroomtype = inFromClient.readLine();
                        String prevbedtype =  inFromClient.readLine();

                        Date newcheckin = Date.valueOf(inFromClient.readLine());
                        Date newcheckout = Date.valueOf(inFromClient.readLine());
                        String newstatus = inFromClient.readLine();
                        Boolean newsmoking = Boolean.valueOf(inFromClient.readLine());
                        String newroomtype = inFromClient.readLine();
                        String newbedtype =  inFromClient.readLine();
                        
                        
                        
                        deleteReservation(resesrvationNumber);
                        if (reserveWithID(newroomtype,newbedtype,newcheckin,newcheckout,newsmoking,resesrvationNumber,newstatus) == -1) {
                            //restore reservation
                            reserveWithID(prevroomtype,prevbedtype,prevcheckin,prevcheckout,prevsmoking,resesrvationNumber,prevstatus);
                            outToClient.writeBytes("No\n");
                        }
                        else {
                            outToClient.writeBytes("Yes\n");
                        }

                    }


                    if (clientSentence.equals("Get Reservations")) {
                        getCurrentReservations();
                    }

                    if (clientSentence.equals("Get Past Reservations")) {
                        getPastReservations();
                    }

                    if (clientSentence.equals("Get Log Dates")) {
                        getLogDates();
                    }

                    if (clientSentence.equals("Get Logs from Dates")) {
                        Date d1 = Date.valueOf(inFromClient.readLine());
                        Date d2 = Date.valueOf(inFromClient.readLine());
                        getLogFromDates(d1,d2);
                    }

                    if (clientSentence.equals("Get Logs from Room")) {
                        int roomnum = Integer.parseInt(inFromClient.readLine());
                        getLogFromRoom(roomnum);
                    }

                    if (clientSentence.equals("Get Logs from User")) {
                        String user = inFromClient.readLine();
                        getLogFromUser(user);
                    }


                    clientSentence = inFromClient.readLine();
                }
                terminate();
            }
            catch (IOException | SQLException e) {
                e.printStackTrace();
                if (isLogged) {
                    try {
                        setLogin(false);
                        insertlog(0, "Username '"+ username + "' logged out");
                        isLogged = false;
                    } catch (SQLException e1) {
                        e1.printStackTrace();
                    }
                }
            }
            if (isLogged) {
                try {
                    setLogin(false);
                    insertlog(0, "Username '"+ username + "' logged out");
                    isLogged = false;
                } catch (SQLException e1) {
                    e1.printStackTrace();
                }
            }
            System.out.println("Socket closed");


        }
    }
}
