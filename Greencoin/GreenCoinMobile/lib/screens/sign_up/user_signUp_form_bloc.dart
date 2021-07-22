import 'dart:convert';
import 'dart:io';
import 'package:http_parser/http_parser.dart';

import 'package:dio/dio.dart';
import 'package:flutter/services.dart';
import 'package:flutter_absolute_path/flutter_absolute_path.dart';
// import 'package:http/http.dart' as http;
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

import '../../constants.dart';
import '../../locator.dart';

class UserSignUpInfoFormBloc extends FormBloc<String, String> {
  final SharedPreferences prefs = locator<SharedPreferences>();
  String userId;
  File image;
  List<Asset> images = List<Asset>();
  String code;
  Map<String, String> locationToArabic = {};
  Map<String, String> data = {};

  // final userType = SelectFieldBloc(
  //   validators: [_required],
  //   items: ['user', 'business'],
  // );

  final ownerFirstName = TextFieldBloc(
    name: 'owner_first_name',
    validators: [_required],
  );

  final ownerLastName = TextFieldBloc(
    name: 'owner_last_name',
    validators: [_required],
  );

  final category = TextFieldBloc(
    name: 'category',
    validators: [_required],
  );

  final description = TextFieldBloc(
    name: 'description',
    validators: [_required],
  );

  // final userName = TextFieldBloc(
  //   name: 'user_name',
  //   validators: [_required],
  // );

  final firstName = TextFieldBloc(
    name: 'first_name',
    validators: [_required],
  );

  final lastName = TextFieldBloc(
    name: 'last_name',
    validators: [_required],
  );

  final businessName = TextFieldBloc(
    name: 'business_name',
    validators: [_required],
  );

  final email = TextFieldBloc(
    name: 'email',
    validators: [FieldBlocValidators.email],
  );

  // final phoneNumber = TextFieldBloc(
  //   name: 'phone_number',
  //   validators: [_required],
  // );

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

  // final password = TextFieldBloc(
  //   name: 'password',
  //   validators: [_required],
  // );

  // final confirmPassword = TextFieldBloc(
  //   name: 'confirm_password',
  //   validators: [_required],
  // );

  // final employeeType = SelectFieldBloc(
  //   name: 'employee_type',
  //   items: ['operator', 'manager'],
  //   validators: [_required],
  // );

