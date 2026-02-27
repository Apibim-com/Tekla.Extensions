using Moq;
using System;
using Tekla.Structures.ModelInternal;
#if TEKLA2020
#else
using Tekla.Structures.RemotingHelper;
#endif

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
#if TEKLA2020
#else
            CDelegateSetter.SetInstanceForUnitTesting(new GenericDelegateFake<ReturnDefaultStrategy>());
#endif
        }

        public void Dispose()
        {
            //CDelegateSetter.ResetInstance(); 'CDelegateSetter' does not contain a definition for 'ResetInstance'
        }
    }
}
