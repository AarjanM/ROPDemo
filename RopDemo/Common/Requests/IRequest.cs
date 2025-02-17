namespace RopDemo.Common.Requests;

public interface IRequest
{
    public abstract ValidationResult Validate();
}