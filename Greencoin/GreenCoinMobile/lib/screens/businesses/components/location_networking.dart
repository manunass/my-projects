import 'package:http/http.dart' as http;
import 'dart:convert';

class NetworkHelper {
  NetworkHelper({this.startLng, this.startLat, this.endLng, this.endLat});

  final String url = 'https://api.openrouteservice.org/v2/directions/';
  final String apiKey =
      '5b3ce3597851110001cf6248268a355227ff48108d31db4f1ee4136e';
  final String journeyMode =
      'driving-car'; // Change it if you want or make it variable
  final double startLng;
  final double startLat;
  final double endLng;
  final double endLat;

  Future getData() async {
    http.Response response = await http.get(
        '$url$journeyMode?api_key=$apiKey&start=$startLng,$startLat&end=$endLng,$endLat&radiuses=500');

    if (response.statusCode == 200) {
      String data = response.body;
      print(data);
      return jsonDecode(data);
    } else {
      print(response.body);
    }
  }
}
