package sample;

import com.sun.javafx.css.StyleManager;
import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.stage.Stage;

public class Main extends Application {

    @Override
    public void start(Stage primaryStage) throws Exception{

        Application.setUserAgentStylesheet(Application.STYLESHEET_MODENA);
        StyleManager.getInstance().addUserAgentStylesheet("/sample/design2.css");

        Logic mainLogic = new Logic();
        FXMLLoader loaderlogin = new FXMLLoader(getClass().getResource("login.fxml"));
        FXMLLoader loadersignup = new FXMLLoader(getClass().getResource("signup.fxml"));
        FXMLLoader loaderreservation = new FXMLLoader(getClass().getResource("NewReservation.fxml"));
        FXMLLoader loadmainmenu = new FXMLLoader(getClass().getResource("mainMenu.fxml"));
        FXMLLoader loadviewreservation = new FXMLLoader(getClass().getResource("viewReservation.fxml"));
        FXMLLoader loadprofile = new FXMLLoader(getClass().getResource("profile.fxml"));
        FXMLLoader loadinvoice = new FXMLLoader(getClass().getResource("invoice.fxml"));
        FXMLLoader loadwelcome = new FXMLLoader(getClass().getResource("welcomePage.fxml"));
        FXMLLoader loadabout = new FXMLLoader(getClass().getResource("aboutUs.fxml"));
        FXMLLoader loadsearchlogs = new FXMLLoader(getClass().getResource("searchLogs.fxml"));

        Parent rootlogin = loaderlogin.load();
        Parent rootsignup = loadersignup.load();
        Parent rootreservation = loaderreservation.load();
        Parent rootmainmenu = loadmainmenu.load();
        Parent rootviewreservation = loadviewreservation.load();
        Parent rootprofile = loadprofile.load();
        Parent rootinvoice = loadinvoice.load();
        Parent rootwelcome = loadwelcome.load();
        Parent rootabout = loadabout.load();
        Parent rootsearchlogs = loadsearchlogs.load();

        Scene loginScene = new Scene(rootlogin, 330, 330);
        Scene signupScene = new Scene(rootsignup, 400, 450);
        Scene reservationScene = new Scene(rootreservation,556,393);
        Scene mainmenuScene = new Scene(rootmainmenu,600,400);
        Scene viewreservationScene =  new Scene(rootviewreservation,1112,421);
        Scene profileScene = new Scene(rootprofile,435,443);
        Scene invoiceScene = new Scene(rootinvoice,600,400);
        Scene welcomeScene = new Scene(rootwelcome,640,420);
        Scene aboutScene = new Scene(rootabout,600,400);
        Scene searchlogsScene = new Scene(rootsearchlogs,600,500);

        LoginPageController login = loaderlogin.getController();
        SignupController signup = loadersignup.getController();
        NewReservationController newreservation = loaderreservation.getController();
        MainMenuController mainmenu = loadmainmenu.getController();
        ViewReservationController viewreservation = loadviewreservation.getController();
        ProfileController profile = loadprofile.getController();
        InvoiceController invoice = loadinvoice.getController();
        WelcomePageController welcome = loadwelcome.getController();
        AboutUsController about = loadabout.getController();
        SearchLogsController searchlogs = loadsearchlogs.getController();


        welcome.login = loginScene;
        welcome.aboutus = aboutScene;
        welcome.l = mainLogic;

        about.welcome = welcomeScene;

        login.signup = signupScene;
        login.mainmenu = mainmenuScene;
        login.l = mainLogic;
        login.menuController = mainmenu;
        login.welcome = welcomeScene;


        signup.login = loginScene;
        signup.l = mainLogic;


        newreservation.l = mainLogic;
        newreservation.mainmenu = mainmenuScene;

        viewreservation.l = mainLogic;
        viewreservation.mainmenu = mainmenuScene;

        profile.l = mainLogic;
        profile.menuController = mainmenu;
        profile.mainmenu = mainmenuScene;

        invoice.l = mainLogic;
        invoice.mainmenu = mainmenuScene;

        searchlogs.l = mainLogic;
        searchlogs.mainmenu = mainmenuScene;

        mainmenu.l = mainLogic;
        mainmenu.login = loginScene;
        mainmenu.newreservation = reservationScene;
        mainmenu.editreservation = viewreservationScene;
        mainmenu.reservationController = viewreservation;
        mainmenu.profile = profileScene;
        mainmenu.profileController = profile;
        mainmenu.invoice = invoiceScene;
        mainmenu.invoiceController = invoice;
        mainmenu.searchlogs = searchlogsScene;
        mainmenu.searchLogsController = searchlogs;


        primaryStage.setTitle("Welcome Page");
        primaryStage.setScene(welcomeScene);
        primaryStage.show();


    }


    public static void main(String[] args) {

        launch(args);
    }
}
