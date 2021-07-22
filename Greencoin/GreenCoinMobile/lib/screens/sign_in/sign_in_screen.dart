import 'package:flutter/material.dart';
import 'package:flutter_form_bloc/flutter_form_bloc.dart';
import 'package:fyp/constants.dart';
import 'package:fyp/services/navigation_service.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:country_picker/country_picker.dart';

import '../../locator.dart';
import '../../routes_names.dart';
import 'login_fresh_loading.dart';
import 'user_login_form_bloc.dart';

class SignInScreen extends StatefulWidget {
  @override
  _SignInScreenState createState() => _SignInScreenState();
}

class _SignInScreenState extends State<SignInScreen> {
  TextEditingController _textEditingControllerPassword =
      TextEditingController();
  TextEditingController _textEditingControllerUser = TextEditingController();

  bool isNoVisiblePassword = false;

  bool isSubmitting = false;

  final focus = FocusNode();

  final bool isLoginRequest = false;

  String dialCode;
  final NavigationService _navigationService = locator<NavigationService>();
  final SharedPreferences prefs = locator<SharedPreferences>();

  // LoginFreshWords loginFreshWords;

  @override
  void initState() {
    // TODO: implement initState
    isSubmitting = false;
    dialCode = "+961";
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    // loginFreshWords = (widget.loginFreshWords == null)
    //     ? LoginFreshWords()
    //     : widget.loginFreshWords;

    return BlocProvider(
      create: (context) => UserInfoFormBloc(),
      child: Builder(builder: (context) {
        final formBloc = context.read<UserInfoFormBloc>();
        return Theme(
          data: ThemeData(
            // Define the default brightness and colors.
            primaryColor: kPrimaryColor,
          ),
          child: Scaffold(
            backgroundColor: kPrimaryColor,
            appBar: AppBar(
                leading: InkWell(
                    onTap: () => _navigationService.navPop(),
                    child: Icon(Icons.arrow_back)),
                iconTheme: IconThemeData(color: Colors.white),
                centerTitle: true,
                elevation: 0,
                title: Text(
                  "",
                  maxLines: 1,
                  overflow: TextOverflow.ellipsis,
                  style: TextStyle(
                      color: Colors.black54,
                      fontWeight: FontWeight.bold,
                      fontSize: 18),
                )),
            body: FormBlocListener<UserInfoFormBloc, String, String>(
              onSuccess: (context, state) {
                // showSuccessDialog();
                setState(() {
                  isSubmitting = false;
                });
              },
              onSubmissionFailed: (context, state) {
                setState(() {
                  isSubmitting = false;
                  // failure = false;
                });
              },
              onSubmissionCancelled: (context, state) {
                setState(() {
                  // failure = true;
                  isSubmitting = false;
                });
              },
              onFailure: (context, state) {
                // LoadingDialog.hide();
                setState(() {
                  isSubmitting = false;
                  // failure = false;
                });
                Scaffold.of(context).showSnackBar(
                    SnackBar(content: Text(state.failureResponse)));
              },
              child: SingleChildScrollView(
                child: Column(
                  children: <Widget>[
                    Align(
                      alignment: Alignment.topCenter,
                      child: Container(
                        height: MediaQuery.of(context).size.height * 0.25,
                        width: MediaQuery.of(context).size.width,
                        color: Color.fromRGBO(45, 206, 137, 1.0),
                        child: Padding(
                          padding: const EdgeInsets.symmetric(
                              horizontal: 50, vertical: 3),
                          child: Hero(
                            tag: 'hero-login',
                            child: Image.asset(
                              "assets/images/logo2.png",
                              fit: BoxFit.fitWidth,
                            ),
                          ),
                        ),
                      ),
                    ),
                    Align(
                      alignment: Alignment.bottomCenter,
                      child: Container(
                        height: MediaQuery.of(context).size.height * 0.75,
                        width: MediaQuery.of(context).size.width,
                        decoration: new BoxDecoration(
                            color: Color(0xFFF3F3F5),
                            borderRadius: new BorderRadius.only(
                              topLeft: const Radius.circular(50.0),
                              topRight: const Radius.circular(50.0),
                            )),
                        child: buildBody(formBloc),
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
        );
      }),
    );
  }

  Widget buildBody(UserInfoFormBloc formBloc) {
    return Center(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        children: <Widget>[
          // GestureDetector(
          //   child: Padding(
          //     padding: const EdgeInsets.only(top: 25, left: 10, right: 10),
          //     child: RichText(
          //       textAlign: TextAlign.center,
          //       text: TextSpan(children: [
          //         TextSpan(
          //             text: '',
          //             style: TextStyle(
          //                 color: Color(0xFF0F2E48),
          //                 fontWeight: FontWeight.normal,
          //                 fontSize: 15)),
          //         TextSpan(
          //             text: "Recover Password",
          //             style: TextStyle(
          //                 decoration: TextDecoration.underline,
          //                 color: Color(0xFF0F2E48),
          //                 fontWeight: FontWeight.bold,
          //                 fontSize: 16)),
          //       ]),
          //     ),
          //   ),
          //   onTap: () {
          //     // TODO: handle recover
          //   },
          // ),
          SizedBox(
            height: 30,
          ),
          Padding(
            padding: const EdgeInsets.symmetric(vertical: 15, horizontal: 10),
            child: RichText(
              textAlign: TextAlign.center,
              text: TextSpan(children: [
                TextSpan(
                    text: "Continue with your phone number" + ' \n',
                    style: TextStyle(
                        color: Color(0xFF0F2E48),
                        fontWeight: FontWeight.normal,
                        fontSize: 19)),
                // TextSpan(
                //     text: "Sign Up",
                //     style: TextStyle(
                //         decoration: TextDecoration.underline,
                //         color: Color(0xFF0F2E48),
                //         fontWeight: FontWeight.bold,
                //         fontSize: 16)),
              ]),
            ),
          ),
          Padding(
            padding: const EdgeInsets.symmetric(horizontal: 20),
            child: DropdownFieldBlocBuilder(
                selectFieldBloc: formBloc.userType,
                itemBuilder: (context, String value) => getArabicString(
                    value[0].toUpperCase() + value.substring(1), false),
                decoration: InputDecoration(
                  labelText: getArabicString('Who are you?', false),
                  prefixIcon: Padding(
                      padding: const EdgeInsets.all(8.0),
                      child: Icon(Icons.badge)),
                  enabledBorder: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(25),
                      borderSide: BorderSide(color: Color(0xFFAAB5C3))),
                  filled: true,
                  fillColor: Color(0xFFF3F3F5),
                  focusColor: Color(0xFFF3F3F5),
                  focusedBorder: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(25),
                      borderSide: BorderSide(color: Color(0xFFAAB5C3))),
                  border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(25),
                      borderSide:
                          BorderSide(color: Color.fromRGBO(45, 206, 137, 1.0))),
                )),
          ),

          Padding(
            padding: const EdgeInsets.symmetric(horizontal: 20),
            child: TextFieldBlocBuilder(
                textFieldBloc: formBloc.phoneNumber,
                // controller: this._textEditingControllerUser,
                keyboardType: TextInputType.phone,
                style: TextStyle(color: Color(0xFF0F2E48), fontSize: 18),
                autofocus: false,
                onSubmitted: (v) {
                  FocusScope.of(context).requestFocus(focus);
                },
                decoration: InputDecoration(
                    prefix: InkWell(
                      child: RichText(
                        text: TextSpan(
                          children: [
                            WidgetSpan(
                              child: Icon(
                                Icons.edit,
                                size: 22,
                                color: Colors.black54,
                              ),
                            ),
                            TextSpan(
                                text: " $dialCode",
                                style: TextStyle(
                                    color: Colors.black54, fontSize: 18)),
                          ],
                        ),
                      ),
                      onTap: () {
                        showCountryPicker(
                            showPhoneCode: true,
                            context: context,
                            exclude: ["IL"],
                            onSelect: (country) {
                              setState(() {
                                dialCode = "+${country.phoneCode}";
                              });
                            });
                      },
                    ),
                    suffixIcon: Padding(
                        padding: const EdgeInsets.all(8.0),
                        child: Icon(Icons.phone_android)),
                    enabledBorder: OutlineInputBorder(
                        borderRadius: BorderRadius.circular(25),
                        borderSide: BorderSide(color: Color(0xFFAAB5C3))),
                    filled: true,
                    fillColor: Color(0xFFF3F3F5),
                    focusColor: Color(0xFFF3F3F5),
                    focusedBorder: OutlineInputBorder(
                        borderRadius: BorderRadius.circular(25),
                        borderSide: BorderSide(color: Color(0xFFAAB5C3))),
                    border: OutlineInputBorder(
                        borderRadius: BorderRadius.circular(25),
                        borderSide: BorderSide(
                            color: Color.fromRGBO(45, 206, 137, 1.0))),
                    hintText: "Phone Number")),
          ),
          isSubmitting
              ? Padding(
                  padding: const EdgeInsets.all(8.0),
                  child: LoadingLoginFresh(
                    textLoading: "Loading",
                    colorText: Colors.black,
                    backgroundColor: kPrimaryColor,
                    elevation: 0,
                  ),
                )
              : GestureDetector(
                  onTap: () {
                    setState(() {
                      isSubmitting = true;
                    });
                    formBloc.dialCode = dialCode;
                    formBloc.submit();
                  },
                  child: SizedBox(
                      height: MediaQuery.of(context).size.height * 0.07,
                      width: MediaQuery.of(context).size.width * 0.7,
                      child: Card(
                          elevation: 10,
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(40),
                          ),
                          color: Color.fromRGBO(45, 206, 137, 1.0),
                          child: Padding(
                            padding: const EdgeInsets.symmetric(horizontal: 10),
                            child: Center(
                              child: Text(
                                "Continue",
                                style: TextStyle(
                                    color: Colors.white,
                                    fontSize: 15,
                                    fontWeight: FontWeight.bold),
                              ),
                            ),
                          ))),
                ),
        ],
      ),
    );
  }

  void setIsRequest(bool isSubmitting) {
    setState(() {
      this.isSubmitting = isSubmitting;
    });
  }
}
