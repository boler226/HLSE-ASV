namespace Contracts.Events;

public record UserRegisteredEvent(Guid UserId, string Email, DateTime RegisteredAt);