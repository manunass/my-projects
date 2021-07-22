import 'dart:convert';

import 'package:fyp/locator.dart';
import 'package:fyp/models/Business.dart';
import 'package:http/http.dart';
import 'package:shared_preferences/shared_preferences.dart';

class BusinessController {
  final String apiUrl = "https://greencoin.azurewebsites.net/api/Businesses";
  final SharedPreferences prefs = locator<SharedPreferences>();

  Future<Business> getBusinessById(String id) async {
    final response = await get('$apiUrl/$id');

    if (response.statusCode == 200) {
      return Business.fromJson(json.decode(response.body));
    } else {
      throw Exception('Failed to load a business');
    }
  }

  Future<List<Business>> getBusinesses({String query, String filter}) async {
    String municipalityId = prefs.getString('municipality_id');

    String request = "$apiUrl?municipalityId=$municipalityId";

    if (query != null && query != "") {
      if (filter != null && filter == 'By Category')
        request += "&category=$query";
      else if (filter != null && filter == 'By Name') request += "&name=$query";
    }

    final response = await get(request);

    if (response.statusCode == 200) {
      List<Business> businesses = [];
      List<dynamic> listBusinesses = json.decode(response.body)["Businesses"];

      for (Map<String, dynamic> business in listBusinesses) {
        businesses.add(Business.fromJson(business));
      }

      return businesses;
    } else {
      throw Exception('Failed to load a business');
    }
  }

  Future<Business> createBusiness(Business business) async {
    print(business.toJson());
    final Response response = await post(
      apiUrl,
      headers: <String, String>{
        'accept': 'text/plain',
        'Content-Type': 'application/json'
      },
      body: json.encode(business.toJson()),
    );
    if (response.statusCode == 200 || response.statusCode == 201) {
      return Business.fromJson(json.decode(response.body));
    } else {
      print(response.statusCode);
      throw Exception(response.body);
    }
  }

  Future<Business> getBusinessByCodeId(String id) async {
    final response =
        await get('https://greencoin.azurewebsites.net/api/Codes/$id');

    if (response.statusCode == 200) {
      Map<String, dynamic> code = json.decode(response.body);

      if (code["Type"] == 'Business') {
        return getBusinessById(code['BusinessId']);
      }
    } else {
      throw Exception('Failed to load a business');
    }
  }

  Future<String> getCodeByBusinessId(String id) async {
    final response =
        await get('https://greencoin.azurewebsites.net/api/Codes/business/$id');

    if (response.statusCode == 200) {
      Map<String, dynamic> code = json.decode(response.body);

      if (code["Type"] == 'Business') {
        return '${code['Id']}';
      }
    } else {
      throw Exception('Failed to load a business');
    }
  }

  Future<int> updateBusiness(String id, Map<String, dynamic> request) async {
    final Response response = await put(
      '$apiUrl/$id',
      headers: <String, String>{
        'accept': 'text/plain',
        'Content-Type': 'application/json'
      },
      body: json.encode(request),
    );
    if (response.statusCode == 200 || response.statusCode == 201) {
      return 200;
    } else {
      print(response.statusCode);
      throw Exception(response.body);
    }
  }
}
