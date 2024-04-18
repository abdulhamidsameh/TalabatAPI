using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Repostiory.Data;

namespace Talabat.APIs.Controllers
{
	public class BuggyController : BaseApiController
	{
		private readonly StoreContext _dbContext;

		public BuggyController(StoreContext dbContext)
        {
			_dbContext = dbContext;
		}
        
		[HttpGet("notfound")] // GET: api/Buggy/notfound
		public ActionResult GetNotFoundRequest()
		{
			var product = _dbContext.Products.Find(100);
			if(product is null)
				return NotFound();
			
			else
				return Ok(product); 
		}

		[HttpGet("servererror")] // GET : api/Buggy/servererror
		public ActionResult GetServerErrorRequest()
		{
			var product = _dbContext.Products.Find(100);
			var productToReturn = product.ToString(); // Will throw Exception [Null Reference Exception]
			
			return Ok(productToReturn);
		}

		[HttpGet("badrequest")] // GET : api/Buggy/badrequest
		public ActionResult GetBadRequest()
		{
			return BadRequest();
		}

		[HttpGet("badrequest/{id}")] // GET : api/Buggy/badrequest/5
		public ActionResult GetBadRequest(int id)
		{
			return Ok();
		}
	}
}
