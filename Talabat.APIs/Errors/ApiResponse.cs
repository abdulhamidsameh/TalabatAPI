
namespace Talabat.APIs.Errors
{
	public class ApiResponse
	{
		public int StatusCode { get; set; }
		public string? Message { get; set; }

		public ApiResponse(int statusCode, string? message = null)
		{
			StatusCode = statusCode;
			Message = message ?? GetDefaultMessageForStatusCode(statusCode);
		}

		private string? GetDefaultMessageForStatusCode(int statusCode)
		{
			return statusCode switch
			{
				400 => "is sent when no other error is applicable, or if the exact error is unknown or does not have its own error code",
				401 => "that the requested resource requires authentication. The WWW-Authenticate header contains the details of how to perform the authentication",
				404 => "the requested resource does not exist on the server",
				500 => "Errors are the path to the dark side. Errors lead to anger. Anger leads to hate. Hate leads to career change",
				_ => null,
			};
		}
	}
}
