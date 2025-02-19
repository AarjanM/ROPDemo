namespace RopDemo.Domain;

internal sealed record AccountCreatedDomainEvent(
    AccountId AccountId,
    string EmailAddress,
    string DisplayName) : IDomainEvent;