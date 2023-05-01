using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Project.Entities.ViewModels
{
    public class Pager
    {
        //pagination
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPage { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }

        public Pager()
        {

        }
        public Pager(int totalItems, int page, int pageSize = 3)
        {
            int totalpage = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            int currentPage = page;

            int startPage = currentPage - 2;
            int endPage = currentPage + 2;

            if (startPage <= 0)
            {
                endPage = endPage - (startPage);
                startPage = 1;

            }

            if (endPage > totalpage)
            {
                endPage = totalpage;
                if (endPage > pageSize)
                {
                    startPage = endPage - 4;
                }
            }

            TotalItems = totalItems;
            TotalPage = totalpage;
            PageSize = pageSize;
            StartPage = startPage;
            EndPage = endPage;
            CurrentPage = currentPage;
        }
    }
}
