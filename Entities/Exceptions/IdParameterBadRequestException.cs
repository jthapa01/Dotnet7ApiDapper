namespace Entities.Exceptions;

public class IdParameterBadRequestException : BadRequestException
{
	public IdParameterBadRequestException()
		:base("Parameter ids is null")
	{
	}
}