  final municipalities = SelectFieldBloc(
    name: 'municipalities',
    items: [''],
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

  // Validator<String> _confirm(TextFieldBloc passwordF) {
  //   return (String confirmPassword) {
  //     if (confirmPassword == passwordF.value) {
  //       return null;
  //     }
  //     return "     Passwords doesn't match";
  //   };
  // }

  void getCurrentLocation() async {
    // TODO: dont call recursivly just implement on resume for example
    // Geolocator.checkPermission().then((value) {
    //   if (value == LocationPermission.deniedForever) {
    //     Geolocator.openAppSettings().then((value) {
    //       Geolocator.checkPermission().then((value) {
    //         if (value == LocationPermission.deniedForever) {
    //           getCurrentLocation();
    //         }
    //       });
    //     });
    //   }
    // });

    await Geolocator.requestPermission().then((value) async {
      if (value == LocationPermission.denied ||
          value == LocationPermission.deniedForever) {
        getCurrentLocation();
      } else {
        Position position = await Geolocator.getCurrentPosition(
            desiredAccuracy: LocationAccuracy.best);
        // currentPosition = position;

        LatLng latLngCurrent = LatLng(position.latitude, position.longitude);

        CameraPosition cameraPosition =
            new CameraPosition(target: latLngCurrent, zoom: 14);

        List<Placemark> fullLocation = await GeocodingPlatform.instance
            .placemarkFromCoordinates(
                latLngCurrent.latitude, latLngCurrent.longitude);

        print(fullLocation[0].country);
        print(fullLocation[0].street);
        print(fullLocation[0].administrativeArea); //mohafaza
        print(fullLocation[0].subAdministrativeArea); //qaza
        print(fullLocation[0].name);
        print(fullLocation[0].isoCountryCode);
        print(fullLocation[0].subLocality);
        print(fullLocation[0].subThoroughfare);
        print(fullLocation[0].thoroughfare);
        print(fullLocation);

        latitude.updateInitialValue(position.latitude.toString());
        longitude.updateInitialValue(position.longitude.toString());
        street.updateInitialValue(fullLocation[0].street);
      }
    });
  }

  getMunicipalities() async {
    List<dynamic> allMunicipalities =
        await MunicipalityController().getAllMunicipalities();

    for (dynamic m in allMunicipalities) {
      data.addAll({'${m["MunicipalityName"]}': '${m["Id"]}'});
    }
    municipalities.updateItems([]);
    municipalities.updateItems(data.keys.toList());
  }

  UserSignUpInfoFormBloc() {
    print("hi");
    getMunicipalities();

    addFieldBlocs(
      step: 1,
      fieldBlocs: [
        municipalities,
        street,
        building,
        floorNumber,
        latitude,
        longitude,
      ],
    );

    // confirmPassword
    //   ..addValidators([_confirm(password)])
    //   ..subscribeToFieldBlocs([password]);

    if (prefs.getString("user_type") == 'business') {
      addFieldBlocs(
        step: 0,
        fieldBlocs: [
          ownerFirstName,
          ownerLastName,
          category,
          businessName,
          description
        ],
      );
    } else {
      addFieldBlocs(
        step: 0,
        fieldBlocs: [firstName, lastName, email],
      );
    }
  }

  @override
  void onSubmitting() async {
    if (state.currentStep != 1) {
      if (state.currentStep == 0) {
        getCurrentLocation();
      }
      emitSuccess();
    } else {
      emitSubmitting();

      Address address = new Address(
          building: building.value,
          flat: floorNumber.value,
          latitude: double.parse(latitude.value),
          longitude: double.parse(longitude.value),
          areaOrStreet: street.value);

      if (prefs.getString("user_type") == 'business') {
        Business business = new Business(
            address: address,
            category: category.value,
            about: description.value,
            // TODO: to be changed
            municipalityId: data[municipalities.value],
            name: businessName.value,
            ownerFirstName: ownerFirstName.value,
            ownerLastName: ownerLastName.value,
            phoneNumber: prefs.getString('phone_number'),
            firebaseUid: prefs.getString('firebase_user_id'));

        // TODO: check attributes and handle multipart request

        BusinessController businessController = new BusinessController();

        try {
          businessController.createBusiness(business).then((value) async {
            if (value.id == null)
              emitSubmissionCancelled();
            else {
              // TODO: why user id and municipality id
              prefs.setString('user_id', '${value.id}');
              prefs.setString('municipality_id', '${value.municipalityId}');
              prefs.setString('user_type', 'business');
              prefs.setString("business_object", jsonEncode(value));
              prefs.remove("user_type_temp");

              // final String url =
              //     "https://greencoin.azurewebsites.net/api/Images/${value.id}";

              // List<String> filePaths = [];

              // for (Asset image in images) {
              //   String filePath =
              //       await FlutterAbsolutePath.getAbsolutePath(image.identifier);
              //   filePaths.add(filePath);
              //   print(filePath);
              // }

              // var formData = FormData();
              // for (String filePath in filePaths) {
              //   formData.files.addAll([
              //     MapEntry(
              //       'files',
              //       MultipartFile.fromFileSync(filePath,
              //           filename: filePath.split('/').last),
              //     )
              //   ]);
              // }

              // Dio dio = new Dio();
              // var response = await dio.post(url,
              //     data: formData,
              //     options: Options(
              //       contentType: "multipart/form-data",
              //     ));

              // print(response.statusCode);
              // print(response.data);
              // print(response.statusMessage);

              emitSuccess();
            }
          });
        } catch (_) {
          emitFailure();
        }
      } else if (prefs.getString("user_type") == 'user') {
        User user = new User(
          email: email.value,
          firstName: firstName.value,
          lastName: lastName.value,
          municipalityId: data[municipalities.value],
          phoneNumber: prefs.getString('phone_number'),
          firebaseUid: prefs.getString('firebase_user_id'),
          address: address,
        );

        UserController userController = new UserController();

        try {
          userController.createUser(user).then((value) {
            if (value.id == null)
              emitSubmissionCancelled();
            else {
              prefs.setString('user_id', '${value.id}');
              prefs.setString('municipality_id', '${value.municipalityId}');
              prefs.setString('user_type', 'user');
              prefs.setString("user_object", jsonEncode(value));
              prefs.remove("user_type_temp");
              emitSuccess();
            }
          });
        } catch (_) {
          emitFailure();
        }
      }
    }
  }
}
