using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DotNetGooglePagination
{
    public static class Paging
    {
        public static async Task<PagingModel<T>> DataPagingAsync<T>(this IQueryable<T> queryable, int pageSize, int currentPage)
        {
            int skip = (currentPage - 1) * pageSize;

            int totalItemsCount = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling((decimal)(totalItemsCount + 1) / pageSize);

            var startPage = currentPage - 5;
            int endPage = currentPage + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            return new PagingModel<T>()
            {
                DataList = queryable.Skip(skip).Take(pageSize),
                CurrentPage = currentPage,
                TotalPages = totalPages,
                StartPage = startPage,
                EndPage = endPage
            };

        }

    }
}
