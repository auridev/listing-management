using System;
using System.Collections.Generic;

namespace Core.Application.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int TotalCount { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(List<T> items, int totalCount, int currentPage, int pageSize)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            if (totalCount < 0)
                throw new ArgumentException(nameof(totalCount));
            if (currentPage < 1)
                throw new ArgumentException(nameof(currentPage));
            if (pageSize < 1)
                throw new ArgumentException(nameof(pageSize));

            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            AddRange(items);
        }

        public static PagedList<T> CreateEmpty()
        {
            return new PagedList<T>(new List<T>(), 0, 1, 1);
        }
    }
}
