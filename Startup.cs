using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AngleSharp;
using WebScraper.Models;

namespace WebScraper
{
    public class Startup
    {
        public async Task Run()
        {
            //Reading the HTML from .txt file

            var document = File.ReadAllText("htmlInfo.txt");

            //Configure Angle Sharp

            var config = Configuration.Default;
            using var context = BrowsingContext.New(config);
            using var doc = await context.OpenAsync(req => req.Content(document));

            //Get all ratings

            var ratingsInfo = doc.QuerySelectorAll(".item").Select(m => m.GetAttribute("rating")).ToList();
            var ratings = new List<string>();
            for (int i = 0; i < ratingsInfo.Count; i++)
            {
                // Check if rating is between (0, 5.00)
                // I devide it by 2 as in your solution but it could be made this way
                /*
                if (rating > 5)
                {
                    rating = 5;
                } 
                */
                var rating = decimal.Parse(ratingsInfo[i]);
                if (rating > 5)
                {
                    rating /= 2;
                }
                else if (rating < 0)
                {
                    rating = 0;
                }
                ratings.Add(rating.ToString());
            }

            //Get all products

            var names = doc.QuerySelectorAll("img").Select(m => m.GetAttribute("alt")).ToList().Where(x => !x.Contains("Sold Out")).ToList();

            //Get all prices

            var pricesInfo = doc.QuerySelectorAll(".pricing > .price").Select(x => x.TextContent).ToList();
            var prices = new List<string>();
            foreach (var price in pricesInfo)
            {
                var splittedPriceInfo = price.Split('$', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault().Replace(",", "");
                prices.Add(splittedPriceInfo);
            }


            //Adding Products
            var products = AddProducts(names, ratings, prices);

            //Serializing Products

            var result = SerializeProducts(products);
            Console.WriteLine(result);

        }
        private static List<Product> AddProducts(List<string> names, List<string> ratings, List<string> prices)
        {
            var products = new List<Product>();
            for (int i = 0; i < names.Count; i++)
            {
                products.Add(new Product
                {
                    Name = names[i],
                    Rating = ratings[i],
                    Price = prices[i]
                });
            }
            return products;
        }
        private static string SerializeProducts(List<Product> products)
        {
            var options = new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(products, options);
            return json;
        }

    }
}