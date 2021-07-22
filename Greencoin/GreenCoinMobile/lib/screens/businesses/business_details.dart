import 'dart:convert';

import 'package:carousel_slider/carousel_slider.dart';
import 'package:flutter/material.dart';
import 'package:flutter_polyline_points/flutter_polyline_points.dart';
import 'package:fyp/constants.dart';
import 'package:fyp/models/Business.dart';
import 'package:fyp/models/User.dart';
import 'package:fyp/screens/home/hotel_list_data.dart';
import 'package:fyp/services/navigation_service.dart';
import 'package:geolocator/geolocator.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:url_launcher/url_launcher.dart';
import '../../locator.dart';
import 'components/design_course_app_theme.dart';
import 'package:http/http.dart' as http;

import 'components/location_networking.dart';

class BusinessDetailsScreen extends StatefulWidget {
  BusinessDetailsScreen(this.business);

  final Business business;

  @override
  _BusinessDetailsScreenState createState() => _BusinessDetailsScreenState();
}

class _BusinessDetailsScreenState extends State<BusinessDetailsScreen>
    with TickerProviderStateMixin {
  final double infoHeight = 364.0;
  AnimationController animationController;
  Animation<double> animation;
  double opacity1 = 0.0;
  double opacity2 = 0.0;
  double opacity3 = 0.0;

  List<HotelListData> hotelList = HotelListData.hotelList;
  int current_index;
  BitmapDescriptor customIcon1;

  final List<LatLng> polyPoints = []; // For holding Co-ordinates as LatLng
  final Set<Polyline> polyLines = {}; // For holding instance of Polyline
  final Set<Marker> markers = {}; // For holding instance of Marker
  var data;
  bool ready = false;

  GoogleMapController mapController;
  User user;

  // Dummy Start and Destination Points
  double startLat;
  double startLng;
  double endLat;
  double endLng;

  final NavigationService _navigationService = locator<NavigationService>();
  final SharedPreferences prefs = locator<SharedPreferences>();

  void getJsonData() async {
    // Create an instance of Class NetworkHelper which uses http package
    // for requesting data to the server and receiving response as JSON format

    NetworkHelper network = NetworkHelper(
      startLat: startLat,
      startLng: startLng,
      endLat: endLat,
      endLng: endLng,
    );

    try {
      // getData() returns a json Decoded data
      data = await network.getData();
      if (data != null)
        setState(() {
          ready = true;
        });

      // We can reach to our desired JSON data manually as following
      LineString ls =
          LineString(data['features'][0]['geometry']['coordinates']);

      for (int i = 0; i < ls.lineString.length; i++) {
        polyPoints.add(LatLng(ls.lineString[i][1], ls.lineString[i][0]));
      }

      if (polyPoints.length == ls.lineString.length) {
        setPolyLines();
      }
    } catch (e) {
      print(e);
    }
  }

  setPolyLines() {
    Polyline polyline = Polyline(
      polylineId: PolylineId("polyline"),
      color: Colors.redAccent,
      width: 4,
      points: polyPoints,
    );
    polyLines.add(polyline);
    setState(() {});
  }

  @override
  void initState() {
    current_index = 0;
    ready = false;
    endLat = widget.business.address.latitude;
    endLng = widget.business.address.longitude;

    user = User.fromJson(json.decode(prefs.getString('user_object')));

    startLat = user.address.latitude;
    startLng = user.address.longitude;

    animationController = AnimationController(
        duration: const Duration(milliseconds: 1000), vsync: this);
    animation = Tween<double>(begin: 0.0, end: 1.0).animate(CurvedAnimation(
        parent: animationController,
        curve: Interval(0, 1.0, curve: Curves.fastOutSlowIn)));
    setData();
    super.initState();
    getJsonData();
  }

  Future<void> setData() async {
    animationController.forward();
    await Future<dynamic>.delayed(const Duration(milliseconds: 200));
    setState(() {
      opacity1 = 1.0;
    });
    await Future<dynamic>.delayed(const Duration(milliseconds: 200));
    setState(() {
      opacity2 = 1.0;
    });
    await Future<dynamic>.delayed(const Duration(milliseconds: 200));
    setState(() {
      opacity3 = 1.0;
    });
  }

  @override
  Widget build(BuildContext context) {
    getIcon(context);
    final double tempHeight = MediaQuery.of(context).size.height -
        (MediaQuery.of(context).size.width / 1.2) +
        24.0;
    return Scaffold(
      appBar: AppBar(
        leading: InkWell(
          onTap: () => _navigationService.navPop(),
          child: Icon(
            Icons.arrow_back,
            color: Colors.white,
          ),
        ),
        title: Text(
          "Business Details",
          style: TextStyle(color: Colors.white),
        ),
        centerTitle: true,
        backgroundColor: kPrimaryColor,
      ),
      backgroundColor: Colors.white,
      body: SingleChildScrollView(
        child: Column(
          children: <Widget>[
            Column(
              mainAxisAlignment: MainAxisAlignment.start,
              children: <Widget>[
                Padding(
                  padding: const EdgeInsets.only(top: 8.0),
                  child: CarouselSlider(
                    items: widget.business.imagesUris == null ||
                            widget.business.imagesUris.isEmpty
                        ? [
                            Image.asset(
                              "assets/images/no_images.jpg",
                              fit: BoxFit.fill,
                            )
                          ]
                        : List.generate(widget.business.imagesUris.length,
                            (index) {
                            return Image.network(
                              widget.business.imagesUris[index],
                              fit: BoxFit.fill,
                              loadingBuilder: (BuildContext context,
                                  Widget child,
                                  ImageChunkEvent loadingProgress) {
                                if (loadingProgress == null) return child;
                                return Center(
                                  child: CircularProgressIndicator(
                                    backgroundColor:
                                        Colors.grey.withOpacity(0.1),
                                    valueColor: AlwaysStoppedAnimation<Color>(
                                        kPrimaryColor),
                                    value: loadingProgress.expectedTotalBytes !=
                                            null
                                        ? loadingProgress
                                                .cumulativeBytesLoaded /
                                            loadingProgress.expectedTotalBytes
                                        : null,
                                  ),
                                );
                              },
                            );
                          }).toList(),
                    options: CarouselOptions(
                      onPageChanged: (index, reason) {
                        setState(() {
                          current_index = index;
                        });
                      },
                      height: MediaQuery.of(context).size.height / 3,

                      // aspectRatio: 2,
                      viewportFraction: 2,
                      initialPage: 0,
                      enableInfiniteScroll: false,
                      autoPlayInterval: Duration(seconds: 3),
                      autoPlayAnimationDuration: Duration(milliseconds: 800),
                      autoPlayCurve: Curves.fastOutSlowIn,
                      enlargeCenterPage: true,
                      scrollDirection: Axis.horizontal,
                    ),
                  ),
                ),
                SizedBox(
                  height: 15,
                ),
                Row(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: List.generate(
                      widget.business.imagesUris == null ||
                              widget.business.imagesUris.isEmpty
                          ? 1
                          : widget.business.imagesUris.length, (index) {
                    print(index);
                    return Icon(
                      current_index == index
                          ? Icons.stop_rounded
                          : Icons.stop_outlined,
                      color: kPrimaryColor,
                    );
                  }),
                )
                // AspectRatio(
                //   aspectRatio: 1,
                //   child: Image.asset(
                //       hotelList[int.parse(widget.businessId)].imagePath),
                // ),
              ],
            ),
            SizedBox(
              height: 15,
            ),
            Container(
              decoration: BoxDecoration(
                color: Colors.white,
                borderRadius: const BorderRadius.only(
                    topLeft: Radius.circular(32.0),
                    topRight: Radius.circular(32.0)),
                boxShadow: <BoxShadow>[
                  BoxShadow(
                      color: Colors.grey.withOpacity(0.2),
                      offset: const Offset(1.1, 1.1),
                      blurRadius: 10.0),
                ],
              ),
              child: Padding(
                padding: const EdgeInsets.only(left: 8, right: 8),
                child: SingleChildScrollView(
                  child: Container(
                    // height: MediaQuery.of(context).size.height / 1.1,
                    // constraints: BoxConstraints(
                    //     minHeight: infoHeight,
                    //     maxHeight: tempHeight > infoHeight
                    //         ? tempHeight
                    //         : infoHeight),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.start,
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: <Widget>[
                        Padding(
                          padding: const EdgeInsets.only(
                              top: 32.0, left: 18, right: 16),
                          child: Text(
                            widget.business.name,
                            textAlign: TextAlign.left,
                            style: TextStyle(
                              fontWeight: FontWeight.w600,
                              fontSize: 22,
                              letterSpacing: 0.27,
                              color: Colors.black,
                            ),
                          ),
                        ),
                        Padding(
                          padding: const EdgeInsets.only(
                              left: 16, right: 16, bottom: 8, top: 16),
                          child: Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            crossAxisAlignment: CrossAxisAlignment.center,
                            children: <Widget>[
                              Text(
                                widget.business.category,
                                textAlign: TextAlign.left,
                                style: TextStyle(
                                  fontWeight: FontWeight.w300,
                                  fontSize: 22,
                                  letterSpacing: 0.27,
                                  color: Colors.grey,
                                ),
                              ),
                              // Container(
                              //   child: Row(
                              //     children: <Widget>[
                              //       Icon(
                              //         Icons.location_on,
                              //         color: DesignCourseAppTheme.nearlyBlue,
                              //         size: 24,
                              //       ),
                              // Text(
                              //   'Show on map',
                              //   textAlign: TextAlign.left,
                              //   style: TextStyle(
                              //     fontWeight: FontWeight.w200,
                              //     fontSize: 12,
                              //     letterSpacing: 0.27,
                              //     color: DesignCourseAppTheme.grey,
                              //   ),
                              // ),
                              //     ],
                              //   ),
                              // )
                            ],
                          ),
                        ),
                        AnimatedOpacity(
                          duration: const Duration(milliseconds: 500),
                          opacity: opacity2,
                          child: Padding(
                            padding: const EdgeInsets.only(
                                left: 16, right: 16, top: 8, bottom: 8),
                            child: Text(
                              widget.business.about,
                              textAlign: TextAlign.justify,
                              style: TextStyle(
                                fontWeight: FontWeight.w300,
                                fontSize: 14,
                                letterSpacing: 0.27,
                                color: Colors.black45,
                              ),
                              maxLines: 5,
                              overflow: TextOverflow.ellipsis,
                            ),
                          ),
                        ),
                        SizedBox(
                          height: 15,
                        ),
                        showMap(context, endLat, endLng),
                        SizedBox(
                          height: 15,
                        ),
                        AnimatedOpacity(
                          duration: const Duration(milliseconds: 500),
                          opacity: opacity1,
                          child: Padding(
                            padding: const EdgeInsets.all(8),
                            child: Row(
                              mainAxisAlignment: MainAxisAlignment.spaceAround,
                              children: <Widget>[
                                getTimeBoxUI(
                                    Icons.timer,
                                    !ready
                                        ? 'Loading...'
                                        : data['features'] != null ||
                                                data['features'].isNotEmpty
                                            ? '${data['features'][0]['properties']['summary']['duration']} sec\naway'
                                            : "NaN"),
                                getTimeBoxUI(
                                    Icons.location_on,
                                    !ready
                                        ? 'Loading...'
                                        : data['features'] != null ||
                                                data['features'].isNotEmpty
                                            ? '${data['features'][0]['properties']['summary']['distance']} m\naway'
                                            : "NaN"),
                                InkWell(
                                  onTap: () => launch(
                                      "tel://${widget.business.phoneNumber}"),
                                  child: getTimeBoxUI(
                                      Icons.phone_android_outlined,
                                      'Call\nBusiness'),
                                ),

                                // getTimeBoxUI(
                                //     Icons.location_on, 'Show on map'),
                                // getTimeBoxUI('2hours', 'Time'),
                                // getTimeBoxUI('24', 'Seat'),
                              ],
                            ),
                          ),
                        ),
                        // AnimatedOpacity(
                        //   duration: const Duration(milliseconds: 500),
                        //   opacity: opacity3,
                        //   child: Padding(
                        //     padding: const EdgeInsets.only(
                        //         left: 16, bottom: 16, right: 16),
                        //     child: Row(
                        //       mainAxisAlignment: MainAxisAlignment.center,
                        //       crossAxisAlignment: CrossAxisAlignment.center,
                        //       children: <Widget>[
                        //         Container(
                        //           width: 48,
                        //           height: 48,
                        //           child: Container(
                        //             decoration: BoxDecoration(
                        //               color: DesignCourseAppTheme.nearlyWhite,
                        //               borderRadius: const BorderRadius.all(
                        //                 Radius.circular(16.0),
                        //               ),
                        //               border: Border.all(
                        //                   color: DesignCourseAppTheme.grey
                        //                       .withOpacity(0.2)),
                        //             ),
                        //             child: Icon(
                        //               Icons.add,
                        //               color: DesignCourseAppTheme.nearlyBlue,
                        //               size: 28,
                        //             ),
                        //           ),
                        //         ),
                        //         const SizedBox(
                        //           width: 16,
                        //         ),
                        //         Expanded(
                        //           child: Container(
                        //             height: 48,
                        //             decoration: BoxDecoration(
                        //               color: DesignCourseAppTheme.nearlyBlue,
                        //               borderRadius: const BorderRadius.all(
                        //                 Radius.circular(16.0),
                        //               ),
                        //               boxShadow: <BoxShadow>[
                        //                 BoxShadow(
                        //                     color: DesignCourseAppTheme
                        //                         .nearlyBlue
                        //                         .withOpacity(0.5),
                        //                     offset: const Offset(1.1, 1.1),
                        //                     blurRadius: 10.0),
                        //               ],
                        //             ),
                        //             child: Center(
                        //               child: Text(
                        //                 'Join Course',
                        //                 textAlign: TextAlign.left,
                        //                 style: TextStyle(
                        //                   fontWeight: FontWeight.w600,
                        //                   fontSize: 18,
                        //                   letterSpacing: 0.0,
                        //                   color: DesignCourseAppTheme
                        //                       .nearlyWhite,
                        //                 ),
                        //               ),
                        //             ),
                        //           ),
                        //         )
                        //       ],
                        //     ),
                        //   ),
                        // ),
                        // SizedBox(
                        //   height: MediaQuery.of(context).padding.bottom,
                        // )
                      ],
                    ),
                  ),
                ),
              ),
            ),
            // Positioned(
            //   top: (MediaQuery.of(context).size.width / 1.2) - 24.0 - 35,
            //   right: 35,
            //   child: ScaleTransition(
            //     alignment: Alignment.center,
            //     scale: CurvedAnimation(
            //         parent: animationController, curve: Curves.fastOutSlowIn),
            //     child: Card(
            //       color: DesignCourseAppTheme.nearlyBlue,
            //       shape: RoundedRectangleBorder(
            //           borderRadius: BorderRadius.circular(50.0)),
            //       elevation: 10.0,
            //       child: Container(
            //         width: 60,
            //         height: 60,
            //         child: Center(
            //           child: Icon(
            //             Icons.favorite,
            //             color: DesignCourseAppTheme.nearlyWhite,
            //             size: 30,
            //           ),
            //         ),
            //       ),
            //     ),
            //   ),
            // ),
            // Padding(
            //   padding: EdgeInsets.only(top: MediaQuery.of(context).padding.top),
            //   child: SizedBox(
            //     width: AppBar().preferredSize.height,
            //     height: AppBar().preferredSize.height,
            //     child: Material(
            //       color: Colors.transparent,
            //       child: InkWell(
            //         borderRadius:
            //             BorderRadius.circular(AppBar().preferredSize.height),
            //         child: Icon(
            //           Icons.arrow_back_ios,
            //           color: Colors.black,
            //         ),
            //         onTap: () {
            //           Navigator.pop(context);
            //         },
            //       ),
            //     ),
            //   ),
            // )
          ],
        ),
      ),
    );
  }

  Widget getTimeBoxUI(icon, String txt2) {
    return Padding(
      padding: const EdgeInsets.all(8.0),
      child: Container(
        // width: MediaQuery.of(context).size.width / 4,
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: const BorderRadius.all(Radius.circular(16.0)),
          boxShadow: <BoxShadow>[
            BoxShadow(
                color: Colors.grey.withOpacity(0.2),
                offset: const Offset(1.1, 1.1),
                blurRadius: 8.0),
          ],
        ),
        child: Padding(
          padding: const EdgeInsets.only(
              left: 18.0, right: 18.0, top: 12.0, bottom: 12.0),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            crossAxisAlignment: CrossAxisAlignment.center,
            children: <Widget>[
              // Text(
              //   text1,
              //   textAlign: TextAlign.center,
              //   style: TextStyle(
              //     fontWeight: FontWeight.w600,
              //     fontSize: 14,
              //     letterSpacing: 0.27,
              //     color: DesignCourseAppTheme.nearlyBlue,
              //   ),
              // ),
              Icon(
                icon,
                color: kPrimaryColor,
                size: 24,
              ),
              SizedBox(
                height: 10,
              ),
              Text(
                txt2,
                textAlign: TextAlign.center,
                style: TextStyle(
                  fontWeight: FontWeight.w300,
                  fontSize: 12,
                  letterSpacing: 0.27,
                  color: Colors.black45,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget showMap(BuildContext context, double lat, double long) {
    return Container(
      decoration: BoxDecoration(
          color: Colors.blue,
          borderRadius: BorderRadius.all(Radius.circular(50))),
      height: MediaQuery.of(context).size.height / 3,
      width: MediaQuery.of(context).size.width,
      child: GoogleMap(
        zoomControlsEnabled: false,
        scrollGesturesEnabled: false,
        // polylines: polyLines,
        mapType: MapType.terrain,
        initialCameraPosition:
            CameraPosition(target: LatLng(lat, long), zoom: 18),
        onMapCreated: (GoogleMapController controller) {
          mapController = controller;
          // updateCamera();
        },
        mapToolbarEnabled: false,
        onTap: (argument) {
          //  String mapOptions = [
          //     'origin=$originPlaceId',
          //     'origin_place_id=$originPlaceId',
          //     'destination=$destinationPlaceId',
          //     'destination_place_id=$destinationPlaceId',
          //     'dir_action=navigate'
          //   ].join('&');
          final url =
              'https://www.google.com/maps/dir/?api=1&origin=$startLat,$startLng&destination=$endLat,$endLng&travelmode=driving&dir_action=navigate';
          launch(url);
        },
        markers: {
          // Marker(
          //   markerId: MarkerId("x"),
          //   position: LatLng(startLat, startLng),
          //   infoWindow: InfoWindow(
          //     title: 'Start',
          //     snippet: "_startAddress",
          //   ),
          // ),
          Marker(
            markerId: MarkerId("xx"),
            position: LatLng(endLat, endLng),
            draggable: false,
            onTap: null,
            infoWindow: InfoWindow(
              title: widget.business.name,
              snippet: widget.business.address.areaOrStreet,
            ),
          )
        },
      ),
    );
  }

  updateCamera() {
    print('// camera view of the map');
    mapController.animateCamera(
      CameraUpdate.newLatLngBounds(
        LatLngBounds(
          northeast: LatLng(
            startLat <= endLat ? endLat : startLat,
            startLng <= endLng ? endLng : startLng,
          ),
          southwest: LatLng(
            startLat > endLat ? endLat : startLat,
            startLng > endLng ? endLng : startLng,
          ),
        ),
        50.0, // padding
      ),
    );
  }

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
}

class LineString {
  LineString(this.lineString);
  List<dynamic> lineString;
}
