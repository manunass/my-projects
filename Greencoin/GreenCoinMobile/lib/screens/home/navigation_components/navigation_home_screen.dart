import 'package:fyp/screens/businesses/businesses_screen.dart';
import 'package:fyp/screens/home/bag_scans/bag_scans.dart';
import 'package:fyp/screens/home/business_special/business_home_screen.dart';
import 'package:fyp/screens/home/business_special/update_business/tabbed_update_screen.dart';
import 'package:fyp/screens/home/business_special/update_business/update_business.dart';
import 'package:fyp/screens/home/employee_special/bag_scan.dart';
import 'package:fyp/screens/home/employee_special/employee_home_screen.dart';
import 'package:fyp/screens/home/business_special/qr_create.dart';
import 'package:fyp/screens/home/user_special/qr_scan.dart';
import 'package:fyp/screens/home/user_special/update_user/update_user.dart';
import 'package:fyp/screens/transactions/tabbed_transactions_bag_scans.dart';
import 'package:fyp/screens/transactions/transactions.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../../locator.dart';
import '../app_theme.dart';
import 'package:flutter/material.dart';
import '../user_special/user_home_screen.dart';
import 'home_drawer.dart';
import 'drawer_user_controller.dart';

class NavigationHomeScreen extends StatefulWidget {
  @override
  _NavigationHomeScreenState createState() => _NavigationHomeScreenState();
}

class _NavigationHomeScreenState extends State<NavigationHomeScreen> {
  Widget screenView;
  DrawerIndex drawerIndex;

  final SharedPreferences prefs = locator<SharedPreferences>();
  String userType;

  @override
  void initState() {
    userType = prefs.getString('user_type');
    drawerIndex = userType == 'business'
        ? DrawerIndex.Business_Home
        : userType == 'employee'
            ? DrawerIndex.Employee_Home
            : DrawerIndex.User_Home;
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    // screenView =

    return Container(
      color: AppTheme.nearlyWhite,
      child: SafeArea(
        top: false,
        bottom: false,
        child: Scaffold(
          backgroundColor: AppTheme.nearlyWhite,
          body: DrawerUserController(
            screenIndex: drawerIndex,
            drawerWidth: MediaQuery.of(context).size.width * 0.75,
            onDrawerCall: (DrawerIndex drawerIndexdata) {
              changeIndex(drawerIndexdata);
              //callback from drawer for replace screen as user need with passing DrawerIndex(Enum index)
            },
            // TODO: to be changed
            screenView: drawerIndex == DrawerIndex.User_Home
                ? HomeScreen((DrawerIndex drawerIndexdata) {
                    changeIndex(drawerIndexdata);
                  })
                : drawerIndex == DrawerIndex.Business_Home
                    ? BusinessHomeScreen((DrawerIndex drawerIndexdata) {
                        changeIndex(drawerIndexdata);
                      })
                    : drawerIndex == DrawerIndex.Employee_Home
                        ? EmployeeHomeScreen((DrawerIndex drawerIndexdata) {
                            changeIndex(drawerIndexdata);
                          })
                        : screenView,
            //we replace screen view as we need on navigate starting screens like MyHomePage, HelpScreen, FeedbackScreen, etc...
          ),
        ),
      ),
    );
  }

  void changeIndex(DrawerIndex drawerIndexdata) {
    if (drawerIndex != drawerIndexdata) {
      drawerIndex = drawerIndexdata;
      if (drawerIndex == DrawerIndex.User_Home) {
        setState(() {
          screenView = HomeScreen((DrawerIndex index) {
            changeIndex(index);
          });
        });
      } else if (drawerIndex == DrawerIndex.Businesses) {
        setState(() {
          screenView = BusinessesScreen();
        });
      } else if (drawerIndex == DrawerIndex.Business_Home) {
        setState(() {
          screenView = BusinessHomeScreen((DrawerIndex index) {
            changeIndex(index);
          });
        });
      } else if (drawerIndex == DrawerIndex.Employee_Home) {
        setState(() {
          screenView = EmployeeHomeScreen((DrawerIndex index) {
            changeIndex(index);
          });
        });
      } else if (drawerIndex == DrawerIndex.Transactions) {
        setState(() {
          screenView = userType == 'business'
              ? TabbedTransactionsBagScansScreen('business', () {
                  changeIndex(DrawerIndex.Business_Home);
                })
              : userType == 'employee'
                  ? BagScansScreen()
                  : TabbedTransactionsBagScansScreen('user', () {
                      changeIndex(DrawerIndex.User_Home);
                    });
        });
      } else if (drawerIndex == DrawerIndex.User_Scan_QRCode) {
        setState(() {
          screenView = QRScanScreen();
        });
      } else if (drawerIndex == DrawerIndex.Business_QRCode) {
        setState(() {
          screenView = QRCreateScreen();
        });
      } else if (drawerIndex == DrawerIndex.Employee_Scan_QRCode) {
        setState(() {
          screenView = BagScanScreen();
        });
      } else if (drawerIndex == DrawerIndex.Update_Profile) {
        setState(() {
          screenView =
              userType == 'user' ? UserUpdateScreen() : TabbedUpdateScreen(0);
        });
      } else {
        //do in your way......
      }
    }
  }
}
