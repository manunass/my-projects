import 'dart:convert';
import 'package:fyp/models/Transaction.dart';
import 'package:http/http.dart';

class WalletController {
  final String apiUrl = "https://greencoin.azurewebsites.net/api/Wallets";

  Future<int> payBusiness(String fromWalletId, String toWalletId, int amount,
      String description) async {
    final response = await patch(
      '$apiUrl/pay',
      headers: {"Content-Type": "application/json"},
      body: json.encode({
        "FromWalletId": fromWalletId,
        "ToWalletId": toWalletId,
        "Amount": amount,
        "Description": description
      }),
    );

    if (response.statusCode == 200) {
      return response.statusCode;
    } else {
      throw Exception(response.body);
    }
  }

  Future<List<Transaction>> getTransactions(String walletId) async {
    final response = await get("$apiUrl/$walletId/transactions");

    if (response.statusCode == 200) {
      List<Transaction> transactions = [];
      List<dynamic> listTransactions =
          json.decode(response.body)["Transactions"];

      for (Map<String, dynamic> business in listTransactions) {
        transactions.add(Transaction.fromJson(business));
      }

      return transactions;
    } else {
      throw Exception('Failed to load transactions');
    }
  }
}
