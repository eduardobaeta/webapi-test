using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("/v1/products")]
public class ProductController : Controller
{
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Product>>> GetAll([FromServices] DataContext context)
    {
        var products = await context.Products
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();
        return products;
    }

    [HttpGet]
    [Route("{skip:int}/{take:int}")]
    public async Task<ActionResult> GetPaginanation([FromServices] DataContext context, 
        [FromRoute] int skip = 0, 
        [FromRoute] int take = 10)
    {
        var products = await context.Products
            .Include(x => x.Category)
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync();
        
        return Ok(new
        {
            quantity = products.Count(),
            products
        });
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, Guid id)
    {
        var product = await context.Products
            .Include(x => x.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return product;

        return BadRequest(product);
    }

    [HttpGet]
    [Route("categories/{id:guid}")]
    public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, Guid id)
    {
        var products = await context.Products
            .Where(x => x.CategoryId == id)
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();
        return Ok(products);
    }
    
    [HttpPost]
    [Route("")]
    public async Task<ActionResult<Product>> Post([FromServices] DataContext context, [FromBody] Product model)
    {
        if(ModelState.IsValid){
            context.Products.Add(model);
            await context.SaveChangesAsync();

            return context.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == model.Id) ?? throw new InvalidOperationException();
        }

        return BadRequest(ModelState);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<ActionResult<Product>> Delete([FromServices] DataContext context, Guid id)
    {
        var product = await context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        context.Products.Remove(product);
        await context.SaveChangesAsync();

        return product;
    }
}