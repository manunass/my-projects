import 'package:expandable/expandable.dart';
import 'package:flutter/material.dart';
import 'package:fyp/controllers/request_controller.dart';
import 'package:fyp/locator.dart';
import 'package:fyp/models/Request.dart';
import 'package:fyp/screens/home/navigation_components/home_drawer.dart';
import 'package:fyp/services/navigation_service.dart';

import '../../../../constants.dart';

class RequestCard extends StatelessWidget {
  RequestCard(this.request, this.changeIndex);

  final Function changeIndex;

  final Request request;
  final NavigationService _navigationService = locator<NavigationService>();

  @override
  Widget build(BuildContext context) {
    return ExpandableNotifier(
        child: Container(
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(10),
        shape: BoxShape.rectangle,
        color: Colors.grey.withOpacity(0.05),
        boxShadow: [
          BoxShadow(
            color: Colors.grey.withOpacity(0.05),
            offset: Offset(0.0, 2.0), //(x,y)
            blurRadius: 6.0,
          ),
        ],
      ),
      child: Column(
        children: <Widget>[
          Padding(
            padding: const EdgeInsets.fromLTRB(8, 15, 8, 8),
            child: Row(
              crossAxisAlignment: CrossAxisAlignment.center,
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                // Container(
                //     width: MediaQuery.of(context).size.width / 3,
                //     child: Text(request.quality,
                //         style: TextStyle(
                //           fontWeight: FontWeight.w900,
                //         ))),
                Container(
                  width: MediaQuery.of(context).size.width / 5,
                  child: Text('Checkout Request'),
                ),
                Container(
                    width: MediaQuery.of(context).size.width / 4,
                    child: Text(getDate(DateTime.fromMillisecondsSinceEpoch(
                        request.unixTimeRequested * 1000)))),
                Container(
                  width: MediaQuery.of(context).size.width / 5,
                  child: Text(request.status,
                      style: TextStyle(
                          color: request.status == 'Completed'
                              ? Colors.green
                              : request.status == 'Approved'
                                  ? Colors.purpleAccent
                                  : request.status == 'Declined'
                                      ? Colors.redAccent
                                      : request.status == 'Pending'
                                          ? Colors.blue
                                          : Colors.grey)),
                ),
              ],
            ),
          ),
          SizedBox(
            height: 10,
          ),
          ScrollOnExpand(
            scrollOnExpand: true,
            scrollOnCollapse: false,
            child: ExpandablePanel(
              theme: const ExpandableThemeData(
                headerAlignment: ExpandablePanelHeaderAlignment.center,
                tapBodyToCollapse: true,
              ),
              header: Padding(
                padding: const EdgeInsets.all(8.0),
                child: Text(
                  "Show Details",
                  style: Theme.of(context).textTheme.body2,
                ),
              ),
              expanded: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: <Widget>[
                  SizedBox(
                    height: 20,
                  ),
                  request.status == 'Pending' || request.status == 'Approved'
                      ? Padding(
                          padding: EdgeInsets.only(bottom: 10),
                          child: Align(
                            alignment: Alignment.bottomRight,
                            child: Container(
                              color: kPrimaryColor.withOpacity(0.5),
                              child: InkWell(
                                onTap: () {
                                  showDialog<void>(
                                    context: context,
                                    barrierDismissible:
                                        false, // user must tap button!
                                    builder: (BuildContext context) {
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
                                                      'Are you sure you want to cancel your request for a checkout?',
                                                      false),
                                                  style: TextStyle(
                                                      fontSize: 16,
                                                      color: Colors.black),
                                                  textAlign: TextAlign.center,
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
                                                    'Cancelling your request',
                                                    false,
                                                    context);
                                                RequestController()
                                                    .cancelRequest(request.id)
                                                    .then((value) {
                                                  hideDialog();
                                                  _navigationService.navPop();
                                                  changeIndex();

                                                  print(value);
                                                }).catchError((error) {
                                                  hideDialog();
                                                  _navigationService.navPop();
                                                  print(error);
                                                });
                                              },
                                            ),
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
                                },
                                child: Padding(
                                  padding: const EdgeInsets.all(10),
                                  child: Column(
                                    mainAxisAlignment:
                                        MainAxisAlignment.spaceAround,
                                    crossAxisAlignment:
                                        CrossAxisAlignment.center,
                                    children: [
                                      Text(
                                        "Cancel Request",
                                        style: TextStyle(
                                            color: Colors.white,
                                            fontWeight: FontWeight.normal,
                                            fontSize: 12),
                                      )
                                    ],
                                  ),
                                ),
                              ),
                            ),
                          ),
                        )
                      : SizedBox(),
                ],
              ),
              builder: (_, collapsed, expanded) {
                return Padding(
                  padding: EdgeInsets.only(left: 10, right: 10, bottom: 10),
                  child: Expandable(
                    collapsed: collapsed,
                    expanded: expanded,
                    theme: const ExpandableThemeData(crossFadePoint: 0),
                  ),
                );
              },
            ),
          ),
        ],
      ),
    ));
  }

  String getDate(DateTime dateTime) {
    DateTime now = DateTime.now();

    var difference = now.difference(dateTime);

    if (difference.inSeconds >= 0 && difference.inSeconds < 60)
      return getArabicString("Just now", false);
    else if (difference.inMinutes >= 0 && difference.inMinutes < 60)
      return "${difference.inMinutes} ${getArabicString("min ago", false)}";
    else if (difference.inHours >= 0 && difference.inHours < 24)
      return "${difference.inHours} ${getArabicString("hrs ago", false)}";
    else if (difference.inDays >= 0 && difference.inDays < 31)
      return "${difference.inDays} ${getArabicString("days ago", false)}";
    else if (difference.inDays >= 31 && difference.inDays < 366)
      return "${(difference.inDays / 31).ceil()} ${getArabicString("months ago", false)}";
    else
      return "${(difference.inDays / 366).ceil()} ${getArabicString("years ago", false)}";
  }
}
