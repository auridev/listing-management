using BusinessLine.Core.Domain.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class ImageContent_should
    {
        private readonly ImageContent _sut;
        private static byte[] _content;
        private static FileName _generatedFileName;
        public ImageContent_should()
        {
            _content = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 };
            _generatedFileName = FileName.Create("name.txt");
            _sut = ImageContent.Create(_generatedFileName,_content);
        }

        [Fact]
        public void have_FileName_property()
        {
            _sut.FileName.Should().Be(_generatedFileName);
        }

        [Fact]
        public void have_Content_property()
        {
            _sut.Content.Should().BeEquivalentTo(_content);
        }

        [Fact]
        public void be_treated_as_equal_using_generic_equals_method_if_values_match()
        {
            var first = ImageContent.Create(
                _generatedFileName,
               _content);
            var second = ImageContent.Create(
                _generatedFileName,
               _content);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_object_equals_method_if_values_match()
        {
            var first = (object) ImageContent.Create(
                _generatedFileName,
               _content);
            var second = (object) ImageContent.Create(
               _generatedFileName,
               _content);

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_Values_match()
        {
            var parentId = Guid.NewGuid();
            var first = ImageContent.Create(
                _generatedFileName,
               _content);
            var second = ImageContent.Create(
                _generatedFileName,
               _content);

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Values_dont_match()
        {
            var first = (object)ImageContent.Create(
                _generatedFileName,
               _content);
            var second = (object)ImageContent.Create(
                _generatedFileName,
               _content);

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }

        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] {  _generatedFileName, _content },
            new object[] { _generatedFileName, _content },
            new object[] { null, _content },
            new object[] { _generatedFileName, null },
            new object[] { _generatedFileName, new byte [0] },
        };

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void thrown_an_exception_if_arguments_are_not_valid(FileName generatedFileName, byte[] content)
        {
            Action createAction = () => ImageContent.Create(generatedFileName, content);

            createAction.Should().Throw<ArgumentNullException>();
        }
    }
}
