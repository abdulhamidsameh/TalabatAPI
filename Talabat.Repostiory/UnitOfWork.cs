using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repostiory.Data;

namespace Talabat.Repostiory
{
	internal class UnitOfWork : IUnitOfWork
	{
		private readonly StoreContext _dbContext;

		private Dictionary<string, GenericRepository<BaseEntity>> _repositories;
		public UnitOfWork(StoreContext dbContext)
        {
			_dbContext = dbContext;
			_repositories = _repositories = new Dictionary<string, GenericRepository<BaseEntity>>();
		}
        public async Task<int> CompleteAsync()
			=> await _dbContext.SaveChangesAsync();

		public async ValueTask DisposeAsync()
			=> await _dbContext.DisposeAsync();

		public IGenricRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
		{
			var key = typeof(TEntity).Name;
			if(!_repositories.ContainsKey(key))
			{
				var repository = new GenericRepository<TEntity>(_dbContext) as GenericRepository<BaseEntity>;
				_repositories.Add(key,repository);
			}
			return (IGenricRepository<TEntity>) _repositories[key];


		}
	}
}
