using Core.Application.Profiles.Queries;
using Core.Application.Profiles.Queries.GetPassiveProfileDetails;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Xunit;

namespace Core.Application.UnitTests.Profiles.Queries.GetPassiveProfileDetails
{
    public class GetPassiveProfileDetailsQuery_should
    {
        private readonly GetPassiveProfileDetailsQuery _sut;
        private readonly GetPassiveProfileDetailsQueryParams _queryParams;
        private readonly AutoMocker _mocker;
        private readonly PassiveProfileDetailsModel _model;

        public GetPassiveProfileDetailsQuery_should()
        {
            _queryParams = new GetPassiveProfileDetailsQueryParams()
            {
                ProfileId = new Guid("fd9a30da-74eb-461e-8976-ce3b489c26c1")
            };
            _model = new PassiveProfileDetailsModel();
            _mocker = new AutoMocker();

            _sut = _mocker.CreateInstance<GetPassiveProfileDetailsQuery>();
        }

        [Fact]
        public void retrieve_passive_profile_details_from_repository()
        {
            Option<PassiveProfileDetailsModel> model = _sut.Execute(_queryParams);

            _mocker
                .GetMock<IProfileReadOnlyRepository>()
                .Verify(s => s.FindPassiveProfile(_queryParams.ProfileId), Times.Once);
        }

        [Fact]
        public void return_none_if_query_param_is_not_valid()
        {
            // act
            Option<PassiveProfileDetailsModel> optionalModel = _sut.Execute(null);

            // assert
            optionalModel.IsNone.Should().BeTrue();
        }

        [Fact]
        public void return_PassiveProfileDetails()
        {
            // arrange
            _mocker
              .GetMock<IProfileReadOnlyRepository>()
              .Setup(s => s.FindPassiveProfile(It.IsAny<Guid>()))
              .Returns(_model);

            // act
            Option<PassiveProfileDetailsModel> model = _sut.Execute(_queryParams);

            // assert
            model.IsSome.Should().BeTrue();
            model
                .IfSome(m => m.Should().NotBeNull());
        }
    }
}
