using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]/[action]")]
public class CategoryController : ControllerBase
{

    //Static value _username and _password
    private static string _username;
    private static string _password;


    //Register
    [HttpPost]
    public IActionResult Register(string username, string password)
    {
        //Hash password
        _username = username;
        _password = BCrypt.Net.BCrypt.HashPassword(password);
        //return username and password
        return Ok("Register Success  " + new { username = _username, password = _password });
    }

    //Login
    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        if (username == _username && BCrypt.Net.BCrypt.Verify(password, _password))
        {
            //Create Token
            var token = JwtAuthenticationManager.GenerateJWTToken(username);

            //Return Token
            return Ok(token);
        }
        else
        {
            //Return Error
            return BadRequest("Username or Password Incorrect");
        }
    }



    //Create Static Value List Cat
    public static List<Cat> cats = new List<Cat>()
        {
            new Cat(){Id=1, Name="Cat1"},
            new Cat(){Id=2, Name="Cat2"},
            new Cat(){Id=3, Name="Cat3"},
            new Cat(){Id=4, Name="Cat4"},
            new Cat(){Id=5, Name="Cat5"}
        };

    //get all cat
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(cats);
    }

    //Add new cat
    [HttpPost]
    [Authorize]
    public IActionResult Post(Cat cat)
    {
        cats.Add(cat);
        return Ok("Add new cat Success");
    }

    //Update cat
    [HttpPut]
    [Authorize]
    public IActionResult Put(Cat cat)
    {
        var catUpdate = cats.FirstOrDefault(catItem => catItem.Id == cat.Id);
        catUpdate.Name = cat.Name;
        return Ok("Update Success");
    }

    //Delete cat
    [HttpDelete]
    [Authorize]
    public IActionResult Delete(int id)
    {
        var catDelete = cats.FirstOrDefault(catItem => catItem.Id == id);
        cats.Remove(catDelete);
        return Ok("Delete Success");
    }
}



//Create class JwtAuthenticationManager
public static class JwtAuthenticationManager
{
    //Create method GenerateJWTToken
    public static string GenerateJWTToken(string username)
    {
        //Create Key
        var key = Encoding.ASCII.GetBytes("this is iman sabet custom secret key ,do not copy");

        //Create Token Handler
        var tokenHandler = new JwtSecurityTokenHandler();

        //Create token Descriptor
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };

        //Create Token
        var token = tokenHandler.CreateToken(tokenDescriptor);

        //Return Token
        return tokenHandler.WriteToken(token);
    }
}