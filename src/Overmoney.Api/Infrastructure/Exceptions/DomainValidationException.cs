namespace Overmoney.Api.Infrastructure.Exceptions;

public sealed class DomainValidationException(string message) : Exception(message) 
{ }
