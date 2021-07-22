import 'dart:async';

import 'package:flutter/gestures.dart';
import 'package:flutter/material.dart';
import 'package:flutter_form_bloc/flutter_form_bloc.dart';
import 'package:font_awesome_flutter/font_awesome_flutter.dart';
import 'package:fyp/constants.dart';
import 'package:fyp/services/navigation_service.dart';
import 'package:geolocator/geolocator.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:multi_image_picker/multi_image_picker.dart';
import 'package:shared_preferences/shared_preferences.dart';

import 'package:geocoding/geocoding.dart';

import '../../locator.dart';
import '../../routes_names.dart';
import 'user_signUp_form_bloc.dart';
import '../sign_in/login_fresh_loading.dart';

class SignUpScreen extends StatefulWidget {
  @override
  _SignUpScreenState createState() => _SignUpScreenState();
}

class _SignUpScreenState extends State<SignUpScreen> {
  bool isNoVisiblePassword = false;

  bool isSubmitting = false;

  List<Marker> currentLocation;
  List<Asset> images = List<Asset>();
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

  Widget buildGridView() {
    return IgnorePointer(
      child: GridView.count(
        crossAxisCount: 4,
        primary: true,
        children: List.generate(images.length, (index) {
          Asset asset = images[index];
          return AssetThumb(
            asset: asset,
            width: 100,
            height: 100,
          );
        }),
      ),
    );
  }

  Future<void> loadAssets() async {
    List<Asset> resultList = List<Asset>();
    String error = 'No Error Dectected';

    try {
      // TODO: details true or false
      resultList = await MultiImagePicker.pickImages(
        maxImages: 4,
        enableCamera: true,
        selectedAssets: images,
        cupertinoOptions: CupertinoOptions(takePhotoIcon: "chat"),
        materialOptions: MaterialOptions(
          actionBarColor: "#FF2DCE96",
          statusBarColor: "#FF3BA384",
          actionBarTitle: getArabicString(" Add photos", false),
          allViewTitle: "All Photos",
          useDetailsView: true,
          selectCircleStrokeColor: "#000000",
        ),
      );
    } on Exception catch (e) {
      error = e.toString();
    }

    setState(() {
      images = resultList;
      // _error = error;
      // gridHeight = MediaQuery.of(context).size.height/4;
    });
  }

