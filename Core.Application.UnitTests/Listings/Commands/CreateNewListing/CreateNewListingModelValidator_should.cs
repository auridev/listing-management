using Core.Application.Listings.Commands.CreateNewListing;
using FluentValidation.TestHelper;
using Xunit;

namespace Core.Application.UnitTests.Listings.Commands.CreateNewListing
{
    public class CreateNewListingModelValidator_should
    {
        private readonly CreateNewListingModelValidator _sut;
        private readonly CreateNewListingModel _model;

        public CreateNewListingModelValidator_should()
        {
            _sut = new CreateNewListingModelValidator();
            _model = new CreateNewListingModel()
            {
                Title = "valid title",
                MaterialTypeId = 10,
                Weight = 2.3F,
                MassUnit = "kg",
                Description = "valid description",
                FirstName = "firstname",
                LastName = "lasname",
                Company = "my company",
                Phone = "+333 111 22222",
                CountryCode = "code",
                State = "state",
                City = "city",
                PostCode = "12",
                Address = "disneyland",
                Latitude = 1.1D,
                Longitude = 2.2D,
            };
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void have_validation_error_if_Title_is_not_valid(string invalidTitle)
        {
            _model.Title = invalidTitle;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.Title);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(11)]
        [InlineData(1_000)]
        public void have_validation_error_if_MaterialTypeId_is_not_valid(int invalidMaterialType)
        {
            _model.MaterialTypeId = invalidMaterialType;
     
            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.MaterialTypeId);
        }

        [Theory]
        [InlineData(-1F)]
        [InlineData(0)]
        public void have_validation_error_if_Weight_is_not_valid(float invalidWeight)
        {
            _model.Weight = invalidWeight;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.Weight);
        }

        [Theory]
        //[InlineData(null)]
        [InlineData("")]
        [InlineData("aa")]
        public void have_validation_error_if_MassUnit_is_not_valid(string invalidMassUnit)
        {
            _model.MassUnit = invalidMassUnit;
            
            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.MassUnit);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(
            @"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
            bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb
            cccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccc
            dddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd
            eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee
            ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")]
        public void have_validation_error_if_Description_is_not_valid(string invalidDescription)
        {

            _model.Description = invalidDescription;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.Description);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a ")]
        public void have_validation_error_if_FirstName_is_not_valid(string invalidFirstName)
        {
            _model.FirstName = invalidFirstName;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.FirstName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("b b b b b b b b b b b b b b b b bbbbbbbbbbbbbbbbbbbbbbbbbbb b b b b b b b b b b b b b b b b b b b b b")]
        public void have_validation_error_if_LastName_is_not_valid(string invalidLastName)
        {
            _model.LastName = invalidLastName;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.LastName);
        }

        [Theory]
        [InlineData("company company company company company company company company company company company company company")]
        public void have_validation_error_if_Company_is_not_valid(string invalidCompany)
        {
            _model.Company = invalidCompany;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.Company);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("132klj1l2k3j123jkl12k3jl1a")]
        public void have_validation_error_if_Phone_is_not_valid(string invalidPhone)
        {
            _model.Phone = invalidPhone;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.Phone);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("aaaaaB")]
        public void have_validation_error_if_CountryCode_is_not_valid(string invalidCountryCode)
        {
            _model.CountryCode = invalidCountryCode;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.CountryCode);
        }

        [Theory]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaB")]
        public void have_validation_error_if_State_is_not_valid(string invalidState)
        {
            _model.State = invalidState;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.State);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaB")]
        public void have_validation_error_if_City_is_not_valid(string invalidCity)
        {
            _model.City = invalidCity;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.City);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("aaaaaaaaaaaaaaaB")]
        public void have_validation_error_if_PostCode_is_not_valid(string invalidPostCode)
        {
            _model.PostCode = invalidPostCode;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.PostCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(
          @"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
            bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb
            cccccccccccccccccccccccccccccccccccccccccccccccccc
            ddddddddddddddddddddddddddddddddddddddddddddddddddEEEEEEEEE")]
        public void have_validation_error_if_Address_is_not_valid(string invalidAddress)
        {
            _model.Address = invalidAddress;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.Address);
        }

        [Theory]
        [InlineData(-85.05112878D)]
        [InlineData(-86D)]
        [InlineData(85.05112878D)]
        [InlineData(86D)]
        public void have_validation_error_if_Latitude_is_not_valid(double invalidLatitude)
        {
            _model.Latitude = invalidLatitude;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.Latitude);
        }

        [Theory]
        [InlineData(-180.00000001D)]
        [InlineData(-181)]
        [InlineData(180.0000000001D)]
        [InlineData(181D)]
        public void have_validation_error_if_Longitude_is_not_valid(double invalidLongitude)
        {
            _model.Longitude = invalidLongitude;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.Longitude);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void not_have_any_validation_errors_if_Company_is_not_provided(string company)
        {
            _model.Company = company;

            var result = _sut.TestValidate(_model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void not_have_any_validation_errors_if_State_is_not_provided(string state)
        {
            _model.State = state;

            var result = _sut.TestValidate(_model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void not_have_any_validation_errors_if_all_properties_are_valid()
        {
            var result = _sut.TestValidate(_model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
