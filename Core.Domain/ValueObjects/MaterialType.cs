using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using U2U.ValueObjectComparers;

namespace Core.Domain.ValueObjects
{
    public sealed class MaterialType : IEquatable<MaterialType>
    {
        public static readonly MaterialType NonFerrous = new MaterialType(10, "NonFerrous");
        public static readonly MaterialType Ferrous = new MaterialType(20, "Ferrous");
        public static readonly MaterialType Paper = new MaterialType(30, "Paper");
        public static readonly MaterialType Plastic = new MaterialType(40, "Plastic");
        public static readonly MaterialType Glass = new MaterialType(50, "Glass");
        public static readonly MaterialType Electronics = new MaterialType(60, "Electronics");
        public static readonly MaterialType TyresAndRubber = new MaterialType(70, "TyresAndRubber");
        public static readonly MaterialType Textiles = new MaterialType(80, "Textiles");
        public static readonly MaterialType Wood = new MaterialType(90, "Wood");

        public string Name { get; }
        public int Id { get; }

        private MaterialType() { }

        private MaterialType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        private static readonly MaterialType[] _allMaterialTypes = new MaterialType[]
        {
            NonFerrous,
            Ferrous,
            Paper,
            Plastic,
            Glass,
            Electronics,
            TyresAndRubber,
            Textiles,
            Wood
        };

        public static MaterialType ById(int id)
        {
            // dont know how to handle this properly without exception
            return _allMaterialTypes.First(mt => mt.Id == id);
        }

        public override bool Equals([AllowNull] object obj)
            => ValueObjectComparer<MaterialType>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] MaterialType other)
            => ValueObjectComparer<MaterialType>.Instance.Equals(this, other);

        public override int GetHashCode()
            => ValueObjectComparer<MaterialType>.Instance.GetHashCode();

        public static bool operator ==(MaterialType left, MaterialType right)
            => ValueObjectComparer<MaterialType>.Instance.Equals(left, right);

        public static bool operator !=(MaterialType left, MaterialType right)
            => !(left == right);


    }
}
