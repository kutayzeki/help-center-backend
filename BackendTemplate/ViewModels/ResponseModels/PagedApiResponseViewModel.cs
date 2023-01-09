namespace BackendTemplate.ViewModels.ResponseModels
{
    public class PagedApiResponseViewModel<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}

/*
 * public IActionResult GetProducts(int pageNumber, int pageSize)
{
    // Retrieve the total number of products
    var totalRecords = _context.Products.Count();

    // Calculate the total number of pages
    var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

    // Retrieve the paginated products
    var products = _context.Products
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToList();

    // Create the view model
    var model = new PagedApiResponseViewModel<Product>
    {
        PageNumber = pageNumber,
        PageSize = pageSize,
        TotalPages = totalPages,
        TotalRecords = totalRecords,
        Data = products
    };

    // Return the view model in the response
    return Ok(model);
}
}*/