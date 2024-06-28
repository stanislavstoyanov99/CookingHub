﻿namespace CookingHub.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    public class PaginatedList<T> : List<T>
    {
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            this.PageIndex = pageIndex;
            this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public int PageIndex { get; private set; }

        public int TotalPages { get; private set; }

        public bool HasPreviousPage
        {
            get
            {
                return this.PageIndex > 1;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return this.PageIndex < this.TotalPages;
            }
        }

        public static async Task<PaginatedList<TEntity>> CreateAsync<TEntity>(IQueryable<TEntity> source, int pageIndex, int pageSize)
             where TEntity : class
        {
            var count = await source.CountAsync();
            var items = await source
                .AsSingleQuery()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<TEntity>(items, count, pageIndex, pageSize);
        }
    }
}
