﻿using System;
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
        public void AddBook(string authorName, string bookName, int yearRelease)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Books (author_Name_Surname, nameBook, yearRelease) VALUES (@author, @name, @year)", sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@author", authorName);
                    cmd.Parameters.AddWithValue("@name", bookName);
                    cmd.Parameters.AddWithValue("@year", yearRelease);
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
    }
}