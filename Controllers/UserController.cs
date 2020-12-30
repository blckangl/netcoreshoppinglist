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

        public UserController(DataService ds)
        {
            dataService = ds;


        }

        [HttpGet()]
        public JsonResult getUsers()
        {

            var response = new JsonResult(dataService.list);
            response.StatusCode = 200;
            return response;
        }

        [HttpGet("{id}")]

        public Person getUserById(int id)
        {

            try
            {
                return dataService.list[id];
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        [HttpPost()]
        public Person addPerson([FromBody]Person prs)
        {
            Person person = new Person();
            person.Name = prs.Name;

            dataService.list.Add(person);
            return person;
        }

        [HttpDelete("{id}")]
        public string deletePerson(string id)
        {
            return id;
        }
    }
}
