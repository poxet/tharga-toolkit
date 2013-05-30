using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Tharga.Toolkit.Storage
{
    [System.Diagnostics.DebuggerStepThroughAttribute]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "OnSyncRequest", Namespace = "http://schemas.datacontract.org/2004/07/Tharga.Toolkit.ServerStorage.DataTransfer")]
    [Serializable]
    [ServiceKnownType("GetKnownTypes", typeof(KnownEntityTypesProvider))]
    public class OnSyncResponse : IOnSyncRequest
    {
        [DataMember]
        public object Default { get; set; } //Used so that the type can be determined

        [DataMember]
        public Guid RealmId { get; set; }

        [DataMember]
        public List<object> Changed { get; set; }

        [DataMember]
        public List<object> Deleted { get; set; }
    }
}