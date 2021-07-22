import 'dart:convert';

import 'package:fyp/models/Employee.dart';
import 'package:http/http.dart';

class EmployeeController {
  final String apiUrl = "https://greencoin.azurewebsites.net/employees";

  Future<Employee> getEmployeeById(String id) async {
    final response = await get('$apiUrl/$id');

    if (response.statusCode == 200) {
      print(response.body);
      return Employee.fromJson(json.decode(response.body));
    } else {
      throw Exception('Failed to load a user');
    }
  }
}
