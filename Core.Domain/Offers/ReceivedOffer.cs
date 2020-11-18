using Core.Domain.Common;
using LanguageExt;
using System;

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
            DateTimeOffset createdDate,
            Option<SeenDate> seenDate)
            : base(id, owner, monetaryValue, createdDate)
        {
            SeenDate = seenDate;
        }

        public void HasBeenSeen(SeenDate seenDate)
        {
            SeenDate = seenDate;
        }
    }
}
