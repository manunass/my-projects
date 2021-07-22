import 'package:flutter/material.dart';
import 'package:flutter_rating_bar/flutter_rating_bar.dart';
import 'package:fyp/controllers/bag_scan_controller.dart';
import 'package:fyp/routes_names.dart';
import 'package:fyp/services/navigation_service.dart';
import 'package:intl/intl.dart' as intl;

import '../../../constants.dart';
import '../../../locator.dart';

class BagScanForm extends StatefulWidget {
  BagScanForm(this.userInfo);

  final Map<String, dynamic> userInfo;
  @override
  _BagScanFormState createState() => _BagScanFormState();
}

class _BagScanFormState extends State<BagScanForm> {
  final _formKey = GlobalKey<FormState>();
  String weight = '';
  String rating = '';
  DateTime date = DateTime.now();
  double maxValue = 0;
  bool ratingError = false;
  bool weightError = false;

  TextEditingController controllerWeight = TextEditingController();
  final NavigationService _navigationService = locator<NavigationService>();

  @override
  Widget build(BuildContext context) {
    return Theme(
      data: ThemeData(
        // Define the default brightness and colors.
        primaryColor: kPrimaryColor,
      ),
      child: Scaffold(
        appBar: AppBar(
          leading: InkWell(
            onTap: () => _navigationService.navPop(),
            child: Icon(
              Icons.arrow_back,
              color: Colors.white,
            ),
          ),
          title: Text(
            "Bag Scan",
            style: TextStyle(color: Colors.white),
          ),
          centerTitle: true,
          backgroundColor: kPrimaryColor,
        ),
        body: Form(
          key: _formKey,
          child: Scrollbar(
            child: Align(
              alignment: Alignment.center,
              child: Card(
                child: SingleChildScrollView(
                  padding: EdgeInsets.all(16),
                  child: ConstrainedBox(
                    constraints: BoxConstraints(maxWidth: 400),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      crossAxisAlignment: CrossAxisAlignment.center,
                      children: [
                        ...[
                          Text(
                            "Fill the info below and submit",
                            style: Theme.of(context).textTheme.headline6,
                          ),
                          TextFormField(
                            keyboardType: TextInputType.number,
                            autofocus: true,
                            decoration: InputDecoration(
                                alignLabelWithHint: true,
                                contentPadding: EdgeInsets.only(
                                    left: 24, bottom: 16, right: 16, top: 16),
                                filled: true,
                                fillColor: kPrimaryColor.withOpacity(0.1),
                                hintText: 'Enter a the weight in Kg...',
                                labelText: 'Weight',
                                enabledBorder: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(15),
                                    borderSide:
                                        BorderSide(color: Color(0xFFAAB5C3))),
                                focusedBorder: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(15),
                                    borderSide:
                                        BorderSide(color: Color(0xFFAAB5C3))),
                                border: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(15),
                                    borderSide: BorderSide(
                                        color: Color.fromRGBO(
                                            45, 206, 137, 1.0)))),
                            onChanged: (value) {
                              String temp = value;
                              if (!value.contains('.')) {
                                temp = '$value.0';
                              }
                              setState(() {
                                weight = value;
                              });
                            },
                          ),
                          Column(
                            mainAxisAlignment: MainAxisAlignment.start,
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Row(
                                mainAxisAlignment:
                                    MainAxisAlignment.spaceBetween,
                                children: [
                                  Text(
                                    'Rate the bag quality',
                                    style:
                                        Theme.of(context).textTheme.subtitle1,
                                  ),
                                ],
                              ),
                              Align(
                                child: RatingBar.builder(
                                  initialRating: -1,
                                  itemCount: 5,
                                  itemSize: 50,
                                  tapOnlyMode: true,
                                  itemPadding: EdgeInsets.all(8),
                                  unratedColor: kPrimaryColor.withOpacity(0.4),
                                  itemBuilder: (context, index) {
                                    switch (index) {
                                      case 0:
                                        return Column(
                                          children: [
                                            Icon(
                                              Icons.star,
                                              color: Colors.amber,
                                              size: 44,
                                              // Icons.sentiment_very_dissatisfied,
                                              // color: Colors.red,
                                            ),
                                            Text('Very Bad')
                                          ],
                                        );
                                      case 1:
                                        return Column(
                                          children: [
                                            Icon(
                                              Icons.star,
                                              color: Colors.amber,
                                              size: 44,
                                              // Icons.sentiment_dissatisfied,
                                              // color: Colors.redAccent,
                                            ),
                                            Text('Bad')
                                          ],
                                        );
                                      case 2:
                                        return Column(
                                          children: [
                                            Icon(
                                              Icons.star,
                                              color: Colors.amber,
                                              size: 44,
                                              // Icons.sentiment_neutral,
                                              // color: Colors.amber,
                                            ),
                                            Text('Good')
                                          ],
                                        );
                                      case 3:
                                        return Column(
                                          children: [
                                            Icon(
                                              Icons.star,
                                              color: Colors.amber,
                                              size: 50,
                                              // Icons.sentiment_satisfied,
                                              // color: Colors.lightGreen,
                                            ),
                                            Text(
                                              'Very Good',
                                              style: TextStyle(fontSize: 13),
                                            )
                                          ],
                                        );
                                      case 4:
                                        return Column(
                                          children: [
                                            Icon(
                                              Icons.star,
                                              color: Colors.amber,
                                              size: 44,
                                              // Icons.sentiment_very_satisfied,
                                              // color: Colors.green,
                                            ),
                                            Text('Excellent')
                                          ],
                                        );
                                    }
                                  },
                                  onRatingUpdate: (rating1) {
                                    String temp = "";
                                    if (rating1 == 1.0)
                                      temp = "VeryBad";
                                    else if (rating1 == 2.0)
                                      temp = "Bad";
                                    else if (rating1 == 3.0)
                                      temp = "Good";
                                    else if (rating1 == 4.0)
                                      temp = "VeryGood";
                                    else if (rating1 == 5.0) temp = "Excellent";

                                    setState(() {
                                      rating = temp;
                                    });
                                  },
                                ),
                              ),
                              // Slider(
                              //   min: 0,
                              //   max: 500,
                              //   divisions: 500,
                              //   value: maxValue,
                              //   onChanged: (value) {
                              //     setState(() {
                              //       maxValue = value;
                              //     });
                              //   },
                              // ),
                            ],
                          ),
                          false
                              ? TextFormField(
                                  decoration: InputDecoration(
                                      // border: const OutlineInputBorder(),
                                      filled: true,
                                      fillColor: kPrimaryColor.withOpacity(0.1),
                                      hintText: 'Enter your feedback...',
                                      labelText: 'Feedback',
                                      enabledBorder: OutlineInputBorder(
                                          borderRadius:
                                              BorderRadius.circular(15),
                                          borderSide: BorderSide(
                                              color: Color(0xFFAAB5C3))),
                                      focusedBorder: OutlineInputBorder(
                                          borderRadius:
                                              BorderRadius.circular(15),
                                          borderSide: BorderSide(
                                              color: Color(0xFFAAB5C3))),
                                      border: OutlineInputBorder(
                                          borderRadius:
                                              BorderRadius.circular(15),
                                          borderSide: BorderSide(
                                              color: Color.fromRGBO(
                                                  45, 206, 137, 1.0)))),
                                  onChanged: (value) {
                                    //  = value;
                                  },
                                  maxLines: 5,
                                )
                              : SizedBox(),
                          Text(
                            ratingError
                                ? 'Click on the stars to rate the quality'
                                : weightError
                                    ? 'Please Enter the weight'
                                    : '',
                            style: TextStyle(color: Colors.redAccent),
                          ),
                          GestureDetector(
                            onTap: () {
                              if (weight == null || weight == '') {
                                setState(() {
                                  ratingError = false;
                                  weightError = true;
                                });
                              } else if (rating == null || rating == '') {
                                setState(() {
                                  ratingError = true;
                                  weightError = false;
                                });
                              } else {
                                showDialogWindow(
                                    "Creating Bag Scan", false, context);
                                setState(() {
                                  ratingError = false;
                                  weightError = false;
                                });
                                widget.userInfo['Weight'] = weight;
                                widget.userInfo['Quality'] = rating;

                                print(widget.userInfo);

                                BagScanController()
                                    .createBagScan(widget.userInfo)
                                    .then((value) {
                                  if (value != null) {
                                    //show dialog of success then scan again
                                    hideDialog();
                                    showSuccessDialog();
                                    // _navigationService.navPop();
                                  }
                                });
                              }
                            },
                            child: SizedBox(
                                height:
                                    MediaQuery.of(context).size.height * 0.07,
                                width: MediaQuery.of(context).size.width * 0.7,
                                child: Card(
                                    elevation: 10,
                                    shape: RoundedRectangleBorder(
                                      borderRadius: BorderRadius.circular(10),
                                    ),
                                    color: Color.fromRGBO(45, 206, 137, 1.0),
                                    child: Padding(
                                      padding: const EdgeInsets.symmetric(
                                          horizontal: 10),
                                      child: Center(
                                        child: Text(
                                          "Submit",
                                          style: TextStyle(
                                              color: Colors.white,
                                              fontSize: 17,
                                              fontWeight: FontWeight.bold),
                                        ),
                                      ),
                                    ))),
                          ),
                        ].expand(
                          (widget) => [
                            widget,
                            SizedBox(
                              height: 30,
                            )
                          ],
                        )
                      ],
                    ),
                  ),
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }

  showSuccessDialog() {
    return showDialog<void>(
      context: context,
      barrierDismissible: false, // user must tap button!
      builder: (BuildContext context) {
        return WillPopScope(
          onWillPop: () async => false,
          child: AlertDialog(
            title: Text(''),
            content: SingleChildScrollView(
              child: ListBody(
                children: <Widget>[
                  Icon(
                    Icons.assignment_turned_in_outlined,
                    size: 55,
                    color: kPrimaryColor,
                  ),
                  SizedBox(height: 10),
                  Text(
                    getArabicString('Bag Scan added.', false),
                    style: TextStyle(fontSize: 24, color: Colors.black),
                    textAlign: TextAlign.center,
                  ),
                ],
              ),
            ),
            actions: <Widget>[
              TextButton(
                child: Text(
                  getArabicString('Scan Again', false),
                  style: TextStyle(color: kPrimaryColor),
                ),
                onPressed: () {
                  _navigationService.navPop();
                  _navigationService.navPop();
                },
              ),
            ],
          ),
        );
      },
    );
  }
}
