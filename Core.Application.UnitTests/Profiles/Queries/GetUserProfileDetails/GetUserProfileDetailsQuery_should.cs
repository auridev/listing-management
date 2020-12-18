using Core.Application.Profiles.Queries;
using Core.Application.Profiles.Queries.GetUserProfileDetails;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace Core.Application.UnitTests.Profiles.Queries.GetUserProfileDetails
{
    public class GetUserProfileDetailsQuery_should
    {
        private readonly GetUserProfileDetailsQuery _sut;
        private readonly GetUserProfileDetailsQueryParams _queryParams;
        private readonly AutoMocker _mocker;
        private readonly UserProfileDetailsModel _model;

        public GetUserProfileDetailsQuery_should()
        {
            _queryParams = new GetUserProfileDetailsQueryParams()
            {
                UserId = new Guid("25160098-c566-41ad-866a-277bb29318c3")
            };

            _model = new UserProfileDetailsModel()
            {
                Id = new Guid("c8512c9b-b561-4665-9c32-718c4d1cfff4"),
                Email = "a",
                FirstName = "a",
                LastName = "a",
                Company = "a",
                Phone = "a",
                CountryCode = "a",
                State = "a",
                City = "a",
                PostCode = "a",
                Address = "a",
                Latitude = 1D,
                Longitude = 1D,
                DistanceUnit = "a",
                MassUnit = "a",
                CurrencyCode = "a",
                IntroductionSeen = true,
                CreatedDate = DateTimeOffset.UtcNow
            };

            _mocker = new AutoMocker();
            _sut = _mocker.CreateInstance<GetUserProfileDetailsQuery>();
        }

        [Fact]
        public void retrieve_user_profile_details_from_repository()
        {
            Option<UserProfileDetailsModel> model = _sut.Execute(_queryParams);

            _mocker
                .GetMock<IProfileReadOnlyRepository>()
                .Verify(s => s.FindUserProfile(_queryParams.UserId), Times.Once);
        }

        [Fact]
        public void return_none_if_query_param_is_not_valid()
        {
            // act
            Option<UserProfileDetailsModel> optionalModel = _sut.Execute(null);

            // assert
            optionalModel.IsNone.Should().BeTrue();
        }

        [Fact]
        public void return_ProfileDetails()
        {
            // arrange
            _mocker
              .GetMock<IProfileReadOnlyRepository>()
              .Setup(s => s.FindUserProfile(It.IsAny<Guid>()))
              .Returns(_model);

            // act
            Option<UserProfileDetailsModel> model = _sut.Execute(_queryParams);

            // assert
            model.IsSome.Should().BeTrue();
            model
                .IfSome(m => m.Should().NotBeNull());
        }
    }
}
