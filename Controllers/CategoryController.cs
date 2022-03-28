using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("/v1/categories")]
public class CategoryController : Controller
{

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context){
        var categories = await context.Categories.ToListAsync();
        return categories;
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<Category>> Post([FromServices] DataContext context, [FromBody] Category model){
        if(ModelState.IsValid){
            context.Categories.Add(model);
            await context.SaveChangesAsync();
            return model;
        }

        else{
            return BadRequest(ModelState);
        }   
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<ActionResult<Category>> Delete([FromServices] DataContext context, Guid id){
        
        var category = context.Categories.FirstOrDefault(x => x.Id == id);
        context.Categories.Remove(category);
        await context.SaveChangesAsync();

        return Ok(category);
    }

}