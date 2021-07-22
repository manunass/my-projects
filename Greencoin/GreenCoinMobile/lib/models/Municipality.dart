import 'dart:ffi';

import 'Address.dart';

class Municipality {
  String id;
  String municipalityName;
  String mohafazaName;
  String qazaName;
  Address address;
  String phoneNumber;
  String email;
  double lbpCoinRatio;

  Municipality(
      {this.id,
      this.municipalityName,
      this.mohafazaName,
      this.qazaName,
      this.address,
      this.phoneNumber,
      this.email,
      this.lbpCoinRatio});

  Municipality.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    municipalityName = json['MunicipalityName'];
    mohafazaName = json['MohafazaName'];
    qazaName = json['QazaName'];
    address =
        json['Address'] != null ? new Address.fromJson(json['Address']) : null;
    phoneNumber = json['PhoneNumber'];
    email = json['Email'];
    lbpCoinRatio = json['LbpCoinRatio'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['Id'] = this.id;
    data['MunicipalityName'] = this.municipalityName;
    data['MohafazaName'] = this.mohafazaName;
    data['QazaName'] = this.qazaName;
    if (this.address != null) {
      data['Address'] = this.address.toJson();
    }
    data['PhoneNumber'] = this.phoneNumber;
    data['Email'] = this.email;
    data['LbpCoinRatio'] = this.lbpCoinRatio;
    return data;
  }
}
