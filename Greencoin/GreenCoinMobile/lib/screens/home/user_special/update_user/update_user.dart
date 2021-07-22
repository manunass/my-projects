import 'dart:async';

import 'package:flutter/gestures.dart';
import 'package:flutter/material.dart';
import 'package:flutter_form_bloc/flutter_form_bloc.dart';
import 'package:font_awesome_flutter/font_awesome_flutter.dart';
import 'package:fyp/constants.dart';
import 'package:fyp/screens/sign_in/login_fresh_loading.dart';
import 'package:fyp/services/navigation_service.dart';
import 'package:geolocator/geolocator.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:multi_image_picker/multi_image_picker.dart';
import 'package:shared_preferences/shared_preferences.dart';

import 'package:geocoding/geocoding.dart';

import '../../../../locator.dart';
import '../../../../routes_names.dart';
import 'update_user_form_bloc.dart';

class UserUpdateScreen extends StatefulWidget {
  @override
  _UserUpdateScreenState createState() => _UserUpdateScreenState();
}

class _UserUpdateScreenState extends State<UserUpdateScreen> {
  bool isNoVisiblePassword = false;

  bool isSubmitting = false;

  List<Marker> currentLocation;
  bool submitting = false;
  bool failure = false;
  bool isSettingLocation = false;
  bool locationSet = false;
  bool gettingLocation = false;
  double gridHeight;
  LatLng dataToMap = LatLng(33.8983, 35.4763);

  Completer<GoogleMapController> _controller = Completer();

  BitmapDescriptor customIcon1;

  final focus = FocusNode();

  final bool isLoginRequest = false;

  final NavigationService _navigationService = locator<NavigationService>();
  final SharedPreferences prefs = locator<SharedPreferences>();

  // LoginFreshWords loginFreshWords;

  @override
  void initState() {
    currentLocation = [];
    isSubmitting = false;
    isSettingLocation = false;
    locationSet = false;
    gettingLocation = false;
    gridHeight = 100;
    super.initState();
  }

  Position currentPosition;

  var geoLocator = Geolocator();

