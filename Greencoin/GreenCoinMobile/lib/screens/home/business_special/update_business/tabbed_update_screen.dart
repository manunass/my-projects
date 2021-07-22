import 'package:flutter/material.dart';

import '../../../../constants.dart';
import 'update_business.dart';
import 'update_images.dart';

class TabbedUpdateScreen extends StatelessWidget {
  TabbedUpdateScreen(this.index);

  final int index;

  @override
  Widget build(BuildContext context) {
    return DefaultTabController(
      length: 2,
      initialIndex: index,
      child: Scaffold(
        appBar: AppBar(
          title: Text(
            "Update Business",
            style: TextStyle(color: Colors.white),
          ),
          centerTitle: true,
          backgroundColor: kPrimaryColor,
          bottom: TabBar(
            indicatorColor: Colors.white,
            tabs: [
              Tab(
                text: 'Update Info',
              ),
              Tab(text: 'Update Images'),
            ],
          ),
        ),
        body: TabBarView(
          physics: NeverScrollableScrollPhysics(),
          children: [
            BusinessUpdateScreen(),
            ImagesUpdate(),
          ],
        ),
      ),
    );
  }
}
