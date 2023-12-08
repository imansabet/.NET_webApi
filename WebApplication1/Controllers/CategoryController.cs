using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class CategoryController: ControllerBase
    {
        private static List<CategoryModel> categories = new List<CategoryModel>()
        {
            new CategoryModel(){Id = 1,Name = "dram" },
            new CategoryModel(){Id = 2,Name = "war" },
            new CategoryModel(){Id = 3,Name = "history" },
            new CategoryModel(){Id = 4,Name = "horror" },
        };

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(categories);
        }

        [HttpPost]
        public IActionResult AddCategory(CategoryModel obj) 
        {
            categories.Add(obj);
            return Ok("New Category Created Successfully");
            
        }
        
        [HttpPut]
        public IActionResult EditCategory(CategoryModel obj) 
        {
            var selectedCategory = categories.FirstOrDefault(u => u.Id == obj.Id);
            selectedCategory.Name  = obj.Name;
            selectedCategory.Id = obj.Id;
            return Ok("Category Updated Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id) 
        {
            var selectedCategory = categories.FirstOrDefault(u => u.Id == id);
            categories.Remove(selectedCategory);
            return Ok(" DONE  . .  . ");

        }



    }
}
