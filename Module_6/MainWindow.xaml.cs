using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace Module_6
{
    public partial class MainWindow : Window
    {
        DataBase database = new DataBase();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string yearText = Год_выпуска.Text;

                if (Regex.IsMatch(yearText, @"^\d{4}$"))
                {
                    int year = int.Parse(yearText);

                    bool isRented = false; // Новая книга по умолчанию не арендована
                    database.AddBook(ФИО.Text, Название.Text, year, isRented);

                    // Очищаем текстовые поля
                    ФИО.Text = "";
                    Название.Text = "";
                    Год_выпуска.Text = "";
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите год в правильном формате (YYYY)");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении данных: {ex.Message}");
            }
        }

        private void RentCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox rentCheckBox = (CheckBox)sender;
            Book book = (Book)rentCheckBox.DataContext; // Получаем книгу, связанную с CheckBox

            if (rentCheckBox.IsChecked == true)
            {
                if (database.RentBook(book.Id, true))
                {
                    MessageBox.Show("Книга успешно арендована");
                }
                else
                {
                    MessageBox.Show("Книга уже арендована или произошла ошибка при аренде книги");
                    rentCheckBox.IsChecked = false; // Отменить изменение CheckBox
                }
            }
            else
            {
                // Если снимается аренда, вызываем метод RentBook и передаем false
                if (database.RentBook(book.Id, false))
                {
                    MessageBox.Show("Аренда книги успешно снята");
                }
                else
                {
                    MessageBox.Show("Произошла ошибка при снятии аренды книги");
                    rentCheckBox.IsChecked = true; // Восстановить галочку CheckBox
                }
            }
        }

        private void OutputButton_Click(object sender, RoutedEventArgs e)
        {
            List<Book> books = database.GetBooks();
            View_information.ItemsSource = books;
        }

    }
}
