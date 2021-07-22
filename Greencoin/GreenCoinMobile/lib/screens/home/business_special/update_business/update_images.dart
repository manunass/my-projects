import 'dart:convert';
import 'dart:io';

import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:flutter_staggered_grid_view/flutter_staggered_grid_view.dart';
import 'package:fyp/controllers/business_controller.dart';
import 'package:fyp/models/Business.dart';
import 'package:fyp/services/navigation_service.dart';
import 'package:http/http.dart' as http;
import 'package:image_picker/image_picker.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../../../constants.dart';
import '../../../../locator.dart';
import '../../../../routes_names.dart';

class ImagesUpdate extends StatefulWidget {
  @override
  _ImagesUpdateState createState() => _ImagesUpdateState();
}

class _ImagesUpdateState extends State<ImagesUpdate> {
  File _image;
  final picker = ImagePicker();

  Map<int, File> images = {};

  List<bool> isFilled;

  String msg = '1';

  Business business;

  Map<String, String> oldImageUris = {};

  final NavigationService _navigationService = locator<NavigationService>();

  final SharedPreferences prefs = locator<SharedPreferences>();

  onSubmit() async {
    final String url =
        "https://greencoin.azurewebsites.net/api/Images/${business.id}";

    bool isUpdated = false;

    List<String> idsToBeDeleted = [];

    showDialogWindow('Deleting Old Images', false, context);

    for (String uri in oldImageUris.keys) {
      if (!business.imagesUris.contains(uri)) {
        idsToBeDeleted.add(oldImageUris[uri]);
      }
    }

    if (idsToBeDeleted.isNotEmpty) {
      isUpdated = true;
      for (String id in idsToBeDeleted) {
        final response = await http.delete('$url/$id');

        if (response.statusCode == 200) {
        } else {
          throw Exception('Failed to delete');
        }
      }
    }

    print(idsToBeDeleted);

    hideDialog();

    if (images.isNotEmpty) {
      showDialogWindow(
          'Adding New Photos ($msg/${images.length})', false, context);

      List<File> imagesToAdd = [];
      for (File file in images.values) {
        if (file != null) {
          imagesToAdd.add(file);
        }
      }

      if (imagesToAdd.isNotEmpty) {
        int index = 1;
        for (File filePath in imagesToAdd) {
          var formData = FormData();

          formData.files.addAll([
            MapEntry(
              'files',
              MultipartFile.fromFileSync(filePath.path,
                  filename: filePath.path.split('/').last),
            )
          ]);
          Dio dio = new Dio();
          var response = await dio.post(url,
              data: formData,
              options: Options(
                contentType: "multipart/form-data",
              ));
          setState(() {
            index++;
            msg = '$index';
          });
          if (index < imagesToAdd.length) {
            hideDialog();
            showDialogWindow(
                'Adding New Photos ($msg/${images.length})', false, context);
          }
          print(response.statusCode);
          print(response.data);
          print(response.statusMessage);
        }

        isUpdated = true;
      }
      hideDialog();
    }

    // if (isUpdated) {
    //   showDialogWindow('Updating your profile', false, context);
    //   BusinessController().
    // }

    _navigationService.replaceAndClearNav(SplashScreenRoute);
  }

  Map<String, String> prepareImages() {
    Map<String, String> temp = {};

    for (String uri in business.imagesUris) {
      List<String> total = uri.split('/');
      temp[uri] = total[5];
    }

    return temp;
  }

  Future getImage(int index, {String uri}) async {
    final pickedFile =
        await picker.getImage(source: ImageSource.gallery, imageQuality: 25);

    setState(() {
      if (pickedFile != null) {
        print('hi');
        images[index] = File(pickedFile.path);
        if (uri != null) {
          business.imagesUris.remove(uri);
        }
      } else {
        print('No image selected.');
      }
    });
  }

