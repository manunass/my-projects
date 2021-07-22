import 'dart:convert';
import 'dart:io';

import 'package:http/http.dart' as http;
import 'package:flutter_form_bloc/flutter_form_bloc.dart';
import 'package:fyp/controllers/business_controller.dart';
import 'package:fyp/controllers/employee_controller.dart';
import 'package:fyp/controllers/municipality_controller.dart';
import 'package:fyp/controllers/user_controller.dart';
import 'package:fyp/models/Address.dart';
import 'package:fyp/models/Business.dart';
import 'package:fyp/models/Employee.dart';
import 'package:fyp/models/Municipality.dart';
import 'package:fyp/models/User.dart';
import 'package:geocoding/geocoding.dart';
import 'package:geolocator/geolocator.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:multi_image_picker/multi_image_picker.dart';
import 'package:shared_preferences/shared_preferences.dart';

import 'package:fyp/controllers/municipality_controller.dart';

import '../../../../constants.dart';
import '../../../../locator.dart';

class UserUpdateInfoFormBloc extends FormBloc<String, String> {
  final SharedPreferences prefs = locator<SharedPreferences>();
  String userId;
  String code;
  Map<String, String> locationToArabic = {};
  Map<String, String> data = {};

  User oldUser;

  // final firstName = TextFieldBloc(
  //   name: 'first_name',
  //   validators: [_required],
  // );

  // final lastName = TextFieldBloc(
  //   name: 'last_name',
  //   validators: [_required],
  // );

  final email = TextFieldBloc(
    name: 'email',
    validators: [FieldBlocValidators.email],
  );

  final latitude = TextFieldBloc(
    name: 'latitude',
    validators: [],
  );

  final longitude = TextFieldBloc(
    name: 'longitude',
    validators: [],
  );

  final street = TextFieldBloc(
    name: 'street',
    validators: [_required],
  );

  final building = TextFieldBloc(
    name: 'building',
    validators: [_required],
  );

  final floorNumber = TextFieldBloc(
    name: 'floorNumber',
    validators: [_required],
  );

  static String _required(String title) {
    if (title == null)
      return getArabicString('     This field is required', false);
    if (title.length == 0) {
      return getArabicString('     This field is required', false);
    }
    return null;
  }

  UserUpdateInfoFormBloc() {
    oldUser = User.fromJson(json.decode(prefs.getString('user_object')));

    latitude.updateInitialValue(oldUser.address.latitude.toString());
    longitude.updateInitialValue(oldUser.address.longitude.toString());
    street.updateInitialValue(oldUser.address.areaOrStreet);
    floorNumber.updateInitialValue(oldUser.address.flat.toString());
    building.updateInitialValue(oldUser.address.building);
    if (oldUser.email != null) email.updateInitialValue(oldUser.email);

    addFieldBlocs(
      fieldBlocs: [
        email,
        street,
        building,
        floorNumber,
        latitude,
        longitude,
      ],
    );
  }

  @override
  void onSubmitting() async {
    emitSubmitting();

    Map<String, dynamic> updateRequest = {
      "Address": {
        "Building": building.value,
        "Flat": floorNumber.value,
        "Latitude": double.parse(latitude.value),
        "Longitude": double.parse(longitude.value),
        "AreaOrStreet": street.value,
      }
    };

    if (email.value != null) updateRequest["Email"] = email.value;

    UserController userController = new UserController();

    try {
      userController.updateUser(oldUser.id, updateRequest).then((value) {
        if (value == 200) {
          userController.getUserById(oldUser.id).then((value) {
            prefs.setString('user_object', jsonEncode(value));
            emitSuccess();
          });
        }
      });
    } catch (_) {
      emitFailure();
    }
  }
}
