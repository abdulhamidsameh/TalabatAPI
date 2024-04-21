﻿using Talabat.APIs.Dtos;

namespace Talabat.APIs.Helper
{
	public class Pagination<T>
	{
        public int PageIndex { get; set; }
		public int PageSize { get; set; }
		public int Count { get; set; }
        public IReadOnlyList<T> Date { get; set; }


		public Pagination(int pageIndex, int pageSize,int count, IReadOnlyList<T> data)
		{
			PageIndex = pageIndex;
			PageSize = pageSize;
			Date = data;
			Count = count;
		}
	}
}