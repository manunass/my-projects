import 'package:async/async.dart';
import 'package:flutter/material.dart';
import 'package:flutter_search_bar/flutter_search_bar.dart';
import 'package:fyp/constants.dart';
import 'package:fyp/controllers/business_controller.dart';
import 'package:fyp/models/Business.dart';
import 'package:fyp/routes_names.dart';
import 'package:fyp/screens/home/navigation_components/home_drawer.dart';
import 'package:fyp/services/navigation_service.dart';
import 'package:group_radio_button/group_radio_button.dart';

import '../../locator.dart';
import '../home/hotel_list_data.dart';
import '../home/user_special/custom_list_view.dart';
import 'components/filters_screen.dart';

class BusinessesScreen extends StatefulWidget {
  BusinessesScreen();

  @override
  _BusinessesScreenState createState() => _BusinessesScreenState();
}

class _BusinessesScreenState extends State<BusinessesScreen>
    with TickerProviderStateMixin {
  AnimationController animationController;
  // AnimationController animationController2;

  List<HotelListData> hotelList = HotelListData.hotelList;
  final NavigationService _navigationService = locator<NavigationService>();

  Future<dynamic> businesses;
  String filter;

  getBusinesses() {
    return this._memoizer.runOnce(() async {
      setState(() {
        businesses = BusinessController().getBusinesses();
      });
    });
  }

  final AsyncMemoizer _memoizer = AsyncMemoizer();

  @override
  void initState() {
    getBusinesses();

    filter = 'By Name';

    animationController = AnimationController(
        duration: const Duration(milliseconds: 1000), vsync: this);
    super.initState();
  }

  Color getColor(Set<MaterialState> states) {
    return kPrimaryColor;
  }

  @override
  Widget build(BuildContext context) {
    return Theme(
      data: ThemeData(
        // Define the default brightness and colors.
        primaryColor: kPrimaryColor,
        unselectedWidgetColor: kPrimaryColor,
        radioTheme: RadioThemeData(
            fillColor: MaterialStateProperty.resolveWith(getColor)),
      ),
      child: Scaffold(
        appBar: AppBar(
          title: Text(
            "Businesses",
            style: TextStyle(color: Colors.white),
          ),
          backgroundColor: kPrimaryColor,
          centerTitle: true,
        ),
        body: SingleChildScrollView(
          child: Padding(
              padding: const EdgeInsets.only(top: 8.0, bottom: 8),
              child: Column(
                children: [
                  Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Text('Search by: '),
                      RadioGroup<String>.builder(
                        direction: Axis.horizontal,
                        groupValue: filter,
                        onChanged: (value) => setState(() {
                          filter = value;
                        }),
                        items: ['By Name', 'By Category'],
                        itemBuilder: (item) => RadioButtonBuilder(item),
                      ),
                    ],
                  ),
                  Padding(
                    padding: EdgeInsets.all(12),
                    child: TextField(
                      onSubmitted: (value) {
                        setState(() {
                          businesses = BusinessController().getBusinesses(
                              query: value.toString(), filter: filter);
                        });
                      },
                      decoration: InputDecoration(
                        labelText: getArabicString('Search', false),
                        labelStyle: TextStyle(color: kPrimaryColor),
                        prefixIcon: Padding(
                            padding: const EdgeInsets.all(8.0),
                            child: Icon(
                              Icons.search,
                              color: kPrimaryColor,
                            )),
                        enabledBorder: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(15),
                            borderSide: BorderSide(color: Color(0xFFAAB5C3))),
                        filled: true,
                        focusedBorder: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(15),
                            borderSide: BorderSide(color: Color(0xFFAAB5C3))),
                        border: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(25),
                            borderSide: BorderSide(
                                color: Color.fromRGBO(45, 206, 137, 1.0))),
                      ),
                    ),
                  ),
                  FutureBuilder(
                    future: businesses,
                    builder: (BuildContext context,
                        AsyncSnapshot<dynamic> snapshot) {
                      if (snapshot.connectionState == ConnectionState.done &&
                          snapshot.hasData) {
                        if (snapshot.data.length == 0) {
                          return Center(
                            heightFactor: 10,
                            child: Text('There are no results right now'),
                          );
                        }
                        return SafeArea(
                          child: Column(
                            children: List.generate(
                              snapshot.data.length,
                              // physics: const NeverScrollableScrollPhysics(),
                              // itemCount: hotelList.length,
                              // padding: const EdgeInsets.only(top: 8),
                              // scrollDirection: Axis.vertical,
                              (index) {
                                final int count = snapshot.data.length > 10
                                    ? 10
                                    : snapshot.data.length;
                                final Animation<double> animation =
                                    Tween<double>(begin: 0.0, end: 1.0).animate(
                                        CurvedAnimation(
                                            parent: animationController,
                                            curve: Interval(
                                                (1 / count) * index, 1.0,
                                                curve: Curves.fastOutSlowIn)));
                                animationController.forward();
                                return CustomListView(
                                  callback: () {
                                    _navigationService.navigateTo(
                                        BusinessDetailsScreenRoute,
                                        arguments: snapshot.data[index]);
                                  },
                                  businessData: snapshot.data[index],
                                  animation: animation,
                                  animationController: animationController,
                                );
                              },
                            ),
                          ),
                        );
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
              )),
        ),
      ),
    );
  }

  // Widget getFilterBarUI() {
  //   return InkWell(
  //     focusColor: Colors.transparent,
  //     highlightColor: Colors.transparent,
  //     hoverColor: Colors.transparent,
  //     splashColor: Colors.grey.withOpacity(0.2),
  //     borderRadius: const BorderRadius.all(
  //       Radius.circular(4.0),
  //     ),
  //     onTap: () {
  //       FocusScope.of(context).requestFocus(FocusNode());
  //       Navigator.push<dynamic>(
  //         context,
  //         MaterialPageRoute<dynamic>(
  //             builder: (BuildContext context) => FiltersScreen(),
  //             fullscreenDialog: true),
  //       );
  //     },
  //     child: Padding(
  //       padding: const EdgeInsets.only(left: 8),
  //       child: Row(
  //         children: <Widget>[
  //           Text(
  //             'Filter',
  //             style: TextStyle(
  //               fontWeight: FontWeight.normal,
  //               fontSize: 16,
  //             ),
  //           ),
  //           Padding(
  //             padding: const EdgeInsets.all(8.0),
  //             child: Icon(Icons.sort, color: Colors.white),
  //           ),
  //         ],
  //       ),
  //     ),
  //   );
  // }
}
