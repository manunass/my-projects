import 'dart:async';

import 'package:flutter/material.dart';
import 'package:fyp/screens/home/navigation_components/navigation_home_screen.dart';
import 'package:fyp/services/navigation_service.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../../constants.dart';
import '../../../locator.dart';
import '../../../routes_names.dart';

class Body extends StatefulWidget {
  @override
  _BodyState createState() {
    return _BodyState();
  }
}

class _BodyState extends State<Body> {
  int currentPage = 0;
  String country;

  String phoneNumber;

  String userId;
  bool tempUser;
  bool loading;

  final NavigationService _navigationService = locator<NavigationService>();
  final SharedPreferences prefs = locator<SharedPreferences>();

  @override
  void initState() {
    // TODO: implement initState
    loading = true;
    super.initState();
    getLanguage();
    getGlobalUserId();
    getGlobalUserName();
    // getPhoneNumber();
    getUserId();
    getTempUser();
    startTime();
  }

  List<bool> isSelected = [false, true];

  startTime() async {
    var duration = new Duration(seconds: 4);
    return new Timer(duration, route);
  }

  route() {
    if (userId != null) {
      getUserInfo().then((value) {
        _navigationService.replaceAndClearNav(MainNavRoute);
      });
    } else {
      setState(() {
        loading = false;
      });
    }
  }

  getUserId() async {
    userId = prefs.getString('user_id');
  }

  getTempUser() async {
    tempUser = prefs.getBool('temp_user');
  }

  getPhoneNumber() async {
    phoneNumber = prefs.getString('phone_number');
  }

  saveLanguage() {
    bool ar = isSelected[0];
    prefs.setBool("arabic", ar);
  }

  FlatButton getButton(String text, Color color) {
    return FlatButton(
      child: Text(text,
          style: TextStyle(color: color, fontWeight: FontWeight.w900)),
      onPressed: null,
    );
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      height: MediaQuery.of(context).size.height,
      decoration: BoxDecoration(color: kPrimaryColor),
      child: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Padding(
              padding: const EdgeInsets.all(15.0),
              child: Image.asset(
                "assets/images/logo2.png",
                height: MediaQuery.of(context).size.height / 5,
              ),
            ),
            loading
                ? CircularProgressIndicator(
                    valueColor: new AlwaysStoppedAnimation<Color>(Colors.white),
                  )
                : Column(
                    children: [
                      SizedBox(
                        height: 20,
                      ),
                      ToggleButtons(
                        children: [
                          getButton("عربي",
                              isSelected[0] ? kPrimaryColor : Colors.white),
                          getButton("English",
                              isSelected[1] ? kPrimaryColor : Colors.white)
                        ],
                        onPressed: (index) {
                          if (index == 1) {
                            isSelected[0] = false;
                            isSelected[1] = true;
                          } else {
                            isSelected[1] = false;
                            isSelected[0] = true;
                          }
                          setState(() {});
                        },
                        fillColor: Colors.white,
                        color: Colors.orange,
                        selectedColor: Colors.orange,
                        borderRadius: BorderRadius.circular(15),
                        borderColor: Colors.white,
                        isSelected: isSelected,
                      ),
                      SizedBox(
                        height: 10,
                      ),
                      Align(
                        alignment: Alignment.bottomCenter,
                        // child: getButton( , Colors.white)
                        child: OutlineButton(
                            color: Colors.white,
                            onPressed: () {
                              saveLanguage();
                              getLanguage();
                              _navigationService.navigateTo(SignInScreenRoute);
                              // _navigationService
                              // .replaceAndClearNav(MainNavRoute);
                            },
                            shape: new RoundedRectangleBorder(
                                side: BorderSide(color: Colors.white),
                                borderRadius: new BorderRadius.circular(15.0)),
                            child: Text(isSelected[0] ? "إختيار" : "Choose",
                                style: TextStyle(
                                    color: Colors.white,
                                    fontWeight: FontWeight.w900))),
                      )
                    ],
                  ),
          ],
        ),
      ),
    );
  }
}
