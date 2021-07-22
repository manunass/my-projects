import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:fyp/routes_names.dart';
import 'package:fyp/services/navigation_service.dart';
import 'package:shared_preferences/shared_preferences.dart';

import 'controllers/business_controller.dart';
import 'controllers/employee_controller.dart';
import 'controllers/user_controller.dart';
import 'locator.dart';
import 'models/User.dart';
import 'size_config.dart';

final themeColor = Color(0xfff5a623);
final primaryColor = Color(0xff203152);
final greyColor = Color(0xffaeaeae);
final greyColor2 = Color(0xffE8E8E8);

const kPrimaryColor = Color.fromRGBO(45, 206, 137, 1.0);
const kPrimaryLightColor = Color(0xFFFFECDF);
const kPrimaryGradientColor = LinearGradient(
  begin: Alignment.topLeft,
  end: Alignment.bottomRight,
  colors: [Color(0xFFFFA53E), Color(0xFF00A651)],
);
const kSecondaryColor = Color(0xFF979797);
const kTextColor = Color(0xFF757575);

const kAnimationDuration = Duration(milliseconds: 200);

final headingStyle = TextStyle(
  fontSize: getProportionateScreenWidth(28),
  fontWeight: FontWeight.bold,
  color: Colors.black,
  height: 1.5,
);

const defaultDuration = Duration(milliseconds: 250);

// Form Error
final RegExp emailValidatorRegExp =
    RegExp(r"^[a-zA-Z0-9.]+@[a-zA-Z0-9]+\.[a-zA-Z]+");
String kEmailNullError =
    arabic ? "الرجاء إدخال رقم الهاتف" : "Please Enter your phone number";
const String kInvalidEmailError = "Please Enter Valid Email";
const String kPassNullError = "Please Enter your password";
const String kShortPassError = "Password is too short";
const String kMatchPassError = "Passwords don't match";
const String kNamelNullError = "Please Enter your name";
const String kPhoneNumberNullError = "Please Enter your phone number";
const String kAddressNullError = "Please Enter your address";
const String unknown_image =
    "https://firebasestorage.googleapis.com/v0/b/la-tarmi.appspot.com/o/profile_images%2Fanonymous-user.png?alt=media&token=b85198da-a83f-4f09-8df7-a722c0d84bc7";
final otpInputDecoration = InputDecoration(
  contentPadding:
      EdgeInsets.symmetric(vertical: getProportionateScreenWidth(15)),
  border: outlineInputBorder(),
  focusedBorder: outlineInputBorder(),
  enabledBorder: outlineInputBorder(),
);

OutlineInputBorder outlineInputBorder() {
  return OutlineInputBorder(
    borderRadius: BorderRadius.circular(getProportionateScreenWidth(15)),
    borderSide: BorderSide(color: kTextColor),
  );
}

bool arabic = false;
bool chosen = false;

String globalUserId;
String globalUserName;
ValueNotifier<int> unReadMessagesChanged;

final SharedPreferences _prefs = locator<SharedPreferences>();
final NavigationService _navigationService = locator<NavigationService>();

getLanguage() {
  arabic = _prefs.getBool("arabic") ?? false;
}

getGlobalUserId() {
  globalUserId = _prefs.getString("user_id");
}

getGlobalUserName() {
  globalUserName = _prefs.getString("name");
}

String getArabicString(String text, bool opposite) {
  if (!arabic) if (!opposite)
    return text;
  else
    return toArabic[text];
  else if (!opposite)
    return toArabic[text];
  else
    return text;
}

