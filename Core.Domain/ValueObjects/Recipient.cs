﻿using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Functions;

namespace Core.Domain.ValueObjects
{
    public sealed class Recipient : IEquatable<Recipient>
    {
        public Guid UserId { get; }

        private Recipient() { }

        private Recipient(Guid userId)
        {
            UserId = userId;
        }

        public static Either<Error, Recipient> Create(Guid userId)
            =>
                EnsureNonDefault(userId)
                    .Map(userId => new Recipient(userId));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<Recipient>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] Recipient other)
            =>
                ValueObjectComparer<Recipient>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<Recipient>.Instance.GetHashCode();

        public static bool operator ==(Recipient left, Recipient right)
            =>
                ValueObjectComparer<Recipient>.Instance.Equals(left, right);

        public static bool operator !=(Recipient left, Recipient right)
            =>
                !(left == right);
    }
}
