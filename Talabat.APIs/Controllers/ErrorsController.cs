using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using System;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Controllers
{
	[Route("errors/{code}")]
	[ApiController]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class ErrorsController : ControllerBase
	{
		public ActionResult Error(int code)
		{
			switch(code)
			{
				case 400:
					return BadRequest(new ApiResponse(400));
				case 401:
					return Unauthorized(new ApiResponse(401));
				case 404:
					return NotFound(new ApiResponse(404));
				default:
					return StatusCode(code);
			}

		}
	}
}