  @override
  Widget build(BuildContext context) {
    getIcon(context);
    return BlocProvider(
      create: (context) => UserSignUpInfoFormBloc(),
      child: Builder(builder: (context) {
        final formBloc = context.read<UserSignUpInfoFormBloc>();
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
                        "Sign up",
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                        style: TextStyle(
                            color: Colors.black54,
                            fontWeight: FontWeight.bold,
                            fontSize: 18),
                      )),
                  body:
                      FormBlocListener<UserSignUpInfoFormBloc, String, String>(
                    onSuccess: (context, state) {
                      if (state.stepCompleted == state.lastStep) {
                        hideDialog();
                        _navigationService.replaceAndClearNav(MainNavRoute);
                        // _navigationService.replaceAndClearNav(HomeScreenRoute);
                      } else {
                        FocusScope.of(context).requestFocus(FocusNode());
                        setState(() {
                          isSettingLocation = false;
                          locationSet = false;
                        });
                      }
                      // showSuccessDialog();
                    },
                    onSubmissionFailed: (context, state) {
                      hideDialog();
                      setState(() {
                        isSubmitting = false;
                        // failure = false;
                      });
                    },
                    onSubmissionCancelled: (context, state) {
                      hideDialog();
                      setState(() {
                        // failure = true;
                        isSubmitting = false;
                      });
                    },
                    onSubmitting: (context, state) {
                      if (state.isLastStep) {
                        showDialogWindow("Signing you up", false, context);
                      }
                    },
                    onFailure: (context, state) {
                      // LoadingDialog.hide();
                      hideDialog();
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
                              height: MediaQuery.of(context).size.height * 0.8,
                              width: MediaQuery.of(context).size.width,
                              decoration: new BoxDecoration(
                                  color: Color(0xFFF3F3F5),
                                  borderRadius: new BorderRadius.only(
                                    topLeft: const Radius.circular(50.0),
                                    topRight: const Radius.circular(50.0),
                                  )),
                              child: StepperFormBlocBuilder<
                                  UserSignUpInfoFormBloc>(
                                type: StepperType.vertical,
                                physics: ClampingScrollPhysics(),
                                stepsBuilder: (formBloc) {
                                  return [
                                    _personalStep(formBloc),
                                    // _credentialsStep(formBloc),
                                    _locationStep(formBloc),
                                  ];
                                },
                              ),
                            ),
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

// FormBlocStep _personalStep(UserSignUpInfoFormBloc wizardFormBloc) {
//     return FormBlocStep(
//       title: Text('Personal'),
//       content: Column(
//         children: <Widget>[
//           TextFieldBlocBuilder(
//             textFieldBloc: wizardFormBloc.firstName,
//             keyboardType: TextInputType.emailAddress,
//             decoration: InputDecoration(
//               labelText: 'First Name',
//               prefixIcon: Icon(Icons.person),
//             ),
//           ),
//           TextFieldBlocBuilder(
//             textFieldBloc: wizardFormBloc.lastName,
//             keyboardType: TextInputType.emailAddress,
//             decoration: InputDecoration(
//               labelText: 'Last Name',
//               prefixIcon: Icon(Icons.person),
//             ),
//           ),

//         ],
//       ),
//     );
//   }

  FormBlocStep _personalStep(UserSignUpInfoFormBloc formBloc) {
    return FormBlocStep(
      title: Text('Personal Info'),
      content: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        textBaseline: TextBaseline.alphabetic,
        children: <Widget>[
          TextFieldBlocBuilder(
              textFieldBloc: formBloc.firstName,
              keyboardType: TextInputType.emailAddress,
              style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
              autofocus: false,
              decoration:
                  fieldsDesign("First Name", Icon(Icons.content_paste))),
          TextFieldBlocBuilder(
              textFieldBloc: formBloc.lastName,
              keyboardType: TextInputType.emailAddress,
              style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
              autofocus: false,
              onSubmitted: (v) {
                FocusScope.of(context).requestFocus(focus);
              },
              decoration: fieldsDesign("Last Name", Icon(Icons.content_paste))),
          TextFieldBlocBuilder(
              textFieldBloc: formBloc.businessName,
              keyboardType: TextInputType.emailAddress,
              style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
              autofocus: false,
              decoration:
                  fieldsDesign("Business Name", Icon(Icons.content_paste))),
          TextFieldBlocBuilder(
              textFieldBloc: formBloc.ownerFirstName,
              keyboardType: TextInputType.emailAddress,
              style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
              autofocus: false,
              decoration:
                  fieldsDesign("Owner First Name", Icon(Icons.content_paste))),
          TextFieldBlocBuilder(
              textFieldBloc: formBloc.ownerLastName,
              keyboardType: TextInputType.emailAddress,
              style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
              autofocus: false,
              onSubmitted: (v) {
                FocusScope.of(context).requestFocus(focus);
              },
              decoration:
                  fieldsDesign("Owner Last Name", Icon(Icons.content_paste))),
          // TextFieldBlocBuilder(
          //     textFieldBloc: formBloc.phoneNumber,
          //     keyboardType: TextInputType.phone,
          //     style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
          //     autofocus: false,
          //     onSubmitted: (v) {
          //       FocusScope.of(context).requestFocus(focus);
          //     },
          //     decoration:
          //         fieldsDesign("Phone Number", Icon(Icons.phone_android))),
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
          TextFieldBlocBuilder(
              textFieldBloc: formBloc.category,
              // controller: this._textEditingControllerUser,
              keyboardType: TextInputType.emailAddress,
              style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
              autofocus: false,
              onSubmitted: (v) {
                FocusScope.of(context).requestFocus(focus);
              },
              decoration: fieldsDesign("Category", Icon(Icons.category))),
          TextFieldBlocBuilder(
              textFieldBloc: formBloc.description,
              // controller: this._textEditingControllerUser,
              keyboardType: TextInputType.multiline,
              minLines: 1,
              maxLines: 10,
              style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
              autofocus: false,
              onSubmitted: (v) {
                FocusScope.of(context).requestFocus(focus);
              },
              decoration: fieldsDesign("Description", Icon(Icons.description))),
        ],
      ),
    );
  }

  // FormBlocStep _credentialsStep(UserSignUpInfoFormBloc formBloc) {
  //   return FormBlocStep(
  //     title: Text('Credentials Info'),
  //     content: Column(
  //       crossAxisAlignment: CrossAxisAlignment.center,
  //       children: <Widget>[
  //         TextFieldBlocBuilder(
  //             textFieldBloc: formBloc.userName,
  //             // controller: this._textEditingControllerUser,
  //             keyboardType: TextInputType.emailAddress,
  //             style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
  //             autofocus: false,
  //             onSubmitted: (v) {
  //               FocusScope.of(context).requestFocus(focus);
  //             },
  //             decoration: fieldsDesign("Username", Icon(Icons.person))),
  //         TextFieldBlocBuilder(
  //             textFieldBloc: formBloc.password,
  //             suffixButton: SuffixButton.obscureText,
  //             style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
  //             onSubmitted: (value) {
  //               // TODO:  handle onsibmit
  //             },
  //             decoration: fieldsDesign("Password", Icon(Icons.lock))),
  //         TextFieldBlocBuilder(
  //             textFieldBloc: formBloc.confirmPassword,
  //             suffixButton: SuffixButton.obscureText,
  //             style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
  //             onSubmitted: (value) {
  //               // TODO:  handle onsibmit
  //             },
  //             decoration: fieldsDesign("Confirm Password", Icon(Icons.lock))),
  //         GestureDetector(
  //           child: Padding(
  //             padding: const EdgeInsets.symmetric(vertical: 15, horizontal: 10),
  //             child: RichText(
  //               textAlign: TextAlign.center,
  //               text: TextSpan(children: [
  //                 TextSpan(
  //                     text: "Already have an account?" + ' \n',
  //                     style: TextStyle(
  //                         color: Color(0xFF0F2E48),
  //                         fontWeight: FontWeight.normal,
  //                         fontSize: 15)),
  //                 TextSpan(
  //                     text: "Sign In",
  //                     style: TextStyle(
  //                         decoration: TextDecoration.underline,
  //                         color: Color(0xFF0F2E48),
  //                         fontWeight: FontWeight.bold,
  //                         fontSize: 16)),
  //               ]),
  //             ),
  //           ),
  //           onTap: () {
  //             _navigationService.navigateTo(SignInScreenRoute);
  //           },
  //         ),
  //       ],
  //     ),
  //   );
  // }

  FormBlocStep _locationStep(UserSignUpInfoFormBloc formBloc) {
    return FormBlocStep(
      title: Text('Location Info'),
      content: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        textBaseline: TextBaseline.alphabetic,
        children: <Widget>[
          DropdownFieldBlocBuilder(
              selectFieldBloc: formBloc.municipalities,
              itemBuilder: (context, String value) => value,
              decoration: fieldsDesign(
                  "Choose your municipality?", Icon(Icons.corporate_fare))),
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
                        !locationSet
                            ? "Set your location"
                            : "Update your location",
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
          locationSet
              ? TextFieldBlocBuilder(
                  textFieldBloc: formBloc.street,
                  keyboardType: TextInputType.emailAddress,
                  style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
                  autofocus: false,
                  onSubmitted: (v) {
                    FocusScope.of(context).requestFocus(focus);
                  },
                  decoration: fieldsDesign("Street", Icon(Icons.add_road)))
              : SizedBox(),
          locationSet
              ? TextFieldBlocBuilder(
                  textFieldBloc: formBloc.building,
                  keyboardType: TextInputType.emailAddress,
                  style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
                  autofocus: false,
                  // onSubmitted: (v) {
                  //   FocusScope.of(context).requestFocus(focus);
                  // },
                  decoration: fieldsDesign("Building", Icon(Icons.home)))
              : SizedBox(),
          locationSet
              ? TextFieldBlocBuilder(
                  textFieldBloc: formBloc.floorNumber,
                  keyboardType: TextInputType.number,
                  style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
                  autofocus: false,
                  onSubmitted: (v) {
                    FocusScope.of(context).requestFocus(focus);
                  },
                  decoration:
                      fieldsDesign("Floor Number", Icon(Icons.business)))
              : SizedBox(),
          // TextFieldBlocBuilder(
          //     textFieldBloc: formBloc.latitude,
          //     keyboardType: TextInputType.number,
          //     style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
          //     autofocus: false,
          //     onSubmitted: (v) {
          //       FocusScope.of(context).requestFocus(focus);
          //     },
          //     decoration: fieldsDesign("Latitude", Icon(Icons.location_on))),
          // TextFieldBlocBuilder(
          //     textFieldBloc: formBloc.longitude,
          //     keyboardType: TextInputType.number,
          //     style: TextStyle(color: Color(0xFF0F2E48), fontSize: 14),
          //     autofocus: false,
          //     onSubmitted: (v) {
          //       FocusScope.of(context).requestFocus(focus);
          //     },
          //     decoration: fieldsDesign("Longitude", Icon(Icons.location_on))),
          // prefs.getString('user_type') != 'business'
          //     ? SizedBox()
          //     : locationSet
          //         ? BlocBuilder<UserSignUpInfoFormBloc, FormBlocState>(
          //             builder: (context, state) {
          //               return SizedBox(
          //                 height: gridHeight,
          //                 child: Column(
          //                   children: <Widget>[
          //                     Center(
          //                         child: Text(
          //                       getArabicString(
          //                           'Choose at least one photo', false),
          //                       style: TextStyle(
          //                           color: failure
          //                               ? Colors.redAccent
          //                               : Colors.black),
          //                     )),
          //                     RaisedButton(
          //                       child: RichText(
          //                         text: TextSpan(children: [
          //                           WidgetSpan(
          //                               child: Icon(
          //                             Icons.add_a_photo_outlined,
          //                             color: Colors.white,
          //                           )),
          //                           TextSpan(
          //                             text:
          //                                 getArabicString(" Add photos", false),
          //                             style: TextStyle(
          //                                 fontWeight: FontWeight.bold),
          //                           )
          //                         ]),
          //                       ),
          //                       color: kPrimaryColor,
          //                       onPressed: () {
          //                         state is FormBlocSubmitting
          //                             ? null
          //                             : loadAssets().then((value) {
          //                                 formBloc.images = images;
          //                                 setState(() {
          //                                   gridHeight = MediaQuery.of(context)
          //                                           .size
          //                                           .height /
          //                                       4;
          //                                 });
          //                                 // var result = await FlutterImageCompress.compressWithFile(images[0].)
          //                               });
          //                       },
          //                     ),
          //                     SizedBox(
          //                       height: 5,
          //                     ),
          //                     Expanded(
          //                       child: buildGridView(),
          //                     )
          //                   ],
          //                 ),
          //               );
          //             },
          //           )
          //         : SizedBox(),
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

  Widget _bigContainer(BuildContext context, UserSignUpInfoFormBloc formBloc) {
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
              crossAxisAlignment: CrossAxisAlignment.baseline,
              mainAxisAlignment: MainAxisAlignment.spaceAround,
              textBaseline: TextBaseline.alphabetic,
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
      BuildContext context, UserSignUpInfoFormBloc formBloc) {
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
