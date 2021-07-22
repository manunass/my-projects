import 'dart:convert';
import 'dart:ui';
import 'package:async/async.dart';
import 'package:flutter_staggered_grid_view/flutter_staggered_grid_view.dart';
import 'package:fyp/controllers/municipality_controller.dart';
import 'package:fyp/controllers/request_controller.dart';
import 'package:fyp/controllers/wallet_controller.dart';
import 'package:fyp/models/Business.dart';
import 'package:fyp/models/Municipality.dart';
import 'package:fyp/screens/home/navigation_components/home_drawer.dart';
import 'package:fyp/screens/transactions/components/transaction_card.dart';
import 'package:fyp/services/navigation_service.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../../constants.dart';
import '../../../locator.dart';
import '../../../routes_names.dart';
import 'business_profile_card.dart';
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

class BusinessHomeScreen extends StatefulWidget {
  BusinessHomeScreen(this.onDrawerCall);

  final Function(DrawerIndex) onDrawerCall;

  @override
  _BusinessHomeScreenState createState() => _BusinessHomeScreenState();
}

class _BusinessHomeScreenState extends State<BusinessHomeScreen>
    with TickerProviderStateMixin {
  AnimationController animationController;
  // AnimationController animationController2;

  List<HotelListData> hotelList = HotelListData.hotelList;
  final ScrollController _scrollController = ScrollController();
  final NavigationService _navigationService = locator<NavigationService>();
  final SharedPreferences prefs = locator<SharedPreferences>();

  DateTime startDate = DateTime.now();
  DateTime endDate = DateTime.now().add(const Duration(days: 5));

  Business business;
  Municipality municipality;

  Future<dynamic> getTransactions() {
    return this._memoizer.runOnce(() async {
      municipality = await MunicipalityController().getMunicipalityById();
      setState(() {});
      return await WalletController().getTransactions(business.wallet.id);
    });
  }

  final AsyncMemoizer _memoizer = AsyncMemoizer();

  @override
  void initState() {
    business =
        Business.fromJson(json.decode(prefs.getString('business_object')));

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
        body: business.imagesUris.isEmpty && !chosen
            ? completeProfile(context)
            : SingleChildScrollView(
                child: Column(
                  children: <Widget>[
                    // getAppBarUI(),
                    municipality != null
                        ? BusinessProfileCard(
                            business,
                            municipality.municipalityName,
                            animation: Tween<double>(begin: 0.0, end: 1.0)
                                .animate(CurvedAnimation(
                                    parent: animationController,
                                    curve: Interval((1 / 9) * 1, 1.0,
                                        curve: Curves.fastOutSlowIn))),
                            animationController: animationController,
                          )
                        : Center(
                            heightFactor: 3,
                            child: CircularProgressIndicator(
                                valueColor: new AlwaysStoppedAnimation<Color>(
                                    kPrimaryColor)),
                          ),

                    municipality != null && business != null
                        ? SafeArea(
                            child: StaggeredGridView.count(
                              shrinkWrap: true,
                              physics: NeverScrollableScrollPhysics(),
                              crossAxisCount: 4,
                              staggeredTiles: <StaggeredTile>[
                                StaggeredTile.count(2, 1),
                                StaggeredTile.count(2, 1),
                                StaggeredTile.count(4, 1),
                              ],
                              mainAxisSpacing: 4,
                              crossAxisSpacing: 4,
                              padding: const EdgeInsets.all(4),
                              children: [
                                GridTile(
                                    kPrimaryColor.withOpacity(0.8),
                                    Icons.attach_money_outlined,
                                    "Request checkout", () {
                                  {
                                    {
                                      BuildContext context1 = context;
                                      showDialog<void>(
                                        context: context,
                                        barrierDismissible:
                                            false, // user must tap button!
                                        builder: (BuildContext context) {
                                          print(municipality.lbpCoinRatio);
                                          dynamic ratio =
                                              municipality.lbpCoinRatio != 0
                                                  ? business.wallet.balance /
                                                      municipality.lbpCoinRatio
                                                  : 0;
                                          return WillPopScope(
                                            onWillPop: () async => false,
                                            child: AlertDialog(
                                              title: Text(''),
                                              content: SingleChildScrollView(
                                                child: ListBody(
                                                  children: <Widget>[
                                                    Icon(
                                                      Icons.warning,
                                                      size: 50,
                                                      color: kPrimaryColor,
                                                    ),
                                                    SizedBox(height: 10),
                                                    Text(
                                                      getArabicString(
                                                          'Are you sure you want to Checkout your ${business.wallet.balance} coins to $ratio LBP ?',
                                                          false),
                                                      style: TextStyle(
                                                          fontSize: 16,
                                                          color: Colors.black),
                                                      textAlign:
                                                          TextAlign.center,
                                                    ),
                                                  ],
                                                ),
                                              ),
                                              actions: <Widget>[
                                                TextButton(
                                                    child: Text(
                                                      getArabicString(
                                                          'I agree', false),
                                                      style: TextStyle(
                                                          color: kPrimaryColor),
                                                    ),
                                                    onPressed: () {
                                                      showDialogWindow(
                                                          'Requesting Checkout',
                                                          false,
                                                          context);
                                                      RequestController()
                                                          .createRequest(
                                                              business.id,
                                                              'business')
                                                          .then((value) {
                                                        hideDialog();
                                                        _navigationService
                                                            .navPop();
                                                        showDialog<void>(
                                                          context: context1,
                                                          barrierDismissible:
                                                              false, // user must tap button!
                                                          builder: (BuildContext
                                                              context) {
                                                            return WillPopScope(
                                                              onWillPop:
                                                                  () async =>
                                                                      false,
                                                              child:
                                                                  AlertDialog(
                                                                title: Text(''),
                                                                content:
                                                                    SingleChildScrollView(
                                                                  child:
                                                                      ListBody(
                                                                    children: <
                                                                        Widget>[
                                                                      Icon(
                                                                        Icons
                                                                            .assignment_turned_in,
                                                                        size:
                                                                            50,
                                                                        color:
                                                                            kPrimaryColor,
                                                                      ),
                                                                      SizedBox(
                                                                          height:
                                                                              10),
                                                                      Text(
                                                                        getArabicString(
                                                                            'Your request is submitted you can check it in transactions section',
                                                                            false),
                                                                        style: TextStyle(
                                                                            fontSize:
                                                                                16,
                                                                            color:
                                                                                Colors.black),
                                                                        textAlign:
                                                                            TextAlign.center,
                                                                      ),
                                                                    ],
                                                                  ),
                                                                ),
                                                                actions: <
                                                                    Widget>[
                                                                  TextButton(
                                                                    child: Text(
                                                                      getArabicString(
                                                                          'Okay',
                                                                          false),
                                                                      style: TextStyle(
                                                                          color:
                                                                              kPrimaryColor),
                                                                    ),
                                                                    onPressed:
                                                                        () {
                                                                      _navigationService
                                                                          .navPop();
                                                                    },
                                                                  ),
                                                                ],
                                                              ),
                                                            );
                                                          },
                                                        );
                                                        print(value);
                                                      }).catchError((error) {
                                                        hideDialog();
                                                        showDialog<void>(
                                                          context: context,
                                                          barrierDismissible:
                                                              false, // user must tap button!
                                                          builder: (BuildContext
                                                              context) {
                                                            return WillPopScope(
                                                              onWillPop:
                                                                  () async =>
                                                                      false,
                                                              child:
                                                                  AlertDialog(
                                                                title: Text(''),
                                                                content:
                                                                    SingleChildScrollView(
                                                                  child:
                                                                      ListBody(
                                                                    children: <
                                                                        Widget>[
                                                                      Icon(
                                                                        Icons
                                                                            .sentiment_dissatisfied_outlined,
                                                                        size:
                                                                            50,
                                                                        color:
                                                                            kPrimaryColor,
                                                                      ),
                                                                      SizedBox(
                                                                          height:
                                                                              10),
                                                                      Text(
                                                                        getArabicString(
                                                                            error.toString(),
                                                                            false),
                                                                        style: TextStyle(
                                                                            fontSize:
                                                                                16,
                                                                            color:
                                                                                Colors.black),
                                                                        textAlign:
                                                                            TextAlign.center,
                                                                      ),
                                                                    ],
                                                                  ),
                                                                ),
                                                                actions: <
                                                                    Widget>[
                                                                  TextButton(
                                                                    child: Text(
                                                                      getArabicString(
                                                                          'Okay',
                                                                          false),
                                                                      style: TextStyle(
                                                                          color:
                                                                              kPrimaryColor),
                                                                    ),
                                                                    onPressed:
                                                                        () {
                                                                      _navigationService
                                                                          .navPop();
                                                                    },
                                                                  ),
                                                                ],
                                                              ),
                                                            );
                                                          },
                                                        );
                                                        print(error);
                                                      });
                                                    }),
                                                TextButton(
                                                  child: Text(
                                                    getArabicString(
                                                        'Cancel', false),
                                                    style: TextStyle(
                                                        color: kPrimaryColor),
                                                  ),
                                                  onPressed: () {
                                                    _navigationService.navPop();
                                                  },
                                                )
                                              ],
                                            ),
                                          );
                                        },
                                      );
                                    }
                                  }
                                }),
                                GridTile(
                                    kPrimaryColor.withOpacity(0.8),
                                    Icons.qr_code_scanner_sharp,
                                    "Show QR Code", () {
                                  widget.onDrawerCall(
                                      DrawerIndex.Business_QRCode);
                                }),
                                GridTile(
                                    kPrimaryColor.withOpacity(0.8),
                                    Icons.compare_arrows_outlined,
                                    "Show all transacions", () {
                                  widget.onDrawerCall(DrawerIndex.Transactions);
                                }),
                              ],
                            ),
                          )
                        : SizedBox(),
                    TitleView(
                        titleTxt: 'Latest Transactions',
                        subTxt: '',
                        icon: null,
                        animation: Tween<double>(begin: 0.0, end: 1.0).animate(
                            CurvedAnimation(
                                parent: animationController,
                                curve: Interval((1 / 9) * 0, 1.0,
                                    curve: Curves.fastOutSlowIn))),
                        animationController: animationController,
                        onclick: () {
                          widget.onDrawerCall(DrawerIndex.Transactions);
                        }),
                    Column(
                      children: [
                        FutureBuilder(
                          future: getTransactions(),
                          builder: (context, snapshot) {
                            int size = 10;

                            if (snapshot.connectionState ==
                                    ConnectionState.done &&
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
                              if (snapshot.data.length < 10)
                                size = snapshot.data.length;

                              return Column(
                                  crossAxisAlignment: CrossAxisAlignment.center,
                                  mainAxisAlignment:
                                      MainAxisAlignment.spaceBetween,
                                  children:
                                      List<Widget>.generate(size, (int index) {
                                    return Padding(
                                      padding: const EdgeInsets.all(10.0),
                                      child: SafeArea(
                                        child: Container(
                                          // height: MediaQuery.of(context).size.height / 15,
                                          child: Column(
                                            children: [
                                              TransactionCard(
                                                  snapshot.data[index])
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
                                    valueColor:
                                        new AlwaysStoppedAnimation<Color>(
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

  completeProfile(BuildContext context) {
    return WillPopScope(
      onWillPop: () async => false,
      child: AlertDialog(
        title: Text('Complete Your Profile'),
        content: SingleChildScrollView(
          child: ListBody(
            children: <Widget>[
              Icon(
                Icons.photo_album,
                size: 50,
                color: kPrimaryColor,
              ),
              SizedBox(height: 10),
              Text(
                'Add images to your profile so that users will preview and buy from you',
                style: TextStyle(fontSize: 16, color: Colors.black),
                textAlign: TextAlign.center,
              ),
            ],
          ),
        ),
        actions: <Widget>[
          TextButton(
            child: Text(
              getArabicString('Skip for now', false),
              style: TextStyle(color: kPrimaryColor),
            ),
            onPressed: () {
              setState(() {
                chosen = true;
              });
            },
          ),
          TextButton(
            child: Text(
              getArabicString('Add Images', false),
              style: TextStyle(color: kPrimaryColor),
            ),
            onPressed: () {
              setState(() {
                chosen = true;
              });
              _navigationService.navigateTo(TabbedUpdateScreenRoute,
                  arguments: 1);
            },
          ),
        ],
      ),
    );
  }
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
