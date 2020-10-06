using BusinessLine.Core.Application.Helpers;

namespace BusinessLine.Core.Application.Profiles.Queries.GetProfileList
{
    public class GetProfileListQueryParams : ListQueryParams
    {
        public string Search { get; set; }
        public bool? IsActive { get; set; }
    }
}
