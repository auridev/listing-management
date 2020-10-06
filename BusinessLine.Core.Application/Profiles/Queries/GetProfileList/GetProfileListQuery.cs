using System;
using System.Collections.Generic;

namespace BusinessLine.Core.Application.Profiles.Queries.GetProfileList
{
    public sealed class GetProfileListQuery : IGetProfileListQuery
    {
        private readonly IProfileDataService _dataService;

        public GetProfileListQuery(IProfileDataService dataService)
        {
            _dataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
        }

        public ICollection<ProfileModel> Execute(Guid userId, GetProfileListQueryParams queryParams)
        {
            if (queryParams == null)
                return new ProfileModel[0];

            return _dataService.Get(userId, queryParams);
        }
    }
}
