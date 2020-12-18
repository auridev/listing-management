using Core.Application.Listings.Queries.GetMyListings;
using Core.Application.Listings.Queries.GetPublicListings;
using Dapper;
using FluentAssertions;
using Persistence.Queries.Listings.Factory;
using System;
using Xunit;

namespace Persistence.Queries.UnitTests.Listings.Factory
{
    public class ListingQueryFactory_should
    {

        [Fact]
        public void have_CreateSqlForMyListings_method()
        {
            var sut = new ListingQueryFactory();

            string text = sut.CreateSqlForMyListings(new GetMyListingsQueryParams());

            text.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void create_text_that_queries_all_listing_tables_for_my_listings()
        {
            var sut = new ListingQueryFactory();

            string text = sut.CreateSqlForMyListings(new GetMyListingsQueryParams());

            text.Should().ContainAll(
                "[dbo].[new_listings]",
                "[dbo].[active_listings]",
                "[dbo].[passive_listings]",
                "[dbo].[closed_listings]",
                "[dbo].[suspicious_listings]");
        }

        [Fact]
        public void have_CreateParametersForMyListings_method()
        {   
            // arrange
            var userId = Guid.NewGuid();
            var queryParameters = new GetMyListingsQueryParams()
            {
                PageNumber = 1,
                PageSize = 15
            };
            var sut = new ListingQueryFactory();

            // act
            DynamicParameters parameters = sut.CreateParametersForMyListings(userId, queryParameters);

            // assert
            parameters.Should().NotBeNull();
        }

        [Fact]
        public void have_create_correct_parameters_for_my_listings()
        {
            // arrange
            var userId = Guid.NewGuid();
            var queryParameters = new GetMyListingsQueryParams()
            {
                PageNumber = 2,
                PageSize = 15
            };
            var sut = new ListingQueryFactory();

            // act
            DynamicParameters parameters = sut.CreateParametersForMyListings(userId, queryParameters);

            // assert
            parameters.Get<Guid>("owner").Should().Be(userId);
            parameters.Get<int>("offset").Should().Be(15);
            parameters.Get<int>("page_size").Should().Be(15);
        }


        [Fact]
        public void have_CreateSqlForPublicListings_method()
        {
            var sut = new ListingQueryFactory();

            string sql = sut.CreateSqlForPublicListings(new GetPublicListingsQueryParams());

            sql.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void create_sql_with_inner_join_if_OnlyWithMyOffers_is_true()
        {
            var sut = new ListingQueryFactory();
            var parameters = new GetPublicListingsQueryParams()
            {
                OnlyWithMyOffers = true
            };

            string sql = sut.CreateSqlForPublicListings(parameters);
            sql.Should().Contain("inner join");
        }

        [Fact]
        public void create_sql_with_left_join_if_OnlyWithMyOffers_is_false()
        {
            var sut = new ListingQueryFactory();
            var parameters = new GetPublicListingsQueryParams()
            {
                OnlyWithMyOffers = false
            };

            string sql = sut.CreateSqlForPublicListings(parameters);
            sql.Should().Contain("left join");
        }

        [Fact]
        public void create_sql_with_search_condition_if_SearchParam_is_not_empty()
        {
            var sut = new ListingQueryFactory();
            var parameters = new GetPublicListingsQueryParams()
            {
                SearchParam = "aaa"
            };

            string sql = sut.CreateSqlForPublicListings(parameters);
            sql.Should().Contain("[title] like concat('%',@search,'%')");
        }

        [Fact]
        public void create_sql_without_search_condition_if_SearchParam_is_empty()
        {
            var sut = new ListingQueryFactory();
            var parameters = new GetPublicListingsQueryParams()
            {
                SearchParam = ""
            };

            string sql = sut.CreateSqlForPublicListings(parameters);
            sql.Should().NotContain("[title] like concat('%',@search,'%')");
        }

        [Fact]
        public void have_CreateParametersForPublicListings_method()
        {
            // arrange
            var userId = Guid.NewGuid();
            var queryParameters = new GetPublicListingsQueryParams();
            var sut = new ListingQueryFactory();

            // act
            DynamicParameters parameters = sut.CreateParametersForPublicListings(userId, queryParameters);

            // assert
            parameters.Should().NotBeNull();
        }

        [Fact]
        public void have_create_correct_parameters_for_public_listings()
        {
            // arrange
            var userId = Guid.NewGuid();
            var queryParameters = new GetPublicListingsQueryParams()
            {
                SearchParam = "asd",
                OnlyWithMyOffers = true,
                PageNumber = 3,
                PageSize = 20
            };
            var sut = new ListingQueryFactory();

            // act
            DynamicParameters parameters = sut.CreateParametersForPublicListings(userId, queryParameters);

            // assert
            parameters.Get<Guid>("owner").Should().Be(userId);
            parameters.Get<int>("offset").Should().Be(40);
            parameters.Get<int>("page_size").Should().Be(20);
            parameters.Get<string>("search").Should().Be("asd");
        }
    }
}
