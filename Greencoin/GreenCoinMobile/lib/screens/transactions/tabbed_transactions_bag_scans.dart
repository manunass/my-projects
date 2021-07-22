import 'package:flutter/material.dart';
import 'package:fyp/screens/home/bag_scans/bag_scans.dart';
import 'package:fyp/screens/home/navigation_components/home_drawer.dart';
import 'package:fyp/screens/home/requests/requests.dart';
import 'package:fyp/screens/transactions/transactions.dart';

import '../../constants.dart';

class TabbedTransactionsBagScansScreen extends StatefulWidget {
  TabbedTransactionsBagScansScreen(this.userType, this.changeIndex);

  final String userType;
  final Function changeIndex;

  @override
  _TabbedTransactionsBagScansScreenState createState() =>
      _TabbedTransactionsBagScansScreenState();
}

class _TabbedTransactionsBagScansScreenState
    extends State<TabbedTransactionsBagScansScreen> {
  @override
  void initState() {
    // TODO: implement initState

    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
        home: DefaultTabController(
      length: widget.userType == 'user' ? 3 : 2,
      child: Scaffold(
        appBar: AppBar(
          bottom: widget.userType == 'user'
              ? TabBar(indicatorColor: Colors.white, tabs: [
                  Tab(text: "Transactions"),
                  Tab(text: "Bag Scans"),
                  Tab(text: "Requests"),
                ])
              : TabBar(indicatorColor: Colors.white, tabs: [
                  Tab(text: "Transactions"),
                  Tab(text: "Requests"),
                ]),
          title: Text(
            "History",
            style: TextStyle(color: Colors.white),
          ),
          centerTitle: true,
          backgroundColor: kPrimaryColor,
        ),
        body: widget.userType == 'user'
            ? TabBarView(
                children: [
                  TransactionsScreen(),
                  BagScansScreen(),
                  RequestsScreen(widget.changeIndex)
                ],
              )
            : TabBarView(
                children: [
                  TransactionsScreen(),
                  RequestsScreen(widget.changeIndex)
                ],
              ),
      ),
    ));
  }
}
