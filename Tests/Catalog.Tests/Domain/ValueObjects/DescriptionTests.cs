using Catalog.Domain.Exceptions;
using Catalog.Domain.ValueObjects;
using Xunit;

namespace Catalog.Tests.Domain.ValueObjects
{
    public class DescriptionTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("     ")]
        public void Should_Throw_When_Description_Is_Null_Or_Empty(string value)
        {
            Assert.Throws<DomainException>(() => new Description(value));
        }

        [Fact]
        public void Should_Throw_When_Description_Is_Too_Short()
        {
            var shortText = "small";
            Assert.Throws<DomainException>(() => new Description(shortText));
        }

        [Fact]
        public void Should_Throw_When_Description_Is_Too_Long()
        {
            var longText = new string('a', 501);
            Assert.Throws<DomainException>(() => new Description(longText));
        }

        [Fact]
        public void Should_Create_Valid_Description()
        {
            var description = new Description("Valid description with more than 10 characters");
            Assert.Equal("Valid description with more than 10 characters", description.Value);
        }
    }
}
