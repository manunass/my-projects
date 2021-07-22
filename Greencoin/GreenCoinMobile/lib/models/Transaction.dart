class Transaction {
  String id;
  String walletId;
  String transferId;
  int unixTime;
  String description;
  int preAmount;
  int amount;
  int postAmount;

  Transaction(
      {this.id,
      this.walletId,
      this.transferId,
      this.unixTime,
      this.description,
      this.preAmount,
      this.amount,
      this.postAmount});

  Transaction.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    walletId = json['WalletId'];
    transferId = json['TransferId'];
    unixTime = json['UnixTime'];
    description = json['Description'];
    preAmount = json['PreAmount'];
    amount = json['Amount'];
    postAmount = json['PostAmount'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['Id'] = this.id;
    data['WalletId'] = this.walletId;
    data['TransferId'] = this.transferId;
    data['UnixTime'] = this.unixTime;
    data['Description'] = this.description;
    data['PreAmount'] = this.preAmount;
    data['Amount'] = this.amount;
    data['PostAmount'] = this.postAmount;
    return data;
  }
}