  getIcon(BuildContext context) {
    if (customIcon1 == null) {
      ImageConfiguration configuration = createLocalImageConfiguration(context);

      BitmapDescriptor.fromAssetImage(
              configuration, 'assets/images/pin_green.png')
          .then((icon) {
        setState(() {
          customIcon1 = icon;
        });
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    getIcon(context);
    return BlocProvider(
      create: (context) => UserUpdateInfoFormBloc(),
      child: Builder(builder: (context) {
        final formBloc = context.read<UserUpdateInfoFormBloc>();
        return Theme(
          data: ThemeData(
            // Define the default brightness and colors.
            primaryColor: kPrimaryColor,
          ),
          child: !isSettingLocation
              ? Scaffold(
                  backgroundColor: kPrimaryColor,
                  appBar: AppBar(
                      iconTheme: IconThemeData(color: Colors.white),
                      backgroundColor: Color.fromRGBO(45, 206, 137, 1.0),
                      centerTitle: true,
                      elevation: 0,
                      title: Text(
                        "Update Profile",
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                        style: TextStyle(
                            color: Colors.black54,
                            fontWeight: FontWeight.bold,
                            fontSize: 18),
                      )),
                  body:
                      FormBlocListener<UserUpdateInfoFormBloc, String, String>(
                    onSuccess: (context, state) {
                      hideDialog();
                      _navigationService.replaceAndClearNav(MainNavRoute);
                    },
                    onSubmitting: (context, state) {
                      showDialogWindow("Updating your profile", false, context);
                    },
                    onSubmissionFailed: (context, state) {
                      setState(() {
                        isSubmitting = false;
                        // failure = false;
                      });
                    },
                    onSubmissionCancelled: (context, state) {
                      setState(() {
                        // failure = true;
                        isSubmitting = false;
                      });
                    },
                    onFailure: (context, state) {
                      // LoadingDialog.hide();
                      setState(() {
                        isSubmitting = false;
                        // failure = false;
                      });
                      Scaffold.of(context).showSnackBar(
                          SnackBar(content: Text(state.failureResponse)));
                    },
                    child: SingleChildScrollView(
                      child: Column(
                        children: <Widget>[
                          Align(
                            alignment: Alignment.topCenter,
                            child: Container(
                              height: MediaQuery.of(context).size.height * 0.15,
                              width: MediaQuery.of(context).size.width,
                              color: Color.fromRGBO(45, 206, 137, 1.0),
                              child: Padding(
                                padding: const EdgeInsets.symmetric(
                                    horizontal: 50, vertical: 3),
                                child: Hero(
                                  tag: 'hero-login',
                                  child: Image.asset(
                                    "assets/images/logo2.png",
                                    fit: BoxFit.fitWidth,
                                  ),
                                ),
                              ),
                            ),
                          ),
                          Align(
                            alignment: Alignment.bottomCenter,
                            child: Container(
                                height:
                                    MediaQuery.of(context).size.height * 0.8,
                                width: MediaQuery.of(context).size.width,
                                decoration: new BoxDecoration(
                                    color: Color(0xFFF3F3F5),
                                    borderRadius: new BorderRadius.only(
                                      topLeft: const Radius.circular(50.0),
                                      topRight: const Radius.circular(50.0),
                                    )),
                                child: _formDetails(formBloc)),
                          ),
                        ],
                      ),
                    ),
                  ),
                )
              : _bigContainer(context, formBloc),
        );
      }),
    );
  }

  Widget _formDetails(UserUpdateInfoFormBloc formBloc) {
    return Padding(
      padding: const EdgeInsets.only(top: 20, left: 12, right: 12, bottom: 20),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
        children: <Widget>[
          TextFieldBlocBuilder(
              textFieldBloc: formBloc.email,
              // controller: this._textEditingControllerUser,
              keyboardType: TextInputType.emailAddress,
              style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
              autofocus: false,
              onSubmitted: (v) {
                FocusScope.of(context).requestFocus(focus);
              },
              decoration: fieldsDesign("Email (Optional)", Icon(Icons.email))),
          SizedBox(
            height: 15,
          ),
          Text(
            "Set your house location on the map, this location will be used by your minicipality for garbage pickups and supplying your house with QR Codes.",
            textAlign: TextAlign.center,
          ),
          SizedBox(
            height: 15,
          ),
          GestureDetector(
            onTap: () {
              setState(() {
                isSettingLocation = !isSettingLocation;
              });
            },
            child: SizedBox(
                height: MediaQuery.of(context).size.height * 0.07,
                width: MediaQuery.of(context).size.width * 0.5,
                child: Card(
                    elevation: 10,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(40),
                    ),
                    color: kPrimaryColor,
                    child: Padding(
                      padding: const EdgeInsets.symmetric(horizontal: 10),
                      child: Center(
                          child: Text(
                        "Update your location",
                        style: TextStyle(
                            color: Colors.white,
                            fontSize: 15,
                            fontWeight: FontWeight.bold),
                      )),
                    ))),
          ),
          SizedBox(
            height: 15,
          ),
          TextFieldBlocBuilder(
              textFieldBloc: formBloc.street,
              keyboardType: TextInputType.emailAddress,
              style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
              autofocus: false,
              onSubmitted: (v) {
                FocusScope.of(context).requestFocus(focus);
              },
              decoration: fieldsDesign("Street", Icon(Icons.add_road))),
          TextFieldBlocBuilder(
              textFieldBloc: formBloc.building,
              keyboardType: TextInputType.emailAddress,
              style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
              autofocus: false,
              // onSubmitted: (v) {
              //   FocusScope.of(context).requestFocus(focus);
              // },
              decoration: fieldsDesign("Building", Icon(Icons.home))),
          TextFieldBlocBuilder(
              textFieldBloc: formBloc.floorNumber,
              keyboardType: TextInputType.number,
              style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
              autofocus: false,
              onSubmitted: (v) {
                FocusScope.of(context).requestFocus(focus);
              },
              decoration: fieldsDesign("Floor Number", Icon(Icons.business))),
          GestureDetector(
            onTap: () {
              setState(() {
                isSubmitting = true;
              });
              formBloc.submit();
            },
            child: SizedBox(
                height: MediaQuery.of(context).size.height * 0.07,
                width: MediaQuery.of(context).size.width * 0.7,
                child: Card(
                    elevation: 10,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(40),
                    ),
                    color: Color.fromRGBO(45, 206, 137, 1.0),
                    child: Padding(
                      padding: const EdgeInsets.symmetric(horizontal: 10),
                      child: Center(
                        child: Text(
                          "Save",
                          style: TextStyle(
                              color: Colors.white,
                              fontSize: 15,
                              fontWeight: FontWeight.bold),
                        ),
                      ),
                    ))),
          ),
        ],
      ),
    );
  }

