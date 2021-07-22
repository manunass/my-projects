import 'package:flutter/material.dart';
import 'package:fyp/models/Business.dart';
import 'package:fyp/screens/businesses/business_details.dart';
import 'package:fyp/screens/businesses/businesses_screen.dart';
import 'package:fyp/screens/home/employee_special/bag_scan_form.dart';
import 'package:fyp/screens/home/navigation_components/home_drawer.dart';
import 'package:fyp/screens/home/user_special/user_home_screen.dart';
import 'package:fyp/screens/sign_up/sign_up_screen.dart';
import 'package:fyp/screens/transactions/transactions.dart';
import 'package:fyp/services/otp.dart';
import 'package:fyp/screens/home/navigation_components/navigation_home_screen.dart';

import 'constants.dart';
import 'routes_names.dart';
import 'screens/home/business_special/update_business/tabbed_update_screen.dart';
import 'screens/sign_in/sign_in_screen.dart';
import 'screens/splash/splash_screen.dart';

Route<dynamic> generateRoute(RouteSettings settings) {
  switch (settings.name) {
    case SplashScreenRoute:
      return _getPageRoute(
        routeName: settings.name,
        viewToShow: SplashScreen(),
      );
    case MainNavRoute:
      return _getPageRoute(
        routeName: settings.name,
        viewToShow: NavigationHomeScreen(),
      );
    case SignUpScreenRoute:
      return _getPageRoute(
        routeName: settings.name,
        viewToShow: SignUpScreen(),
      );
    case SignInScreenRoute:
      return _getPageRoute(
        routeName: settings.name,
        viewToShow: SignInScreen(),
      );
    case HomeScreenRoute:
      return _getPageRoute(
        routeName: settings.name,
        viewToShow: HomeScreen((DrawerIndex index) {}),
      );
    case BusinessesScreenRoute:
      return _getPageRoute(
        routeName: settings.name,
        viewToShow: BusinessesScreen(),
      );
    case BusinessDetailsScreenRoute:
      Business business = settings.arguments as Business;
      return _getPageRoute(
        routeName: settings.name,
        viewToShow: BusinessDetailsScreen(business),
      );
    case TabbedUpdateScreenRoute:
      int index = settings.arguments as int;
      return _getPageRoute(
        routeName: settings.name,
        viewToShow: TabbedUpdateScreen(index),
      );
    case TransactionsScreenRoute:
      return _getPageRoute(
        routeName: settings.name,
        viewToShow: TransactionsScreen(),
      );
    case BagScanFormRoute:
      var userInfo = settings.arguments as Map<String, dynamic>;

      return _getPageRoute(
          routeName: settings.name, viewToShow: BagScanForm(userInfo));
    case OTPScreenRoute:
      var phone = settings.arguments as String;
      return _getPageRoute(
        routeName: settings.name,
        viewToShow: OTPScreen(phone),
      );
    // case GetCountriesRoute:
    //   var data = settings.arguments as Map<String, dynamic> ?? null;
    //   return _getPageRoute(
    //     routeName: settings.name,
    //     viewToShow:
    //       data == null ? GetCountries()
    //         : GetCountries(data: data),
    //   );
    default:
      return MaterialPageRoute(
          builder: (_) => Scaffold(
                body: Center(
                    child: Text('No route defined for ${settings.name}')),
              ));
  }
}

PageRoute _getPageRoute({String routeName, Widget viewToShow}) {
  return MaterialPageRoute(
      settings: RouteSettings(
        name: routeName,
      ),
      builder: (_) => viewToShow);
}
