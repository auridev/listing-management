using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.ValueObjects
{
    public sealed class ListingDetails : IEquatable<ListingDetails>
    {
        public Title Title { get; }
        public MaterialType MaterialType { get; }
        public Weight Weight { get; }
        public Description Description { get; }

        private ListingDetails() { }
        private ListingDetails(
            Title title,
            MaterialType materialType,
            Weight weight,
            Description description)
        {
            Title = title;
            MaterialType = materialType;
            Weight = weight;
            Description = description;
        }

        public static Either<Error, ListingDetails> Create(
            string title,
            int materialTypeId,
            float weight,
            string massUnit,
            string description)
        {
            Either<Error, Title> eitherTitle = Title.Create(title);
            Either<Error, MaterialType> eitherMaterialType = MaterialType.ById(materialTypeId);
            Either<Error, Weight> eitherWeight = Weight.Create(weight, massUnit);
            Either<Error, Description> eitherDescription = Description.Create(description);

            Either<Error, (Title title, MaterialType materialType, Weight weight, Description description)> combined =
                from t in eitherTitle
                from mt in eitherMaterialType
                from w in eitherWeight
                from d in eitherDescription
                select (t, mt, w, d);

            return
                combined.Map(
                    combined =>
                        new ListingDetails(
                            combined.title,
                            combined.materialType,
                            combined.weight,
                            combined.description));
        }

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<ListingDetails>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] ListingDetails other)
            =>
                ValueObjectComparer<ListingDetails>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<ListingDetails>.Instance.GetHashCode();

        public static bool operator ==(ListingDetails left, ListingDetails right)
            =>
                ValueObjectComparer<ListingDetails>.Instance.Equals(left, right);

        public static bool operator !=(ListingDetails left, ListingDetails right)
            =>
                !(left == right);
    }
}
