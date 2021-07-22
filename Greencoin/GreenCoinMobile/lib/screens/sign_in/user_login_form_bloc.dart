import 'dart:io';

import 'package:flutter_form_bloc/flutter_form_bloc.dart';
import 'package:fyp/controllers/business_controller.dart';
import 'package:fyp/controllers/employee_controller.dart';
import 'package:fyp/controllers/user_controller.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../constants.dart';
import '../../locator.dart';
import '../../routes_names.dart';
import '../../services/navigation_service.dart';

class UserInfoFormBloc extends FormBloc<String, String> {
  final SharedPreferences prefs = locator<SharedPreferences>();
  String userId;
  File image;
  String code;
  String dialCode;

  Map<String, String> locationToArabic = {};

  final userType = SelectFieldBloc(
    validators: [_required],
    items: ['user', 'business', 'employee'],
  );

  final phoneNumber = TextFieldBloc(
    name: 'user_name',
    validators: [_required],
  );

  // final employeeType = SelectFieldBloc(
  //   name: 'employee_type',
  //   items: ['operator', 'manager'],
  //   validators: [_required],
  // );

  static String _required(String title) {
    if (title == null)
      return getArabicString('     This field is required', false);
    if (title.length == 0) {
      return getArabicString('     This field is required', false);
    }
    return null;
  }

  UserInfoFormBloc() {
    addFieldBlocs(
      fieldBlocs: [userType],
    );

    userType.onValueChanges(onData: (previous, current) async* {
      removeFieldBlocs(
        fieldBlocs: [phoneNumber],
      );

      if (current.value == null) {
      } else {
        addFieldBlocs(
          fieldBlocs: [phoneNumber],
        );
      }
    });
  }

  @override
  void onSubmitting() async {
    emitSubmitting();

    String phoneNumberText = '$dialCode${phoneNumber.value}';

    final NavigationService _navigationService = locator<NavigationService>();
    prefs.setString("user_type_temp", userType.value);

    _navigationService.navigateTo(OTPScreenRoute, arguments: phoneNumberText);

    emitSubmissionCancelled();
  }
}
