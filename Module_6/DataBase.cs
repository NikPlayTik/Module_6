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
        SqlConnection sqlConnection = new SqlConnection(@"Data source=DESKTOP-VM99VUJ;Initial Catalog=Books;Integrated Security=True");

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
        public int AddBook(string authorName, string bookName, int yearRelease)
        {
            int bookId = -1; // Идентификатор (ID) по умолчанию
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Books (author_Name_Surname, nameBook, yearRelease) OUTPUT INSERTED.id_books VALUES (@author, @name, @year)", sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@author", authorName);
                    cmd.Parameters.AddWithValue("@name", bookName);
                    cmd.Parameters.AddWithValue("@year", yearRelease);
                    sqlConnection.Open();
                    // Используем ExecuteScalar для получения ID после вставки
                    bookId = (int)cmd.ExecuteScalar();
                }
            }
            finally
            {
                closeConnection();
            }

            return bookId; // Возвращаем ID
        }
        public List<string> GetBooks()
        {
            List<string> books = new List<string>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT author_Name_Surname, nameBook, yearRelease FROM Books", sqlConnection))
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

        public bool RentBook(int id_books, DateTime rentedDate)
        {
            try
            {
                openConnection();

                // Проверяем, арендована ли книга. Если да, то не арендуем снова.
                using (SqlCommand checkCmd = new SqlCommand("SELECT RentedDate FROM Books WHERE Id = @id_books", sqlConnection))
                {
                    checkCmd.Parameters.AddWithValue("@id_books", id_books);
                    var existingRentedDate = checkCmd.ExecuteScalar();
                    if (existingRentedDate != DBNull.Value)
                    {
                        return false; // Книга уже арендована
                    }
                }

                // Если книга еще не арендована, арендуем её.
                using (SqlCommand rentCmd = new SqlCommand("UPDATE Books SET RentedDate = @rentedDate WHERE Id = @id_books", sqlConnection))
                {
                    rentCmd.Parameters.AddWithValue("@id_books", id_books);
                    rentCmd.Parameters.AddWithValue("@rentedDate", rentedDate);

                    rentCmd.ExecuteNonQuery();
                    return true; // Книга успешно арендована
                }
            }
            finally
            {
                closeConnection();
            }
        }
    }
}
