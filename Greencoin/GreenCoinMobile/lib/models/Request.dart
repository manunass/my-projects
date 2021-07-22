class Request {
  String id;
  String userId;
  int unixTimeRequested;
  int unixTimeCompleted;
  int unixTimeApproved;
  String type;
  String status;

  Request(
      {this.id,
      this.userId,
      this.unixTimeRequested,
      this.unixTimeCompleted,
      this.unixTimeApproved,
      this.type,
      this.status});

  Request.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    userId = json['UserId'];
    unixTimeRequested = json['UnixTimeRequested'];
    unixTimeCompleted = json['UnixTimeCompleted'];
    unixTimeApproved = json['UnixTimeApproved'];
    type = json['Type'];
    status = json['Status'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['Id'] = this.id;
    data['UserId'] = this.userId;
    data['UnixTimeRequested'] = this.unixTimeRequested;
    data['UnixTimeCompleted'] = this.unixTimeCompleted;
    data['UnixTimeApproved'] = this.unixTimeApproved;
    data['Type'] = this.type;
    data['Status'] = this.status;
    return data;
  }
}
