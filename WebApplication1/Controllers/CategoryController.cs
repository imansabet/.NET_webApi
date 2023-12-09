using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public IActionResult AddCategory(CategoryModel obj) 
        {
            categories.Add(obj);
            return Ok("New Category Created Successfully");
            
        }
        
        [HttpPut]
        [Authorize]
        public IActionResult EditCategory(CategoryModel obj) 
        {
            var selectedCategory = categories.FirstOrDefault(u => u.Id == obj.Id);
            selectedCategory.Name  = obj.Name;
            selectedCategory.Id = obj.Id;
            return Ok("Category Updated Successfully");
        }

        
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteCategory(int id) 
        {
            var selectedCategory = categories.FirstOrDefault(u => u.Id == id);
            categories.Remove(selectedCategory);
            return Ok(" DONE  . .  . ");

        }

        private static string _username;
        private static string _password;

        [HttpPost]
        public IActionResult Register(string username, string password) 
        {
            _username = username;
            _password = BCrypt.Net.BCrypt.HashPassword(password);
            return Ok(new { username = _username, password = _password });
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == _username && BCrypt.Net.BCrypt.Verify(password, _password)) 
            {
                return Ok("Login was Completed ");
            }
            else
            {
                return BadRequest("Username or Pass are  Invalid");
            }
        }




    }
}
