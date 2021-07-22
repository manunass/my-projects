class Employee {
  String id;
  String firebaseUid;
  String municipalityId;
  String firstName;
  String lastName;
  String phoneNumber;
  String email;
  String type;

  Employee(
      {this.id,
      this.firebaseUid,
      this.municipalityId,
      this.firstName,
      this.lastName,
      this.phoneNumber,
      this.email,
      this.type});

  Employee.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    firebaseUid = json['FirebaseUid'];
    municipalityId = json['MunicipalityId'];
    firstName = json['FirstName'];
    lastName = json['LastName'];
    phoneNumber = json['PhoneNumber'];
    email = json['Email'];
    type = json['Type'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['Id'] = this.id;
    data['FirebaseUid'] = this.firebaseUid;
    data['MunicipalityId'] = this.municipalityId;
    data['FirstName'] = this.firstName;
    data['LastName'] = this.lastName;
    data['PhoneNumber'] = this.phoneNumber;
    data['Email'] = this.email;
    data['Type'] = this.type;
    return data;
  }
}
