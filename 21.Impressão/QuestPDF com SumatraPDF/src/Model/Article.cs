using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Teste.src.Model;

public class Article
{
    public int ArticleId { get; set; }
    public string ProductName { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
}

public static class ArticleData
{
    public static IEnumerable<Article> getMockArticles()
    {
        return new List<Article>
        {
            new Article { ArticleId = 1, ProductName = "Product 1", Stock = 10, Price = 100 },
            new Article { ArticleId = 2, ProductName = "Product 2", Stock = 20, Price = 200 },
            new Article { ArticleId = 3, ProductName = "Product 3", Stock = 30, Price = 300 },
            new Article { ArticleId = 4, ProductName = "Product 4", Stock = 40, Price = 400 },
            new Article { ArticleId = 5, ProductName = "Product 5", Stock = 50, Price = 500 },
            new Article { ArticleId = 6, ProductName = "Product 6", Stock = 60, Price = 600 },
            new Article { ArticleId = 7, ProductName = "Product 7", Stock = 70, Price = 700 },
            new Article { ArticleId = 8, ProductName = "Product 8", Stock = 80, Price = 800 },
            new Article { ArticleId = 9, ProductName = "Product 9", Stock = 90, Price = 900 },
            new Article { ArticleId = 10, ProductName = "Product 10", Stock = 100, Price = 1000 },
            new Article { ArticleId = 11, ProductName = "Product 11", Stock = 110, Price = 1100 },
            new Article { ArticleId = 12, ProductName = "Product 12", Stock = 120, Price = 1200 },
            new Article { ArticleId = 13, ProductName = "Product 13", Stock = 130, Price = 1300 },
            new Article { ArticleId = 14, ProductName = "Product 14", Stock = 140, Price = 1400 },
            new Article { ArticleId = 15, ProductName = "Product 15", Stock = 150, Price = 1500 },
            new Article { ArticleId = 16, ProductName = "Product 16", Stock = 160, Price = 1600 },
            new Article { ArticleId = 17, ProductName = "Product 17", Stock = 170, Price = 1700 },
            new Article { ArticleId = 18, ProductName = "Product 18", Stock = 180, Price = 1800 },
            new Article { ArticleId = 19, ProductName = "Product 19", Stock = 190, Price = 1900 },
            new Article { ArticleId = 20, ProductName = "Product 20", Stock = 200, Price = 2000 },
            new Article { ArticleId = 21, ProductName = "Product 21", Stock = 210, Price = 2100 }
        };
    }
}