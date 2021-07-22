
using HumanityService.DataContracts.CompositeDesignPattern;
using HumanityService.Exceptions;
using HumanityService.Services.Interfaces;
using HumanityService.Stores.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HumanityService.Services
{
    public class UserService : IUserService
    {

        private readonly IUserStore _userStore;

        public UserService(IUserStore userStore)
        {
            _userStore = userStore;
        }

        public async Task AddNgo(Ngo ngo)
        {
            IsValidNgo(ngo);
            await _userStore.AddNgo(ngo);
        }

        public async Task AddUser(User user)
        {
            IsValidUser(user);
            await _userStore.AddUser(user);
        }

        public async Task DeleteNgo(string ngoUsername)
        {
            await _userStore.DeleteNgo(ngoUsername);
        }

        public async Task DeleteUser(string username)
        {
            await _userStore.DeleteUser(username);
        }

        public async Task<Ngo> GetNgo(string ngoUsername)
        {
            var ngo = await _userStore.GetNgo(ngoUsername);
            return ngo;
        }

        public async Task<User> GetUser(string username)
        {
            var user = await _userStore.GetUser(username);
            return user;
        }

        public async Task UpdateNgo(Ngo ngo)
        {
            IsValidNgo(ngo);
            await _userStore.UpdateNgo(ngo);
        }

        public async Task UpdateUser(User user)
        {
            IsValidUser(user);
            await _userStore.UpdateUser(user);
        }

        private static void IsValidNgo(Ngo user)
        {
            if (!(IsValidString(user.Username)
                 && IsValidString(user.Name)
                 && IsValidString(user.RegistrationNumber)
                 && IsValidString(user.Password)
                 && IsValidString(user.PhoneNumber)
                 && IsValidString(user.Email)
                 && IsValidString(user.Location.Description)
                 && IsValidCoordinates(user.Location.Longitude, user.Location.Latitude)))
            {
                throw new BadRequestException("One of the required fields is empty");
            }

            else if (!IsValidEmail(user.Email))
            {
                throw new BadRequestException($"The Email {user.Email} is not a valid Email");
            }

            else if (!IsValidPassword(user.Password))
            {
                throw new BadRequestException("The Password is not valid, it should contain a number, a capital letter, and has a minimum of 8 characters");
            }

            else if (!IsLebanesePhoneNumber(user.PhoneNumber))
            {
                throw new BadRequestException("The phone number is incorrect, it should contain 8 digits");
            }
        }

        private static bool IsValidCoordinates(double longitude, double latitude)
        {
            return longitude >= -180 && longitude <= 180 && latitude >= -90 && latitude <= 90;
        }

        private static void IsValidUser(User user)
        {
            if (!(IsValidString(user.Username)
                 && IsValidString(user.FirstName)
                 && IsValidString(user.LastName)
                 && IsValidString(user.Password)
                 && IsValidString(user.PhoneNumber)
                 && IsValidString(user.Email)
                 && IsValidCoordinates(user.Location.Longitude, user.Location.Latitude)
                 && IsValidString(user.Location.Description)))
            {
                throw new BadRequestException("One of the required fields is empty");
            }

            else if (!IsValidEmail(user.Email))
            {
                throw new BadRequestException($"The Email {user.Email} is not a valid Email");
            }

            else if (!IsValidPassword(user.Password))
            {
                throw new BadRequestException("The Password is not valid, it should contain a number, a capital letter, and has a minimum of 8 characters");
            }

            else if (!IsLebanesePhoneNumber(user.PhoneNumber))
            {
                throw new BadRequestException("The phone number is incorrect, it should contain 8 digits");
            }
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            var emailValidator = new EmailAddressAttribute();
            return emailValidator.IsValid(email);
        }

        public static bool IsValidPassword(string plainText)
        {
            var hasNumber = CompiledRegex(@"[0-9]+");
            var hasUpperChar = CompiledRegex(@"[A-Z]+");
            var hasMinimum8Chars = CompiledRegex(@".{8,}");
            return hasNumber.IsMatch(plainText) && hasUpperChar.IsMatch(plainText) && hasMinimum8Chars.IsMatch(plainText);
        }

        public static bool IsLebanesePhoneNumber(string number)
        {
            var isLebanesePhone = CompiledRegex(@"^[0-9]{8}$");
            return isLebanesePhone.IsMatch(number);
        }

        private static Regex CompiledRegex(string regExp)
        {
            return new Regex(regExp, RegexOptions.Compiled);
        }

        public static bool IsValidString(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}
