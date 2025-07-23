using Domain.ViewModel.BusinessCardViewModel;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain.Common
{
    public class CommonClass
    {
        public class ResponseStandardJsonApi
        {
            public bool Success { get; set; }
            public int Code { get; set; }
            public object? Message { get; set; } 
            public object? Result { get; set; } 
        }
        public class ResponseStandardJson<T>
        {
            public bool Success { get; set; }
            public int Code { get; set; }
            public object? Message { get; set; } 
            public T? Result { get; set; } 
        }
        public class NullColumns
        {

        }
        public class JwtAuthResponse
        {
            public string? nameid { get; set; }
            public string? given_name { get; set; }
            public string? jti { get; set; }
        }
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
        public class BusinessCardValidator
        {
            private const int MaxNameLength = 100;
            private const int MaxTitleLength = 100;
            private const int MaxPhoneLength = 20;
            private const int MaxEmailLength = 256;
            private const int MaxCompanyLength = 150;
            private const int MaxWebsiteLength = 2083;
            private const int MaxAddressLength = 300;
            private const int MaxNotesLength = 500;
            private static readonly Regex PhoneRegex = new(@"^[\d\s\+\-\(\)]+$", RegexOptions.Compiled);
            private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            public List<string> Validate(BusinessCardExcelModelCreate card)
            {
                var errors = new List<string>();

                if (string.IsNullOrWhiteSpace(card.BusinessCardName))
                    errors.Add("Business Card Name is required.");

                if (string.IsNullOrWhiteSpace(card.BusinessCardTitle))
                    errors.Add("Business Card Title is required.");

                if (string.IsNullOrWhiteSpace(card.BusinessCardPhone))
                    errors.Add("Business Card Phone is required.");

                if (string.IsNullOrWhiteSpace(card.BusinessCardEmail))
                    errors.Add("Business Card Email is required.");

                if (string.IsNullOrWhiteSpace(card.BusinessCardCompany))
                    errors.Add("Business Card Company is required.");

                if (string.IsNullOrWhiteSpace(card.BusinessCardWebsite))
                    errors.Add("Business Card Website is required.");

                if (string.IsNullOrWhiteSpace(card.BusinessCardAddress))
                    errors.Add("Business Card Address is required.");

                if (!string.IsNullOrWhiteSpace(card.BusinessCardName) && card.BusinessCardName.Length > MaxNameLength)
                    errors.Add($"Business Card Name cannot exceed {MaxNameLength} characters.");

                if (!string.IsNullOrWhiteSpace(card.BusinessCardTitle) && card.BusinessCardTitle.Length > MaxTitleLength)
                    errors.Add($"Business Card Title cannot exceed {MaxTitleLength} characters.");

                if (!string.IsNullOrWhiteSpace(card.BusinessCardPhone) && card.BusinessCardPhone.Length > MaxPhoneLength)
                    errors.Add($"Business Card Phone cannot exceed {MaxPhoneLength} characters.");

                if (!string.IsNullOrWhiteSpace(card.BusinessCardEmail) && card.BusinessCardEmail.Length > MaxEmailLength)
                    errors.Add($"Business Card Email cannot exceed {MaxEmailLength} characters.");

                if (!string.IsNullOrWhiteSpace(card.BusinessCardCompany) && card.BusinessCardCompany.Length > MaxCompanyLength)
                    errors.Add($"Business Card Company cannot exceed {MaxCompanyLength} characters.");

                if (!string.IsNullOrWhiteSpace(card.BusinessCardWebsite) && card.BusinessCardWebsite.Length > MaxWebsiteLength)
                    errors.Add($"Business Card Website URL cannot exceed {MaxWebsiteLength} characters.");

                if (!string.IsNullOrWhiteSpace(card.BusinessCardAddress) && card.BusinessCardAddress.Length > MaxAddressLength)
                    errors.Add($"Business Card Address cannot exceed {MaxAddressLength} characters.");

                if (!string.IsNullOrWhiteSpace(card.BusinessCardNotes) && card.BusinessCardNotes.Length > MaxNotesLength)
                    errors.Add($"Business Card Notes cannot exceed {MaxNotesLength} characters.");

                if (!string.IsNullOrWhiteSpace(card.BusinessCardEmail) && !EmailRegex.IsMatch(card.BusinessCardEmail))
                    errors.Add("Business Card Email is invalid.");

                if (!string.IsNullOrWhiteSpace(card.BusinessCardPhone) && !PhoneRegex.IsMatch(card.BusinessCardPhone))
                    errors.Add("Business Card Phone is invalid.");

                return errors;
            }
        }
        public class HeaderValidationResult
        {
            public bool IsValid { get; set; }
            public List<string> MissingHeaders { get; set; } = new();
            public string Message { get; set; } = string.Empty;
        }
    }
}
