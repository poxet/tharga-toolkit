using System;
using NUnit.Framework;

namespace Tharga.Toolkit.Test
{
    public abstract class AaaTest
    {
        protected Type ExpectedExceptionType;
        protected Exception ThrownException;

        protected abstract void Arrange();
        protected abstract void Act();

        protected Type ThrownExceptionType { get { return ThrownException == null ? null : ThrownException.GetType(); } }

        [TestFixtureSetUp]
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

        [TestFixtureTearDown]
        public virtual void Teardown()
        {

        }
    }
}