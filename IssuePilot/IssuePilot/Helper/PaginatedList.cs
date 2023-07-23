using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Code ist von https://docs.microsoft.com/de-de/aspnet/core/data/ef-mvc/sort-filter-page?view=aspnetcore-3.1
namespace IssuePilot.Helper
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
        public static async Task<PaginatedList<T>> CreateFromListAsync(List<T> source, int pageIndex, int pageSize)
        {
            var count = await Task.FromResult(source.Count);
            var items = await Task.FromResult(source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
