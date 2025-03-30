namespace LayerHub.Api.Core.Domain.Exceptions;

public abstract class CustomException(string error) : Exception(error);