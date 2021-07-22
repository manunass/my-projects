import 'package:get_it/get_it.dart';
import 'package:shared_preferences/shared_preferences.dart';

import 'services/navigation_service.dart';

GetIt locator = GetIt.instance;

Future<void> setupLocator() async {

  SharedPreferences prefs = await SharedPreferences.getInstance();

  locator.registerLazySingleton(() => NavigationService());
  locator.registerSingleton(prefs);
}
