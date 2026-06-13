using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ENyayPath.PICS.Core.Helpers.Pagination
{

    public class CustomPagedAndSortedResultRequestDto<T> : PagedAndSortedResultRequestDto where T : class
    {
        public CustomPagedAndSortedResultRequestDto()
        {

        }

        public virtual string SortBy { get; set; }

        public string Keyword { get; set; }

        public bool IsSearchRegEx { get; set; }

#nullable enable
        public virtual T? Filter { get; set; }
#nullable disable
    }


    public class CustomPagedAndSortedResultRequestDto : PagedAndSortedResultRequestDto //where T : class?
    {
        public CustomPagedAndSortedResultRequestDto()
        {

        }

        public virtual string SortBy { get; set; }

        public string Keyword { get; set; }

        public bool IsSearchRegEx { get; set; }
    }
}
