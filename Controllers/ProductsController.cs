using APIWebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;
using System.Text.Json;

namespace APIWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataDbContext Context;
        public ProductsController(DataDbContext context)
        {
            Context = context;
        }
        [Route("GetData")]
        [HttpGet]
        public IActionResult GetData(string id)
        {
            var data=Context.Products.FirstOrDefault(x=>x.Id== id);

            return Ok(data);
        }
        [Route("GetAllData")]
        [HttpGet]
        public IActionResult GetAllData(string modelname)
        {
           
            List<string> names = new List<string>() { "Products", "Users" };

            if (names.Contains(modelname))
            {
              
                var dbSetProperty = Context.GetType().GetProperty(modelname);

                if (dbSetProperty != null)
                {
   
                    var dbSet = dbSetProperty.GetValue(Context);
                    var dataMethod = dbSet.GetType().GetMethod("ToList");
                    var data = dataMethod.Invoke(dbSet, null);

                    return Ok(data);
                }
            }

         
            return NotFound("Model not found");
        }

        [Route("UpdateData")]
        [HttpPost]
        public IActionResult UpdateData(string json)
        {
            
            var updateData = JsonConvert.DeserializeObject<Products>(json);
            var res = Context.Products.FirstOrDefault(x=>x.Id == updateData.Id);
            if (res != null )

            {

                res.Price = updateData.Price;
                res.ProductName = !string.IsNullOrEmpty(updateData.ProductName)
                                   ? updateData.ProductName : res.ProductName; 
                res.ProductDescription= updateData.ProductDescription;
                Context.SaveChanges();

            }

            return Ok(res);
        }

        [Route("DeleteData")]
        [HttpDelete]
        public IActionResult DeleteData(string id)
        {
            if(id == null)
            {
                return BadRequest("Id does not exist");

            }
            var res = Context.Products.FirstOrDefault( x=>x.Id== id);
            if (res == null )
            {
                return BadRequest("Id does not exist");
   
            }
            else {
                Context.Products.Remove(res);
                Context.SaveChanges();
            }
            var data = Context.Products.ToList();

            return Ok(data);

        }

    }

}
