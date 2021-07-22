import 'dart:convert';

import 'package:fyp/locator.dart';
import 'package:fyp/models/Municipality.dart';
import 'package:fyp/models/User.dart';
import 'package:http/http.dart';
import 'package:shared_preferences/shared_preferences.dart';

class UserController {
  final String apiUrl = "https://greencoin.azurewebsites.net/api/Users";

  final SharedPreferences prefs = locator<SharedPreferences>();
  Future<User> getUserById(String id) async {
    final response = await get('$apiUrl/$id');

    if (response.statusCode == 200) {
      return User.fromJson(json.decode(response.body));
    } else {
      throw Exception('Failed to load a user');
    }
  }

  Future<User> createUser(User user) async {
    print(user.toJson());
    final Response response = await post(
      apiUrl,
      headers: <String, String>{
        'accept': 'text/plain',
        'Content-Type': 'application/json'
      },
      body: json.encode(user.toJson()),
    );
    if (response.statusCode == 200 || response.statusCode == 201) {
      return User.fromJson(json.decode(response.body));
    } else {
      print(response.statusCode);
      throw Exception(response.body);
    }
  }

  Future<int> updateUser(String id, Map<String, dynamic> request) async {
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

  Future<Map<String, dynamic>> getUserInfoByCodeId(String id) async {
    final response =
        await get('https://greencoin.azurewebsites.net/api/Codes/$id');

    if (response.statusCode == 200) {
      Map<String, dynamic> code = json.decode(response.body);

      if (code["Type"] == 'Waste') {
        Map<String, dynamic> returnResposne = {
          "UserId": '${code['UserId']}',
          "RecyclableId": '${code['RecyclableId']}'
        };
        return returnResposne;
      }
    } else {
      throw Exception('Failed to load a business');
    }
  }

  // Future<User> createUser(User user) async {
  //   Map data = {
  //     'username': user.username,
  //     'password': user.password,
  //     'first_name': user.firstName,
  //     'last_name': user.lastName,
  //     'phone_number': user.phoneNumber,
  //     'email': user.email,
  //     'municipality': user.municipality,
  //     'address': user.address,
  //   };

  //   final Response response = await post(
  //     apiUrl,
  //     headers: <String, String>{
  //       'Content-Type': 'application/json; charset=UTF-8',
  //     },
  //     body: jsonEncode(data),
  //   );
  //   if (response.statusCode == 200) {
  //     return User.fromJson(json.decode(response.body));
  //   } else {
  //     throw Exception('Failed to post user');
  //   }
  // }

  // Future<User> updateUser(String id, User user) async {
  //   Map data = {
  //     'username': user.username,
  //     'password': user.password,
  //     'first_name': user.firstName,
  //     'last_name': user.lastName,
  //     'phone_number': user.phoneNumber,
  //     'email': user.email,
  //     'municipality': user.municipality,
  //     'address': user.address,
  //   };

  //   final Response response = await put(
  //     '$apiUrl/$id',
  //     headers: <String, String>{
  //       'Content-Type': 'application/json; charset=UTF-8',
  //     },
  //     body: jsonEncode(data),
  //   );
  //   if (response.statusCode == 200) {
  //     return User.fromJson(json.decode(response.body));
  //   } else {
  //     throw Exception('Failed to update a user');
  //   }
  // }

  // Future<void> deleteUser(String id) async {
  //   Response res = await delete('$apiUrl/$id');

  //   if (res.statusCode == 200) {
  //     print("User deleted");
  //   } else {
  //     throw "Failed to delete a user.";
  //   }
  // }

  // Future<void> signUserIn(String username, String password) async {
  //   final Response response = await post(
  //     apiUrl,
  //     headers: <String, String>{
  //       'Content-Type': 'application/json; charset=UTF-8',
  //     },
  //     body: jsonEncode({'username': username, 'password': password}),
  //   );
  //   if (response.statusCode == 200) {
  //     return User.fromJson(json.decode(response.body));
  //   } else {
  //     throw Exception('Failed to post user');
  //   }
  // }
}
