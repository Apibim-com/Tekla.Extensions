using FluentAssertions;
using System;
using System.ComponentModel;
using System.Security.Cryptography;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.ModelInternal;
#if TEKLA2020
#else
using Tekla.Structures.RemotingHelper;
#endif

namespace Tekla.Extension.Tests
{
    internal static class TestHelpers
    {
        public const double Tolerance = 1e-6;

        public static void AssertPointEqual(Point actual, double expectedX, double expectedY, double expectedZ)
        {
            actual.X.Should().BeApproximately(expectedX, Tolerance);
            actual.Y.Should().BeApproximately(expectedY, Tolerance);
            actual.Z.Should().BeApproximately(expectedZ, Tolerance);
        }

        public static void AssertVectorEqual(Vector actual, double expectedX, double expectedY, double expectedZ)
        {
            actual.X.Should().BeApproximately(expectedX, Tolerance);
            actual.Y.Should().BeApproximately(expectedY, Tolerance);
            actual.Z.Should().BeApproximately(expectedZ, Tolerance);
        }
    }

}
