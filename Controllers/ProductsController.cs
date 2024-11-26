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
        [Route("InsertData")]
        [HttpPost]
        public IActionResult InsertData(string json)
        {
            // Deserialize the incoming JSON string into a Product object
            var newProduct = JsonConvert.DeserializeObject<Products>(json);
            var res = Context.Products.Find(newProduct.Id);
            if (res == null)
            {
                Context.Products.Add(newProduct);

                // Save the changes to the database
                Context.SaveChanges();

                // Return the newly inserted product as response
                var data = Context.Products;

                return Ok(data);
            }
            else
            {
                return BadRequest("Id already exist");
            }
            // Add the new Product to the DbSet
            
        }




        [Route("GetAllData")]
        [HttpGet]
        public IActionResult GetAllData(string modelname)
        {
            // List of allowed model names to prevent invalid modelname input
            List<string> names = new List<string>() { "Products", "Users" };

            // Check if the modelname exists in the list
            if (!names.Contains(modelname))
            {
                return NotFound("Model not found");
            }

            // Get the property that matches the modelname (DbSet<T> type)
            var dbSetProperty = Context.GetType().GetProperty(modelname);

            if (dbSetProperty == null)
            {
                return NotFound("Model not found");
            }

            // Get the DbSet from the context (which is a DbSet<T>)
            var dbSet = dbSetProperty.GetValue(Context);

            if (dbSet == null)
            {
                return NotFound("Model not found");
            }

            // Ensure the DbSet is an IQueryable (which it should be)
            if (!(dbSet is IQueryable queryable))
            {
                return NotFound("Error: The DbSet is not IQueryable.");
            }

            // Use reflection to get the ToList method from IQueryable<T>
            var toListMethod = typeof(Enumerable).GetMethod("ToList").MakeGenericMethod(dbSet.GetType().GetGenericArguments()[0]);

            if (toListMethod == null)
            {
                return NotFound("Error: Could not find the ToList method.");
            }

            // Invoke ToList to retrieve the data
            var data = toListMethod.Invoke(null, new object[] { queryable });

            // Return the data
            return Ok(data);
        }


    }

}
