namespace BackendTemplate.Core.Helpers.ResponseModels
{
    public class ApiResponseViewModel
    {
        public string Id { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

}

/* 
 
public IActionResult PostProduct(Product product)
{
    // Save the product to the database
    _context.Products.Add(product);
    _context.SaveChanges();

    // Create the view model
    var model = new ApiResponseViewModel
    {
        Id = product.Id,
        IsSuccess = true,
        Message = "Product created successfully"
    };

    // Return the view model in the response
    return Ok(model);
}

 */