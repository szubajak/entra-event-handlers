using AutoFixture;
using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Security.Decryptors;
using FluentAssertions;

namespace Entra.EventHandlers.Security.UnitTests.Decryptors;

public class PasswordContextPayloadTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task Validate_NullPassword_ShouldThrow()
    {
        // Arrange
        var payload = new PasswordContextPayload()
        {
            Password = null!,
            Nonce = _fixture.Create<string>()
        };

        // Act
        Action act = () => payload.Validate();

        // Assert
        act.Should().Throw<EntraValidationException>()
           .WithMessage("The decrypted password context does not contain a valid password.");
    }

    [Fact]
    public async Task Validate_NullNonce_ShouldThrow()
    {
        // Arrange
        var payload = new PasswordContextPayload()
        {
            Password = _fixture.Create<string>(),
            Nonce = null!
        };

        // Act
        Action act = () => payload.Validate();

        // Assert
        act.Should().Throw<EntraValidationException>()
           .WithMessage("The decrypted password context does not contain a valid nonce.");
    }
}
