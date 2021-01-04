using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shoppinglist.Models;
using shoppinglist.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shoppinglist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        DataService dataService;
        ShoppingListContext _context;

        public UserController(DataService ds,ShoppingListContext context)
        {
            dataService = ds;
            _context = context;

        }

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
    }
}
