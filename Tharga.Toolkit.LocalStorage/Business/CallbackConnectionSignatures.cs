using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Tharga.Toolkit.LocalStorage.Entity;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.LocalStorage.Business
{
    class CallbackConnectionSignatures
    {
        private readonly List<CallbackConnectionSignature> _callbackConnectionSignatures = new List<CallbackConnectionSignature>();

        public CallbackConnectionSignature Get(ISubscriptionServiceClient client, Func<InstanceContext, IMessageServiceClient> createEventServiceClient)
        {
            var callbackConnectionPattern = _callbackConnectionSignatures.FirstOrDefault(x => ReferenceEquals(x.Client, client));

            if (callbackConnectionPattern == null)
            {
                var callbackInstance = new InstanceContext(client);
                var serviceEventClient = createEventServiceClient(callbackInstance);
                callbackConnectionPattern = new CallbackConnectionSignature(client, callbackInstance, serviceEventClient);

                callbackConnectionPattern.ConnectionClosedEvent += callbackConnectionPattern_ConnectionClosedEvent;

                _callbackConnectionSignatures.Add(callbackConnectionPattern);
            }

            return callbackConnectionPattern;
        }

        void callbackConnectionPattern_ConnectionClosedEvent(object sender, CallbackConnectionSignature.ConnectionClosedEventArgs e)
        {
            var callbackConnectionSignature = (CallbackConnectionSignature)sender;
            callbackConnectionSignature.ConnectionClosedEvent -= callbackConnectionPattern_ConnectionClosedEvent;
            _callbackConnectionSignatures.Remove(callbackConnectionSignature);

            callbackConnectionSignature.Client.Stop(SubscriptionStoppedEventArgs.StopReason.ConnectionLost, e.ClientAddress, e.ServerAddress);
        }

        public void Remove(ISubscriptionServiceClient client)
        {
            _callbackConnectionSignatures.RemoveAll(x => ReferenceEquals(x.Client, client));
        }
    }
}