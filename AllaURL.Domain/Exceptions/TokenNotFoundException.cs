namespace AllaURL.Domain.Exceptions;

public class TokenNotFoundException : Exception
{ 
    public TokenNotFoundException(string message) : base(message) { }

    public TokenNotFoundException(string message, Exception innerException) : base(message, innerException) { }
        
}
