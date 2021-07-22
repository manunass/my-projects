import 'dart:convert';

import 'package:barcode_widget/barcode_widget.dart';
import 'package:flutter/material.dart';
import 'package:fyp/controllers/business_controller.dart';
import 'package:fyp/models/Business.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../../constants.dart';
import '../../../locator.dart';

class QRCreateScreen extends StatefulWidget {
  @override
  _QRCreateScreenState createState() => _QRCreateScreenState();
}

class _QRCreateScreenState extends State<QRCreateScreen> {
  final controller = TextEditingController();
  final SharedPreferences prefs = locator<SharedPreferences>();
  Business business;
  String code;

  getCode() {
    BusinessController().getCodeByBusinessId(business.id).then((value) {
      setState(() {
        code = value;
      });
    });
  }

  @override
  void initState() {
    business =
        Business.fromJson(json.decode(prefs.getString('business_object')));

    getCode();

    super.initState();
  }

  @override
  Widget build(BuildContext context) => Scaffold(
        appBar: AppBar(
          title: Text(
            "Your QR Code",
            style: TextStyle(color: Colors.white),
          ),
          centerTitle: true,
          backgroundColor: kPrimaryColor,
        ),
        body: Center(
          child: SingleChildScrollView(
            padding: EdgeInsets.all(24),
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Text(
                  "Let your customers scan this QR Code to pay you.",
                  textAlign: TextAlign.center,
                  style: TextStyle(
                    fontSize: 16,
                    // color: Colors.white54,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                SizedBox(height: 40),
                code != null
                    ? BarcodeWidget(
                        barcode: Barcode.qrCode(),
                        // color: Colors.white,
                        data: code,
                        width: MediaQuery.of(context).size.width / 1.5,
                        height: MediaQuery.of(context).size.width / 1.5,
                      )
                    : Center(
                        heightFactor: 10,
                        child: CircularProgressIndicator(
                            valueColor: new AlwaysStoppedAnimation<Color>(
                                kPrimaryColor)),
                      ),
                SizedBox(height: 40),
                // Row(
                //   children: [
                //     Expanded(child: buildTextField(context)),
                //     const SizedBox(width: 12),
                //     FloatingActionButton(
                //       // backgroundColor: Theme.of(context).primaryColor,
                //       child: Icon(Icons.done, size: 30),
                //       onPressed: () => setState(() {}),
                //     )
                //   ],
                // ),
              ],
            ),
          ),
        ),
      );

  Widget buildTextField(BuildContext context) => TextField(
        controller: controller,
        style: TextStyle(
          // color: Colors.white,
          fontWeight: FontWeight.bold,
          fontSize: 20,
        ),
        decoration: InputDecoration(
          hintText: 'Enter the data',
          hintStyle: TextStyle(color: Colors.grey),
          enabledBorder: OutlineInputBorder(
            borderRadius: BorderRadius.circular(16),
            borderSide: BorderSide(color: Colors.white),
          ),
          focusedBorder: OutlineInputBorder(
            borderRadius: BorderRadius.circular(16),
            borderSide: BorderSide(
              color: Theme.of(context).primaryColor,
            ),
          ),
        ),
      );
}
