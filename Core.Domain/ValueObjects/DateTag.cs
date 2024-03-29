﻿using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;
using static Common.Helpers.Functions;

namespace Core.Domain.ValueObjects
{
    public class DateTag : IEquatable<DateTag>
    {
        public int Year { get; }

        public int Month { get; }

        public int Day { get; }

        protected DateTag() { }

        private DateTag(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public static Either<Error, DateTag> Create(DateTimeOffset value)
            =>
                EnsureNonDefault(value)
                    .Map(value => new DateTag(value.Year, value.Month, value.Day));

        public override bool Equals([AllowNull] object obj)
            =>
                ValueObjectComparer<DateTag>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] DateTag other)
            =>
                ValueObjectComparer<DateTag>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<DateTag>.Instance.GetHashCode();

        public static bool operator ==(DateTag left, DateTag right)
            =>
                ValueObjectComparer<DateTag>.Instance.Equals(left, right);

        public static bool operator !=(DateTag left, DateTag right)
            =>
                !(left == right);
    }
}
