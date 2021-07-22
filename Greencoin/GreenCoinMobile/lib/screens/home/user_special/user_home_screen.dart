import 'dart:convert';
import 'dart:ui';
import 'package:async/async.dart';
import 'package:fyp/controllers/bag_scan_controller.dart';
import 'package:fyp/controllers/business_controller.dart';
import 'package:fyp/controllers/municipality_controller.dart';
import 'package:fyp/controllers/wallet_controller.dart';
import 'package:fyp/models/BagScan.dart';
import 'package:fyp/models/Business.dart';
import 'package:fyp/models/Municipality.dart';
import 'package:fyp/models/User.dart';
import 'package:fyp/screens/home/bag_scans/components/bag_scan_card.dart';
import 'package:fyp/screens/home/navigation_components/home_drawer.dart';
import 'package:fyp/screens/transactions/components/transaction_card.dart';
import 'package:fyp/services/navigation_service.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../../constants.dart';
import '../../../locator.dart';
import '../../../routes_names.dart';
import 'custom_list_view.dart';
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

class HomeScreen extends StatefulWidget {
  HomeScreen(this.onDrawerCall);

  final Function(DrawerIndex) onDrawerCall;

  @override
  _HomeScreenState createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> with TickerProviderStateMixin {
  AnimationController animationController;
  AnimationController animationController2;

  List<HotelListData> hotelList = HotelListData.hotelList;
  final ScrollController _scrollController = ScrollController();
  final SharedPreferences prefs = locator<SharedPreferences>();

  User user;
  bool loading;

  Municipality municipality;

  DateTime startDate = DateTime.now();
  DateTime endDate = DateTime.now().add(const Duration(days: 5));

  Future<dynamic> getBagScans() {
    return this._memoizer.runOnce(() async {
      municipality = await MunicipalityController().getMunicipalityById();
      setState(() {});
      return await BagScanController().getBagScans(user.id, 'user');
    });
  }

  final AsyncMemoizer _memoizer = AsyncMemoizer();
  final NavigationService _navigationService = locator<NavigationService>();

  getUser() async {
    getUserInfo().then((value) {
      setState(() {
        user = value as User;

        print(user.wallet.balance);
        loading = false;
      });
    });
  }

  @override
  void initState() {
    loading = true;
    getUser();
    animationController = AnimationController(
        duration: const Duration(milliseconds: 1000), vsync: this);

    animationController2 = AnimationController(
        duration: const Duration(milliseconds: 1000), vsync: this);

    super.initState();
    animationController2.forward();
  }

  Future<bool> getData() async {
    await Future<dynamic>.delayed(const Duration(milliseconds: 200));
    return true;
  }

  @override
  void dispose() {
    animationController.dispose();
    animationController2.dispose();

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
        body: !loading
            ? RefreshIndicator(
                onRefresh: () {
                  loading = true;
                  user = null;
                  getUser();
                  setState(() {});
                  return;
                },
                child: SingleChildScrollView(
                  child: Column(
                    children: [
                      Column(children: <Widget>[
                        // getAppBarUI(),
                        municipality != null
                            ? UserProfileCard(
                                user,
                                municipality.municipalityName,
                                animation: Tween<double>(begin: 0.0, end: 1.0)
                                    .animate(CurvedAnimation(
                                        parent: animationController2,
                                        curve: Interval((1 / 9) * 1, 1.0,
                                            curve: Curves.fastOutSlowIn))),
                                animationController: animationController2,
                              )
                            : SizedBox(),

                        SizedBox(
                          height: 10,
                        ),
                        FutureBuilder(
                          future: getBagScans(),
                          builder: (context, snapshot) {
                            int size = 0;

                            if (snapshot.connectionState ==
                                    ConnectionState.done &&
                                snapshot.hasData) {
                              snapshot.data.sort((BagScan b1, BagScan b2) {
                                if (!b1.processed || !b2.processed) {
                                  if (b1.unixTimeScanned >=
                                      b2.unixTimeScanned) {
                                    return -1;
                                  } else {
                                    return 1;
                                  }
                                } else
                                  return 0;
                              });

                              if (snapshot.data.length > 0) size = 1;

                              if (snapshot.data.length == 0 ||
                                  snapshot.data[0].processed) {
                                return SizedBox();
                              }

                              return Column(
                                  crossAxisAlignment: CrossAxisAlignment.center,
                                  mainAxisAlignment:
                                      MainAxisAlignment.spaceBetween,
                                  children: List<Widget>.generate(size + 1,
                                      (int index) {
                                    if (index == 0) {
                                      return TitleView(
                                          titleTxt: 'Pending Bag Scans',
                                          subTxt: 'Show more',
                                          icon: Icons.arrow_forward,
                                          animation: Tween<double>(
                                                  begin: 0.0, end: 1.0)
                                              .animate(CurvedAnimation(
                                                  parent: animationController2,
                                                  curve: Interval(
                                                      (1 / 9) * 0, 1.0,
                                                      curve: Curves
                                                          .fastOutSlowIn))),
                                          animationController:
                                              animationController2,
                                          onclick: () {
                                            widget.onDrawerCall(
                                                DrawerIndex.Transactions);
                                          });
                                    }
                                    return Padding(
                                      padding: const EdgeInsets.all(10.0),
                                      child: SafeArea(
                                        child: Container(
                                          // height: MediaQuery.of(context).size.height / 15,
                                          child: Column(
                                            children: [
                                              BagScanCard(
                                                  snapshot.data[index - 1])
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
                                        new AlwaysStoppedAnimation<Color>(
                                            kPrimaryColor)),
                              );
                            }
                          },
                        ),
                      ]),
                      Column(
                        children: [
                          TitleView(
                            titleTxt: 'Nearest Businesses',
                            subTxt: 'Show more',
                            icon: Icons.arrow_forward,
                            animation: Tween<double>(begin: 0.0, end: 1.0)
                                .animate(CurvedAnimation(
                                    parent: animationController,
                                    curve: Interval((1 / 9) * 0, 1.0,
                                        curve: Curves.fastOutSlowIn))),
                            animationController: animationController,
                            onclick: () {
                              widget.onDrawerCall(DrawerIndex.Businesses);
                            },
                          ),
                          Businesses(animationController)
                        ],
                      )
                    ],
                  ),
                ),
              )
            : Center(
                heightFactor: 20,
                child: CircularProgressIndicator(
                    valueColor:
                        new AlwaysStoppedAnimation<Color>(kPrimaryColor)),
              ),
      ),
    );
  }

  // Widget getListUI() {
  //   return Container(
  //     decoration: BoxDecoration(
  //       color: HotelAppTheme.buildLightTheme().backgroundColor,
  //       boxShadow: <BoxShadow>[
  //         BoxShadow(
  //             color: Colors.grey.withOpacity(0.2),
  //             offset: const Offset(0, -2),
  //             blurRadius: 8.0),
  //       ],
  //     ),
  //     child: Column(
  //       children: <Widget>[
  //         Container(
  //           height: MediaQuery.of(context).size.height - 156 - 50,
  //           child: FutureBuilder<bool>(
  //             future: getData(),
  //             builder: (BuildContext context, AsyncSnapshot<bool> snapshot) {
  //               if (!snapshot.hasData) {
  //                 return const SizedBox();
  //               } else {
  //                 return ListView.builder(
  //                   itemCount: hotelList.length,
  //                   scrollDirection: Axis.vertical,
  //                   itemBuilder: (BuildContext context, int index) {
  //                     final int count =
  //                         hotelList.length > 10 ? 10 : hotelList.length;
  //                     final Animation<double> animation =
  //                         Tween<double>(begin: 0.0, end: 1.0).animate(
  //                             CurvedAnimation(
  //                                 parent: animationController,
  //                                 curve: Interval((1 / count) * index, 1.0,
  //                                     curve: Curves.fastOutSlowIn)));
  //                     animationController.forward();

  //                     return CustomListView(
  //                       callback: () {},
  //                       hotelData: hotelList[index],
  //                       animation: animation,
  //                       animationController: animationController,
  //                     );
  //                   },
  //                 );
  //               }
  //             },
  //           ),
  //         )
  //       ],
  //     ),
  //   );
  // }

  // Widget getHotelViewList() {
  //   final List<Widget> hotelListViews = <Widget>[];
  //   for (int i = 0; i < hotelList.length; i++) {
  //     final int count = hotelList.length;
  //     final Animation<double> animation =
  //         Tween<double>(begin: 0.0, end: 1.0).animate(
  //       CurvedAnimation(
  //         parent: animationController,
  //         curve: Interval((1 / count) * i, 1.0, curve: Curves.fastOutSlowIn),
  //       ),
  //     );
  //     hotelListViews.add(
  //       CustomListView(
  //         callback: () {},
  //         hotelData: hotelList[i],
  //         animation: animation,
  //         animationController: animationController,
  //       ),
  //     );
  //   }
  //   animationController.forward();
  //   return Column(
  //     children: hotelListViews,
  //   );
  // }

  Widget getTimeDateUI() {
    return Padding(
      padding: const EdgeInsets.only(left: 18, bottom: 16),
      child: Row(
        children: <Widget>[
          Expanded(
            child: Row(
              children: <Widget>[
                Material(
                  color: Colors.transparent,
                  child: InkWell(
                    focusColor: Colors.transparent,
                    highlightColor: Colors.transparent,
                    hoverColor: Colors.transparent,
                    splashColor: Colors.grey.withOpacity(0.2),
                    borderRadius: const BorderRadius.all(
                      Radius.circular(4.0),
                    ),
                    onTap: () {
                      FocusScope.of(context).requestFocus(FocusNode());
                      // setState(() {
                      //   isDatePopupOpen = true;
                      // });
                      // showDemoDialog(context: context);
                    },
                    child: Padding(
                      padding: const EdgeInsets.only(
                          left: 8, right: 8, top: 4, bottom: 4),
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: <Widget>[
                          Text(
                            'Choose date',
                            style: TextStyle(
                                fontWeight: FontWeight.w100,
                                fontSize: 16,
                                color: Colors.grey.withOpacity(0.8)),
                          ),
                          const SizedBox(
                            height: 8,
                          ),
                          Text(
                            '${DateFormat("dd, MMM").format(startDate)} - ${DateFormat("dd, MMM").format(endDate)}',
                            style: TextStyle(
                              fontWeight: FontWeight.w100,
                              fontSize: 16,
                            ),
                          ),
                        ],
                      ),
                    ),
                  ),
                ),
              ],
            ),
          ),
          Padding(
            padding: const EdgeInsets.only(right: 8),
            child: Container(
              width: 1,
              height: 42,
              color: Colors.grey.withOpacity(0.8),
            ),
          ),
          Expanded(
            child: Row(
              children: <Widget>[
                Material(
                  color: Colors.transparent,
                  child: InkWell(
                    focusColor: Colors.transparent,
                    highlightColor: Colors.transparent,
                    hoverColor: Colors.transparent,
                    splashColor: Colors.grey.withOpacity(0.2),
                    borderRadius: const BorderRadius.all(
                      Radius.circular(4.0),
                    ),
                    onTap: () {
                      FocusScope.of(context).requestFocus(FocusNode());
                    },
                    child: Padding(
                      padding: const EdgeInsets.only(
                          left: 8, right: 8, top: 4, bottom: 4),
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: <Widget>[
                          Text(
                            'Number of Rooms',
                            style: TextStyle(
                                fontWeight: FontWeight.w100,
                                fontSize: 16,
                                color: Colors.grey.withOpacity(0.8)),
                          ),
                          const SizedBox(
                            height: 8,
                          ),
                          Text(
                            '1 Room - 2 Adults',
                            style: TextStyle(
                              fontWeight: FontWeight.w100,
                              fontSize: 16,
                            ),
                          ),
                        ],
                      ),
                    ),
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget getSearchBarUI() {
    return Padding(
      padding: const EdgeInsets.only(left: 16, right: 16, top: 8, bottom: 8),
      child: Row(
        children: <Widget>[
          Expanded(
            child: Padding(
              padding: const EdgeInsets.only(right: 16, top: 8, bottom: 8),
              child: Container(
                decoration: BoxDecoration(
                  color: HotelAppTheme.buildLightTheme().backgroundColor,
                  borderRadius: const BorderRadius.all(
                    Radius.circular(38.0),
                  ),
                  boxShadow: <BoxShadow>[
                    BoxShadow(
                        color: Colors.grey.withOpacity(0.2),
                        offset: const Offset(0, 2),
                        blurRadius: 8.0),
                  ],
                ),
                child: Padding(
                  padding: const EdgeInsets.only(
                      left: 16, right: 16, top: 4, bottom: 4),
                  child: TextField(
                    onChanged: (String txt) {},
                    style: const TextStyle(
                      fontSize: 18,
                    ),
                    cursorColor: HotelAppTheme.buildLightTheme().primaryColor,
                    decoration: InputDecoration(
                      border: InputBorder.none,
                      hintText: 'London...',
                    ),
                  ),
                ),
              ),
            ),
          ),
          Container(
            decoration: BoxDecoration(
              color: HotelAppTheme.buildLightTheme().primaryColor,
              borderRadius: const BorderRadius.all(
                Radius.circular(38.0),
              ),
              boxShadow: <BoxShadow>[
                BoxShadow(
                    color: Colors.grey.withOpacity(0.4),
                    offset: const Offset(0, 2),
                    blurRadius: 8.0),
              ],
            ),
            child: Material(
              color: Colors.transparent,
              child: InkWell(
                borderRadius: const BorderRadius.all(
                  Radius.circular(32.0),
                ),
                onTap: () {
                  FocusScope.of(context).requestFocus(FocusNode());
                },
                child: Padding(
                  padding: const EdgeInsets.all(16.0),
                  child: Icon(FontAwesomeIcons.search,
                      size: 20,
                      color: HotelAppTheme.buildLightTheme().backgroundColor),
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget getAppBarUI() {
    return Container(
      decoration: BoxDecoration(
        color: HotelAppTheme.buildLightTheme().backgroundColor,
        boxShadow: <BoxShadow>[
          BoxShadow(
              color: Colors.grey.withOpacity(0.2),
              offset: const Offset(0, 2),
              blurRadius: 8.0),
        ],
      ),
      child: Padding(
        padding: EdgeInsets.only(
            top: MediaQuery.of(context).padding.top, left: 8, right: 8),
        child: Row(
          children: <Widget>[
            Container(
              alignment: Alignment.centerLeft,
              width: AppBar().preferredSize.height + 40,
              height: AppBar().preferredSize.height,
              child: Material(
                color: Colors.transparent,
                child: InkWell(
                  borderRadius: const BorderRadius.all(
                    Radius.circular(32.0),
                  ),
                  onTap: () {
                    Navigator.pop(context);
                  },
                  child: Padding(
                    padding: const EdgeInsets.all(8.0),
                    child: Icon(Icons.arrow_back),
                  ),
                ),
              ),
            ),
            Expanded(
              child: Center(
                child: Text(
                  'Explore',
                  style: TextStyle(
                    fontWeight: FontWeight.w600,
                    fontSize: 22,
                  ),
                ),
              ),
            ),
            Container(
              width: AppBar().preferredSize.height + 40,
              height: AppBar().preferredSize.height,
              child: Row(
                crossAxisAlignment: CrossAxisAlignment.center,
                mainAxisAlignment: MainAxisAlignment.end,
                children: <Widget>[
                  Material(
                    color: Colors.transparent,
                    child: InkWell(
                      borderRadius: const BorderRadius.all(
                        Radius.circular(32.0),
                      ),
                      onTap: () {},
                      child: Padding(
                        padding: const EdgeInsets.all(8.0),
                        child: Icon(Icons.favorite_border),
                      ),
                    ),
                  ),
                  Material(
                    color: Colors.transparent,
                    child: InkWell(
                      borderRadius: const BorderRadius.all(
                        Radius.circular(32.0),
                      ),
                      onTap: () {},
                      child: Padding(
                        padding: const EdgeInsets.all(8.0),
                        child: Icon(FontAwesomeIcons.mapMarkerAlt),
                      ),
                    ),
                  ),
                ],
              ),
            )
          ],
        ),
      ),
    );
  }
}

class Businesses extends StatefulWidget {
  Businesses(this.animationController);

  final AnimationController animationController;

  @override
  _BusinessesState createState() => _BusinessesState();
}

class _BusinessesState extends State<Businesses> {
  final NavigationService _navigationService = locator<NavigationService>();

  Future<dynamic> getBusinesses() {
    return this._memoizer.runOnce(() async {
      return await BusinessController().getBusinesses();
    });
  }

  final AsyncMemoizer _memoizer = AsyncMemoizer();

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return FutureBuilder(
      future: getBusinesses(),
      builder: (BuildContext context, AsyncSnapshot<dynamic> snapshot) {
        int size = 10;
        if (snapshot.connectionState == ConnectionState.done &&
            snapshot.hasData) {
          if (snapshot.data.length < 10) size = snapshot.data.length;
          return SafeArea(
            child: Column(
              children: List.generate(
                size,
                // physics: const NeverScrollableScrollPhysics(),
                // itemCount: hotelList.length,
                // padding: const EdgeInsets.only(top: 8),
                // scrollDirection: Axis.vertical,
                (index) {
                  final int count =
                      snapshot.data.length > 10 ? 10 : snapshot.data.length;
                  final Animation<double> animation =
                      Tween<double>(begin: 0.0, end: 1.0).animate(
                          CurvedAnimation(
                              parent: widget.animationController,
                              curve: Interval((1 / count) * index, 1.0,
                                  curve: Curves.fastOutSlowIn)));
                  widget.animationController.forward();
                  return CustomListView(
                    callback: () {
                      _navigationService.navigateTo(BusinessDetailsScreenRoute,
                          arguments: snapshot.data[index]);
                    },
                    businessData: snapshot.data[index],
                    animation: animation,
                    animationController: widget.animationController,
                  );
                },
              ),
            ),
          );
        } else {
          return Center(
            child: CircularProgressIndicator(
                valueColor: new AlwaysStoppedAnimation<Color>(kPrimaryColor)),
          );
        }
      },
    );
  }
}

class ContestTabHeader extends SliverPersistentHeaderDelegate {
  ContestTabHeader(
    this.searchUI,
  );
  final Widget searchUI;

  @override
  Widget build(
      BuildContext context, double shrinkOffset, bool overlapsContent) {
    return searchUI;
  }

  @override
  double get maxExtent => 52.0;

  @override
  double get minExtent => 52.0;

  @override
  bool shouldRebuild(SliverPersistentHeaderDelegate oldDelegate) {
    return false;
  }
}
