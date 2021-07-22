import 'dart:convert';
import 'dart:ui';
import 'package:async/async.dart';
import 'package:flutter_staggered_grid_view/flutter_staggered_grid_view.dart';
import 'package:fyp/controllers/bag_scan_controller.dart';
import 'package:fyp/controllers/employee_controller.dart';
import 'package:fyp/controllers/municipality_controller.dart';
import 'package:fyp/controllers/wallet_controller.dart';
import 'package:fyp/models/Employee.dart';
import 'package:fyp/models/Municipality.dart';
import 'package:fyp/screens/home/bag_scans/components/bag_scan_card.dart';
import 'package:fyp/screens/home/navigation_components/home_drawer.dart';
import 'package:fyp/screens/transactions/components/transaction_card.dart';
import 'package:fyp/services/navigation_service.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../../constants.dart';
import '../../../locator.dart';
import '../../../routes_names.dart';
import 'employee_profile_card.dart';
import '../user_special/custom_list_view.dart';
import '../hotel_list_data.dart';
import 'package:flutter/material.dart';
import 'package:font_awesome_flutter/font_awesome_flutter.dart';
import 'package:intl/intl.dart';
// import 'filters_screen.dart';
import '../title_view.dart';
import '../hotel_app_theme.dart';
import '../user_special/user_profile_card.dart';
import 'package:carousel_slider/carousel_slider.dart';
import 'package:expandable/expandable.dart';

class EmployeeHomeScreen extends StatefulWidget {
  EmployeeHomeScreen(this.onDrawerCall);

  final Function(DrawerIndex) onDrawerCall;

  @override
  _EmployeeHomeScreenState createState() => _EmployeeHomeScreenState();
}

