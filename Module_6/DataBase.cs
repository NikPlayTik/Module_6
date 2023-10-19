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
                using (SqlCommand cmd = new SqlCommand("INSERT INTO BooksData1 (author_Name_Surname, nameBook, yearRelease, Rented) OUTPUT INSERTED.id_books VALUES (@author, @name, @year, @rented)", sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@author", authorName);
                    cmd.Parameters.AddWithValue("@name", bookName);
                    cmd.Parameters.AddWithValue("@year", yearRelease);
                    cmd.Parameters.AddWithValue("@rented", Rented);
                    sqlConnection.Open();
                    int id = (int)cmd.ExecuteScalar(); // Получить значение id_books
                }
            }
            finally
            {
                closeConnection();
            }
        }
        public bool RentBook(int id_books, bool isRented)
        {
            try
            {
                openConnection();

                // Обновляем состояние аренды в базе данных в соответствии с isRented
                using (SqlCommand rentCmd = new SqlCommand("UPDATE BooksData1 SET Rented = @isRented WHERE id_books = @id_books", sqlConnection))
                {
                    rentCmd.Parameters.AddWithValue("@id_books", id_books);
                    rentCmd.Parameters.AddWithValue("@isRented", isRented);
                    int rowsAffected = rentCmd.ExecuteNonQuery();

                    return rowsAffected > 0; // Возвращаем true, если аренда успешно обновлена
                }
            }
            finally
            {
                closeConnection();
            }
        }
        public List<Book> GetBooks()
        {
            List<Book> books = new List<Book>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT id_books, author_Name_Surname, nameBook, yearRelease FROM BooksData1", sqlConnection))
                {
                    openConnection();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Book book = new Book
                        {
                            Id = Convert.ToInt32(reader["id_books"]), // Установите Id книги из базы данных
                            Author = reader["author_Name_Surname"].ToString(),
                            Title = reader["nameBook"].ToString(),
                            Year = Convert.ToInt32(reader["yearRelease"])
                        };
                        books.Add(book);
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
