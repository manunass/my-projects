import 'package:flutter/material.dart';
import 'package:fyp/locator.dart';
import 'package:fyp/router.dart';
import 'package:fyp/routes_names.dart';
import 'package:fyp/services/navigation_service.dart';
import 'package:firebase_core/firebase_core.dart';

import 'theme.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await Firebase.initializeApp();
  await setupLocator();
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'Green Coin',
      theme: theme(),
      navigatorKey: locator<NavigationService>().navigationKey,
      // home: UserInfoForm(),
      // We use routeName so that we dont need to remember the name
      initialRoute: SplashScreenRoute,
      onGenerateRoute: generateRoute,
    );
  }
}
