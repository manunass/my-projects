class Address {
  double latitude;
  double longitude;
  String areaOrStreet;
  String building;
  String flat;

  Address(
      {this.latitude,
      this.longitude,
      this.areaOrStreet,
      this.building,
      this.flat});

  Address.fromJson(Map<String, dynamic> json) {
    latitude = json['Latitude'].toDouble();
    longitude = json['Longitude'].toDouble();
    areaOrStreet = json['AreaOrStreet'];
    building = json['Building'];
    flat = json['Flat'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['Latitude'] = this.latitude;
    data['Longitude'] = this.longitude;
    data['AreaOrStreet'] = this.areaOrStreet;
    data['Building'] = this.building;
    data['Flat'] = this.flat;
    return data;
  }
}
