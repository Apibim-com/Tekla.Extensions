using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Tekla.Extension.Tests
{
    public class LinqExtensionTests
    {
        [Fact]
        public void ToIEnumerable_ArrayList_ConvertsAllElements()
        {
            var list = new ArrayList { "one", "two", "three" };
            var result = list.GetEnumerator().ToIEnumerable<string>().ToList();
            result.Should().HaveCount(3);
            result.Should().ContainInOrder("one", "two", "three");
        }

        [Fact]
        public void ToIEnumerable_FiltersMatchingType()
        {
            var list = new ArrayList { "text", 42, "another", 3.14 };
            var result = list.GetEnumerator().ToIEnumerable<string>().ToList();
            result.Should().HaveCount(2);
            result.Should().ContainInOrder("text", "another");
        }

        [Fact]
        public void ToIEnumerable_EmptyEnumerator_ReturnsEmpty()
        {
            var list = new ArrayList();
            var result = list.GetEnumerator().ToIEnumerable<string>().ToList();
            result.Should().BeEmpty();
        }

        [Fact]
        public void ToIEnumerable_NoMatchingType_ReturnsEmpty()
        {
            var list = new ArrayList { 1, 2, 3 };
            var result = list.GetEnumerator().ToIEnumerable<string>().ToList();
            result.Should().BeEmpty();
        }
    }
}
