using Dapper;
using DapperApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DapperApi.Repos
{
    public class CategoryRepository
    {
        private string connectionStr;
        public CategoryRepository()
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
        public void Add(Category category)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sql = @"INSERT INTO Category (name, title) VALUES(@name, @title)";
                dbConnection.Open();
                dbConnection.Execute(sql, category);
            }
        }
        public IEnumerable<Category> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sql = @"SELECT * FROM Category";
                dbConnection.Open();
                return dbConnection.Query<Category>(sql);
            }
        }
        public Category FindById(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sql = "SELECT * FROM Category WHERE id=@Id";
                dbConnection.Open();
                return dbConnection.Query<Category>(sql, new { Id = id }).FirstOrDefault();
            }
        }
        public void Delete(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sql = "DELETE FROM Category WHERE id=@Id";
                dbConnection.Open();
                dbConnection.Execute(sql, new { Id = id });
            }
        }
        public void Update(Category category)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sql = "UPDATE Product SET name=@name, title=@title WHERE id=@Id";
                dbConnection.Open();
                dbConnection.Query(sql, category);
            }
        }
    }
}