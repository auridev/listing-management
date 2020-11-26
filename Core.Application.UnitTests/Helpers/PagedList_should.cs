using Core.Application.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Core.Application.UnitTests.Helpers
{
    public class PagedList_should
    {
        private readonly PagedList<string> _sut;

        public PagedList_should()
        {
            _sut = new PagedList<string>(new List<string>(), 1, 2, 3);
        }

        [Fact]
        public void have_TotalCount_property()
        {
            _sut.TotalCount.Should().Be(1);
        }

        [Fact]
        public void have_CurrentPage_property()
        {
            _sut.CurrentPage.Should().Be(2);
        }

        [Fact]
        public void have_PageSize_property()
        {
            _sut.PageSize.Should().Be(3);
        }

        [Fact]
        public void have_TotalPages_property()
        {
            _sut.TotalPages.Should().Be(1);
        }

        [Fact]
        public void throw_an_exception_during_creation_if_item_list_is_null()
        {
            Action createAction = () => new PagedList<int>(null, 1, 1, 1);

            createAction.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(10, 3, 4)]
        [InlineData(0, 10, 0)]
        [InlineData(3, 5, 1)]
        public void correctly_calculate_TotalPages_property(int totalCount, int pageSize, int expectedTotalPages)
        {
            var sut = new PagedList<long>(new List<long>(), totalCount, 1, pageSize);

            sut.TotalPages.Should().Be(expectedTotalPages);
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, true)]
        [InlineData(10, true)]
        public void correctly_calculate_HasPrevious_property(int currentPage, bool expectedHasPrevious)
        {
            var sut = new PagedList<long>(new List<long>(), 1, currentPage, 1);

            sut.HasPrevious.Should().Be(expectedHasPrevious);
        }

        [Theory]
        [InlineData(10, 1, 3, true)]
        [InlineData(10, 4, 3, false)]
        public void correctly_calculate_HasNext_property(int totalCount, int currentPage, int pageSize, bool expectedHasNext)
        {
            var sut = new PagedList<long>(new List<long>(), totalCount, currentPage, pageSize);

            sut.HasNext.Should().Be(expectedHasNext);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void throw_an_exception_during_creation_if_PageSize_is_0_or_less(int pageSize )
        {
            Action createAction = () => new PagedList<int>(new List<int>(), 1, 1, pageSize);

            createAction.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void throw_an_exception_during_creation_if_TotalCount_is_negative()
        {
            Action createAction = () => new PagedList<int>(new List<int>(), -1, 1, 1);

            createAction.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void throw_an_exception_during_creation_if_CurrentPage_is_0_or_less(int currenPage)
        {
            Action createAction = () => new PagedList<int>(new List<int>(), 1, currenPage, 1);

            createAction.Should().Throw<ArgumentException>();
        }
    }
}
