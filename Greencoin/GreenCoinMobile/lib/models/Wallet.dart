class Wallet {
  String id;
  int balance;
  int score;

  Wallet({this.id, this.balance, this.score});

  Wallet.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    balance = json['Balance'];
    score = json['Score'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['Id'] = this.id;
    data['Balance'] = this.balance;
    data['Score'] = this.score;

    return data;
  }
}
