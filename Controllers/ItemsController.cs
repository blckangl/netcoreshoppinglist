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
    [Route("api/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        ShoppingListContext _context;
        public ItemsController(ShoppingListContext context)
        {
            _context = context;
        }



        [HttpPost]
        public IActionResult AddNewItem(ShoppingItem item)
        {
            JsonResult result = new JsonResult(new { });

            try
            {
                if (item != null)
                {
                    _context.items.Add(item);
                    _context.SaveChanges();
                    result.Value = new { message = "item added" };
                }
                else
                {
                    result.Value = new { message = "Invalid Item" };
                    result.StatusCode = 500;

                }

                return result;
            }
            catch(Exception ex)
            {

                result.Value = new { message = ex.Message };
                return result;
            }




        }

        [HttpDelete("{id}")]
        public IActionResult RemoveItem(int id)
        {
            JsonResult result = new JsonResult(new { });

            try
            {

                var item = _context.items.Find(id);

                if (item != null)
                {
                    _context.items.Remove(item);
                    _context.SaveChanges();
                    result.Value = new { message = "item deleted" };
                }
                else
                {
                    result.Value = new { message = "item not found" };
                    result.StatusCode = 404;

                }

                return result;
            }
            catch (Exception ex)
            {

                result.Value = new { message = ex.Message };
                return result;
            }


        }
        [HttpGet]
        public IActionResult GetAllItems()
        {

            JsonResult result = new JsonResult(new { });

            try
            {

                result.Value = _context.items.ToList();

                return result;
            }
            catch (Exception ex)
            {

                result.Value = new { message = ex.Message };
                return result;
            }


        }

        [HttpGet("{id}")]
        public IActionResult GetItemById(int id)
        {
            JsonResult result = new JsonResult(new { });

            try
            {

                var item = _context.items.Find(id);

                if (item != null)
                {
              
                    result.Value =item;
                }
                else
                {
                    result.Value = new { message = "item not found" };
                    result.StatusCode = 404;

                }

                return result;
            }
            catch (Exception ex)
            {

                result.Value = new { message = ex.Message };
                return result;
            }
        }
    }
}
