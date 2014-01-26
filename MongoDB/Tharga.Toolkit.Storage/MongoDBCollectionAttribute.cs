using System;

namespace Tharga.Toolkit.Storage
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class MongoDBCollectionAttribute : Attribute
    {
        public string Name { get; set; }
    }
}