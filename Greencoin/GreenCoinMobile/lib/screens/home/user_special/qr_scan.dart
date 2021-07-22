import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_barcode_scanner/flutter_barcode_scanner.dart';
import 'package:fyp/controllers/business_controller.dart';
import 'package:fyp/controllers/user_controller.dart';
import 'package:fyp/controllers/wallet_controller.dart';
import 'package:fyp/models/Business.dart';
import 'package:fyp/models/User.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../../constants.dart';
import '../../../locator.dart';

class QRScanScreen extends StatefulWidget {
  @override
  State<StatefulWidget> createState() => _QRScanScreenState();
}

class _QRScanScreenState extends State<QRScanScreen> {
  String qrCode = '-1';
  bool scanDone = false;
  bool payDone = false;
  bool error = false;

  int nbCoins = -1;
  Business business;
  User user;

  final SharedPreferences prefs = locator<SharedPreferences>();

  @override
  void initState() {
    user = User.fromJson(json.decode(prefs.getString('user_object')));
    // TODO: implement initState
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
          "Scan QR Code",
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
                !scanDone && !error
                    ? 'First, Please scan the business\'s QR Code'
                    : error && scanDone && business == null
                        ? "Scan Again"
                        : payDone
                            ? "Payed Successfully, Scan Again"
                            : business != null && !error
                                ? "Second, Enter the number of coins that you have to pay to ${business.name}"
                                : error
                                    ? "Make sure you have enough balance"
                                    : "Loading...",
                textAlign: TextAlign.center,
                style: TextStyle(
                  fontSize: 16,
                  // color: Colors.white54,
                  fontWeight: FontWeight.bold,
                ),
              ),
              SizedBox(height: 20),
              business != null
                  ? TextField(
                      keyboardType: TextInputType.number,
                      maxLines: 1,
                      decoration:
                          fieldsDesign("Amount of Coins", Icon(Icons.money)),
                      onChanged: (value) {
                        setState(() {
                          nbCoins = int.parse(value);
                        });
                      },
                    )
                  : SizedBox(),
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
              (!scanDone || error) && business == null || payDone
                  ? RaisedButton(
                      color: kPrimaryColor,
                      onPressed: () => scanQRCode(),
                      child: Text(
                        'Start QR scan',
                        style: TextStyle(
                            color: Colors.white, fontWeight: FontWeight.bold),
                      ),
                    )
                  : RaisedButton(
                      color: kPrimaryColor,
                      onPressed: () => payBusiness(),
                      // handle submit and show dialog before                      ,
                      child: Text(
                        'Confirm Payment',
                        style: TextStyle(
                            color: Colors.white, fontWeight: FontWeight.bold),
                      ),
                    ),
            ],
          ),
        ),
      ),
    );
  }

  Future<void> scanQRCode() async {
    try {
      final qrCode = await FlutterBarcodeScanner.scanBarcode(
        '#FF2DCE96',
        'Cancel',
        true,
        ScanMode.QR,
      );

      if (!mounted) return;

      showDialogWindow("Getting the business", true, context);

      BusinessController().getBusinessByCodeId(qrCode).then((value) {
        hideDialog();
        if (value != null) {
          setState(() {
            payDone = false;
            error = false;
            scanDone = true;
            business = value;
          });
        } else {
          print("Errorrrr");
        }
      }).onError((error1, stackTrace) {
        hideDialog();
        setState(() {
          scanDone = true;
          error = true;
        });
      });
    } on PlatformException {
      qrCode = 'Failed to get platform version.';
    }
  }

  Future<void> payBusiness() async {
    showDialogWindow("Paying Business", false, context);
    WalletController()
        .payBusiness(
            user.wallet.id, business.wallet.id, nbCoins, "Paying the business")
        .then((value) {
      UserController().getUserById(user.id).then((value) {
        hideDialog();
        if (value != null) {
          prefs.setString("user_object", jsonEncode(value));
        }

        setState(() {
          payDone = true;
          scanDone = true;
          error = false;
          business = null;
        });
      });
    }).onError((error1, stackTrace) {
      hideDialog();
      setState(() {
        // payDone = true;
        // scanDone = false;
        error = true;
        scanDone = true;
      });
    });
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
