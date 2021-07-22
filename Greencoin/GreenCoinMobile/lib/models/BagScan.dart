class BagScan {
  String id;
  String userId;
  String employeeId;
  String batchId;
  double weight;
  String quality;
  bool processed;
  int unixTimeScanned;
  int unixTimeProcessed;

  BagScan(
      {this.id,
      this.userId,
      this.employeeId,
      this.batchId,
      this.weight,
      this.quality,
      this.processed,
      this.unixTimeScanned,
      this.unixTimeProcessed});

  BagScan.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    userId = json['UserId'];
    employeeId = json['EmployeeId'];
    batchId = json['BatchId'];
    weight = json['Weight'].toDouble();
    quality = json['Quality'];
    processed = json['Processed'];
    unixTimeScanned = json['UnixTimeScanned'];
    unixTimeProcessed = json['UnixTimeProcessed'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['Id'] = this.id;
    data['UserId'] = this.userId;
    data['EmployeeId'] = this.employeeId;
    data['BatchId'] = this.batchId;
    data['Weight'] = this.weight;
    data['Quality'] = this.quality;
    data['Processed'] = this.processed;
    data['UnixTimeScanned'] = this.unixTimeScanned;
    data['UnixTimeProcessed'] = this.unixTimeProcessed;
    return data;
  }
}
