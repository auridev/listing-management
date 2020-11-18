using Core.Application.Helpers;

namespace Core.Application.Profiles.Queries.GetProfileList
{
    public class GetProfileListQueryParams : ListQueryParams
    {
        public string Search { get; set; }
        public bool? IsActive { get; set; }
    }
}
