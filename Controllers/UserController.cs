using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using shoppinglist.Models;
using shoppinglist.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace shoppinglist.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ShoppingListContext _context;
        private readonly IConfiguration _config;

        public UserController(ShoppingListContext context,IConfiguration config)
        {
            _context = context;
            _config = config;

        }

        [Authorize]
        [HttpGet()]
        public IActionResult getUsers()
        {

            
            var response = new JsonResult(_context.persons.ToList()) { StatusCode = 200 };
          
            return response;
        }

        [HttpGet("{id}")]

        public IActionResult getUserById(int id)
        {

            try
            {
                var person = _context.persons.Find(id);
                if (person != null)
                {
                    return new JsonResult(person) { StatusCode = 200 };

                }
                else
                {
                    return new JsonResult(new { message = "user not found" }) { StatusCode = 404 };

                }

            }
            catch(Exception ex)
            {
                return new JsonResult(new { message = ex.Message }) { StatusCode = 500 };
            }
            
        }

        [HttpPost()]
        public async Task<Person> addPerson([FromBody]Person prs)
        {
            Person person = new Person();
            person.Name = prs.Name;
            person.Age = prs.Age;

           await _context.persons.AddAsync(person);
            await _context.SaveChangesAsync();
            return person;
        }

        [HttpDelete("{id}")]
        public string deletePerson(int id)
        {
            var person = _context.persons.Find(id);


            if (person != null)
            {
           

                _context.persons.Remove(person);
                _context.SaveChanges();

                return "deleted";

            }

            return "not found";
        }


        [HttpPut("{id}")]
        public Person UpdateUser(int id , Person newPerson)
        {
            var person = _context.persons.Find(id);

                if (person!=null)
            {
                person.Name = newPerson.Name;
                person.Age = newPerson.Age;

                _context.persons.Update(person);
                _context.SaveChanges();

                return person;

            }

            return null;

        }


        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] User login)
        {
            IActionResult response = Unauthorized();
            Person user = AuthenticateUser(login);
            if (user != null)
            {
                var tokenString = GenerateJWTToken(user);
                response = Ok(new
                {
                    token = tokenString,
                    userDetails = user,
                });
            }
            return response;
        }

        Person AuthenticateUser(User loginCredentials)
        {
            Person user = _context.persons.SingleOrDefault(x => x.UserName == loginCredentials.UserName && x.Password == loginCredentials.Password);
            return user;
        }

        string GenerateJWTToken(Person userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt")["SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim("Nom", userInfo.Name.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               };
            var token = new JwtSecurityToken(
            issuer: _config.GetSection("Jwt")["Issuer"],
            audience: _config.GetSection("Jwt")["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
