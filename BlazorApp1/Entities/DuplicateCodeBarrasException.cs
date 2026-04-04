namespace BlazorApp1.Entities;

public class DuplicateCodeBarrasException : Exception
{
    public DuplicateCodeBarrasException() { }
    public DuplicateCodeBarrasException(string message) : base(message) { }
    public DuplicateCodeBarrasException(string message, Exception inner) : base(message, inner) { }
}
