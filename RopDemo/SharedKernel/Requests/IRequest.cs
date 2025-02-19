namespace RopDemo.SharedKernel.Requests;

public interface IRequest
{
    public abstract ValidationResult Validate();
}