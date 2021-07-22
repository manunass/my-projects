import 'package:firebase_auth/firebase_auth.dart';
import 'package:fyp/services/navigation_service.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../../locator.dart';
import '../../../routes_names.dart';
import '../app_theme.dart';
import 'package:flutter/material.dart';
import 'package:fyp/constants.dart';

class HomeDrawer extends StatefulWidget {
  const HomeDrawer(
      {Key key,
      this.screenIndex,
      this.iconAnimationController,
      this.callBackIndex})
      : super(key: key);

  final AnimationController iconAnimationController;
  final DrawerIndex screenIndex;
  final Function(DrawerIndex) callBackIndex;

  @override
  _HomeDrawerState createState() => _HomeDrawerState();
}

class _HomeDrawerState extends State<HomeDrawer> {
  List<DrawerList> drawerList;
  List<DrawerList> drawerList2;
  String userType;
  final SharedPreferences prefs = locator<SharedPreferences>();
  final NavigationService _navigationService = locator<NavigationService>();

  @override
  void initState() {
    userType = prefs.getString('user_type');
    setDrawerListArray();
    print(userType);
    super.initState();
  }

  void setDrawerListArray() {
    drawerList = <DrawerList>[
      DrawerList(
        index: userType == 'business'
            ? DrawerIndex.Business_Home
            : userType == 'employee'
                ? DrawerIndex.Employee_Home
                : DrawerIndex.User_Home,
        labelName: 'Home',
        icon: Icon(Icons.home),
      ),
      DrawerList(
          index: userType == 'business'
              ? DrawerIndex.Business_QRCode
              : userType == 'employee'
                  ? DrawerIndex.Employee_Scan_QRCode
                  : DrawerIndex.User_Scan_QRCode,
          labelName: userType == 'business'
              ? 'Show QR Code'
              : userType == 'user'
                  ? 'Scan QR Code'
                  : 'Scan a Bag',
          icon: Icon(Icons.qr_code)
          // imageName: 'assets/images/supportIcon.png',
          ),
      if (userType == 'user')
        DrawerList(
          index: DrawerIndex.Businesses,
          labelName: 'Local Businesses',
          icon: Icon(Icons.business),
        ),
      DrawerList(
        index: DrawerIndex.Transactions,
        labelName:
            userType == 'employee' ? 'Bag Scans History' : 'Transactions',
        icon: Icon(Icons.compare_arrows),
      ),
    ];

    drawerList2 = <DrawerList>[
      if (userType != 'employee')
        DrawerList(
          index: DrawerIndex.Update_Profile,
          labelName: 'Edit Profile',
          icon: Icon(Icons.badge),
        ),
      DrawerList(
          index: DrawerIndex.Language,
          labelName: 'Change Language',
          icon: Icon(Icons.language)
          // imageName: 'assets/images/supportIcon.png',
          ),
      DrawerList(
          index: DrawerIndex.Help, labelName: 'Help', icon: Icon(Icons.help)),
      DrawerList(
        index: DrawerIndex.Contact,
        labelName: 'Contact Us',
        icon: Icon(Icons.message),
      ),
    ];
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppTheme.notWhite.withOpacity(0.5),
      body: SingleChildScrollView(
        child: Column(
          // crossAxisAlignment: CrossAxisAlignment.stretch,
          // mainAxisAlignment: MainAxisAlignment.start,
          children: <Widget>[
            Container(
              width: double.infinity,
              padding: const EdgeInsets.only(top: 40.0),
              child: Container(
                padding: const EdgeInsets.all(16.0),
                child: Padding(
                  padding: const EdgeInsets.only(top: 8, left: 4),
                  child: Text(
                    'Menu',
                    style: TextStyle(
                      fontWeight: FontWeight.w600,
                      color: AppTheme.grey,
                      fontSize: 18,
                    ),
                  ),
                ),
              ),
            ),
            const SizedBox(
              height: 4,
            ),
            Divider(
              height: 1,
              color: AppTheme.grey.withOpacity(0.6),
            ),
            Container(
              height: MediaQuery.of(context).size.height / 2.7,
              width: MediaQuery.of(context).size.width,
              child: ListView.builder(
                physics: const BouncingScrollPhysics(),
                padding: const EdgeInsets.all(0.0),
                itemCount: drawerList.length,
                itemBuilder: (BuildContext context, int index) {
                  return inkwell(drawerList[index]);
                },
              ),
            ),
            Container(
              width: double.infinity,
              // padding: const EdgeInsets.only(top: 10.0),
              child: Container(
                padding: const EdgeInsets.all(16.0),
                child: Text(
                  'Setting',
                  style: TextStyle(
                    fontWeight: FontWeight.w600,
                    color: AppTheme.grey,
                    fontSize: 18,
                  ),
                ),
              ),
            ),
            const SizedBox(
              height: 4,
            ),
            Divider(
              height: 1,
              color: AppTheme.grey.withOpacity(0.6),
            ),
            Container(
              height: MediaQuery.of(context).size.height / 2.7,
              width: MediaQuery.of(context).size.width,
              child: ListView.builder(
                physics: const BouncingScrollPhysics(),
                padding: const EdgeInsets.all(0.0),
                itemCount: drawerList2.length,
                itemBuilder: (BuildContext context, int index) {
                  return inkwell(drawerList2[index]);
                },
              ),
            ),
            Divider(
              height: 1,
              color: AppTheme.grey.withOpacity(0.6),
            ),
            ListTile(
              title: Text(
                'Sign Out',
                style: TextStyle(
                  fontFamily: AppTheme.fontName,
                  fontWeight: FontWeight.w600,
                  fontSize: 16,
                  color: AppTheme.darkText,
                ),
                textAlign: TextAlign.left,
              ),
              trailing: Icon(
                Icons.power_settings_new,
                color: Colors.red,
              ),
              onTap: () {
                return showDialog<void>(
                  context: context,
                  barrierDismissible: false, // user must tap button!
                  builder: (BuildContext context) {
                    return AlertDialog(
                      title: Text(getArabicString('Alert', false)),
                      content: SingleChildScrollView(
                        child: ListBody(
                          children: <Widget>[
                            Text(getArabicString(
                                'Are you sure you want to log Out from your account?',
                                false)),
                          ],
                        ),
                      ),
                      actions: <Widget>[
                        TextButton(
                          child: Text(getArabicString('Log Out', false)),
                          onPressed: () {
                            _navigationService.navPop();
                            prefs.clear();
                            FirebaseAuth.instance.signOut().then((value) {
                              _navigationService
                                  .replaceAndClearNav(SplashScreenRoute);
                            });
                          },
                        ),
                        TextButton(
                            onPressed: () => _navigationService.navPop(),
                            child: Text(getArabicString("Cancel", false)))
                      ],
                    );
                  },
                );
              },
            )
          ],
        ),
      ),
    );
  }

  Widget inkwell(DrawerList listData) {
    return Material(
      color: Colors.transparent,
      child: InkWell(
        splashColor: Colors.grey.withOpacity(0.1),
        highlightColor: Colors.transparent,
        onTap: () {
          navigationtoScreen(listData.index);
        },
        child: Stack(
          children: <Widget>[
            Container(
              height: MediaQuery.of(context).size.height / 12,
              padding: const EdgeInsets.only(top: 8.0, bottom: 8.0),
              child: Row(
                children: <Widget>[
                  Container(
                    width: 6.0,
                    height: 46.0,
                    // decoration: BoxDecoration(
                    //   color: widget.screenIndex == listData.index
                    //       ? Colors.blue
                    //       : Colors.transparent,
                    //   borderRadius: new BorderRadius.only(
                    //     topLeft: Radius.circular(0),
                    //     topRight: Radius.circular(16),
                    //     bottomLeft: Radius.circular(0),
                    //     bottomRight: Radius.circular(16),
                    //   ),
                    // ),
                  ),
                  const Padding(
                    padding: EdgeInsets.all(4.0),
                  ),
                  listData.isAssetsImage
                      ? Container(
                          width: 24,
                          height: 24,
                          child: Image.asset(listData.imageName,
                              color: widget.screenIndex == listData.index
                                  ? kPrimaryColor
                                  : AppTheme.nearlyBlack),
                        )
                      : Icon(listData.icon.icon,
                          color: widget.screenIndex == listData.index
                              ? kPrimaryColor
                              : AppTheme.nearlyBlack),
                  const Padding(
                    padding: EdgeInsets.all(4.0),
                  ),
                  Text(
                    listData.labelName,
                    style: TextStyle(
                      fontWeight: FontWeight.w500,
                      fontSize: 16,
                      color: widget.screenIndex == listData.index
                          ? kPrimaryColor
                          : AppTheme.nearlyBlack,
                    ),
                    textAlign: TextAlign.left,
                  ),
                ],
              ),
            ),
            widget.screenIndex == listData.index
                ? AnimatedBuilder(
                    animation: widget.iconAnimationController,
                    builder: (BuildContext context, Widget child) {
                      return Transform(
                        transform: Matrix4.translationValues(
                            (MediaQuery.of(context).size.width * 0.75 - 64) *
                                (1.0 -
                                    widget.iconAnimationController.value -
                                    1.0),
                            0.0,
                            0.0),
                        child: Padding(
                          padding: EdgeInsets.only(top: 8, bottom: 8),
                          child: Container(
                            width:
                                MediaQuery.of(context).size.width * 0.75 - 64,
                            height: 46,
                            decoration: BoxDecoration(
                              color: Colors.black12.withOpacity(0.05),
                              borderRadius: new BorderRadius.only(
                                topLeft: Radius.circular(0),
                                topRight: Radius.circular(28),
                                bottomLeft: Radius.circular(0),
                                bottomRight: Radius.circular(28),
                              ),
                            ),
                          ),
                        ),
                      );
                    },
                  )
                : const SizedBox()
          ],
        ),
      ),
    );
  }

  Future<void> navigationtoScreen(DrawerIndex indexScreen) async {
    widget.callBackIndex(indexScreen);
  }
}

enum DrawerIndex {
  User_Home,
  User_Scan_QRCode,
  Businesses,
  Business_Home,
  Business_QRCode,
  Employee_Home,
  Employee_Scan_QRCode,
  Transactions,
  Contact,
  Update_Profile,
  Language,
  Help,
}

class DrawerList {
  DrawerList({
    this.isAssetsImage = false,
    this.labelName = '',
    this.icon,
    this.index,
    this.imageName = '',
  });

  String labelName;
  Icon icon;
  bool isAssetsImage;
  String imageName;
  DrawerIndex index;
}
