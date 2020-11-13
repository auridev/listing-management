using LanguageExt;
using System;

namespace Core.Application.Profiles.Queries.GetProfileDetails
{
    public sealed class GetProfileDetailsQuery : IGetProfileDetailsQuery
    {
        private readonly IProfileDataService _dataService;

        public GetProfileDetailsQuery(IProfileDataService dataService)
        {
            _dataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
        }

        public Option<ProfileDetailsModel> Execute(GetProfileDetailsQueryParams queryParams)
        {
            if (queryParams == null)
                return Option<ProfileDetailsModel>.None;

            ProfileDetailsModel model = _dataService.Find(queryParams.ProfileId);

            return model == null
                ? Option<ProfileDetailsModel>.None
                : model;
        }
    }
}
