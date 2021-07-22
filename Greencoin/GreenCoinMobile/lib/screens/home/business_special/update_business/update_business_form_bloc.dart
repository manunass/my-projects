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

class BusinessUpdateInfoFormBloc extends FormBloc<String, String> {
  final SharedPreferences prefs = locator<SharedPreferences>();
  String userId;
  File image;
  List<Asset> images = List<Asset>();
  String code;
  Map<String, String> locationToArabic = {};
  Map<String, String> data = {};
  Business oldBusiness;

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

  final businessName = TextFieldBloc(
    name: 'business_name',
    validators: [_required],
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

  // void getCurrentLocation() async {
  //   // TODO: dont call recursivly just implement on resume for example
  //   // Geolocator.checkPermission().then((value) {
  //   //   if (value == LocationPermission.deniedForever) {
  //   //     Geolocator.openAppSettings().then((value) {
  //   //       Geolocator.checkPermission().then((value) {
  //   //         if (value == LocationPermission.deniedForever) {
  //   //           getCurrentLocation();
  //   //         }
  //   //       });
  //   //     });
  //   //   }
  //   // });

  //   await Geolocator.requestPermission().then((value) async {
  //     if (value == LocationPermission.denied ||
  //         value == LocationPermission.deniedForever) {
  //       getCurrentLocation();
  //     } else {
  //       Position position = await Geolocator.getCurrentPosition(
  //           desiredAccuracy: LocationAccuracy.best);
  //       // currentPosition = position;

  //       LatLng latLngCurrent = LatLng(position.latitude, position.longitude);

  //       CameraPosition cameraPosition =
  //           new CameraPosition(target: latLngCurrent, zoom: 14);

  //       List<Placemark> fullLocation = await GeocodingPlatform.instance
  //           .placemarkFromCoordinates(
  //               latLngCurrent.latitude, latLngCurrent.longitude);

  //       latitude.updateInitialValue(position.latitude.toString());
  //       longitude.updateInitialValue(position.longitude.toString());
  //       street.updateInitialValue(fullLocation[0].street);
  //     }
  //   });
  // }

  BusinessUpdateInfoFormBloc() {
    oldBusiness =
        Business.fromJson(json.decode(prefs.getString('business_object')));

    ownerFirstName.updateInitialValue(oldBusiness.ownerFirstName);
    ownerLastName.updateInitialValue(oldBusiness.ownerLastName);
    businessName.updateInitialValue(oldBusiness.name);
    category.updateInitialValue(oldBusiness.category);
    description.updateInitialValue(oldBusiness.about);
    building.updateInitialValue(oldBusiness.address.building);
    floorNumber.updateInitialValue(oldBusiness.address.flat);
    latitude.updateInitialValue(oldBusiness.address.latitude.toString());
    longitude.updateInitialValue(oldBusiness.address.longitude.toString());
    street.updateInitialValue(oldBusiness.address.areaOrStreet);

    addFieldBlocs(fieldBlocs: [
      ownerFirstName,
      ownerLastName,
      category,
      businessName,
      description,
      street,
      building,
      floorNumber,
      latitude,
      longitude,
    ]);
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
      },
      "Category": category.value,
      "About": description.value,
      "Name": businessName.value,
      "OwnerFirstName": ownerFirstName.value,
      "OwnerLastName": ownerLastName.value,
    };

    BusinessController businessController = new BusinessController();

    try {
      businessController
          .updateBusiness(oldBusiness.id, updateRequest)
          .then((value) {
        if (value == 200) {
          businessController.getBusinessById(oldBusiness.id).then((value) {
            prefs.setString('business_object', jsonEncode(value));
            emitSuccess();
          });
        }
      });
    } catch (_) {
      emitFailure();
    }

    // final String url =
    //           "https://greencoin.azurewebsites.net/api/Images/${value.id}";

    //       var request = http.MultipartRequest('POST', Uri.parse(url));
    //       request.files.add(http.MultipartFile(
    //         'picture',
    //         File(images[0].identifier).readAsBytes().asStream(),
    //         File(images[0].identifier).lengthSync(),
    //       ));
    //       var res = await request.send();

    //       print(res);

    //       emitSuccess();
  }
}
