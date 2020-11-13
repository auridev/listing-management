using Core.Domain.Common;
using FluentAssertions;
using LanguageExt;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Common
{
    public class MaterialType_should
    {
        [Fact]
        public void have_non_default_string_Name_property()
        {
            var materialType = MaterialType.Ferrous;

            materialType.Name.Should().Be("Ferrous");
        }

        [Fact]
        public void have_non_default_interger_Id_property()
        {
            var materialType = MaterialType.Glass;

            materialType.Id.Should().Be(50);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void have_predefined_options(MaterialType materialType, int expectedId, string expectedName)
        {
            materialType.Id.Should().Be(expectedId);
            materialType.Name.Should().Be(expectedName);
        }

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { MaterialType.NonFerrous,  10, "NonFerrous"},
            new object[] { MaterialType.Ferrous,  20, "Ferrous"},
            new object[] { MaterialType.Paper,  30, "Paper"},
            new object[] { MaterialType.Plastic, 40, "Plastic"},
            new object[] { MaterialType.Glass, 50, "Glass"},
            new object[] { MaterialType.Electronics, 60, "Electronics"},
            new object[] { MaterialType.TyresAndRubber, 70, "TyresAndRubber"},
            new object[] { MaterialType.Textiles, 80, "Textiles"},
            new object[] { MaterialType.Wood, 90, "Wood"}
        };

        [Fact]
        public void be_treated_as_equal_using_Equals_method_if_predefined_values_match()
        {
            var first = MaterialType.NonFerrous;
            var second = MaterialType.NonFerrous;

            var equals = first.Equals(second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_equal_using_the_equals_operator_if_predefined_values_match()
        {
            var first = MaterialType.Wood;
            var second = MaterialType.Wood;

            var equals = (first == second);

            equals.Should().BeTrue();
        }

        [Fact]
        public void be_treated_as_not_equal_using_the_not_equals_operator_if_predefined_values_dont_match()
        {
            var first = MaterialType.Glass;
            var second = MaterialType.Paper;

            var nonEquals = (first != second);

            nonEquals.Should().BeTrue();
        }

        [Fact]
        public void return_optional_material_type_by_id()
        {
            MaterialType optionalMaterialType = MaterialType.ById(20);

            optionalMaterialType.Should().Be(MaterialType.Ferrous);
        }

        [Fact]
        public void throw_an_exception_if_material_type_does_not_exist()
        {
            Action action = () => MaterialType.ById(15);

            action.Should().Throw<Exception>();
        }
    }
}
