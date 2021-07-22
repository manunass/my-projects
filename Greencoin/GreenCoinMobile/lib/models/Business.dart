import 'Address.dart';
import 'Wallet.dart';

class Business {
  String id;
  String firebaseUid;
  String municipalityId;
  Address address;
  Wallet wallet;
  String name;
  String phoneNumber;
  String ownerFirstName;
  String ownerLastName;
  String category;
  String about;
  List<String> imagesUris;

  Business(
      {this.id,
      this.firebaseUid,
      this.municipalityId,
      this.address,
      this.wallet,
      this.name,
      this.phoneNumber,
      this.ownerFirstName,
      this.ownerLastName,
      this.category,
      this.about,
      this.imagesUris});

  Business.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    firebaseUid = json['FirebaseUid'] != null ? json['FirebaseUid'] : null;
    municipalityId = json['MunicipalityId'];
    address =
        json['Address'] != null ? new Address.fromJson(json['Address']) : null;
    wallet =
        json['Wallet'] != null ? new Wallet.fromJson(json['Wallet']) : null;
    name = json['Name'];
    phoneNumber = json['PhoneNumber'];
    ownerFirstName = json['OwnerFirstName'];
    ownerLastName = json['OwnerLastName'];
    category = json['Category'];
    about = json['About'];
    imagesUris =
        json['ImagesUris'] != null ? json['ImagesUris'].cast<String>() : [];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    if (this.id != null) data['Id'] = this.id;
    data['FirebaseUid'] = this.firebaseUid;
    data['MunicipalityId'] = this.municipalityId;
    if (this.address != null) {
      data['Address'] = this.address.toJson();
    }
    if (this.wallet != null) {
      data['Wallet'] = this.wallet.toJson();
    }
    data['Name'] = this.name;
    data['PhoneNumber'] = this.phoneNumber;
    data['OwnerFirstName'] = this.ownerFirstName;
    data['OwnerLastName'] = this.ownerLastName;
    data['Category'] = this.category;
    data['About'] = this.about;
    if (this.imagesUris != null) data['ImagesUris'] = this.imagesUris;
    return data;
  }
}
