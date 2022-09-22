using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShoppingCartList.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCartList
{
    public static class ShoppingCartApi
    {
        static List<ShoppingCartItem> shoppingCartItems = new();

        [FunctionName("GetShoppingCartItems")]
        public static async Task<IActionResult> GetShoppingCartItems(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get",  Route = "ShoppingCartItem")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Getting All Shopping Cart Items");

            return new OkObjectResult(shoppingCartItems);
            
        }

        [FunctionName("GetShoppingCartItemById")]
        public static async Task<IActionResult> GetShoppingCartItemById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ShoppingCartItem/{id}")] HttpRequest req, 
            ILogger log, string id)
        {
            log.LogInformation($"Getting All Shopping Cart Item with ID: {id}" );
            var shoppingCartItem = shoppingCartItems.FirstOrDefault(q => q.Id == id);
            if(shoppingCartItem == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(shoppingCartItem);
        }


        [FunctionName("CreateShoppingCartItem")]
        public static async Task<IActionResult> CreateShoppingCartItem(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "ShoppingCartItem")] HttpRequest req,
           ILogger log)
        {
            log.LogInformation("Creating Shopping Cart Item.");
            string requestData = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<CreateShoppingCartItem>(requestData);

            var item = new ShoppingCartItem 
            { 
                ItemName = data.ItemName 
            };

            shoppingCartItems.Add(item);
            return new OkObjectResult(item);

        }

        [FunctionName("PutShoppingCartItem")]
        public static async Task<IActionResult> PutShoppingCartItem(
          [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ShoppingCartItem/{id}")] HttpRequest req,
          ILogger log, string id)
        {
            log.LogInformation($"Updating Shopping Cart Item with ID: {id}");

            var shoppingCartItem = shoppingCartItems.FirstOrDefault(q=>q.Id == id);
            if(null == shoppingCartItem)
            {
                return new NotFoundResult();
            }

            string requestData = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<UpdateShoppingCartItem>(requestData);
            shoppingCartItem.Collected = data.collected;

            return new OkObjectResult(shoppingCartItem);
        }

        [FunctionName("DeleteShoppingCartItem")]
        public static async Task<IActionResult> DeleteShoppingCartItem(
          [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "ShoppingCartItem/{id}")] HttpRequest req,
          ILogger log, string id)
        {
            log.LogInformation($"Deleting Shopping Cart Item with ID: {id}");

            var shoppingCartItem = shoppingCartItems.FirstOrDefault(q => q.Id == id);
            if(shoppingCartItem == null)
            {
                return new NotFoundResult();
            }

            shoppingCartItems.Remove(shoppingCartItem);
            return new OkResult();



        }
    }
}
