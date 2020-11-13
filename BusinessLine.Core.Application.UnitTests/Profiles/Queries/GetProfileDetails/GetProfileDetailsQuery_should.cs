using Core.Application.Profiles.Queries;
using Core.Application.Profiles.Queries.GetProfileDetails;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Queries.GetProfileDetails
{
    public class GetProfileDetailsQuery_should
    {
        private readonly GetProfileDetailsQuery _sut;
        private readonly GetProfileDetailsQueryParams _queryParams;
        private readonly AutoMocker _mocker;
        private readonly ProfileDetailsModel _model;

        public GetProfileDetailsQuery_should()
        {
            _queryParams = new GetProfileDetailsQueryParams()
            {
                ProfileId = Guid.NewGuid()
            };
            _mocker = new AutoMocker();
            _model = new ProfileDetailsModel()
            {
                Email = "a@b.c"
            };

            _sut = _mocker.CreateInstance<GetProfileDetailsQuery>();
        }

        [Fact]
        public void retrieve_the_correct_ProfileDetails_from_data_access_service()
        {
            Option<ProfileDetailsModel> model = _sut.Execute(_queryParams);

            _mocker
                .GetMock<IProfileDataService>()
                .Verify(s => s.Find(_queryParams.ProfileId), Times.Once);
        }

        [Fact]
        public void return_None_if_ProfileDetails_does_not_exist()
        {
            // arrange
            _mocker
              .GetMock<IProfileDataService>()
              .Setup(s => s.Find(It.IsAny<Guid>()))
              .Returns(() => { return null; });

            // act
            Option<ProfileDetailsModel> optionalModel = _sut.Execute(_queryParams);

            // assert
            optionalModel.IsNone.Should().BeTrue();
        }

        [Fact]
        public void return_None_if_query_params_is_not_valid()
        {
            // act
            Option<ProfileDetailsModel> optionalModel = _sut.Execute(null);

            // assert
            optionalModel.IsNone.Should().BeTrue();
        }

        [Fact]
        public void return_ProfileDetails()
        {
            // arrange
            _mocker
              .GetMock<IProfileDataService>()
              .Setup(s => s.Find(It.IsAny<Guid>()))
              .Returns(_model);

            // act
            Option<ProfileDetailsModel> model = _sut.Execute(_queryParams);

            // assert
            model
                .IfSome(m => m.Should().NotBeNull());
        }
    }
}
