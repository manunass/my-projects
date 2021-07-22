import 'package:fyp/models/Business.dart';

import '../../../constants.dart';
import '../hotel_app_theme.dart';
import 'package:flutter/material.dart';
import 'package:font_awesome_flutter/font_awesome_flutter.dart';
import 'package:smooth_star_rating/smooth_star_rating.dart';

import '../hotel_list_data.dart';

class CustomListView extends StatelessWidget {
  const CustomListView(
      {Key key,
      this.businessData,
      this.animationController,
      this.animation,
      this.callback})
      : super(key: key);

  final VoidCallback callback;
  final Business businessData;
  final AnimationController animationController;
  final Animation<dynamic> animation;

  @override
  Widget build(BuildContext context) {
    return AnimatedBuilder(
      animation: animationController,
      builder: (BuildContext context, Widget child) {
        return FadeTransition(
          opacity: animation,
          child: Transform(
            transform: Matrix4.translationValues(
                0.0, 50 * (1.0 - animation.value), 0.0),
            child: Padding(
              padding: const EdgeInsets.only(
                  left: 24, right: 24, top: 8, bottom: 16),
              child: InkWell(
                splashColor: Colors.transparent,
                onTap: () {
                  callback();
                },
                child: Container(
                  decoration: BoxDecoration(
                    borderRadius: const BorderRadius.all(Radius.circular(16.0)),
                    boxShadow: <BoxShadow>[
                      BoxShadow(
                        color: Colors.grey.withOpacity(0.6),
                        offset: const Offset(4, 4),
                        blurRadius: 20,
                      ),
                    ],
                  ),
                  child: ClipRRect(
                    borderRadius: const BorderRadius.all(Radius.circular(16.0)),
                    child: Stack(
                      children: <Widget>[
                        Column(
                          children: <Widget>[
                            AspectRatio(
                              aspectRatio: 2,
                              child: businessData.imagesUris == null ||
                                      businessData.imagesUris.isEmpty
                                  ? Image.asset(
                                      "assets/images/no_images.jpg",
                                      fit: BoxFit.fill,
                                    )
                                  : Image.network(
                                      businessData.imagesUris[0],
                                      fit: BoxFit.fill,
                                      loadingBuilder: (BuildContext context,
                                          Widget child,
                                          ImageChunkEvent loadingProgress) {
                                        if (loadingProgress == null)
                                          return child;
                                        return Center(
                                          child: CircularProgressIndicator(
                                            backgroundColor: Colors.white,
                                            valueColor:
                                                AlwaysStoppedAnimation<Color>(
                                                    kPrimaryColor),
                                            value: loadingProgress
                                                        .expectedTotalBytes !=
                                                    null
                                                ? loadingProgress
                                                        .cumulativeBytesLoaded /
                                                    loadingProgress
                                                        .expectedTotalBytes
                                                : null,
                                          ),
                                        );
                                      },
                                    ),
                            ),
                            Container(
                              color: HotelAppTheme.buildLightTheme()
                                  .backgroundColor,
                              child: Row(
                                mainAxisAlignment: MainAxisAlignment.center,
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: <Widget>[
                                  Expanded(
                                    child: Container(
                                      child: Padding(
                                        padding: const EdgeInsets.only(
                                            left: 16, top: 8, bottom: 8),
                                        child: Column(
                                          mainAxisAlignment:
                                              MainAxisAlignment.center,
                                          crossAxisAlignment:
                                              CrossAxisAlignment.start,
                                          children: <Widget>[
                                            Text(
                                              businessData.name,
                                              textAlign: TextAlign.left,
                                              style: TextStyle(
                                                fontWeight: FontWeight.w600,
                                                fontSize: 22,
                                              ),
                                            ),
                                            const SizedBox(
                                              height: 4,
                                            ),
                                            Row(
                                              crossAxisAlignment:
                                                  CrossAxisAlignment.center,
                                              mainAxisAlignment:
                                                  MainAxisAlignment.start,
                                              children: <Widget>[
                                                Text(
                                                  businessData.category,
                                                  style: TextStyle(
                                                      fontSize: 14,
                                                      color: Colors.grey
                                                          .withOpacity(0.8)),
                                                ),
                                                const SizedBox(
                                                  width: 4,
                                                ),
                                                // TODO: calculate the distance
                                                // Icon(
                                                //   FontAwesomeIcons.mapMarkerAlt,
                                                //   size: 12,
                                                //   color: HotelAppTheme
                                                //           .buildLightTheme()
                                                //       .primaryColor,
                                                // ),
                                                // Expanded(
                                                //   child: Text(
                                                //     '${businessData.category}',
                                                //     overflow:
                                                //         TextOverflow.ellipsis,
                                                //     style: TextStyle(
                                                //         fontSize: 14,
                                                //         color: Colors.grey
                                                //             .withOpacity(0.8)),
                                                //   ),
                                                // ),
                                              ],
                                            ),
                                            Padding(
                                              padding: const EdgeInsets.only(
                                                  top: 8, right: 8),
                                              child: Text(
                                                businessData.about,
                                                maxLines: 3,
                                                overflow: TextOverflow.ellipsis,
                                                style: TextStyle(
                                                    fontSize: 16,
                                                    color: Colors.black12
                                                        .withOpacity(0.6)),
                                              ),
                                            )
                                            // Padding(
                                            //   padding:
                                            //       const EdgeInsets.only(top: 4),
                                            //   child: Row(
                                            //     children: <Widget>[
                                            //       SmoothStarRating(
                                            //         allowHalfRating: true,
                                            //         starCount: 5,
                                            //         rating: businessData.rating,
                                            //         size: 20,
                                            //         color: HotelAppTheme
                                            //                 .buildLightTheme()
                                            //             .primaryColor,
                                            //         borderColor: HotelAppTheme
                                            //                 .buildLightTheme()
                                            //             .primaryColor,
                                            //       ),
                                            //       Text(
                                            //         ' ${businessData.reviews} Reviews',
                                            //         style: TextStyle(
                                            //             fontSize: 14,
                                            //             color: Colors.grey
                                            //                 .withOpacity(0.8)),
                                            //       ),
                                            //     ],
                                            //   ),
                                            // ),
                                          ],
                                        ),
                                      ),
                                    ),
                                  ),
                                  // Padding(
                                  //   padding: const EdgeInsets.only(
                                  //       right: 16, top: 8),
                                  //   child: Column(
                                  //     mainAxisAlignment:
                                  //         MainAxisAlignment.center,
                                  //     crossAxisAlignment:
                                  //         CrossAxisAlignment.end,
                                  //     children: <Widget>[
                                  //       Text(
                                  //         '\$${businessData.perNight}',
                                  //         textAlign: TextAlign.left,
                                  //         style: TextStyle(
                                  //           fontWeight: FontWeight.w600,
                                  //           fontSize: 22,
                                  //         ),
                                  //       ),
                                  //       Text(
                                  //         '/per night',
                                  //         style: TextStyle(
                                  //             fontSize: 14,
                                  //             color:
                                  //                 Colors.grey.withOpacity(0.8)),
                                  //       ),
                                  //     ],
                                  //   ),
                                  // ),
                                ],
                              ),
                            ),
                          ],
                        ),
                        // Positioned(
                        //   top: 8,
                        //   right: 8,
                        //   child: Material(
                        //     color: Colors.transparent,
                        //     child: InkWell(
                        //       borderRadius: const BorderRadius.all(
                        //         Radius.circular(32.0),
                        //       ),
                        //       onTap: () {},
                        //       child: Padding(
                        //         padding: const EdgeInsets.all(8.0),
                        //         child: Icon(
                        //           Icons.favorite_border,
                        //           color: HotelAppTheme.buildLightTheme()
                        //               .primaryColor,
                        //         ),
                        //       ),
                        //     ),
                        //   ),
                        // )
                      ],
                    ),
                  ),
                ),
              ),
            ),
          ),
        );
      },
    );
  }
}
