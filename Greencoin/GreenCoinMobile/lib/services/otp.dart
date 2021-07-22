import 'dart:convert';

import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:fyp/controllers/business_controller.dart';
import 'package:fyp/controllers/employee_controller.dart';
import 'package:fyp/controllers/user_controller.dart';
import 'package:fyp/models/Employee.dart';
import 'package:pinput/pin_put/pin_put.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../constants.dart';
import '../locator.dart';
import '../routes_names.dart';
import '../size_config.dart';
import 'navigation_service.dart';

class OTPScreen extends StatefulWidget {
  final String phone;
  OTPScreen(this.phone);
  @override
  _OTPScreenState createState() => _OTPScreenState();
}

class _OTPScreenState extends State<OTPScreen> {
  final GlobalKey<ScaffoldState> _scaffoldkey = GlobalKey<ScaffoldState>();
  String _verificationCode;
  final TextEditingController _pinPutController = TextEditingController();
  final FocusNode _pinPutFocusNode = FocusNode();
  final BoxDecoration pinPutDecoration = BoxDecoration(
    color: Colors.white70,
    borderRadius: BorderRadius.circular(5.0),
    border: Border.all(color: kPrimaryColor, width: 1),
  );

  final NavigationService _navigationService = locator<NavigationService>();
  final SharedPreferences prefs = locator<SharedPreferences>();

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: Scaffold(
        key: _scaffoldkey,
        appBar: AppBar(
          leading: IconButton(
              icon: Icon(Icons.arrow_back_outlined, color: Colors.white),
              onPressed: () => _navigationService.navPop()),
          backgroundColor: kPrimaryColor,
          title: Text(getArabicString('Code Verification', false),
              style: TextStyle(
                fontWeight: FontWeight.normal,
                color: Colors.white,
              )),
        ),
        body: SingleChildScrollView(
          child: Column(
            children: [
              SizedBox(
                height: MediaQuery.of(context).size.height / 10,
              ),
              Container(
                margin: EdgeInsets.all(10),
                child: Center(
                  child: Text(
                    ' ${getArabicString("Enter the code sent to", false)} ${widget.phone}',
                    textAlign: TextAlign.center,
                    style: TextStyle(
                      fontWeight: FontWeight.normal,
                      fontSize: 26,
                    ),
                  ),
                ),
              ),
              Padding(
                padding: const EdgeInsets.all(20.0),
                child: PinPut(
                  fieldsCount: 6,
                  textStyle:
                      const TextStyle(fontSize: 25.0, color: kPrimaryColor),
                  eachFieldWidth: 40.0,
                  eachFieldHeight: 55.0,
                  withCursor: true,
                  focusNode: _pinPutFocusNode,
                  controller: _pinPutController,
                  submittedFieldDecoration: pinPutDecoration,
                  selectedFieldDecoration: pinPutDecoration,
                  followingFieldDecoration: pinPutDecoration,
                  pinAnimationType: PinAnimationType.fade,
                  onSubmit: (pin) async {
                    try {
                      showDialogWindow(
                          getArabicString(
                              "Verifying and signing you in", false),
                          false,
                          context);
                      await FirebaseAuth.instance
                          .signInWithCredential(PhoneAuthProvider.credential(
                              verificationId: _verificationCode, smsCode: pin))
                          .then((value) async {
                        if (value.user != null) {
                          prefs.setString(
                              'phone_number', value.user.phoneNumber);

                          prefs.setString('firebase_user_id', value.user.uid);
                          prefs.setString(
                              'phone_number', value.user.phoneNumber);
                          globalUserId = value.user.uid;

                          if (prefs.getString("user_type_temp") == 'employee') {
                            EmployeeController()
                                .getEmployeeById(
                                    '10dd9be1-6e9c-4f6d-b621-ba6b3c915bf5')
                                .then((value) {
                              if (value != null) {
                                prefs.setString(
                                    "employee_object", jsonEncode(value));
                                prefs.remove("user_type_temp");
                                prefs.setString("user_type", 'employee');
                                prefs.setString('user_id', value.id);
                                prefs.setString('municipality_id',
                                    '${value.municipalityId}');
                                _navigationService
                                    .replaceAndClearNav(MainNavRoute);
                              }
                            });
                          }

                          if (value.additionalUserInfo.isNewUser) {
                            prefs.setBool("temp_user", true);
                            String userType = prefs.getString("user_type_temp");
                            prefs.setString("user_type", userType);

                            _scaffoldkey.currentState.showSnackBar(
                                SnackBar(content: Text('Valid Code')));
                            _navigationService
                                .replaceAndClearNav(SignUpScreenRoute);
                          } else {
                            String userType = prefs.getString("user_type_temp");

                            if (userType == 'business') {
                              BusinessController()
                                  .getBusinessById(
                                      "a983534a-1a35-4bab-b3ec-5e2cbece7745")
                                  .then((value) {
                                if (value != null) {
                                  prefs.setString(
                                      "business_object", jsonEncode(value));
                                  prefs.remove("user_type_temp");
                                  prefs.setString("user_type", 'business');
                                  prefs.setString('user_id', value.id);
                                  prefs.setString('municipality_id',
                                      '${value.municipalityId}');
                                  _navigationService
                                      .replaceAndClearNav(MainNavRoute);
                                }
                              });
                            } else if (userType == 'user') {
                              UserController()
                                  .getUserById(
                                      "5c67d409-8b41-4e24-905b-fdda93b70fbb")
                                  .then((value) {
                                if (value != null) {
                                  prefs.setString(
                                      "user_object", jsonEncode(value));
                                  prefs.remove("user_type_temp");
                                  prefs.setString("user_type", 'user');
                                  prefs.setString('user_id', value.id);
                                  prefs.setString('municipality_id',
                                      '${value.municipalityId}');
                                  _navigationService
                                      .replaceAndClearNav(MainNavRoute);
                                }
                              });
                            }

                            // await Firebase.initializeApp();
                            // FirebaseFirestore.instance.collection("user")
                            // .doc(value.user.uid)
                            //     .get().then((value) {
                            //       if (value.exists){
                            //         _scaffoldkey.currentState
                            //             .showSnackBar(SnackBar(content: Text('Valid OTP')));
                            //         adTimer();
                            //         initValueNotifier();
                            //         NotificationController.instance.takeFCMTokenWhenAppLaunch();
                            //         NotificationController.instance.initLocalNotification();
                            //         setCurrentChatRoomID('none');
                            //         _navigationService.replaceAndClearNav(HomeScreenRoute);
                            //       }
                            //       else{
                            //         _scaffoldkey.currentState
                            //             .showSnackBar(SnackBar(content: Text('Valid OTP')));
                            //         _navigationService.replaceAndClearNav(GetCountriesRoute);

                            //       }
                            // });

                          }
                        }
                      });
                    } catch (e) {
                      FocusScope.of(context).unfocus();
                      hideDialog();
                      _scaffoldkey.currentState.showSnackBar(SnackBar(
                          content:
                              Text(getArabicString('Invalid Code', false))));
                    }
                  },
                ),
              ),
              buildTimer(),
              SizedBox(height: SizeConfig.screenHeight * 0.05),
              GestureDetector(
                onTap: () {
                  _navigationService.replaceNav(OTPScreenRoute,
                      arguments: '${widget.phone}');
                },
                child: Text(
                  getArabicString("Resend Code", false),
                  style: TextStyle(decoration: TextDecoration.underline),
                ),
              )
            ],
          ),
        ),
      ),
    );
  }

  Column buildTimer() {
    return Column(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        Text(getArabicString("This code will expired in:\n ", false)),
        TweenAnimationBuilder(
          tween: Tween(begin: 120.0, end: 0.0),
          duration: Duration(seconds: 120),
          builder: (_, value, child) => Text(
            "${value.toInt()} ${getArabicString("seconds", false)}",
            style: TextStyle(color: kPrimaryColor),
          ),
          onEnd: () => _navigationService.navPop(),
        ),
      ],
    );
  }

  _verifyPhone() async {
    await FirebaseAuth.instance.verifyPhoneNumber(
        phoneNumber: widget.phone,
        verificationCompleted: (PhoneAuthCredential credential) async {
          await FirebaseAuth.instance
              .signInWithCredential(credential)
              .then((value) async {
            if (value.user != null) {
              prefs.setString('phone_number', value.user.phoneNumber);

              prefs.setString('user_id', value.user.uid);
              globalUserId = value.user.uid;
              if (value.additionalUserInfo.isNewUser) {
                prefs.setBool("temp_user", true);

                _scaffoldkey.currentState
                    .showSnackBar(SnackBar(content: Text('Valid Code')));
                // _navigationService.replaceAndClearNav(GetCountriesRoute);

              } else {
                // await Firebase.initializeApp();
                // FirebaseFirestore.instance.collection("user")
                // .doc(value.user.uid)
                //     .get().then((value) {
                //       if (value.exists){
                //         _scaffoldkey.currentState
                //             .showSnackBar(SnackBar(content: Text('Valid OTP')));
                //         adTimer();
                //         initValueNotifier();
                //         NotificationController.instance.takeFCMTokenWhenAppLaunch();
                //         NotificationController.instance.initLocalNotification();
                //         setCurrentChatRoomID('none');
                //         _navigationService.replaceAndClearNav(HomeScreenRoute);
                //       }
                //       else{
                //         _scaffoldkey.currentState
                //             .showSnackBar(SnackBar(content: Text('Valid OTP')));
                //         _navigationService.replaceAndClearNav(GetCountriesRoute);

                //       }
                // });

              }
            }
          });
        },
        verificationFailed: (FirebaseAuthException e) {
          print(e.message);
        },
        codeSent: (String verficationID, int resendToken) {
          setState(() {
            _verificationCode = verficationID;
          });
        },
        codeAutoRetrievalTimeout: (String verificationID) {
          setState(() {
            _verificationCode = verificationID;
          });
        },
        timeout: Duration(seconds: 120));
  }

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    _verifyPhone();
  }
}
