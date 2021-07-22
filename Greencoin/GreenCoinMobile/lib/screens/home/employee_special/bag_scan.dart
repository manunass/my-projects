import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_barcode_scanner/flutter_barcode_scanner.dart';
import 'package:fyp/controllers/business_controller.dart';
import 'package:fyp/controllers/user_controller.dart';
import 'package:fyp/controllers/wallet_controller.dart';
import 'package:fyp/models/Business.dart';
import 'package:fyp/models/Employee.dart';
import 'package:fyp/models/User.dart';
import 'package:fyp/routes_names.dart';
import 'package:fyp/services/navigation_service.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../../constants.dart';
import '../../../locator.dart';

class BagScanScreen extends StatefulWidget {
  @override
  State<StatefulWidget> createState() => _BagScanScreenState();
}

class _BagScanScreenState extends State<BagScanScreen> {
  String qrCode = '-1';
  bool scanDone = false;
  bool error = false;

  int nbCoins = -1;
  Map<String, dynamic> userInfo;
  Employee employee;

  final NavigationService _navigationService = locator<NavigationService>();

  final SharedPreferences prefs = locator<SharedPreferences>();

  @override
  void initState() {
    employee =
        Employee.fromJson(json.decode(prefs.getString('employee_object')));
    // TODO: implement initState
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
          "Scan a Bag",
          style: TextStyle(color: Colors.white),
        ),
        centerTitle: true,
        backgroundColor: kPrimaryColor,
      ),
      body: Center(
        child: Padding(
          padding: const EdgeInsets.all(24.0),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: <Widget>[
              Text(
                !error ? 'First, Please scan the Bag\'s QR Code' : "Try Again",
                textAlign: TextAlign.center,
                style: TextStyle(
                  fontSize: 16,
                  // color: Colors.white54,
                  fontWeight: FontWeight.bold,
                ),
              ),

              // Text(
              //   // '',
              //   '$qrCode',
              //   style: TextStyle(
              //     fontSize: 28,
              //     fontWeight: FontWeight.bold,
              //     // color: Colors.white,
              //   ),
              // ),
              Icon(
                Icons.qr_code_outlined,
                size: 48,
              ),
              // SizedBox(height: 10),
              RaisedButton(
                color: kPrimaryColor,
                onPressed: () => scanQRCode(),
                child: Text(
                  'Start QR scan',
                  style: TextStyle(
                      color: Colors.white, fontWeight: FontWeight.bold),
                ),
              )
            ],
          ),
        ),
      ),
    );
  }

  Future<void> scanQRCode() async {
    try {
      scanDone = false;
      final qrCode = await FlutterBarcodeScanner.scanBarcode(
        '#FF2DCE96',
        'Cancel',
        true,
        ScanMode.QR,
      );

      if (!mounted) return;
      showDialogWindow("Checking QR Code", true, this.context);
      UserController().getUserInfoByCodeId(qrCode).then((value) {
        hideDialog();
        if (value != null) {
          setState(() {
            error = false;
            scanDone = true;
            userInfo = value;
          });
          userInfo['EmployeeId'] = employee.id;
          _navigationService.navigateTo(BagScanFormRoute, arguments: userInfo);
        } else {
          setState(() {
            error = true;
          });
          print("Errorrrr");
        }
      }).onError((error1, stackTrace) {
        hideDialog();
        setState(() {
          error = true;
          scanDone = true;
        });
        print("Errorrrr");
      });
    } on PlatformException {
      qrCode = 'Failed to get platform version.';
    }
  }

  InputDecoration fieldsDesign(String hint, Icon preIcon) {
    return InputDecoration(
      labelText: hint,
      prefixIcon: Padding(padding: const EdgeInsets.all(8.0), child: preIcon),
      enabledBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(25),
          borderSide: BorderSide(color: Color(0xFFAAB5C3))),
      filled: true,
      fillColor: Color(0xFFF3F3F5),
      focusColor: Color(0xFFF3F3F5),
      focusedBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(25),
          borderSide: BorderSide(color: Color(0xFFAAB5C3))),
      border: OutlineInputBorder(
          borderRadius: BorderRadius.circular(25),
          borderSide: BorderSide(color: Color.fromRGBO(45, 206, 137, 1.0))),
    );
  }
}
