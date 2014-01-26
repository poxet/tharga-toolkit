using System;

namespace Tharga.Toolkit.LocalStorage.Entity
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ListValidatorAttribute : Attribute
    {
        public enum OccurrenceType
        {
            AtLeastOne,
        };

        public OccurrenceType Occurrence { get; set; }
        public ValidatorAttribute.ValidatorType Type { get; set; }
    }
}