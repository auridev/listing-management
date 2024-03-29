﻿using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Functions;

namespace Core.Domain.ValueObjects
{
    public sealed class Template : IEquatable<Template>
    {
        public string Value { get; }

        private Template() { }

        private Template(string value)
        {
            Value = value;
        }

        public static Either<Error, Template> Create(string value)
            =>
                EnsureNonEmpty(value)
                    .Map(value => new Template(value));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<Template>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Template other)
            =>
                ValueObjectComparer<Template>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<Template>.Instance.GetHashCode();

        public static bool operator ==(Template left, Template right)
            =>
                ValueObjectComparer<Template>.Instance.Equals(left, right);

        public static bool operator !=(Template left, Template right)
            =>
                !(left == right);
    }
}
