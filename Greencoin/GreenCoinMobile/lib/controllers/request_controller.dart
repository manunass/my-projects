import 'dart:convert';

import 'package:fyp/locator.dart';
import 'package:fyp/models/Request.dart';
import 'package:http/http.dart' as http;
import 'package:shared_preferences/shared_preferences.dart';

class RequestController {
  final String apiUrl = "https://greencoin.azurewebsites.net/api/Requests";

  final SharedPreferences prefs = locator<SharedPreferences>();

  Future<Request> createRequest(String id, String userType) async {
    String param = userType == 'user' ? 'UserId' : 'BusinessId';
    Map<String, String> toBeSent = {param: id};
    final response = await http.post(apiUrl,
        headers: <String, String>{
          'accept': 'text/plain',
          'Content-Type': 'application/json'
        },
        body: json.encode(toBeSent));

    if (response.statusCode == 200 || response.statusCode == 201) {
      return Request.fromJson(json.decode(response.body));
    } else {
      if (response.statusCode == 400)
        throw Exception(json.decode(response.body)['Message']);
      else
        throw Exception(response.body);
    }
  }

  Future<int> cancelRequest(String id) async {
    final response = await http.patch('$apiUrl/$id/cancel',
        headers: <String, String>{
          'accept': 'text/plain',
          'Content-Type': 'application/json'
        });

    if (response.statusCode == 200) {
      return response.statusCode;
    } else {
      throw Exception(response.body);
    }
  }

  Future<List<Request>> getRequests(String id, String userType) async {
    String param = userType == 'business' ? 'businessId' : 'userId';
    final response = await http.get('$apiUrl?$param=$id');

    if (response.statusCode == 200) {
      List<Request> requests = [];
      List<dynamic> listRequests = json.decode(response.body)["Requests"];

      for (Map<String, dynamic> request in listRequests) {
        requests.add(Request.fromJson(request));
      }

      return requests;
    } else {
      throw Exception('Failed to load Requests');
    }
  }
}
