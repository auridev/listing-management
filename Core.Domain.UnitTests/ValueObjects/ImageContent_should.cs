using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace Core.Domain.UnitTests.ValueObjects
{
    public class ImageContent_should
    {

        private static byte[] _content = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 };
        private static string _fileName = "name.txt";
        private readonly Either<Error, ImageContent> _sut = ImageContent.Create(_fileName, _content);

        [Fact]
        public void have_FileName_property()
        {
            _sut
                .Right(ic => ic.FileName.Value.Should().Be("name.txt"))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void have_Content_property()
        {
            _sut
                .Right(ic => ic.Content.Should().BeEquivalentTo(_content))
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact(Skip = "Until equality comparison is fixed")]
        public void be_treated_as_equal_using_generic_equals_method_if_values_match()
        {
            var first = ImageContent.Create("name.txt", new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 });
            var second = ImageContent.Create("name.txt", new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 });

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact(Skip = "Until equality comparison is fixed")]
        public void be_treated_as_equal_using_object_equals_method_if_values_match()
        {
            var first = (object)ImageContent.Create("name1.txt", new byte[] { 0x20, 0x30, 0x40, 0x50 });
            var second = (object)ImageContent.Create("name1.txt", new byte[] { 0x20, 0x30, 0x40, 0x50 });

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact(Skip = "Until equality comparison is fixed")]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_match()
        {
            var first = ImageContent.Create("name2.txt", new byte[] { 0x30, 0x40, 0x50 });
            var second = ImageContent.Create("name2.txt", new byte[] { 0x30, 0x40, 0x50 });

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact(Skip = "Until equality comparison is fixed")]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Values_dont_match()
        {
            var first = ImageContent.Create("name.txt", new byte[] { 0x50 });
            var second = ImageContent.Create("name2.txt", new byte[] { 0x50 });

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }

        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] { null, _content },
            new object[] { _fileName, null },
            new object[] { _fileName, new byte [0] },
        };

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void return_EiherLeft_with_proper_error_during_creation_if_arguments_are_not_valid(string generatedFileName, byte[] content)
        {
            Either<Error, ImageContent> eitherImageContent = ImageContent.Create(generatedFileName, content);

            eitherImageContent.IsLeft.Should().BeTrue();
            eitherImageContent
               .Right(_ => throw InvalidExecutionPath.Exception)
               .Left(error => error.Should().BeOfType<Error.Invalid>());
        }
    }
}
