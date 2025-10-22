using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvondaleIslamicCentre.Models
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; } // The current page number
        public int TotalPages { get; private set; } // The total number of pages available

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        // Returns true if there is a page before this one
        public bool HasPreviousPage => PageIndex > 1;

        // Returns true if there is a page after this one
        public bool HasNextPage => PageIndex < TotalPages;

        // Creates a paginated list by running a query on the database
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync(); // Total number of items
            var items = await source
                .Skip((pageIndex - 1) * pageSize) // Skip items from earlier pages
                .Take(pageSize)                   // Get only the items for this page
                .ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
