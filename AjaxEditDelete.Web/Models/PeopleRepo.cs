using System;
using System.Data.SqlClient;

namespace AjaxEditDelete.Web.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }


    public class PeopleRepo
    {
        private string _connectionString;
        public PeopleRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Person> GetAll()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM People";
            connection.Open();
            var list = new List<Person>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Person
                {
                    Id = (int)reader["Id"],
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    Age = (int)reader["Age"]

                });
            }

            return list;
        }

        public void Add(Person p)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO People (FirstName, LastName, Age) 
                                VALUES (@first, @last, @age) SELECT SCOPE_IDENTITY()";

            cmd.Parameters.AddWithValue("@first", p.FirstName);
            cmd.Parameters.AddWithValue("@last", p.LastName);
            cmd.Parameters.AddWithValue("@age", p.Age);
            connection.Open();
            p.Id = (int)(decimal)cmd.ExecuteScalar();
        }

        public int Edit(Person p)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE People SET FirstName = @first, LastName = @last, Age = @age 
                                WHERE Id = @id
                                SELECT SCOPE_IDENTITY()";

            cmd.Parameters.AddWithValue("@first", p.FirstName);
            cmd.Parameters.AddWithValue("@last", p.LastName);
            cmd.Parameters.AddWithValue("@age", p.Age);
            cmd.Parameters.AddWithValue("@id", p.Id);

            connection.Open();

            return (int)(decimal)cmd.ExecuteScalar();
        }

        public void Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM People WHERE Id = @id";

            cmd.Parameters.AddWithValue("@id", id);

            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public Person GetPersonById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT TOP 1 * FROM People WHERE Id = @id";

            cmd.Parameters.AddWithValue("@id", id);

            connection.Open();

            var reader = cmd.ExecuteReader();
            if(!reader.Read())
            {
                return null;
            }

            return new Person
            {
                Id = id,
                FirstName = (string)reader["FirstName"],
                LastName = (string)reader["LastName"],
                Age = (int)reader["Age"]
            };
        }
    }
}
