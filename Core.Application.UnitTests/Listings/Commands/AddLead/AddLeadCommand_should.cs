using Common.Dates;
using Common.Helpers;
using Core.Application.Listings.Commands;
using Core.Application.Listings.Commands.AddLead;
using Core.Domain.Listings;
using FluentAssertions;
using LanguageExt;
using Moq;
using Moq.AutoMock;
using System;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.AddLead
{
    public class AddLeadCommand_should
    {
        private readonly AddLeadCommand _sut;
        private readonly AddLeadModel _model;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly ActiveListing _activeListing;
        private readonly DateTimeOffset _createdDate = DateTimeOffset.UtcNow;

        private readonly AutoMocker _mocker;
        private Either<Error, Unit> _executionResult;

        public AddLeadCommand_should()
        {
            _mocker = new AutoMocker();
            _model = _mocker.CreateInstance<AddLeadModel>();
            _activeListing = DummyData.ActiveListing_1;

            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(It.IsAny<Guid>()))
                .Returns(Option<ActiveListing>.Some(_activeListing));

            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetCurrentUtcDateTime())
                .Returns(_createdDate);

            _sut = _mocker.CreateInstance<AddLeadCommand>();
        }

        private void Execute_Successfully()
        {
            _executionResult = _sut.Execute(_userId, _model);
        }

        private void Execute_WithNonExistingListing()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Setup(r => r.FindActive(It.IsAny<Guid>()))
                .Returns(Option<ActiveListing>.None);

            _executionResult = _sut.Execute(_userId, _model);
        }

        private void Execute_WithFailedLeadCreation()
        {
            // default DateTimeOffset instance causes Lead value object creation to fail
            _mocker
                .GetMock<IDateTimeService>()
                .Setup(s => s.GetCurrentUtcDateTime())
                .Returns((DateTimeOffset)default);

            _executionResult = _sut.Execute(_userId, _model);
        }

        private void Execute_WithFailedAddLead()
        {
            // trying to add a lead by listing owner forces AddLead to fail
            var listingOwnerId = _activeListing.Owner.UserId;
            _executionResult = _sut.Execute(listingOwnerId, _model);
        }

        private void VerifyChangesNotPersisted()
        {
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Update(It.IsAny<ActiveListing>()), Times.Never);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Never);
        }

        [Fact]
        public void return_EitherRight_on_success()
        {
            // act
            Execute_Successfully();

            // assert
            _executionResult
                .Right(u => u.Should().NotBeNull())
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        [Fact]
        public void persist_changes_on_success()
        {
            // act
            Execute_Successfully();

            // assert
            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Update(It.Is<ActiveListing>(activeListing => activeListing.Id == _activeListing.Id)), Times.Once);

            _mocker
                .GetMock<IListingRepository>()
                .Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_active_listing_does_not_exist()
        {
            // act
            Execute_WithNonExistingListing();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.NotFound>();
                    error.Message.Should().Be("active listing not found");
                });
        }

        [Fact]
        public void not_persist_changes_when_active_listing_does_not_exist()
        {
            // act
            Execute_WithNonExistingListing();

            // assert
            VerifyChangesNotPersisted();
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_Lead_creation_failed()
        {
            // act
            Execute_WithFailedLeadCreation();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("invalid dateTimeOffset");
                });
        }

        [Fact]
        public void not_persist_changes_when_Lead_creation_failed()
        {
            // act
            Execute_WithFailedLeadCreation();

            // assert
            VerifyChangesNotPersisted();
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_AddLead_failed()
        {
            // act
            Execute_WithFailedAddLead();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("cannot accept leads from the listing owner");
                });
        }

        [Fact]
        public void not_persist_changes_when_AddLead_failed()
        {
            // act
            Execute_WithFailedAddLead();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_DefaultUserId()
        {
            _executionResult = _sut.Execute(default, _model);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_user_id_is_default()
        {
            // act
            ExecuteWith_DefaultUserId();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("invalid guid");
                });
        }

        [Fact]
        public void not_persist_changes_when_user_id_is_default()
        {
            // act
            ExecuteWith_DefaultUserId();

            // assert
            VerifyChangesNotPersisted();
        }

        private void ExecuteWith_NullModel()
        {
            _executionResult = _sut.Execute(_userId, null);
        }

        [Fact]
        public void return_EitherLeft_with_proper_error_when_model_is_null()
        {
            // act
            ExecuteWith_NullModel();

            // assert
            _executionResult
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error =>
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be("cannot be null");
                });
        }

        [Fact]
        public void not_persist_changes_when_model_is_null()
        {
            // act
            ExecuteWith_NullModel();

            // assert
            VerifyChangesNotPersisted();
        }
    }
}
