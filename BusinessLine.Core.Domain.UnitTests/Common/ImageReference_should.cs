//using BusinessLine.Core.Domain.Common;
//using FluentAssertions;
//using Xunit;

//namespace BusinessLine.Core.Domain.UnitTests.Common
//{
//    public class ImageReference_should
//    {
//        private readonly ImageReference _sut;
//        public ImageReference_should()
//        {
//            _sut = ImageReference.Create(
//                FileName.Create("name.ext"),
//                FileSize.Create(34_000_000L));
//        }

//        [Fact]
//        public void have_FileName_property()
//        {
//            _sut.FileName.Value.Should().Be("name.ext");
//        }

//        [Fact]
//        public void have_FileSize_property()
//        {
//            _sut.FileSize.Should().Be(FileSize.Create(34_000_000L));
//        }

//        [Fact]
//        public void be_treated_as_equal_using_generic_equals_method_if_values_match()
//        {
//            var first = ImageReference.Create(FileName.Create("name.ext"),
//                FileSize.Create(25L));
//            var second = ImageReference.Create(FileName.Create("name.ext"),
//                FileSize.Create(25L));

//            var equals = first.Equals(second);

//            equals.Should().BeTrue();
//        }

//        [Fact]
//        public void be_treated_as_equal_using_object_equals_method_if_values_match()
//        {
//            var first = (object)ImageReference.Create(FileName.Create("name.ext"),
//                FileSize.Create(25L));
//            var second = (object)ImageReference.Create(FileName.Create("name.ext"),
//                FileSize.Create(25L));

//            var equals = first.Equals(second);

//            equals.Should().BeTrue();
//        }

//        [Fact]
//        public void be_treated_as_equal_using_the_equals_operator_if_Values_match()
//        {
//            var first = ImageReference.Create(FileName.Create("name.ext"),
//                  FileSize.Create(25L));
//            var second = ImageReference.Create(FileName.Create("name.ext"),
//                FileSize.Create(25L));

//            var equals = (first == second);

//            equals.Should().BeTrue();
//        }

//        [Fact]
//        public void be_treated_as_not_equal_using_the_not_equals_operator_if_Values_dont_match()
//        {
//            var first = ImageReference.Create(FileName.Create("name.ext1"),
//                FileSize.Create(25L));
//            var second = ImageReference.Create(FileName.Create("name.ext"),
//                FileSize.Create(25L));

//            var nonEquals = (first != second);

//            nonEquals.Should().BeTrue();
//        }
//    }
//}
