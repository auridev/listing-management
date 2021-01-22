using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class ImageReference_should
    {
        private readonly Either<Error, ImageReference> _sut;
        private readonly Guid _id = Guid.NewGuid();
        private readonly Guid _parentReference = Guid.NewGuid();

        public ImageReference_should()
        {
            _sut = ImageReference.Create(
                _id,
                _parentReference,
                "name.ext",
                34_000_000L);
        }

        [Fact]
        public void have_Id_property()
        {
            _sut
                .Right(ir => ir.Id.Should().Be(_id))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }
        [Fact]
        public void have_ParentReference_property()
        {
            _sut
                .Right(ir => ir.ParentReference.Should().Be(_parentReference))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }


        [Fact]
        public void have_FileName_property()
        {
            _sut
                .Right(ir => ir.FileName.Value.Should().Be("name.ext"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_FileSize_property()
        {
            _sut
                .Right(ir => ir.FileSize.Bytes.Should().Be(34_000_000L))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void be_treated_as_equal_using_generic_equals_method_if_values_match()
        {
            var first = ImageReference.Create(_id, _parentReference, "name.ext", 25L);
            var second = ImageReference.Create(_id, _parentReference, "name.ext", 25L);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_equals_method_if_values_match()
        {
            var first = (object)ImageReference.Create(_id, _parentReference, "name.ext", 25L);
            var second = (object)ImageReference.Create(_id, _parentReference, "name.ext", 25L);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_match()
        {
            var first = ImageReference.Create(_id, _parentReference, "name.ext", 25L);
            var second = ImageReference.Create(_id, _parentReference, "name.ext", 25L);

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Values_dont_match()
        {
            var first = ImageReference.Create(_id, _parentReference, "name.ext1", 25L);
            var second = ImageReference.Create(_id, _parentReference, "name.ext", 25L);

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }
    }
}
