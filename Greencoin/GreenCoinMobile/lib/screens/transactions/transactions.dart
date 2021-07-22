import 'dart:convert';

import 'package:async/async.dart';
import 'package:expandable/expandable.dart';
import 'package:flutter/material.dart';
import 'package:fyp/controllers/wallet_controller.dart';
import 'package:fyp/locator.dart';
import 'package:fyp/models/Business.dart';
import 'package:fyp/models/Employee.dart';
import 'package:fyp/models/Transaction.dart';
import 'package:fyp/models/User.dart';
import 'package:geolocator/geolocator.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../constants.dart';
import 'components/transaction_card.dart';

class TransactionsScreen extends StatefulWidget {
  @override
  _TransactionsScreenState createState() => _TransactionsScreenState();
}

class _TransactionsScreenState extends State<TransactionsScreen>
    with AutomaticKeepAliveClientMixin<TransactionsScreen> {
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

  Future<dynamic> getTransactions() {
    return this._memoizer.runOnce(() async {
      return await WalletController().getTransactions(user.wallet.id);
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
    else if (userType == 'business')
      user = Business.fromJson(json.decode(prefs.getString('business_object')));

    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: userType == 'employee'
          ? AppBar(
              title: Text(
                "History of Transactions",
                style: TextStyle(color: Colors.white),
              ),
              centerTitle: true,
              backgroundColor: kPrimaryColor,
            )
          : null,
      body: SingleChildScrollView(
        child: FutureBuilder(
          future: getTransactions(),
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
                return b1.unixTime >= b2.unixTime ? -1 : 1;
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
                                  width: MediaQuery.of(context).size.width / 4,
                                  child: Text('Type'),
                                ),
                                Container(
                                  width: MediaQuery.of(context).size.width / 5,
                                  child: Text('Amount'),
                                ),
                                Container(
                                  width: MediaQuery.of(context).size.width / 4,
                                  child: Text('Date'),
                                ),
                                // Container(
                                //   width: MediaQuery.of(context).size.width / 5,
                                //   child: Text('Status'),
                                // ),
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
                            children: [
                              TransactionCard(snapshot.data[index - 1])
                            ],
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
