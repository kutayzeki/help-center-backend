using BackendTemplate.ViewModels;

namespace BackendTemplate.ViewModels
{
    public class PagedResult<T>
    {
        public List<T>? Data { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public int FirstRowOnPage
        {
            get { return (CurrentPage - 1) * PageSize + 1; }
        }

        public int LastRowOnPage
        {
            get { return Math.Min(CurrentPage * PageSize, RowCount); }
        }
    }
}

/*public pagedresult<product> getproducts(int page, int pagesize)
{
    var products = _dbcontext.products
        .orderby(p => p.name)
        .skip((page - 1) * pagesize)
        .take(pagesize)
        .tolist();

    var totalcount = _dbcontext.products.count();

    var result = new pagedresult<product>
    {
        results = products,
        currentpage = page,
        pagesize = pagesize,
        rowcount = totalcount,
        pagecount = (int)math.ceiling((double)totalcount / pagesize)
    };

    return result;
}*/