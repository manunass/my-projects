import 'Address.dart';
import 'Wallet.dart';

class User {
  String id;
  String firebaseUid;
  String municipalityId;
  Address address;
  Wallet wallet;
  String firstName;
  String lastName;
  String phoneNumber;
  String email;
  bool isVerified;

  User(
      {this.id,
      this.firebaseUid,
      this.municipalityId,
      this.address,
      this.wallet,
      this.firstName,
      this.lastName,
      this.phoneNumber,
      this.email,
      this.isVerified});

  User.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    firebaseUid = json['FirebaseUid'];
    municipalityId = json['MunicipalityId'];
    address =
        json['Address'] != null ? new Address.fromJson(json['Address']) : null;
    wallet =
        json['Wallet'] != null ? new Wallet.fromJson(json['Wallet']) : null;
    firstName = json['FirstName'];
    lastName = json['LastName'];
    phoneNumber = json['PhoneNumber'];
    email = json['Email'];
    isVerified = json['IsVerified'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['Id'] = this.id;
    data['FirebaseUid'] = this.firebaseUid;
    data['MunicipalityId'] = this.municipalityId;
    if (this.address != null) {
      data['Address'] = this.address.toJson();
    }
    if (this.wallet != null) {
      data['Wallet'] = this.wallet.toJson();
    }
    data['FirstName'] = this.firstName;
    data['LastName'] = this.lastName;
    data['PhoneNumber'] = this.phoneNumber;
    data['Email'] = this.email;
    data['IsVerified'] = this.isVerified;
    return data;
  }
}