const Map<String, String> toArabic = {
  "Profile": "حسابي",
  "My Account": "الحساب الشخصي",
  "My Posts": "المنشورات الشخصية",
  "Help Center": "مساعدة",
  "Change to English": "التغيير إلى العربية",
  "Log Out": "تسجيــل الخروج",
  "Alert": "إنذار",
  "Are you sure you want to log Out from your account?":
      "هل أنت متأكد أنك تريد تسجيل الخروج من حسابك؟",
  "Cancel": "إلغاء",
  "User Details": "بيانات المستخدم",
  "Profile picture": "الصور الشخصية",
  "Name": "الإسم",
  "Male": "ذكر",
  "Female": "أنثى",
  "Gender": "الجنس",
  "Date of Birth": "تاريخ الميلاد",
  "Choose your region": "إختيار المحافظة",
  "Save": "حفظ",
  "Photo Library": "مكتبة الصور",
  "Camera": "الكاميرا",
  "Are you sure you want to change the language to english?":
      "هل أنت متأكد أنك تريد تغيير اللّغة إلى العربية؟",
  "Change": "قم بالتغيير",
  "Saved Posts": "المنشورات المحفّظة",
  "You have no internet connection.\n Swipe to refresh":
      " إنك غير متصل بشبكة الإنترنت",
  "There are no items in this section yet.":
      "لا يوجد منشورات في هذا القسم حتى الآن",
  'New Post': "إضافة منشور",
  'Add New Post': "إضافة منشور جديد",
  "Categories": "التصنيفات",
  "Free Items": "أغراض مجانية",
  "Pets Adoption": "تبني الحيوانات الأليفة",
  "Lost & Found": "الموجودات و المفقودات",
  "Latest Posts": "آخر المنشورات",
  "Unable to load Ads": "يوجد مشكلة في عرض الإعلانات",
  "Just now": "في هذه اللحظة",
  "min ago": "دقائق مضت",
  "hrs ago": "ساعات مضت",
  "days ago": "أيام مضت",
  "months ago": "أشهر مضت",
  "years ago": "سنوات مضت",
  "Post is Saved": "تم حفظ المنشور",
  "Post is Unsaved": "تم إلغاء حفظ المنشور",
  "Offered Items": "منتجات معروضة",
  "Requested Items": "منتجات مطلوبة",
  "Offered Pets": "حيوانات أليفة معروضة",
  "Requested Pets": "حيوانات أليفة مطلوبة",
  "Chat List": "المحادثات",
  "View post ": "عرض المنشور",
  "Seems like this post was deleted": "يظهر بأن المنشور قد حذف",
  "Pick Image error": "يوجد مشكلة في عرض الصورة",
  "Chat": "إبدأ المحادثة",
  "Send a message...": "...أرسل رسالة",
  'Write a message': "اكتب الرسالة",
  "You can then continue to chat in the chats section":
      "يمكنك متابعة المحادثة في صفحة المحادثات",
  "Message...": "...الرسالة",
  "Send": "إرسال",
  "Delete": "حذف",
  "Confirm Delete": "تأكيد الحذف",
  'Are you Sure you want to delete this post?':
      "هل انت متأكد انك تريد حذف هذا المنشور؟",
  'Delete Post': "حذف المنشور",
  "Post marked as Found": "تم وضع علامة 'وجد' على المنشور",
  "Post marked as Taken": "تم وضع علامة 'مأخوذ' على المنشور",
  "Post marked as Satisfied": "تم وضع علامة 'تم' على المنشور",
  "Post marked as Done": "تم وضع علامة 'منتهي' على المنشور",
  "Post marked as Adopted": "تم وضع علامة 'تم التبنّي' على المنشور",
  "Found mark is removed": "تم إزالة علامة 'وجد' على المنشور",
  "Taken mark is removed": "تم إزالة علامة 'مأخوذ' على المنشور",
  "Satisfied mark is removed": "تم إزالة علامة 'تم' على المنشور",
  "Done mark is removed": "تم إزالة علامة 'منتهي' على المنشور",
  "Adopted mark is removed": "تم وضع إزالة 'تم التبنّي' على المنشور",
  "Mark as Found": "'وضع علامة 'وجد",
  "Mark as Taken": "'وضع علامة 'مأخوذ",
  "Mark as Satisfied": "'وضع علامة 'تم",
  "Mark as Done": "'وضع علامة 'منتهي",
  "Mark as Adopted": "'وضع علامة 'تم التبنّي",
  "Location: ": "المكان: ",
  "Posted by: ": ":صاحب المنشور",
  "Show less": "  إظهار معلومات أقل  ",
  "Show More Details": "  إظهار المزيد ",
  "You don't have an chats yet.": "ليس لديك اي محادثات حاليا",
  "You have no saved posts.": "ليس لديك اي منشورات محفّظة",
  "Continue with a method of your choice": "تسجيل الدخول بطريقة من إختيارك",
  "Signing you in...": "جاري عملية الدخول",
  "Check your internet connection": "يوجد مشكلة بالإتصال بالإنترنت",
  "Back to Language Selection": "الرجوع إلى إختيار اللّغة",
  "Phone": "الهاتف",
  "Enter your phone number": "...إدخال رقم الهاتف",
  "Continue": "تسجيل الدخول",
  'Confirm Number': "تأكيد رقم الهاتف",
  "Confirm sending the code to": "تأكيد إرسال الكود إلى",
  'Code Verification': "التحقق من الرمز",
  "Enter the code sent to": "أدخل الرمز المرسل إلى ",
  "Verifying and signing you in": "التحقق من الرمز و تسجيل الدخول",
  'Invalid Code': "الرمز غير صحيح",
  "Resend Code": "إرسال الكود مرّة أخرى",
  "This code will expired in:\n ": "ستنتهي صلاحية الرمز بعد:\n",
  "seconds": "ثواني",
  "Please Select Your Country": "الرجاء إختيار البلد",
  "Title": "العنوان",
  'Description': "الوصف",
  'General': "أغراض مجانية",
  'Category:': "التصنيف",
  "Requesting an item": "طلب",
  'Offering an item': "تقديم",
  'Are you requesting or offering an item?': "هل تطلب أو تقدم شيئ؟",
  "Lost": "مفقود",
  "Found": "موجود",
  "Lost or Found?": "مفقود أو موجود؟",
  'Choose at least one photo': "اختر صورة واحدة على الأقل",
  " Add photos": "  إضافة صور",
  'Fill the post details': "أدخل تفاصيل المنشور",
  'SUBMIT': "نشر",
  'Post added.': "تم النشر.",
  'Go Home': "الذهاب إلى الصفحة الرئيسية",
  'Sent you a message.': "أرسل لك رسالة",
  'Sent you a photo.': "أرسل لك صورة",
  'Sent you a post.': "أرسل لك منشور",
  'Home': "الرئيسية",
  'Saves': "المحفوظ",
  'Chats': "المحادثات",
  "  Ideas": "أفكار  ",
  '     This field is required': "     هذه الخانة مطلوبة",
  '     Must have at least 4 characters':
      "     يجب أن تحتوي هذه الخانة على 4 أحرف على الأقل",
  '      Must have at least 20 characters':
      "      يجب أن تحتوي هذه الخانة على 20 أحرف على الأقل",
  '      Must have at most 20 characters':
      "      يجب أن تحتوي هذه الخانة على 20 أحرف على الأكثر",
  "Posts:": "المنشورات:",
  "How to add post?": "كيف تضيف منشور؟",
  "How to view your posts?": "كيف ترى منشوراتك؟",
  "How to delete your post? ": "كيف تحذف منشورك؟ ",
  "How to save a post?": " كيف تحفّظ المنشور؟",
  "Where can I find saved posts? ": "أين يمكنني أن أجد المنشورات المحفّظة؟",
  "Account:": ":الحساب الشخصي",
  "How to change account info?": "كيف تغير معلومات الحساب؟",
  "How to change profile picture?": "كيف تغيّر صورة الحساب الشخصي؟",
  "How to log out?": "كيف تسجل الخروج؟",
  "Chat:": "المحادثات:",
  "How to contact a user?": "كيف تتواصل مع الآخرين؟",
  "Where chats are found?": "أين توجد المحادثات ؟",
  "Can I send a photo in a chat?": "هل يمكنني إرسال صورة في المحادثة؟",
  "Click on \"New Post\" button. Enter required fields and add photos to your post.":
      'انقر على زر "+أضف منشور". أدخل المعلومات المطلوبة وأضف الصور إلى منشورك.',
  "Go to Account page, click on \"My Posts\" to view all posts you added.":
      'انتقل إلى صفحة الحساب ، وانقر على "منشوراتي" لعرض جميع المنشورات التي أضفتها. ',
  "Go to your post page, click \"Delete\" button.":
      'انتقل إلى صفحة المنشور الخاصة بك ، وانقر على الزر "حذف"',
  "Click on \"Heart\" button that is found on Post page or next to the post on lists page.":
      'نقر على زر "اعجاب" الموجود في صفحة النشر أو بجوار المنشور في صفحة القوائم.',
  "Saved posts can be viewed by clicking \"heart\" button found on navigation bar.":
      'يمكنك رؤية المنشورات المحفّطة عن طريق النقر على زر "اعجاب" الموجود على شريط التنقل.',
  "Go to Account page using navigation bar, click on \"My Account\" button.":
      'انتقل إلى صفحة الحساب باستخدام شريط التنقل ، وانقرعلى زر "حسابي".',
  "Go to Account page using navigation bar, click on \"My Account\" button. Tap camera icon and choose a photo.":
      'انتقل إلى صفحة الحساب باستخدام شريط التنقل ، وانقر على زر "حسابي". اضغط على أيقونة الكاميرا واختر صورة.',
  "Go to Account page using navigation bar, click on \"Log Out\" button.":
      'انتقل إلى صفحة الحساب باستخدام شريط التنقل ، وانقر على زر  "تسجيل الخروج".',
  "Go to Post page, click on \"Chat\" button and send a message.":
      'انتقل إلى صفحة النشر ، وانقر على زر "محادثة الآن" وأرسل رسالة',
  "Go to Chats page by clicking the chat icon on navigation bar. All chats are viewed sorted by date.":
      'انتقل إلى صفحة المحادثات بالنقر على أيقونة الدردشة في شريط التنقل. يتم عرض جميع المحادثات، مرتّبة حسب التاريخ.',
  "Yes, just click on the image button in a chat and choose photo.":
      "نعم ، اضغط على زر الصورة في الدردشة واختر الصورة، ثم ارسل."
};

