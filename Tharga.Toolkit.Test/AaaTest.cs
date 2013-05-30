using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tharga.Toolkit.Test
{
    public abstract class AaaTest
    {
        protected Type ExpectedExceptionType;
        protected Exception ThrownException;

        protected abstract void Arrange();
        protected abstract void Act();

        protected Type ThrownExceptionType { get { return ThrownException == null ? null : ThrownException.GetType(); } }

        [TestInitialize]
        public void Setup()
        {
            Arrange();

            try
            {
                Act();
            }
            catch (Exception ex)
            {
                if (ex.GetType() != ExpectedExceptionType)
                {
                    throw;
                }
                ThrownException = ex;
            }
        }

        [TestCleanup]
        public virtual void Teardown()
        {

        }
    }
}