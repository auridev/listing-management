using Common.Helpers;
using LanguageExt;
using System;
using System.Diagnostics.CodeAnalysis;
using U2U.ValueObjectComparers;

namespace Core.Domain.ValueObjects
{
    public sealed class ImageContext : IEquatable<ImageContext>
    {
        public ImageReference Reference { get; }

        // TODO 
        // Fix eqiality comparison is incorrect

        [Ignore]
        public ImageContent Content { get; }

        private ImageContext() { }

        private ImageContext(ImageReference reference, ImageContent content)
        {
            Reference = reference;
            Content = content;
        }

        public static Either<Error, ImageContext> Create(
            Guid id, 
            Guid parentReference, 
            string fileName, 
            byte[] content)
        {
            Either<Error, ImageReference> eitherImageReference = ImageReference.Create(id, parentReference, fileName, content.Length);
            Either<Error, ImageContent> eitherImageContent = ImageContent.Create(fileName, content);

            Either<Error, ImageContext> result =
            (
                from imageReference in eitherImageReference
                from imageContent in eitherImageContent
                select (imageReference, imageContent)
            ).Map(
                combined =>
                    new ImageContext(
                        combined.imageReference,
                        combined.imageContent));

            return result;
        }


        public override bool Equals([AllowNull] object obj)
             =>
                ValueObjectComparer<ImageContext>.Instance.Equals(this, obj);

        public bool Equals([AllowNull] ImageContext other)
            =>
                ValueObjectComparer<ImageContext>.Instance.Equals(this, other);

        public override int GetHashCode()
            =>
                ValueObjectComparer<ImageContext>.Instance.GetHashCode();

        public static bool operator ==(ImageContext left, ImageContext right)
            =>
                ValueObjectComparer<ImageContext>.Instance.Equals(left, right);

        public static bool operator !=(ImageContext left, ImageContext right)
            =>
                !(left == right);
    }
}