// showAlert() {
//   showDialog<void>(
//     context: context1,
//     barrierDismissible: false, // user must tap button!
//     builder: (BuildContext context) {
//       return WillPopScope(
//         onWillPop: () async => false,
//         child: AlertDialog(
//           title: Text(''),
//           content: SingleChildScrollView(
//             child: ListBody(
//               children: <Widget>[
//                 Icon(
//                   Icons.assignment_turned_in,
//                   size: 50,
//                   color: kPrimaryColor,
//                 ),
//                 SizedBox(height: 10),
//                 Text(
//                   getArabicString(
//                       'Your request is submitted you can check it in transactions section',
//                       false),
//                   style: TextStyle(fontSize: 16, color: Colors.black),
//                   textAlign: TextAlign.center,
//                 ),
//               ],
//             ),
//           ),
//           actions: <Widget>[
//             TextButton(
//               child: Text(
//                 getArabicString('Okay', false),
//                 style: TextStyle(color: kPrimaryColor),
//               ),
//               onPressed: () {
//                 _navigationService.navPop();
//               },
//             ),
//           ],
//         ),
//       );
//     },
//   );
// }

showDialogWindow(String msg, bool dismiss, BuildContext context1) {
  return showDialog(
      barrierDismissible: dismiss,
      context: context1,
      builder: (context) {
        // return dismiss
        //     ? WillPopScope(
        //         onWillPop: () async => dismiss,
        //         child: AlertDialog(
        //           content: Text(msg),
        //         ),
        //       )
        //     :
        return WillPopScope(
            onWillPop: () => _navigationService.navPop(),
            child: AlertDialog(
              content: SingleChildScrollView(
                child: ListBody(
                  children: [
                    Text(msg),
                    SizedBox(
                      height: 20,
                    ),
                    SizedBox(
                      width: 50,
                      height: 10,
                      child: LinearProgressIndicator(
                        backgroundColor: Colors.white,
                        valueColor:
                            new AlwaysStoppedAnimation<Color>(kPrimaryColor),
                      ),
                    )
                  ],
                ),
              ),
            ));
      });
}

