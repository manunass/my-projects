import 'dart:convert';

import 'package:fyp/locator.dart';
import 'package:fyp/models/BagScan.dart';
import 'package:fyp/models/Business.dart';
import 'package:http/http.dart';
import 'package:shared_preferences/shared_preferences.dart';

class BagScanController {
  final String apiUrl = "https://greencoin.azurewebsites.net/api/Operations";

  final SharedPreferences prefs = locator<SharedPreferences>();

  Future<BagScan> createBagScan(Map<String, dynamic> request) async {
    final String municipalityId = prefs.getString('municipality_id');
    var recyclableId = request["RecyclableId"];
    request.remove("RecyclableId");

    var toBeSent = request;
    double weight = double.parse('${request['Weight']}');
    toBeSent.remove('Weight');

    toBeSent['Weight'] = weight;

    final response = await post(
        '$apiUrl/$municipalityId/recyclables/$recyclableId/batches/bag-scans',
        headers: <String, String>{
          'accept': 'text/plain',
          'Content-Type': 'application/json'
        },
        body: json.encode(toBeSent));

    if (response.statusCode == 200 || response.statusCode == 201) {
      return BagScan.fromJson(json.decode(response.body));
    } else {
      throw Exception(response.body);
    }
  }

  Future<List<BagScan>> getBagScans(String id, String userType) async {
    String param = userType == 'employee' ? 'employeeId' : 'userId';
    final response =
        await get("$apiUrl/recyclables/batches/bag-scans?$param=$id");

    if (response.statusCode == 200) {
      List<BagScan> bagScans = [];
      List<dynamic> listbagScans = json.decode(response.body)["BagScans"];

      for (Map<String, dynamic> bagScan in listbagScans) {
        bagScans.add(BagScan.fromJson(bagScan));
      }

      return bagScans;
    } else {
      throw Exception('Failed to load Bag scans');
    }
  }
}
