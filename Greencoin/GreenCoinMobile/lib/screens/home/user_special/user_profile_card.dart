import 'package:fyp/constants.dart';
import 'package:fyp/controllers/request_controller.dart';
import 'package:fyp/locator.dart';
import 'package:fyp/models/User.dart';
import 'package:fyp/services/navigation_service.dart';

import '../fintness_app_theme.dart';
import 'package:flutter/material.dart';
import 'dart:math' as math;

class UserProfileCard extends StatelessWidget {
  final AnimationController animationController;
  final Animation animation;
  final User user;
  final String municipality;

  const UserProfileCard(this.user, this.municipality,
      {Key key, this.animationController, this.animation})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    final NavigationService _navigationService = locator<NavigationService>();

    return AnimatedBuilder(
      animation: animationController,
      builder: (BuildContext context, Widget child) {
        return FadeTransition(
          opacity: animation,
          child: new Transform(
            transform: new Matrix4.translationValues(
                0.0, 30 * (1.0 - animation.value), 0.0),
            child: Padding(
              padding: const EdgeInsets.only(
                  left: 14, right: 14, top: 16, bottom: 18),
              child: Container(
                decoration: BoxDecoration(
                  color: Colors.white,
                  borderRadius: BorderRadius.only(
                      topLeft: Radius.circular(8.0),
                      bottomLeft: Radius.circular(8.0),
                      bottomRight: Radius.circular(8.0),
                      topRight: Radius.circular(68.0)),
                  boxShadow: <BoxShadow>[
                    BoxShadow(
                        color: FitnessAppTheme.grey.withOpacity(0.2),
                        offset: Offset(1.1, 1.1),
                        blurRadius: 10.0),
                  ],
                ),
                child: Column(
                  children: <Widget>[
                    Padding(
                      padding: const EdgeInsets.only(
                          top: 16, left: 16, right: 16, bottom: 16),
                      child: Row(
                        children: <Widget>[
                          Expanded(
                            child: Padding(
                              padding: const EdgeInsets.only(
                                  left: 8, right: 8, top: 4),
                              child: Column(
                                children: <Widget>[
                                  Row(
                                    children: <Widget>[
                                      Container(
                                        height: 48,
                                        width: 2,
                                        decoration: BoxDecoration(
                                          color: kPrimaryColor.withOpacity(0.5),
                                          borderRadius: BorderRadius.all(
                                              Radius.circular(4.0)),
                                        ),
                                      ),
                                      Padding(
                                        padding: const EdgeInsets.all(8.0),
                                        child: Column(
                                          mainAxisAlignment:
                                              MainAxisAlignment.center,
                                          crossAxisAlignment:
                                              CrossAxisAlignment.start,
                                          children: <Widget>[
                                            Padding(
                                              padding: const EdgeInsets.only(
                                                  left: 4, bottom: 2),
                                              child: Text(
                                                'Total Score',
                                                textAlign: TextAlign.center,
                                                style: TextStyle(
                                                  fontFamily:
                                                      FitnessAppTheme.fontName,
                                                  fontWeight: FontWeight.w500,
                                                  fontSize: 16,
                                                  letterSpacing: -0.1,
                                                  color: FitnessAppTheme.grey
                                                      .withOpacity(0.5),
                                                ),
                                              ),
                                            ),
                                            Row(
                                              mainAxisAlignment:
                                                  MainAxisAlignment.center,
                                              crossAxisAlignment:
                                                  CrossAxisAlignment.end,
                                              children: <Widget>[
                                                SizedBox(
                                                  width: 28,
                                                  height: 28,
                                                  child: Icon(
                                                    Icons.emoji_events_outlined,
                                                    color: Colors.grey,
                                                  ),
                                                ),
                                                Padding(
                                                  padding:
                                                      const EdgeInsets.only(
                                                          left: 4, bottom: 3),
                                                  child: Text(
                                                    '${(user.wallet.score * animation.value).toInt()}',
                                                    textAlign: TextAlign.center,
                                                    style: TextStyle(
                                                      fontFamily:
                                                          FitnessAppTheme
                                                              .fontName,
                                                      fontWeight:
                                                          FontWeight.w600,
                                                      fontSize: 16,
                                                      color: FitnessAppTheme
                                                          .darkerText,
                                                    ),
                                                  ),
                                                ),
                                                Padding(
                                                  padding:
                                                      const EdgeInsets.only(
                                                          left: 4, bottom: 3),
                                                  child: Text(
                                                    'points',
                                                    textAlign: TextAlign.center,
                                                    style: TextStyle(
                                                      fontFamily:
                                                          FitnessAppTheme
                                                              .fontName,
                                                      fontWeight:
                                                          FontWeight.w600,
                                                      fontSize: 12,
                                                      letterSpacing: -0.2,
                                                      color: FitnessAppTheme
                                                          .grey
                                                          .withOpacity(0.5),
                                                    ),
                                                  ),
                                                ),
                                              ],
                                            )
                                          ],
                                        ),
                                      )
                                    ],
                                  ),
                                  SizedBox(
                                    height: 8,
                                  ),
                                  Row(
                                    children: <Widget>[
                                      Container(
                                        height: 48,
                                        width: 2,
                                        decoration: BoxDecoration(
                                          color: kPrimaryColor.withOpacity(0.5),
                                          borderRadius: BorderRadius.all(
                                              Radius.circular(4.0)),
                                        ),
                                      ),
                                      Padding(
                                        padding: const EdgeInsets.all(8.0),
                                        child: Column(
                                          mainAxisAlignment:
                                              MainAxisAlignment.center,
                                          crossAxisAlignment:
                                              CrossAxisAlignment.start,
                                          children: <Widget>[
                                            Padding(
                                              padding: const EdgeInsets.only(
                                                  left: 4, bottom: 2),
                                              child: Text(
                                                '${user.firstName} ${user.lastName}',
                                                textAlign: TextAlign.center,
                                                style: TextStyle(
                                                  fontFamily:
                                                      FitnessAppTheme.fontName,
                                                  fontWeight: FontWeight.w500,
                                                  fontSize: 16,
                                                  letterSpacing: -0.1,
                                                  color: FitnessAppTheme.grey
                                                      .withOpacity(0.5),
                                                ),
                                              ),
                                            ),
                                            Row(
                                              mainAxisAlignment:
                                                  MainAxisAlignment.center,
                                              crossAxisAlignment:
                                                  CrossAxisAlignment.end,
                                              children: <Widget>[
                                                SizedBox(
                                                    width: 28,
                                                    height: 28,
                                                    child: Icon(
                                                      Icons
                                                          .location_on_outlined,
                                                      color: Colors.grey,
                                                    )),
                                                Padding(
                                                  padding:
                                                      const EdgeInsets.only(
                                                          left: 4, bottom: 3),
                                                  child: Text(
                                                    municipality,
                                                    textAlign: TextAlign.center,
                                                    style: TextStyle(
                                                      fontFamily:
                                                          FitnessAppTheme
                                                              .fontName,
                                                      fontWeight:
                                                          FontWeight.w600,
                                                      fontSize: 16,
                                                      color: FitnessAppTheme
                                                          .darkerText,
                                                    ),
                                                  ),
                                                ),
                                              ],
                                            )
                                          ],
                                        ),
                                      )
                                    ],
                                  ),
                                ],
                              ),
                            ),
                          ),
                          Padding(
                            padding: const EdgeInsets.only(right: 16),
                            child: Center(
                              child: Stack(
                                overflow: Overflow.visible,
                                children: <Widget>[
                                  Padding(
                                    padding: const EdgeInsets.all(8.0),
                                    child: Container(
                                      width: 110,
                                      height: 110,
                                      decoration: BoxDecoration(
                                        color: FitnessAppTheme.white,
                                        borderRadius: BorderRadius.all(
                                          Radius.circular(100.0),
                                        ),
                                        border: new Border.all(
                                            width: 4,
                                            color:
                                                kPrimaryColor.withOpacity(0.2)),
                                      ),
                                      child: Column(
                                        mainAxisAlignment:
                                            MainAxisAlignment.center,
                                        crossAxisAlignment:
                                            CrossAxisAlignment.center,
                                        children: <Widget>[
                                          Text(
                                            '${(user.wallet.balance * animation.value).toInt()}',
                                            textAlign: TextAlign.center,
                                            style: TextStyle(
                                              fontFamily:
                                                  FitnessAppTheme.fontName,
                                              fontWeight: FontWeight.bold,
                                              fontSize: 24,
                                              letterSpacing: 0.0,
                                              color: kPrimaryColor,
                                            ),
                                          ),
                                          Text(
                                            'Coins in\n Wallet',
                                            textAlign: TextAlign.center,
                                            style: TextStyle(
                                              fontFamily:
                                                  FitnessAppTheme.fontName,
                                              fontWeight: FontWeight.bold,
                                              fontSize: 12,
                                              letterSpacing: 0.0,
                                              color: FitnessAppTheme.grey
                                                  .withOpacity(0.5),
                                            ),
                                          ),
                                        ],
                                      ),
                                    ),
                                  ),
                                  // Padding(
                                  //   padding: const EdgeInsets.all(4.0),
                                  //   child: CustomPaint(
                                  //     painter: CurvePainter(
                                  //         colors: [
                                  //           FitnessAppTheme.nearlyDarkBlue,
                                  //           HexColor("#8A98E8"),
                                  //           HexColor("#8A98E8")
                                  //         ],
                                  //         angle: 140 +
                                  //             (360 - 140) *
                                  //                 (1.0 - animation.value)),
                                  //     child: SizedBox(
                                  //       width: 108,
                                  //       height: 108,
                                  //     ),
                                  //   ),
                                  // )
                                ],
                              ),
                            ),
                          )
                        ],
                      ),
                    ),
                    Align(
                      alignment: Alignment.bottomRight,
                      child: Container(
                        color: kPrimaryColor.withOpacity(0.8),
                        child: InkWell(
                          onTap: () {
                            BuildContext context1 = context;
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
                                                'Are you sure you want to Checkout your coins to money?',
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
                                            getArabicString('I agree', false),
                                            style:
                                                TextStyle(color: kPrimaryColor),
                                          ),
                                          onPressed: () {
                                            showDialogWindow(
                                                'Requesting Checkout',
                                                false,
                                                context);
                                            RequestController()
                                                .createRequest(user.id, 'user')
                                                .then((value) {
                                              hideDialog();
                                              _navigationService.navPop();
                                              showDialog<void>(
                                                context: context1,
                                                barrierDismissible:
                                                    false, // user must tap button!
                                                builder:
                                                    (BuildContext context) {
                                                  return WillPopScope(
                                                    onWillPop: () async =>
                                                        false,
                                                    child: AlertDialog(
                                                      title: Text(''),
                                                      content:
                                                          SingleChildScrollView(
                                                        child: ListBody(
                                                          children: <Widget>[
                                                            Icon(
                                                              Icons
                                                                  .assignment_turned_in,
                                                              size: 50,
                                                              color:
                                                                  kPrimaryColor,
                                                            ),
                                                            SizedBox(
                                                                height: 10),
                                                            Text(
                                                              getArabicString(
                                                                  'Your request is submitted you can check it in transactions section',
                                                                  false),
                                                              style: TextStyle(
                                                                  fontSize: 16,
                                                                  color: Colors
                                                                      .black),
                                                              textAlign:
                                                                  TextAlign
                                                                      .center,
                                                            ),
                                                          ],
                                                        ),
                                                      ),
                                                      actions: <Widget>[
                                                        TextButton(
                                                          child: Text(
                                                            getArabicString(
                                                                'Okay', false),
                                                            style: TextStyle(
                                                                color:
                                                                    kPrimaryColor),
                                                          ),
                                                          onPressed: () {
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
                                                builder:
                                                    (BuildContext context) {
                                                  return WillPopScope(
                                                    onWillPop: () async =>
                                                        false,
                                                    child: AlertDialog(
                                                      title: Text(''),
                                                      content:
                                                          SingleChildScrollView(
                                                        child: ListBody(
                                                          children: <Widget>[
                                                            Icon(
                                                              Icons
                                                                  .sentiment_dissatisfied_outlined,
                                                              size: 50,
                                                              color:
                                                                  kPrimaryColor,
                                                            ),
                                                            SizedBox(
                                                                height: 10),
                                                            Text(
                                                              getArabicString(
                                                                  error
                                                                      .toString(),
                                                                  false),
                                                              style: TextStyle(
                                                                  fontSize: 16,
                                                                  color: Colors
                                                                      .black),
                                                              textAlign:
                                                                  TextAlign
                                                                      .center,
                                                            ),
                                                          ],
                                                        ),
                                                      ),
                                                      actions: <Widget>[
                                                        TextButton(
                                                          child: Text(
                                                            getArabicString(
                                                                'Okay', false),
                                                            style: TextStyle(
                                                                color:
                                                                    kPrimaryColor),
                                                          ),
                                                          onPressed: () {
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
                                          getArabicString('Cancel', false),
                                          style:
                                              TextStyle(color: kPrimaryColor),
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
                          child: Center(
                            child: Padding(
                              padding: const EdgeInsets.all(4),
                              child: Column(
                                mainAxisAlignment:
                                    MainAxisAlignment.spaceAround,
                                crossAxisAlignment: CrossAxisAlignment.center,
                                children: [
                                  Icon(
                                    Icons.attach_money_outlined,
                                    color: Colors.white,
                                    size: 25,
                                  ),
                                  Text(
                                    "Request Checkout",
                                    style: TextStyle(
                                        color: Colors.white,
                                        fontWeight: FontWeight.w900,
                                        fontSize: 14),
                                  )
                                ],
                              ),
                            ),
                          ),
                        ),
                      ),
                    ),
                    // Padding(
                    //   padding: const EdgeInsets.only(
                    //       left: 24, right: 24, top: 8, bottom: 8),
                    //   child: Container(
                    //     height: 2,
                    //     decoration: BoxDecoration(
                    //       color: FitnessAppTheme.background,
                    //       borderRadius: BorderRadius.all(Radius.circular(4.0)),
                    //     ),
                    //   ),
                    // ),
                    // Padding(
                    //   padding: const EdgeInsets.only(
                    //       left: 24, right: 24, top: 8, bottom: 16),
                    //   child: Row(
                    //     children: <Widget>[
                    //       Expanded(
                    //         child: Column(
                    //           mainAxisAlignment: MainAxisAlignment.center,
                    //           crossAxisAlignment: CrossAxisAlignment.start,
                    //           children: <Widget>[
                    //             Text(
                    //               'Carbs',
                    //               textAlign: TextAlign.center,
                    //               style: TextStyle(
                    //                 fontFamily: FitnessAppTheme.fontName,
                    //                 fontWeight: FontWeight.w500,
                    //                 fontSize: 16,
                    //                 letterSpacing: -0.2,
                    //                 color: FitnessAppTheme.darkText,
                    //               ),
                    //             ),
                    //             Padding(
                    //               padding: const EdgeInsets.only(top: 4),
                    //               child: Container(
                    //                 height: 4,
                    //                 width: 70,
                    //                 decoration: BoxDecoration(
                    //                   color:
                    //                       HexColor('#87A0E5').withOpacity(0.2),
                    //                   borderRadius: BorderRadius.all(
                    //                       Radius.circular(4.0)),
                    //                 ),
                    //                 child: Row(
                    //                   children: <Widget>[
                    //                     Container(
                    //                       width: ((70 / 1.2) * animation.value),
                    //                       height: 4,
                    //                       decoration: BoxDecoration(
                    //                         gradient: LinearGradient(colors: [
                    //                           HexColor('#87A0E5'),
                    //                           HexColor('#87A0E5')
                    //                               .withOpacity(0.5),
                    //                         ]),
                    //                         borderRadius: BorderRadius.all(
                    //                             Radius.circular(4.0)),
                    //                       ),
                    //                     )
                    //                   ],
                    //                 ),
                    //               ),
                    //             ),
                    //             Padding(
                    //               padding: const EdgeInsets.only(top: 6),
                    //               child: Text(
                    //                 '12g left',
                    //                 textAlign: TextAlign.center,
                    //                 style: TextStyle(
                    //                   fontFamily: FitnessAppTheme.fontName,
                    //                   fontWeight: FontWeight.w600,
                    //                   fontSize: 12,
                    //                   color:
                    //                       FitnessAppTheme.grey.withOpacity(0.5),
                    //                 ),
                    //               ),
                    //             ),
                    //           ],
                    //         ),
                    //       ),
                    //       Expanded(
                    //         child: Row(
                    //           mainAxisAlignment: MainAxisAlignment.center,
                    //           crossAxisAlignment: CrossAxisAlignment.center,
                    //           children: <Widget>[
                    //             Column(
                    //               mainAxisAlignment: MainAxisAlignment.center,
                    //               crossAxisAlignment: CrossAxisAlignment.start,
                    //               children: <Widget>[
                    //                 Text(
                    //                   'Protein',
                    //                   textAlign: TextAlign.center,
                    //                   style: TextStyle(
                    //                     fontFamily: FitnessAppTheme.fontName,
                    //                     fontWeight: FontWeight.w500,
                    //                     fontSize: 16,
                    //                     letterSpacing: -0.2,
                    //                     color: FitnessAppTheme.darkText,
                    //                   ),
                    //                 ),
                    //                 Padding(
                    //                   padding: const EdgeInsets.only(top: 4),
                    //                   child: Container(
                    //                     height: 4,
                    //                     width: 70,
                    //                     decoration: BoxDecoration(
                    //                       color: HexColor('#F56E98')
                    //                           .withOpacity(0.2),
                    //                       borderRadius: BorderRadius.all(
                    //                           Radius.circular(4.0)),
                    //                     ),
                    //                     child: Row(
                    //                       children: <Widget>[
                    //                         Container(
                    //                           width: ((70 / 2) *
                    //                               animationController.value),
                    //                           height: 4,
                    //                           decoration: BoxDecoration(
                    //                             gradient:
                    //                                 LinearGradient(colors: [
                    //                               HexColor('#F56E98')
                    //                                   .withOpacity(0.1),
                    //                               HexColor('#F56E98'),
                    //                             ]),
                    //                             borderRadius: BorderRadius.all(
                    //                                 Radius.circular(4.0)),
                    //                           ),
                    //                         ),
                    //                       ],
                    //                     ),
                    //                   ),
                    //                 ),
                    //                 Padding(
                    //                   padding: const EdgeInsets.only(top: 6),
                    //                   child: Text(
                    //                     '30g left',
                    //                     textAlign: TextAlign.center,
                    //                     style: TextStyle(
                    //                       fontFamily: FitnessAppTheme.fontName,
                    //                       fontWeight: FontWeight.w600,
                    //                       fontSize: 12,
                    //                       color: FitnessAppTheme.grey
                    //                           .withOpacity(0.5),
                    //                     ),
                    //                   ),
                    //                 ),
                    //               ],
                    //             ),
                    //           ],
                    //         ),
                    //       ),
                    //       Expanded(
                    //         child: Row(
                    //           mainAxisAlignment: MainAxisAlignment.end,
                    //           crossAxisAlignment: CrossAxisAlignment.center,
                    //           children: <Widget>[
                    //             Column(
                    //               mainAxisAlignment: MainAxisAlignment.center,
                    //               crossAxisAlignment: CrossAxisAlignment.start,
                    //               children: <Widget>[
                    //                 Text(
                    //                   'Fat',
                    //                   style: TextStyle(
                    //                     fontFamily: FitnessAppTheme.fontName,
                    //                     fontWeight: FontWeight.w500,
                    //                     fontSize: 16,
                    //                     letterSpacing: -0.2,
                    //                     color: FitnessAppTheme.darkText,
                    //                   ),
                    //                 ),
                    //                 Padding(
                    //                   padding: const EdgeInsets.only(
                    //                       right: 0, top: 4),
                    //                   child: Container(
                    //                     height: 4,
                    //                     width: 70,
                    //                     decoration: BoxDecoration(
                    //                       color: HexColor('#F1B440')
                    //                           .withOpacity(0.2),
                    //                       borderRadius: BorderRadius.all(
                    //                           Radius.circular(4.0)),
                    //                     ),
                    //                     child: Row(
                    //                       children: <Widget>[
                    //                         Container(
                    //                           width: ((70 / 2.5) *
                    //                               animationController.value),
                    //                           height: 4,
                    //                           decoration: BoxDecoration(
                    //                             gradient:
                    //                                 LinearGradient(colors: [
                    //                               HexColor('#F1B440')
                    //                                   .withOpacity(0.1),
                    //                               HexColor('#F1B440'),
                    //                             ]),
                    //                             borderRadius: BorderRadius.all(
                    //                                 Radius.circular(4.0)),
                    //                           ),
                    //                         ),
                    //                       ],
                    //                     ),
                    //                   ),
                    //                 ),
                    //                 Padding(
                    //                   padding: const EdgeInsets.only(top: 6),
                    //                   child: Text(
                    //                     '10g left',
                    //                     textAlign: TextAlign.center,
                    //                     style: TextStyle(
                    //                       fontFamily: FitnessAppTheme.fontName,
                    //                       fontWeight: FontWeight.w600,
                    //                       fontSize: 12,
                    //                       color: FitnessAppTheme.grey
                    //                           .withOpacity(0.5),
                    //                     ),
                    //                   ),
                    //                 ),
                    //               ],
                    //             ),
                    //           ],
                    //         ),
                    //       )
                    //     ],
                    //   ),
                    // )
                  ],
                ),
              ),
            ),
          ),
        );
      },
    );
  }
}

class CurvePainter extends CustomPainter {
  final double angle;
  final List<Color> colors;

  CurvePainter({this.colors, this.angle = 140});

  @override
  void paint(Canvas canvas, Size size) {
    List<Color> colorsList = List<Color>();
    if (colors != null) {
      colorsList = colors;
    } else {
      colorsList.addAll([Colors.white, Colors.white]);
    }

    final shdowPaint = new Paint()
      ..color = Colors.black.withOpacity(0.4)
      ..strokeCap = StrokeCap.round
      ..style = PaintingStyle.stroke
      ..strokeWidth = 14;
    final shdowPaintCenter = new Offset(size.width / 2, size.height / 2);
    final shdowPaintRadius =
        math.min(size.width / 2, size.height / 2) - (14 / 2);
    canvas.drawArc(
        new Rect.fromCircle(center: shdowPaintCenter, radius: shdowPaintRadius),
        degreeToRadians(278),
        degreeToRadians(360 - (365 - angle)),
        false,
        shdowPaint);

    shdowPaint.color = Colors.grey.withOpacity(0.3);
    shdowPaint.strokeWidth = 16;
    canvas.drawArc(
        new Rect.fromCircle(center: shdowPaintCenter, radius: shdowPaintRadius),
        degreeToRadians(278),
        degreeToRadians(360 - (365 - angle)),
        false,
        shdowPaint);

    shdowPaint.color = Colors.grey.withOpacity(0.2);
    shdowPaint.strokeWidth = 20;
    canvas.drawArc(
        new Rect.fromCircle(center: shdowPaintCenter, radius: shdowPaintRadius),
        degreeToRadians(278),
        degreeToRadians(360 - (365 - angle)),
        false,
        shdowPaint);

    shdowPaint.color = Colors.grey.withOpacity(0.1);
    shdowPaint.strokeWidth = 22;
    canvas.drawArc(
        new Rect.fromCircle(center: shdowPaintCenter, radius: shdowPaintRadius),
        degreeToRadians(278),
        degreeToRadians(360 - (365 - angle)),
        false,
        shdowPaint);

    final rect = new Rect.fromLTWH(0.0, 0.0, size.width, size.width);
    final gradient = new SweepGradient(
      startAngle: degreeToRadians(268),
      endAngle: degreeToRadians(270.0 + 360),
      tileMode: TileMode.repeated,
      colors: colorsList,
    );
    final paint = new Paint()
      ..shader = gradient.createShader(rect)
      ..strokeCap = StrokeCap.round // StrokeCap.round is not recommended.
      ..style = PaintingStyle.stroke
      ..strokeWidth = 14;
    final center = new Offset(size.width / 2, size.height / 2);
    final radius = math.min(size.width / 2, size.height / 2) - (14 / 2);

    canvas.drawArc(
        new Rect.fromCircle(center: center, radius: radius),
        degreeToRadians(278),
        degreeToRadians(360 - (365 - angle)),
        false,
        paint);

    final gradient1 = new SweepGradient(
      tileMode: TileMode.repeated,
      colors: [Colors.white, Colors.white],
    );

    var cPaint = new Paint();
    cPaint..shader = gradient1.createShader(rect);
    cPaint..color = Colors.white;
    cPaint..strokeWidth = 14 / 2;
    canvas.save();

    final centerToCircle = size.width / 2;
    canvas.save();

    canvas.translate(centerToCircle, centerToCircle);
    canvas.rotate(degreeToRadians(angle + 2));

    canvas.save();
    canvas.translate(0.0, -centerToCircle + 14 / 2);
    canvas.drawCircle(new Offset(0, 0), 14 / 5, cPaint);

    canvas.restore();
    canvas.restore();
    canvas.restore();
  }

  @override
  bool shouldRepaint(CustomPainter oldDelegate) {
    return true;
  }

  double degreeToRadians(double degree) {
    var redian = (math.pi / 180) * degree;
    return redian;
  }
}

class HexColor extends Color {
  HexColor(final String hexColor) : super(_getColorFromHex(hexColor));

  static int _getColorFromHex(String hexColor) {
    hexColor = hexColor.toUpperCase().replaceAll('#', '');
    if (hexColor.length == 6) {
      hexColor = 'FF' + hexColor;
    }
    return int.parse(hexColor, radix: 16);
  }
}
