using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ENyayPath.PICS.Core.Helpers.Pagination
{

    public interface ISortedResultRequest
    {
        //
        // Summary:
        //     Sorting information. Should include sorting field and optionally a direction
        //     (ASC or DESC) Can contain more than one field separated by comma (,).
        string Sorting { get; set; }
    }

    public interface IPagedResultRequest : ILimitedResultRequest
    {
        //
        // Summary:
        //     Skip count (beginning of the page).
        int SkipCount { get; set; }
    }

    public interface IPagedAndSortedResultRequest : IPagedResultRequest, ILimitedResultRequest, ISortedResultRequest
    {
    }

    public interface ILimitedResultRequest
    {
        //
        // Summary:
        //     Max expected result count.
        int MaxResultCount { get; set; }
    }

    public class LimitedResultRequestDto : ILimitedResultRequest
    {
        [Range(1, int.MaxValue)]
        public virtual int MaxResultCount { get; set; } = 10;
    }

    [Serializable]
    public class PagedResultRequestDto : LimitedResultRequestDto, IPagedResultRequest, ILimitedResultRequest
    {
        [Range(0, int.MaxValue)]
        public virtual int SkipCount { get; set; }
    }

    [Serializable]
    public class PagedAndSortedResultRequestDto : PagedResultRequestDto, IPagedAndSortedResultRequest, IPagedResultRequest, ILimitedResultRequest, ISortedResultRequest
    {
        public virtual string Sorting { get; set; }
    }
}
