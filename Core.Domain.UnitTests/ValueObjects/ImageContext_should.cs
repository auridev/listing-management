using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class ImageContext_should
    {
        private readonly Either<Error, ImageContext> _sut;
        private readonly Guid _id = Guid.NewGuid();
        private readonly Guid _parentReference = Guid.NewGuid();

        public ImageContext_should()
        {
            _sut = ImageContext.Create(
                _id,
                _parentReference,
                "name.ext",
                new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 });
        }

        [Fact]
        public void have_Reference_property()
        {
            _sut
                .Right(ic => ic.Reference.Id.Should().Be(_id))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_Content_property()
        {
            _sut
                .Right(ir => ir.Content.FileName.Value.Should().Be("name.ext"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact(Skip = "Until ImageContent equality comparison is fixed")]
        public void be_treated_as_equal_using_generic_equals_method_if_values_match()
        {
            var first = ImageContext.Create(_id, _parentReference, "name.ext", new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 });
            var second = ImageContext.Create(_id, _parentReference, "name.ext", new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 });

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact(Skip = "Until ImageContent equality comparison is fixed")]
        public void be_treated_as_equal_using_object_equals_method_if_values_match()
        {
            var first = (object)ImageContext.Create(_id, _parentReference, "name.ext", new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 });
            var second = (object)ImageContext.Create(_id, _parentReference, "name.ext", new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 });

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact(Skip = "Until ImageContent equality comparison is fixed")]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_match()
        {
            var first = ImageContext.Create(_id, _parentReference, "name.ext", new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 });
            var second = ImageContext.Create(_id, _parentReference, "name.ext", new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 });

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact(Skip = "Until ImageContent equality comparison is fixed")]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Values_dont_match()
        {
            var first = ImageContext.Create(_id, Guid.NewGuid(), "name.ext", new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 });
            var second = ImageContext.Create(_id, _parentReference, "name.ext", new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 });

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