class _EmployeeHomeScreenState extends State<EmployeeHomeScreen>
    with TickerProviderStateMixin {
  AnimationController animationController;
  // AnimationController animationController2;

  List<HotelListData> hotelList = HotelListData.hotelList;
  final ScrollController _scrollController = ScrollController();
  final NavigationService _navigationService = locator<NavigationService>();
  final SharedPreferences prefs = locator<SharedPreferences>();

  DateTime startDate = DateTime.now();
  DateTime endDate = DateTime.now().add(const Duration(days: 5));

  Employee employee;
  Municipality municipality;
  Future<dynamic> getBagScans() {
    return this._memoizer.runOnce(() async {
      municipality = await MunicipalityController().getMunicipalityById();
      setState(() {});
      return await BagScanController().getBagScans(employee.id, 'employee');
    });
  }

  final AsyncMemoizer _memoizer = AsyncMemoizer();

  @override
  void initState() {
    // TODO: to be updated
    // getMunicipality();
    employee =
        Employee.fromJson(json.decode(prefs.getString('employee_object')));

    animationController = AnimationController(
        duration: const Duration(milliseconds: 1000), vsync: this);

    super.initState();
    animationController.forward();
  }

  // Future<bool> getData() async {
  //   await Future<dynamic>.delayed(const Duration(milliseconds: 200));
  //   return true;
  // }

  @override
  void dispose() {
    animationController.dispose();
    // animationController2.dispose();

    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Theme(
      data: HotelAppTheme.buildLightTheme(),
      child: Scaffold(
        appBar: AppBar(
          // leading: Icon(Icons.menu),
          backgroundColor: kPrimaryColor,
          flexibleSpace: Container(
            margin: EdgeInsets.only(top: 15),
            height: MediaQuery.of(context).size.height * 0.1,
            child: Image.asset("assets/images/logo2.png"),
          ),
        ),
        body: SingleChildScrollView(
          child: Column(
            children: <Widget>[
              // getAppBarUI(),
              municipality != null
                  ? EmployeeProfileCard(
                      employee,
                      municipality.municipalityName,
                      animation: Tween<double>(begin: 0.0, end: 1.0).animate(
                          CurvedAnimation(
                              parent: animationController,
                              curve: Interval((1 / 9) * 1, 1.0,
                                  curve: Curves.fastOutSlowIn))),
                      animationController: animationController,
                    )
                  : Center(
                      heightFactor: 3,
                      child: CircularProgressIndicator(
                          valueColor:
                              new AlwaysStoppedAnimation<Color>(kPrimaryColor)),
                    ),

              SafeArea(
                child: StaggeredGridView.count(
                  shrinkWrap: true,
                  physics: NeverScrollableScrollPhysics(),
                  crossAxisCount: 4,
                  staggeredTiles: <StaggeredTile>[
                    StaggeredTile.count(4, 1),
                    StaggeredTile.count(4, 1),
                  ],
                  mainAxisSpacing: 4,
                  crossAxisSpacing: 4,
                  padding: const EdgeInsets.all(4),
                  children: [
                    GridTile(kPrimaryColor.withOpacity(0.8),
                        Icons.qr_code_scanner_sharp, "Scan a Bag", () {
                      widget.onDrawerCall(DrawerIndex.Employee_Scan_QRCode);
                    }),
                    GridTile(
                        kPrimaryColor.withOpacity(0.8),
                        Icons.compare_arrows_outlined,
                        "Show all Bag Scans", () {
                      widget.onDrawerCall(DrawerIndex.Transactions);
                    }),
                  ],
                ),
              ),
              TitleView(
                  titleTxt: 'Latest Bag Scans',
                  subTxt: '',
                  icon: null,
                  animation: Tween<double>(begin: 0.0, end: 1.0).animate(
                      CurvedAnimation(
                          parent: animationController,
                          curve: Interval((1 / 9) * 0, 1.0,
                              curve: Curves.fastOutSlowIn))),
                  animationController: animationController,
                  onclick: () {
                    // widget.onDrawerCall(DrawerIndex.Transactions);
                  }),
              Column(
                children: [
                  FutureBuilder(
                    future: getBagScans(),
                    builder: (context, snapshot) {
                      int size = 10;

                      if (snapshot.connectionState == ConnectionState.done &&
                          snapshot.hasData) {
                        snapshot.data.sort((b1, b2) {
                          if (!b1.processed || !b2.processed) {
                            if (b1.unixTimeScanned >= b2.unixTimeScanned) {
                              return -1;
                            } else {
                              return 1;
                            }
                          } else
                            return 0;
                        });
                        if (snapshot.data.length < 10)
                          size = snapshot.data.length;

                        return Column(
                            crossAxisAlignment: CrossAxisAlignment.center,
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: List<Widget>.generate(size, (int index) {
                              return Padding(
                                padding: const EdgeInsets.all(10.0),
                                child: SafeArea(
                                  child: Container(
                                    // height: MediaQuery.of(context).size.height / 15,
                                    child: Column(
                                      children: [
                                        BagScanCard(snapshot.data[index])
                                      ],
                                    ),
                                  ),
                                ),
                              );
                            }));
                      } else {
                        return Center(
                          heightFactor: 10,
                          child: CircularProgressIndicator(
                              valueColor: new AlwaysStoppedAnimation<Color>(
                                  kPrimaryColor)),
                        );
                      }
                    },
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }

  // Widget getTile(Color backgroundColor, IconData iconData, String text) {

  // }
}

class GridTile extends StatelessWidget {
  GridTile(this.backgroundColor, this.iconData, this.text, this.onTap);
  final Color backgroundColor;
  final IconData iconData;
  final String text;
  final Function onTap;

  @override
  Widget build(BuildContext context) {
    return Card(
      color: backgroundColor,
      child: InkWell(
        onTap: () {
          onTap();
        },
        child: Center(
          child: Padding(
            padding: const EdgeInsets.all(4),
            child: Column(
              mainAxisAlignment: MainAxisAlignment.spaceAround,
              crossAxisAlignment: CrossAxisAlignment.center,
              children: [
                Icon(
                  iconData,
                  color: Colors.white,
                  size: 35,
                ),
                Text(
                  text,
                  style: TextStyle(
                      color: Colors.white,
                      fontWeight: FontWeight.w900,
                      fontSize: 16),
                )
              ],
            ),
          ),
        ),
      ),
    );
  }
}
