using Common.Helpers;
using Core.Domain.ValueObjects;
using LanguageExt;
using System;
using static Common.Helpers.Result;
using static LanguageExt.Prelude;

namespace Core.Domain.Offers
{
    public class ReceivedOffer : Offer
    {
        //// this is to overcome current ORM limitations
        public SeenDate ___efCoreSeenDate { get; private set; }
        public Option<SeenDate> SeenDate
        {
            get
            {
                return ___efCoreSeenDate == null ? Option<SeenDate>.None : ___efCoreSeenDate;
            }
            private set
            {
                value
                    .Some(v =>
                    {
                        ___efCoreSeenDate = v;
                    })
                    .None(() =>
                    {
                        ___efCoreSeenDate = null;
                    });
            }
        }

        private ReceivedOffer() { }
        public ReceivedOffer(Guid id,
            Owner owner,
            MonetaryValue monetaryValue,
            DateTimeOffset createdDate)
            : base(id, owner, monetaryValue, createdDate)
        {
            SeenDate = Option<SeenDate>.None;
        }

        public Either<Error, Unit> HasBeenSeen(SeenDate seenDate)
        {
            if (seenDate == null)
                return Invalid<Unit>(nameof(seenDate));

            SeenDate = seenDate;

            return Success(unit);
        }
    }
}
