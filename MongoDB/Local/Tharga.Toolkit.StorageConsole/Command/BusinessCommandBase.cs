using System;
using System.Linq;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Entity;
using Tharga.Toolkit.LocalStorage.Interface;
using Tharga.Toolkit.Storage;
using Tharga.Toolkit.StorageConsole.Utility;

namespace Tharga.Toolkit.StorageConsole.Command
{
    public abstract class BusinessCommandBase<THandler, TEntity> : ContainerCommandBase
        where THandler : BusinessBase<TEntity>
        where TEntity : IEntity
    {
        private readonly BusinessListCommand<THandler, TEntity> _listCommand;

        protected BusinessCommandBase(string name, THandler business)
            : this(new ClientConsole(), name, business)
        {
        }

        private BusinessCommandBase(IConsole console, string name, THandler instance)
            : base(name)
        {
            _listCommand = new BusinessListCommand<THandler, TEntity>(console, instance);

            RegisterCommand(new BusinessSyncCommand<THandler, TEntity>(instance));
            RegisterCommand(new BusinessResetCommand<THandler, TEntity>(instance));
            RegisterCommand(_listCommand);
            RegisterCommand(new BusinessDeleteCommand<THandler, TEntity>(instance));

            instance.SyncStarted += Instance_SyncStarted;
            instance.SyncCompleted += instance_SyncCompleted;
            instance.EntityChanged += Instance_EntityChanged;
            instance.EntityDeleted += instance_EntityDeleted;
        }

        private void Instance_EntityChanged(object sender, EntityChangedEventArgs<TEntity> e)
        {
            if (e.Exception == null)
                OutputEvent("{0} changed {1} (Id: {2})", OutputLevel.Information, typeof (TEntity).ToShortString(), e.SaveLocation == Location.Locally ? "locally" : "on server", e.Entity.Id);
            else
                OutputError("{0} changed {1} (Id: {2}) {3}", typeof (TEntity).ToShortString(), e.SaveLocation == Location.Locally ? "locally" : "on server", e.Entity.Id, e.Exception.Message);
        }

        private void instance_EntityDeleted(object sender, EntityDeletedEventArgs<TEntity> e)
        {
            OutputEvent("{0} deleted {1} (Id: {2})", OutputLevel.Information, typeof(TEntity).ToShortString(), e.SaveLocation == Location.Locally ? "locally" : "on server", e.Entity.Id);
        }

        private void Instance_SyncStarted(object sender, BusinessBase<TEntity>.SyncStartedEventArgs e)
        {
            OutputEvent("Sync started for {0}. {1} is fetched.", OutputLevel.Information, typeof(TEntity).ToShortString(), e.PreviousServerStoreTime == null ? "All data" : string.Format("Data from {0}", e.PreviousServerStoreTime.ToDateTimeString()));
        }

        private void instance_SyncCompleted(object sender, BusinessBase<TEntity>.SyncCompleteEventArgs e)
        {
            if (e.Exception == null)
            {
                var changeMessage = "There are no changes.";
                if (e.EntityDeletedCount != 0 || e.EntityChangedCount != 0 || e.EntityResendCount != 0)
                    changeMessage = string.Format("(Del: {0}, Changed: {1}, Resent: {2}) {3}", e.EntityDeletedCount, e.EntityChangedCount, e.EntityResendCount, e.ServerStoreTime.ToDateTimeString());

                OutputEvent("Synced {0} in {1}ms. {2}", OutputLevel.Information, typeof(TEntity).ToShortString(), e.Duration.TotalMilliseconds.ToString("0"), changeMessage);
            }
            else
                OutputError("Failed sync {0} after {1}ms. {2}", typeof (TEntity).ToShortString(), e.Duration.TotalMilliseconds.ToString("0"), e.Exception.Message);
        }

        protected void SetEntityOutput(Func<TEntity, string> outputAction)
        {
            _listCommand.SetEntityOutput(outputAction);
        }

        public override bool CanExecute()
        {
            string reasonMessage;
            return SubCommands.Any(x => x.CanExecute(out reasonMessage));
        }
    }
}