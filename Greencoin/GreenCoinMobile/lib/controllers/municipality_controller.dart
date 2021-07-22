import 'dart:convert';

import 'package:fyp/locator.dart';
import 'package:fyp/models/Municipality.dart';
import 'package:fyp/models/User.dart';
import 'package:http/http.dart';
import 'package:shared_preferences/shared_preferences.dart';

class MunicipalityController {
  final String apiUrl =
      "https://greencoin.azurewebsites.net/api/Municipalities";

  final SharedPreferences prefs = locator<SharedPreferences>();

  Future<dynamic> getAllMunicipalities() async {
    final response = await get('$apiUrl/info');

    if (response.statusCode == 200) {
      List<dynamic> municipalities =
          json.decode(response.body)["MunicipalitiesInfo"] as List<dynamic>;
      return municipalities;
    } else {
      throw Exception('Failed to get Municipalities');
    }
  }

  Future<Municipality> getMunicipalityById() async {
    String municipalityId = prefs.getString('municipality_id');

    final response = await get('$apiUrl/$municipalityId');

    if (response.statusCode == 200) {
      return Municipality.fromJson(json.decode(response.body));
    } else {
      throw Exception('Failed to get Municipalities');
    }
  }
}
