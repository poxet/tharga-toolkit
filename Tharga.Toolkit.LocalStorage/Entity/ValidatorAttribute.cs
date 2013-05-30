using System;

namespace Tharga.Toolkit.LocalStorage.Entity
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidatorAttribute : Attribute
    {
        public enum ValidatorType
        {
            MissingString,
            MissingInteger,
            MissingGuid,
            MissingDateTime, MissingNullableDateTime,
            EMailMandatory, EMailOptional,
            OrgNoOptional,
            VatNumberOptional,
            PhoneNumberMandatory, PhoneNumberOptional,
            StreetNameMandatory,
            PostalCodeMandatory,
            CityMandatory,
            OptionalObject,
        };

        public ValidatorType Type { get; set; }
    }
}