  @override
  void initState() {
    business =
        Business.fromJson(json.decode(prefs.getString('business_object')));
    oldImageUris = prepareImages();
    print(oldImageUris);
    // TODO: implement initState
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          SafeArea(
            child: StaggeredGridView.count(
              shrinkWrap: true,
              physics: NeverScrollableScrollPhysics(),
              crossAxisCount: 4,
              staggeredTiles: <StaggeredTile>[
                StaggeredTile.count(2, 2),
                StaggeredTile.count(2, 2),
                StaggeredTile.count(2, 2),
                StaggeredTile.count(2, 2),
              ],
              mainAxisSpacing: 4,
              crossAxisSpacing: 4,
              padding: const EdgeInsets.all(4),
              children: [
                getTile(
                  0,
                  kPrimaryColor.withOpacity(0.1),
                  Icons.add,
                  "Add Image",
                  () {
                    if (business.imagesUris.length > 0)
                      getImage(0, uri: business.imagesUris[0]);
                    else
                      getImage(0);
                  },
                  images,
                  imageUrl: business.imagesUris.length > 0
                      ? business.imagesUris[0]
                      : null,
                ),
                getTile(
                  1,
                  kPrimaryColor.withOpacity(0.1),
                  Icons.add,
                  "Add Image",
                  () {
                    if (business.imagesUris.length > 1)
                      getImage(1, uri: business.imagesUris[1]);
                    else
                      getImage(1);
                  },
                  images,
                  imageUrl: business.imagesUris.length > 1
                      ? business.imagesUris[1]
                      : null,
                ),
                getTile(
                  2,
                  kPrimaryColor.withOpacity(0.1),
                  Icons.add,
                  "Add Image",
                  () {
                    if (business.imagesUris.length > 2)
                      getImage(2, uri: business.imagesUris[2]);
                    else
                      getImage(2);
                  },
                  images,
                  imageUrl: business.imagesUris.length > 2
                      ? business.imagesUris[2]
                      : null,
                ),
                getTile(
                  3,
                  kPrimaryColor.withOpacity(0.1),
                  Icons.add,
                  "Add Image",
                  () {
                    if (business.imagesUris.length > 3)
                      getImage(3, uri: business.imagesUris[3]);
                    else
                      getImage(3);
                  },
                  images,
                  imageUrl: business.imagesUris.length > 3
                      ? business.imagesUris[3]
                      : null,
                ),
              ],
            ),
          ),
          GestureDetector(
            onTap: () {
              setState(() {
                onSubmit();
              });
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

  Widget getTile(int index, Color backgroundColor, IconData iconData,
      String text, Function onTap, Map<int, File> images,
      {String imageUrl}) {
    return Stack(children: [
      Card(
        color: backgroundColor,
        child: InkWell(
          onTap: () {
            if (imageUrl == null && (images[index] == null)) onTap();
          },
          child: imageUrl == null && (images[index] == null)
              ? Center(
                  child: Padding(
                    padding: const EdgeInsets.all(8),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.spaceAround,
                      crossAxisAlignment: CrossAxisAlignment.center,
                      children: [
                        Icon(
                          iconData,
                          color: Colors.white,
                          size: 35,
                        ),
                        Text(
                          text,
                          style: TextStyle(
                              color: Colors.white,
                              fontWeight: FontWeight.w900,
                              fontSize: 16),
                        )
                      ],
                    ),
                  ),
                )
              : (images[index] == null)
                  ? Image.network(
                      imageUrl,
                      fit: BoxFit.cover,
                      loadingBuilder: (context, child, loadingProgress) {
                        if (loadingProgress == null) return child;
                        return Center(
                          heightFactor: 10,
                          child: CircularProgressIndicator(
                              valueColor: new AlwaysStoppedAnimation<Color>(
                                  Colors.white)),
                        );
                      },
                    )
                  : Image.file(
                      images[index],
                      fit: BoxFit.fill,
                    ),
        ),
      ),
      imageUrl != null || (images[index] != null)
          ? Align(
              alignment: Alignment.topRight,
              child: CircleAvatar(
                radius: 14.0,
                backgroundColor: Colors.black,
                child: InkWell(
                    onTap: () {
                      setState(() {
                        images[index] = null;
                        try {
                          business.imagesUris.removeAt(index);
                        } catch (_) {}
                      });
                    },
                    child: Icon(Icons.close, color: Colors.white)),
              ),
            )
          : SizedBox(),
    ]);
  }
}
