import 'package:expandable/expandable.dart';
import 'package:flutter/material.dart';
import 'package:fyp/models/BagScan.dart';

import '../../../../constants.dart';

class BagScanCard extends StatelessWidget {
  BagScanCard(this.bagScan);

  final BagScan bagScan;

  @override
  Widget build(BuildContext context) {
    return ExpandableNotifier(
        child: Container(
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(10),
        shape: BoxShape.rectangle,
        color: Colors.grey.withOpacity(0.05),
        boxShadow: [
          BoxShadow(
            color: Colors.grey.withOpacity(0.05),
            offset: Offset(0.0, 2.0), //(x,y)
            blurRadius: 6.0,
          ),
        ],
      ),
      child: Column(
        children: <Widget>[
          Padding(
            padding: const EdgeInsets.fromLTRB(8, 15, 8, 8),
            child: Row(
              crossAxisAlignment: CrossAxisAlignment.center,
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                // Container(
                //     width: MediaQuery.of(context).size.width / 3,
                //     child: Text(bagScan.quality,
                //         style: TextStyle(
                //           fontWeight: FontWeight.w900,
                //         ))),
                Container(
                  width: MediaQuery.of(context).size.width / 5,
                  child: Text('${bagScan.weight.toString()} Kg'),
                ),
                Container(
                    width: MediaQuery.of(context).size.width / 4,
                    child: Text(getDate(DateTime.fromMillisecondsSinceEpoch(
                        bagScan.unixTimeScanned * 1000)))),
                Container(
                  width: MediaQuery.of(context).size.width / 5,
                  child: Text(bagScan.processed ? 'Processed' : 'Pending',
                      style: TextStyle(
                          color:
                              bagScan.processed ? Colors.green : Colors.blue)),
                ),
              ],
            ),
          ),
          SizedBox(
            height: 10,
          ),
          ScrollOnExpand(
            scrollOnExpand: true,
            scrollOnCollapse: false,
            child: ExpandablePanel(
              theme: const ExpandableThemeData(
                headerAlignment: ExpandablePanelHeaderAlignment.center,
                tapBodyToCollapse: true,
              ),
              header: Padding(
                padding: const EdgeInsets.all(8.0),
                child: Text(
                  "Show Details",
                  style: Theme.of(context).textTheme.body2,
                ),
              ),
              expanded: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: <Widget>[
                  SizedBox(
                    height: 20,
                  ),
                  Padding(
                    padding: EdgeInsets.only(bottom: 10),
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.spaceAround,
                      children: [
                        Text(
                          "Quality: ${bagScan.quality}",
                          softWrap: true,
                          overflow: TextOverflow.fade,
                        ),
                        // Text(
                        //   ": ${transaction.postAmount}",
                        //   softWrap: true,
                        //   overflow: TextOverflow.fade,
                        // ),
                      ],
                    ),
                  ),
                ],
              ),
              builder: (_, collapsed, expanded) {
                return Padding(
                  padding: EdgeInsets.only(left: 10, right: 10, bottom: 10),
                  child: Expandable(
                    collapsed: collapsed,
                    expanded: expanded,
                    theme: const ExpandableThemeData(crossFadePoint: 0),
                  ),
                );
              },
            ),
          ),
        ],
      ),
    ));
  }

  String getDate(DateTime dateTime) {
    DateTime now = DateTime.now();

    var difference = now.difference(dateTime);

    if (difference.inSeconds >= 0 && difference.inSeconds < 60)
      return getArabicString("Just now", false);
    else if (difference.inMinutes >= 0 && difference.inMinutes < 60)
      return "${difference.inMinutes} ${getArabicString("min ago", false)}";
    else if (difference.inHours >= 0 && difference.inHours < 24)
      return "${difference.inHours} ${getArabicString("hrs ago", false)}";
    else if (difference.inDays >= 0 && difference.inDays < 31)
      return "${difference.inDays} ${getArabicString("days ago", false)}";
    else if (difference.inDays >= 31 && difference.inDays < 366)
      return "${(difference.inDays / 31).ceil()} ${getArabicString("months ago", false)}";
    else
      return "${(difference.inDays / 366).ceil()} ${getArabicString("years ago", false)}";
  }
}