  InputDecoration fieldsDesign(String hint, Icon preIcon) {
    return InputDecoration(
      labelText: hint,
      prefixIcon: Padding(padding: const EdgeInsets.all(8.0), child: preIcon),
      enabledBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(25),
          borderSide: BorderSide(color: Color(0xFFAAB5C3))),
      filled: true,
      fillColor: Color(0xFFF3F3F5),
      focusColor: Color(0xFFF3F3F5),
      focusedBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(25),
          borderSide: BorderSide(color: Color(0xFFAAB5C3))),
      border: OutlineInputBorder(
          borderRadius: BorderRadius.circular(25),
          borderSide: BorderSide(color: Color.fromRGBO(45, 206, 137, 1.0))),
    );
  }

  void setIsRequest(bool isSubmitting) {
    setState(() {
      this.isSubmitting = isSubmitting;
    });
  }

  Widget _bigContainer(BuildContext context, UserUpdateInfoFormBloc formBloc) {
    return Scaffold(
      appBar: AppBar(
        leading: SizedBox(),
        centerTitle: true,
        title: Text("Choose your location"),
      ),
      body: Column(
        // mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: <Widget>[
          _buildGoogleMap(context, formBloc),
          Container(
            alignment: Alignment.bottomCenter,
            margin: EdgeInsets.all(8),
            height: MediaQuery.of(context).size.height * 0.15,
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceAround,
              children: [
                Container(
                  width: MediaQuery.of(context).size.width * 0.4,
                  height: double.maxFinite,
                  child: !gettingLocation
                      ? Text(
                          "Place a pin on your house location or drag it.",
                          style: TextStyle(fontSize: 17),
                          textAlign: TextAlign.center,
                        )
                      : Padding(
                          padding: const EdgeInsets.all(8.0),
                          child: LoadingLoginFresh(
                            textLoading: "Setting Location",
                            colorText: Colors.black,
                            backgroundColor: kPrimaryColor,
                            elevation: 0,
                          )),
                ),
                GestureDetector(
                    onTap: () {
                      if (currentLocation.isNotEmpty) {
                        // setState(() {
                        //   gettingLocation = true;
                        // });

                        GeocodingPlatform.instance
                            .placemarkFromCoordinates(
                                currentLocation[0].position.latitude,
                                currentLocation[0].position.longitude)
                            .then((fullLocation) {
                          formBloc.latitude.updateValue(
                              currentLocation[0].position.latitude.toString());
                          formBloc.longitude.updateValue(
                              currentLocation[0].position.longitude.toString());
                          formBloc.street.updateValue(fullLocation[0].street);
                          isSettingLocation = !isSettingLocation;
                          locationSet = true;
                          // gettingLocation = false;
                          setState(() {});
                        });
                      }
                    },
                    child: Container(
                        alignment: Alignment.topCenter,
                        height: MediaQuery.of(context).size.height * 0.07,
                        width: MediaQuery.of(context).size.width * 0.5,
                        // margin: EdgeInsets.only(bottom: 20),
                        child: Card(
                            elevation: 10,
                            shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(30),
                            ),
                            color: kPrimaryColor,
                            child: Padding(
                              padding:
                                  const EdgeInsets.symmetric(horizontal: 10),
                              child: Center(
                                  child: Text(
                                "Confirm Your Location",
                                style: TextStyle(
                                    color: Colors.white,
                                    fontSize: 15,
                                    fontWeight: FontWeight.bold),
                              )),
                            ))))
              ],
            ),
          )
        ],
      ),
    );
  }

  Widget _buildGoogleMap(
      BuildContext context, UserUpdateInfoFormBloc formBloc) {
    return Container(
      padding: EdgeInsets.only(bottom: 10),
      height: MediaQuery.of(context).size.height * 0.7,
      width: MediaQuery.of(context).size.width,
      child: GoogleMap(
        mapType: MapType.hybrid,
        initialCameraPosition: CameraPosition(
            target: currentLocation.isNotEmpty
                ? LatLng(currentLocation[0].position.latitude,
                    currentLocation[0].position.longitude)
                : formBloc.latitude.value != null &&
                        formBloc.latitude.value.isNotEmpty
                    ? LatLng(formBloc.latitude.valueToDouble,
                        formBloc.longitude.valueToDouble)
                    : dataToMap,
            zoom: 18),
        onMapCreated: (GoogleMapController controller) {
          currentLocation = [];
          LatLng latLng = LatLng(
              double.parse(formBloc.latitude.value.toString()),
              double.parse(formBloc.longitude.value.toString()));
          currentLocation.add(Marker(
            icon: customIcon1,
            markerId: MarkerId(latLng.toString()),
            position: latLng,
            draggable: true,
            onDragEnd: (value) {
              currentLocation[0] =
                  currentLocation[0].copyWith(positionParam: value);

              print(value);
            },
          ));
          setState(() {});
          _controller.complete(controller);
        },
        myLocationEnabled: true,
        onTap: (LatLng latLng) {
          setState(() {
            currentLocation = [];

            currentLocation.add(Marker(
              icon: customIcon1,
              markerId: MarkerId(latLng.toString()),
              position: latLng,
              draggable: true,
              onDragEnd: (value) {
                currentLocation[0] =
                    currentLocation[0].copyWith(positionParam: value);

                print(value);
              },
            ));
          });
        },
        markers: Set.from(currentLocation),
      ),
    );
  }

  Future<void> _gotoLocation(double lat, double long) async {
    final GoogleMapController controller = await _controller.future;
    controller.animateCamera(CameraUpdate.newCameraPosition(CameraPosition(
      target: LatLng(lat, long),
      zoom: 18,
      tilt: 50.0,
      bearing: 45.0,
    )));
  }
}
