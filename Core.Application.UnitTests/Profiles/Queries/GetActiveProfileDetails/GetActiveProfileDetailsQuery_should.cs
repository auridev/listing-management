using Core.Application.Profiles.Queries;
using Core.Application.Profiles.Queries.GetActiveProfileDetails;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Profiles.Queries.GetProfileDetails
{
    public class GetActiveProfileDetailsQuery_should
    {
        private readonly GetActiveProfileDetailsQuery _sut;
        private readonly GetActiveProfileDetailsQueryParams _queryParams;
        private readonly AutoMocker _mocker;
        private readonly ActiveProfileDetailsModel _model;

        public GetActiveProfileDetailsQuery_should()
        {
            _queryParams = new GetActiveProfileDetailsQueryParams()
            {
                ProfileId = Guid.NewGuid()
            };
            _mocker = new AutoMocker();
            _model = new ActiveProfileDetailsModel()
            {
                Email = "a@b.c"
            };

            _sut = _mocker.CreateInstance<GetActiveProfileDetailsQuery>();
        }

        [Fact]
        public void retrieve_profile_details_from_repository()
        {
            Option<ActiveProfileDetailsModel> model = _sut.Execute(_queryParams);

            _mocker
                .GetMock<IProfileReadOnlyRepository>()
                .Verify(s => s.FindActiveProfile(_queryParams.ProfileId), Times.Once);
        }

        [Fact]
        public void return_none_if_query_param_is_not_valid()
        {
            // act
            Option<ActiveProfileDetailsModel> optionalModel = _sut.Execute(null);

            // assert
            optionalModel.IsNone.Should().BeTrue();
        }

        [Fact]
        public void return_ProfileDetails()
        {
            // arrange
            _mocker
              .GetMock<IProfileReadOnlyRepository>()
              .Setup(s => s.FindActiveProfile(It.IsAny<Guid>()))
              .Returns(_model);

            // act
            Option<ActiveProfileDetailsModel> model = _sut.Execute(_queryParams);

            // assert
            model
                .IfSome(m => m.Should().NotBeNull());
        }
    }
}
