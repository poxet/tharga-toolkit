using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.LocalStorage.Entity
{
    public abstract class EntityBaseWithValidation : EntityBase, IDataErrorInfo, IEntityWithValidation
    {
        private readonly List<string> _validatedProperties = new List<string>();
        public bool IsValid { get { return _validatedProperties.All(property => GetValidationError(this, property) == null); } }

        protected EntityBaseWithValidation()
        {
            _validatedProperties = GetPropertiesToValidate(this).ToList();
        }

        public string this[string columnName] { get { return GetValidationError(this, columnName); } }
        public string Error
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var property in _validatedProperties)
                {
                    var error = GetValidationError(this, property);
                    if (error != null)
                        sb.AppendLine(error);
                }
                return sb.ToString().TrimEnd(System.Environment.NewLine.ToCharArray());
            }
        }

        private static string GetValidationError(object obj, string propertyName)
        {
            var field = obj.GetType().GetProperty(propertyName);
            var vh = (ValidatorAttribute)field.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(ValidatorAttribute));

            if (null != vh)
            {
                switch (vh.Type)
                {
                    case ValidatorAttribute.ValidatorType.MissingString:
                        return ValidateMissingString(propertyName, field.GetValue(obj, null) as string);
                    case ValidatorAttribute.ValidatorType.MissingInteger:
                        return ValidateMissingInteger(propertyName, (int)field.GetValue(obj, null));
                    case ValidatorAttribute.ValidatorType.MissingDateTime:
                        return ValidateDateTime(propertyName, (DateTime)field.GetValue(obj, null));
                    case ValidatorAttribute.ValidatorType.MissingNullableDateTime:
                        return ValidateNullableDateTime(propertyName, (DateTime?)field.GetValue(obj, null));
                    case ValidatorAttribute.ValidatorType.MissingGuid:
                        return ValidateMissingGuid(propertyName, (Guid)field.GetValue(obj, null));
                    case ValidatorAttribute.ValidatorType.PhoneNumberMandatory:
                        return ValidatePhoneNumber(propertyName, field.GetValue(obj, null) as string, true);
                    case ValidatorAttribute.ValidatorType.PhoneNumberOptional:
                        return ValidatePhoneNumber(propertyName, field.GetValue(obj, null) as string, false);
                    case ValidatorAttribute.ValidatorType.EMailMandatory:
                        return ValidateEmail(propertyName, field.GetValue(obj, null) as string, true);
                    case ValidatorAttribute.ValidatorType.EMailOptional:
                        return ValidateEmail(propertyName, field.GetValue(obj, null) as string, false);
                    case ValidatorAttribute.ValidatorType.StreetNameMandatory:
                        return ValidateStreetName(propertyName, field.GetValue(obj, null) as string, true);
                    case ValidatorAttribute.ValidatorType.PostalCodeMandatory:
                        return ValidatePostalCode(propertyName, field.GetValue(obj, null) as string, true);
                    case ValidatorAttribute.ValidatorType.CityMandatory:
                        return ValidateCity(propertyName, field.GetValue(obj, null) as string, true);
                    case ValidatorAttribute.ValidatorType.OrgNoOptional:
                        return ValidateOrgNo(propertyName, field.GetValue(obj, null) as string, false);
                    case ValidatorAttribute.ValidatorType.VatNumberOptional:
                        return ValidateVatNumber(propertyName, field.GetValue(obj, null) as string, false);
                    case ValidatorAttribute.ValidatorType.OptionalObject:
                        return ValidateObject(propertyName, field.GetValue(obj, null), false);
                    default:
                        throw new ArgumentOutOfRangeException(string.Format("ValidatorType {0} is unknown", vh.Type));
                }
            }

            return null;
        }

        private static string ValidateMissingString(string propertyName, string value)
        {
            return value.IsNullOrEmpty() ? string.Format("{0}Missing", propertyName) : null;
        }

        private static string ValidateMissingInteger(string propertyName, int value)
        {
            return value == 0 ? string.Format("{0}Missing", propertyName) : null;
        }

        private static string ValidateDateTime(string propertyName, DateTime value)
        {
            return value == new DateTime() ? string.Format("{0}Missing", propertyName) : null;
        }

        private static string ValidateNullableDateTime(string propertyName, DateTime? value)
        {
            return value == null ? string.Format("{0}Missing", propertyName) : null;
        }

        private static string ValidateMissingGuid(string propertyName, Guid getValue)
        {
            return getValue == Guid.Empty ? string.Format("{0}Missing", propertyName) : null;
        }

        private static string ValidatePattern(string propertyName, string value, bool mandatory, string pattern)
        {
            if (string.IsNullOrEmpty(value))
                return mandatory ? ValidateMissingString(propertyName, value) : null;

            return !Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase)
                       ? string.Format("{0}Invalid", propertyName)
                       : null;
        }

        private static string ValidatePhoneNumber(string propertyName, string phoneNumber, bool mandatory)
        {
            const string pattern = @"^[+]{0,1}[0-9 -]{4,}$";
            return ValidatePattern(propertyName, phoneNumber, mandatory, pattern);
        }

        private static string ValidateEmail(string propertyName, string email, bool mandatory)
        {
            // This regex pattern came from: http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
            const string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            return ValidatePattern(propertyName, email, mandatory, pattern);
        }

        private static string ValidateStreetName(string propertyName, string streetName, bool mandatory)
        {
            //INVALID PATTERN!
            //const string pattern = @"\d{1,3}.?\d{0,3}\s[a-zA-Z]{2,30}(\s[a-zA-Z]{2,15})?([#\.0-9a-zA-Z]*)?";
            const string pattern = @"^[a-zA-ZåäöÅÄÖ ]{3,30}[0-9a-zA-ZåäöÅÄÖ ]{0,5}$";
            return ValidatePattern(propertyName, streetName, mandatory, pattern);
        }

        private static string ValidatePostalCode(string propertyName, string streetName, bool mandatory)
        {
            //ALLOWS "14265"
            //DOES NOT ALLOW "142 65"
            //const string pattern = @"^(\d{5}-\d{4}|\d{5}|\d{9})$"; //TODO: Move regexp into settings so that rules can be changed without a new release.
            const string pattern = @"^[-0-9 ]{3,6}$";
            return ValidatePattern(propertyName, streetName, mandatory, pattern);
        }

        private static string ValidateCity(string propertyName, string streetName, bool mandatory)
        {
            const string pattern = @"^[-a-zA-ZåäöÅÄÖ'\.\s]{2,128}$";
            return ValidatePattern(propertyName, streetName, mandatory, pattern);
        }

        private static string ValidateOrgNo(string propertyName, string orgNo, bool mandatory)
        {
            const string pattern = @"^[0-9]{6}-[0-9]{4}$";

            var error = ValidatePattern(propertyName, orgNo, mandatory, pattern);
            if (error != null) return error;

            if (!string.IsNullOrEmpty(orgNo))
                if (!Checksum.VerifyLuhnChecksum(orgNo.Replace("-", "")))
                    return string.Format("{0}InvalidChecksum", propertyName);

            return null;
        }

        private static string ValidateVatNumber(string propertyName, string vatNumber, bool mandatory)
        {
            const string pattern = @"^[0-9A-Z]{8,12}$";
            return ValidatePattern(propertyName, vatNumber, mandatory, pattern);
        }

        private static string ValidateObject(string propertyName, object obj, bool mandatory)
        {
            //TODO: Depending on entry, return all on one single issue
            //TODO: How does this object work togetjer with a MVC form. Perhaps validating of sub-items has to be reweitten entierly.

            if (obj == null)
                return mandatory ? string.Format("{0}Missing.", propertyName) : null;

            //Go into the object and validate all its properties.
            var sb = new StringBuilder();
            foreach (var property in GetPropertiesToValidate(obj))
            {
                var error = GetValidationError(obj, property);
                if (error != null)
                    sb.AppendLine(string.Format("{0}.{1}", propertyName, error));
            }

            var allErrors = sb.ToString();
            return string.IsNullOrEmpty(allErrors) ? null : allErrors;
        }

        private static IEnumerable<string> GetPropertiesToValidate(object obj)
        {
            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                foreach (var attr in property.GetCustomAttributes(true))
                {
                    var vh = attr as ValidatorAttribute;
                    if (vh != null)
                        yield return property.Name;

                    var lvh = attr as ListValidatorAttribute;
                    if (lvh != null)
                        yield return property.Name;
                }
            }
        }
    }
}