import 'package:expandable/expandable.dart';
import 'package:flutter/material.dart';
import 'package:fyp/models/Transaction.dart';

import '../../../constants.dart';

class TransactionCard extends StatelessWidget {
  TransactionCard(this.transaction);

  final Transaction transaction;

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
                Container(
                  width: MediaQuery.of(context).size.width / 3,
                  child: Text(
                      transaction.description.contains('Recycling')
                          ? 'Bag Scan Reward'
                          : transaction.description,
                      style: TextStyle(
                          fontWeight: FontWeight.w900,
                          color: transaction.amount.toString().contains('-')
                              ? Colors.red
                              : Colors.green)),
                ),
                Container(
                  width: MediaQuery.of(context).size.width / 5,
                  child: Text(
                    transaction.amount.toString().contains('-')
                        ? transaction.amount.toString()
                        : '+${transaction.amount.toString()}',
                  ),
                ),
                Container(
                    width: MediaQuery.of(context).size.width / 4,
                    child: Text(getDate(DateTime.fromMillisecondsSinceEpoch(
                        transaction.unixTime * 1000)))),
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
                          "Pre-amount: ${transaction.preAmount}",
                          softWrap: true,
                          overflow: TextOverflow.fade,
                        ),
                        Text(
                          "Post-amount: ${transaction.postAmount}",
                          softWrap: true,
                          overflow: TextOverflow.fade,
                        ),
                      ],
                    ),
                  ),
                  SizedBox(
                    height: 20,
                  ),
                  transaction.description.contains('Recycling')
                      ? Padding(
                          padding: const EdgeInsets.all(10.0),
                          child: Text(
                            "Description: ${transaction.description}",
                            softWrap: true,
                            overflow: TextOverflow.fade,
                          ),
                        )
                      : SizedBox()
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