hideDialog() {
  _navigationService.navPop();
}

Future<dynamic> getUserInfo() async {
  final SharedPreferences prefs = locator<SharedPreferences>();

  String id = prefs.getString('user_id');
  String userType = prefs.getString('user_type');
  String municipalityId = prefs.getString('municipality_id');

  if (userType == 'employee') {
    EmployeeController().getEmployeeById(id).then((value) {
      if (value != null) {
        prefs.setString("employee_object", jsonEncode(value));
        prefs.remove("user_type_temp");
        prefs.setString("user_type", 'employee');
        prefs.setString('user_id', value.id);
        prefs.setString('municipality_id', '${value.municipalityId}');
        // _navigationService.replaceAndClearNav(MainNavRoute);
      }
    });
  } else if (userType == 'business') {
    BusinessController().getBusinessById(id).then((value) {
      if (value != null) {
        prefs.setString("business_object", jsonEncode(value));
        prefs.remove("user_type_temp");
        prefs.setString("user_type", 'business');
        prefs.setString('user_id', value.id);
        prefs.setString('municipality_id', '${value.municipalityId}');
        // _navigationService.replaceAndClearNav(MainNavRoute);
      }
    });
  } else if (userType == 'user') {
    User user = await UserController().getUserById(id);
    if (user != null) {
      prefs.setString("user_object", jsonEncode(user));
      prefs.remove("user_type_temp");
      prefs.setString("user_type", 'user');
      prefs.setString('user_id', user.id);
      prefs.setString('municipality_id', '${user.municipalityId}');
      // _navigationService.replaceAndClearNav(MainNavRoute);=
      return user;
    }
  } else {
    throw Exception('Cant fetch user');
  }

  globalUserId = id;
}
