using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Module_6
{
    public class DataBase
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data source=DESKTOP-VM99VUJ;Initial Catalog=BooksData1;Integrated Security=True");

        public void openConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }
        public void closeConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }
        public void AddBook(string authorName, string bookName, int yearRelease, bool Rented)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO BooksData1 (author_Name_Surname, nameBook, yearRelease, Rented) VALUES (@author, @name, @year, @rented)", sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@author", authorName);
                    cmd.Parameters.AddWithValue("@name", bookName);
                    cmd.Parameters.AddWithValue("@year", yearRelease);
                    cmd.Parameters.AddWithValue("@rented", Rented);
                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                closeConnection();
            }
        }
        public List<string> GetBooks()
        {
            List<string> books = new List<string>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT author_Name_Surname, nameBook, yearRelease FROM BooksData1", sqlConnection))
                {
                    openConnection();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string bookInfo = $"{reader["author_Name_Surname"]} - {reader["nameBook"]} ({reader["yearRelease"]})";
                        books.Add(bookInfo);
                    }
                }
            }
            finally
            {
                closeConnection();
            }
            return books;
        }
    }
}
