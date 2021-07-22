import 'dart:convert';

import 'package:async/async.dart';
import 'package:expandable/expandable.dart';
import 'package:flutter/material.dart';
import 'package:fyp/controllers/bag_scan_controller.dart';
import 'package:fyp/controllers/wallet_controller.dart';
import 'package:fyp/locator.dart';
import 'package:fyp/models/BagScan.dart';
import 'package:fyp/models/Business.dart';
import 'package:fyp/models/Employee.dart';
import 'package:fyp/models/Transaction.dart';
import 'package:fyp/models/User.dart';
import 'package:fyp/screens/home/bag_scans/components/bag_scan_card.dart';
import 'package:geolocator/geolocator.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../../constants.dart';
import '../../transactions/components/transaction_card.dart';

class BagScansScreen extends StatefulWidget {
  @override
  _BagScansScreenState createState() => _BagScansScreenState();
}

class _BagScansScreenState extends State<BagScansScreen>
    with AutomaticKeepAliveClientMixin<BagScansScreen> {
  Color getColor(Set<MaterialState> states) {
    const Set<MaterialState> interactiveStates = <MaterialState>{
      MaterialState.pressed,
      MaterialState.hovered,
      MaterialState.focused,
    };
    if (states.any(interactiveStates.contains)) {
      return Colors.blue;
    }
    return Colors.red;
  }

  Future<dynamic> getBagScans() {
    return this._memoizer.runOnce(() async {
      return await BagScanController().getBagScans(user.id, userType);
    });
  }

  final AsyncMemoizer _memoizer = AsyncMemoizer();
  final SharedPreferences prefs = locator<SharedPreferences>();
  String userType;

  dynamic user;

  @override
  void initState() {
    userType = prefs.getString("user_type");
    if (userType == 'user')
      user = User.fromJson(json.decode(prefs.getString('user_object')));
    else if (userType == 'employee')
      user = Employee.fromJson(json.decode(prefs.getString('employee_object')));

    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: userType != 'user'
          ? AppBar(
              title: Text(
                "History of Bag Scans",
                style: TextStyle(color: Colors.white),
              ),
              centerTitle: true,
              backgroundColor: kPrimaryColor,
            )
          : null,
      body: SingleChildScrollView(
        child: FutureBuilder(
          future: getBagScans(),
          builder: (context, snapshot) {
            if (snapshot.connectionState == ConnectionState.done &&
                snapshot.hasData) {
              if (snapshot.data.length == 0) {
                return Center(
                  heightFactor: 10,
                  child: Text('There are no results right now'),
                );
              }

              snapshot.data.sort((b1, b2) {
                return b1.unixTimeScanned >= b2.unixTimeScanned ? -1 : 1;
              });

              return Column(
                  crossAxisAlignment: CrossAxisAlignment.center,
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: List<Widget>.generate(snapshot.data.length + 1,
                      (int index) {
                    if (index == 0) {
                      return Padding(
                        padding: const EdgeInsets.all(12.0),
                        child: SafeArea(
                          child: Container(
                            height: MediaQuery.of(context).size.height / 18,
                            child: Row(
                              crossAxisAlignment: CrossAxisAlignment.center,
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              children: [
                                Container(
                                  width: MediaQuery.of(context).size.width / 5,
                                  child: Text('Weight'),
                                ),
                                Container(
                                  width: MediaQuery.of(context).size.width / 4,
                                  child: Text('Date'),
                                ),
                                Container(
                                  width: MediaQuery.of(context).size.width / 5,
                                  child: Text('Status'),
                                ),
                              ],
                            ),
                          ),
                        ),
                      );
                    }
                    return Padding(
                      padding: const EdgeInsets.all(10.0),
                      child: SafeArea(
                        child: Container(
                          // height: MediaQuery.of(context).size.height / 15,
                          child: Column(
                            children: [BagScanCard(snapshot.data[index - 1])],
                          ),
                        ),
                      ),
                    );
                  }));
            } else {
              return Center(
                heightFactor: 20,
                child: CircularProgressIndicator(
                    valueColor:
                        new AlwaysStoppedAnimation<Color>(kPrimaryColor)),
              );
            }
          },
        ),
      ),
    );
  }

  @override
  // TODO: implement wantKeepAlive
  bool get wantKeepAlive => true;
}
