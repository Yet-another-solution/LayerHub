namespace LayerHub.Api.Core.Domain.Exceptions;

public class NotFoundException(string error) : CustomException(error)
{
    public NotFoundException() : this("Not found") { }
}
