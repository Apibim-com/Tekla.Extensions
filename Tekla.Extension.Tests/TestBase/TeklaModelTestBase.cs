using Moq;
using System;
using Tekla.Structures.ModelInternal;
using Tekla.Structures.RemotingHelper;

namespace Tekla.Extension.Tests.TestBase
{

    /// <summary>
    /// Base class for tests that require Tekla Model internal (ICDelegate) to be mocked.
    /// Constructor sets up the fake delegate; Dispose resets it (xUnit calls both per test).
    /// </summary>
    public abstract class TeklaModelTestBase : IDisposable
    {
        protected TeklaModelTestBase()
        {
            CDelegateSetter.SetInstanceForUnitTesting(new GenericDelegateFake<ReturnDefaultStrategy>());
        }

        public void Dispose()
        {
            //CDelegateSetter.ResetInstance(); 'CDelegateSetter' does not contain a definition for 'ResetInstance'
        }
    }
}
