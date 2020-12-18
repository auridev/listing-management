using Core.Application.Profiles.Queries.GetProfileList;
using Dapper;
using FluentAssertions;
using Persistence.Queries.Profiles.Factory;
using Xunit;

namespace Persistence.Queries.UnitTests.Profiles.Factory
{
    public class ProfileQueryFactory_should
    {
        private readonly ProfileQueryFactory _sut;

        public ProfileQueryFactory_should()
        {
            _sut = new ProfileQueryFactory();
        }


        [Fact]
        public void have_CreateText_method()
        {
            // arrange
            var queryParams = new GetProfileListQueryParams()
            {
                IsActive = true,
                Search = "xxx",
                PageNumber = 1,
                PageSize = 10
            };

            // act
            string queryText = _sut.CreateText(queryParams);

            // assert
            queryText.Should().NotBeNull();
        }

        [Fact]
        public void create_query_which_points_to_passive_profiles_if_IsActive_is_false()
        {
            var queryParams = new GetProfileListQueryParams()
            {
                IsActive = false
            };

            string queryText = _sut.CreateText(queryParams);

            queryText.Should().Contain("[dbo].[passive_profiles]");
            queryText.Should().NotContain("[dbo].[active_profiles]");
        }


        [Fact]
        public void create_query_which_points_to_active_profiles_if_IsActive_is_true()
        {
            var queryParams = new GetProfileListQueryParams()
            {
                IsActive = true
            };

            string queryText = _sut.CreateText(queryParams);

            queryText.Should().Contain("[dbo].[active_profiles]");
            queryText.Should().NotContain("[dbo].[passive_profiles]");
        }

        [Fact]
        public void create_query_which_points_to_active_and_passive_profiles_if_IsActive_has_no_value()
        {
            var queryParams = new GetProfileListQueryParams()
            {
                IsActive = null
            };

            string queryText = _sut.CreateText(queryParams);

            queryText.Should().Contain("[dbo].[active_profiles]");
            queryText.Should().Contain("[dbo].[passive_profiles]");
        }

        [Fact]
        public void have_CreateParameters_method()
        {
            // arrange
            var queryParams = new GetProfileListQueryParams()
            {
                IsActive = false,
                Search = "aaa",
                PageNumber = 1,
                PageSize = 10
            };

            // act
            DynamicParameters parameters = _sut.CreateParameters(queryParams);

            // assert
            parameters.Should().NotBeNull();
        }

        [Fact]
        public void create_correct_parameters()
        {
            // arrange
            var queryParams = new GetProfileListQueryParams()
            {
                IsActive = true,
                Search = "abc",
                PageNumber = 11,
                PageSize = 25
            };

            // act
            DynamicParameters parameters = _sut.CreateParameters(queryParams);

            // assert
            parameters.Get<string>("search").Should().Be("abc");
            parameters.Get<int>("offset").Should().Be(250);
            parameters.Get<int>("page_size").Should().Be(25);
        }

    }
}
