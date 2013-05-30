using System;
using System.ServiceModel;
using SampleBusiness.MessageReference;
using Tharga.Toolkit.LocalStorage;

namespace SampleBusiness
{
    public static class ServiceCommandClientCreator
    {
        public static ServiceCommandClient GetServiceCommandClient()
        {
            try
            {
                return new ServiceCommandClient("NetMsmqBinding_IServiceCommand");
            }
            catch (InvalidOperationException)
            {
                throw new NotImplementedException();
                //const string defaultAddress = "net.msmq://10.10.10.2/private/MarathonMemberActivityServiceCommand";
                //var address = Tharga.Toolkit.Endpoint.Address.Get("Command", defaultAddress);

                //if (address.Mode == AddressInformation.UsageMode.Default)
                //    Tharga.Toolkit.Endpoint.Address.Set("Command", defaultAddress);

                //return new ServiceCommandClient(new NetMsmqBinding("NetMsmqBinding_IServiceCommand"), new EndpointAddress(address.MainAddress));
            }
            //return new ServiceCommandClient("NetMsmqBinding_IServiceCommand");
        }

        public static ServiceMessageClient GetServiceMessageClient(InstanceContext callback)
        {
            try
            {
                return new ServiceMessageClient(callback, "NetTcpBinding_IServiceMessage");
            }
            catch (InvalidOperationException)
            {
                //TODO: Use endpoint module here
                throw new NotImplementedException();
                //const string defaultAddress = "net.tcp://10.10.10.2:9999/MarathonMemberActivityServiceEvent/";
                //var address = Tharga.Toolkit.Endpoint.Address.Get("Event", defaultAddress);

                //if (address.Mode == AddressInformation.UsageMode.Default)
                //    Tharga.Toolkit.Endpoint.Address.Set("Event", defaultAddress);

                //return new ServiceEventClient(callback, new NetTcpBinding("NetTcpBinding_IServiceEvent"), new EndpointAddress(address.MainAddress));
            }
        }

        //public static ServiceMessageClient GetServiceMessageClient(SubscriptionCallback callback)
        //{
        //    var instanceContext = new InstanceContext(callback);
        //    var client = GetServiceMessageClient(instanceContext);
        //    callback.ExecuteCompleteEvent += client.OnExecuteCompleteEvent;
        //    return client;
        //}
    }
}