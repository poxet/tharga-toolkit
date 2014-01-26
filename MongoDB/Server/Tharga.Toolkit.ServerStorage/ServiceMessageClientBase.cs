using System;
using System.ServiceModel;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.ServerStorage
{
    public abstract class ServiceMessageClientBase : DuplexClientBase<IServiceMessage>, IServiceMessage
    {
        protected ServiceMessageClientBase(InstanceContext callbackInstance) :
            base(callbackInstance)
        {
        }

        protected ServiceMessageClientBase(InstanceContext callbackInstance, string endpointConfigurationName) :
            base(callbackInstance, endpointConfigurationName)
        {
        }

        protected ServiceMessageClientBase(InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) :
            base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        protected ServiceMessageClientBase(InstanceContext callbackInstance, string endpointConfigurationName, EndpointAddress remoteAddress) :
            base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        protected ServiceMessageClientBase(InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, EndpointAddress remoteAddress) :
            base(callbackInstance, binding, remoteAddress)
        {
        }

        public void StartSubscription(StartSubscriptionRequest request)
        {
            Channel.StartSubscription(request);
        }

        public void StopSubscription(StopSubscriptionRequest request)
        {
            Channel.StopSubscription(request);
        }

        public void CheckSubscription(CheckSubscriptionRequest request)
        {
            Channel.CheckSubscription(request);
        }

        public void CreateSession(CreateSessionRequest request)
        {
            Channel.CreateSession(request);
        }

        public void EndSession(EndSessionRequest request)
        {
            Channel.EndSession(request);
        }

        public void Execute(Guid sessionToken, object command)
        {
            Channel.Execute(sessionToken, command);
        }

        public abstract void Sync<T>(Guid sessionToken, DateTime? serverStoreTime);}
}