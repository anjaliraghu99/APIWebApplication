using APIWebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [Route("GetData")]
        [HttpPost]
        public IActionResult UpdateData(string json)
        {
            
            var updateData = JsonConvert.DeserializeObject<Products>(json);
            var res = Context.Products.FirstOrDefault(x=>x.Id == updateData.Id);
            if (res != null )

            {
              
                //updateData.ProductName = updateData.ProductName != null ? updateData.ProductName : res.ProductName;
                res.Price = updateData.Price;
                res.ProductName = !string.IsNullOrEmpty(updateData.ProductName)
                                   ? updateData.ProductName : res.ProductName; 
                res.ProductDescription= updateData.ProductDescription;
                Context.SaveChanges();

            }
           
          

            return Ok(res);
        }
           
        



    }
}
