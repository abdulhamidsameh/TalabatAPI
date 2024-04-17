using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Repostiory.Data;

namespace Talabat.Repostiory
{
	public class GenericRepository<T> : IGenricRepository<T> where T : BaseEntity
	{
		private readonly StoreContext _dbContext;

		public GenericRepository(StoreContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
				return await _dbContext.Set<T>().ToListAsync();
		}

		public async Task<T?> GetAsync(int id)
		{
			return await _dbContext.Set<T>().FindAsync(id);
		}

		public async Task<T?> GetwithSpecAsync(ISpecifications<T> spec)
		{
			return await ApplySecification(spec).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
		{
			return await ApplySecification(spec).ToListAsync();
		}
		private IQueryable<T> ApplySecification(ISpecifications<T> spec) 
		{
			return SpecifactionsEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
		}
	}
}
