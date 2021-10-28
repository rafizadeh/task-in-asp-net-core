using System;

namespace Application.Common.Base.Models
{
    public class PaginationModel
    {
        public PaginationModel(int totalCount, int take, int page)
        {
            Page = page;
            PageCount = (int)Math.Ceiling((decimal)totalCount / take);
            Prev = page > 1 ? page - 1 : 1;
            Next = page == PageCount ? PageCount : page + 1;
            StartIndex = page > 5 ? page - 5 : 1;
            EndIndex = (page < PageCount - 5) ? page + 5 : PageCount;
        }

        public int Page { get; set; }

        public int PageCount { get; set; }

        public int Prev { get; set; }

        public int Next { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }
    }
}