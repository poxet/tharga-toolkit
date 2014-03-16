using System;
using NUnit.Framework;

namespace Tharga.Test.Toolkit
{
    public abstract class AaaTest
    {
        protected Type ExpectedExceptionType { get; set; }
        protected Exception ThrownException { get; set; }

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
        public virtual void Teardown() { }
    }
}
