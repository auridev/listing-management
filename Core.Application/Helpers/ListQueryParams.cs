namespace Core.Application.Helpers
{
    public class ListQueryParams
    {
        private readonly int _maxPageSize = 25;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > _maxPageSize)
                    ? _maxPageSize
                    : value;
            }
        }
    }
}
