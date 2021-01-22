using Common.Helpers;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Result;
using static LanguageExt.Prelude;

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

        private static readonly Dictionary<int, MaterialType> _allMaterialTypes = new Dictionary<int, MaterialType>
        {
            { NonFerrous.Id, NonFerrous },
            { Ferrous.Id, Ferrous },
            { Paper.Id, Paper},
            { Plastic.Id, Plastic},
            { Glass.Id, Glass},
            { Electronics.Id, Electronics},
            { TyresAndRubber.Id, TyresAndRubber},
            { Textiles.Id, Textiles},
            { Wood.Id, Wood}
        };

        public static Either<Error, MaterialType> ById(int id)
            =>
                (_allMaterialTypes.ContainsKey(id))
                    ? Right(_allMaterialTypes[id])
                    : Left(Invalid<MaterialType>("unknown material type"));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<MaterialType>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] MaterialType other)
            =>
                ValueObjectComparer<MaterialType>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<MaterialType>.Instance.GetHashCode();

        public static bool operator ==(MaterialType left, MaterialType right)
            =>
                ValueObjectComparer<MaterialType>.Instance.Equals(left, right);

        public static bool operator !=(MaterialType left, MaterialType right)
            =>
                !(left == right);
    }
}
