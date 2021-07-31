using Dapper;
using DapperApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DapperApi.Repos
{
    public class ProductRepository
    {
        private string connectionStr;
        static readonly HttpClient client = new HttpClient();
        public ProductRepository()
        {
            connectionStr = @"server=(localdb)\mssqllocaldb; database=DapperDB; Integrated Security=true;";
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionStr);
            }
        }
        public void Add(Product product)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sql = "INSERT INTO Product (name, detail, price, quantity) VALUES(@name, @detail, @price, @quantity)";
                dbConnection.Open();
                dbConnection.Execute(sql, product);
            }
        }
        public IEnumerable<Product> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sql = "SELECT * FROM Product";
                dbConnection.Open();
                return dbConnection.Query<Product>(sql);
            }
        }
        public Product FindById(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sql = "SELECT * FROM Product WHERE id=@Id";
                dbConnection.Open();
                return dbConnection.Query<Product>(sql, new { Id = id }).FirstOrDefault();
            }
        }
        public void Delete(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sql = "DELETE FROM Product WHERE id=@Id";
                dbConnection.Open();
                dbConnection.Execute(sql, new { Id = id });
            }
        }
        public void Update(Product product)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sql = "UPDATE Product SET name=@name, detail=@detail, quantity=@quantity, price=@price WHERE id=@Id";
                dbConnection.Open();
                dbConnection.Query(sql, product);
            }
        }
        public IEnumerable<SpecificProductColumns> getSpecificField()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sql = @"SELECT name, detail FROM Product";
                dbConnection.Open();
                return dbConnection.Query<Product>(sql).Select(e => new SpecificProductColumns() { name = e.name, detail = e.detail });
            }
        }
        public async Task<List<Post>> CallRestApi()
        {
            List<Post> post = new List<Post>();
            HttpResponseMessage response = await client.GetAsync("https://jsonplaceholder.typicode.com/posts");
            if (response.IsSuccessStatusCode)
            {
                post = await response.Content.ReadAsAsync<List<Post>>();
            }
            return post;
        }
        public IEnumerable<CombinedTables> combineTables()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sql = "SELECT Product.id as productId, Product.name as productName, Category.id as categoryId, Category.name as categoryName FROM Product, Category";
                dbConnection.Open();
                return dbConnection.Query<CombinedTables>(sql);
            }
        }
        public dynamic PassDataToExpando()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sql = "SELECT Product.id as productId, Product.name as productName, Category.id as categoryId, Category.name as categoryName FROM Product, Category";
                dbConnection.Open();
                IEnumerable<CombinedTables> result = dbConnection.Query<CombinedTables>(sql);
                dynamic expando = new ExpandoObject();
                expando.data = result;
                return expando;
            }

        }
    }
}
