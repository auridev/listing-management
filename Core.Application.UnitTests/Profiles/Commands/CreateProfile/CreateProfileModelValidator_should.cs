using Core.Application.Profiles.Commands.CreateProfile;
using FluentValidation.TestHelper;
using Xunit;

namespace Core.Application.UnitTests.Profiles.Commands.CreateProfile
{
    public class CreateProfileModelValidator_should
    {
        private readonly CreateProfileModelValidator _sut;
        private readonly CreateProfileModel _model;
        public CreateProfileModelValidator_should()
        {
            _sut = new CreateProfileModelValidator();
            _model = new CreateProfileModel()
            {
                Email = "aaa@bbb.com",
                FirstName = "Harry",
                LastName = "Potter",
                Company = "Hogwarts",
                Phone = "+333 111 22222",
                CountryCode = "aa",
                State = "bbb",
                City = "dddd",
                PostCode = "121212",
                Address = "4 Privet Drive",
                Latitude = 23D,
                Longitude = 100D,
                DistanceUnit = "km",
                MassUnit = "lb",
                CurrencyCode = "eur"
            };
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("aaa@bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb.com")]
        public void have_validation_error_if_Email_is_not_valid(string invalidEmail)
        {
            _model.Email = invalidEmail;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.Email);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqQ")]
        public void have_validation_error_if_FirstName_is_not_valid(string invalidFirstName)
        {
            _model.FirstName = invalidFirstName;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.FirstName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqQ")]
        public void have_validation_error_if_LastName_is_not_valid(string invalidLastName)
        {
            _model.LastName = invalidLastName;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.LastName);
        }

        [Theory]
        [InlineData("company company company company company company company")]
        public void have_validation_error_if_Company_is_not_valid(string invalidCompany)
        {
            _model.Company = invalidCompany;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.Company);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("11111111111111111111111111")]
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
        [InlineData("bb")]
        public void have_validation_error_if_DistanceUnit_is_not_valid(string invalidDistanceUnit)
        {
            _model.DistanceUnit = invalidDistanceUnit;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.DistanceUnit);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("c")]
        [InlineData("cc")]
        public void have_validation_error_if_MassUnit_is_not_valid(string invalidMassUnit)
        {
            _model.MassUnit = invalidMassUnit;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.MassUnit);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("eu")]
        [InlineData("euro")]
        public void have_validation_error_if_CurrencyCode_is_not_valid(string invalidCurrencyCode)
        {
            _model.CurrencyCode = invalidCurrencyCode;

            var result = _sut.TestValidate(_model);

            result.ShouldHaveValidationErrorFor(model => model.CurrencyCode);
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
