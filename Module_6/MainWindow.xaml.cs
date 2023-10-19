using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Data.Entity;

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

                    // Добавляем данные в базу данных
                    database.AddBook(ФИО.Text, Название.Text, year);

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

        private void OutputButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> books = database.GetBooks();
            View_information.ItemsSource = books;
        }

        private int GetSelectedBookId()
        {
            if (View_information.SelectedItem != null)
            {
                string selectedBook = View_information.SelectedItem.ToString();
                int startIndex = selectedBook.IndexOf(" ") + 1; // Индекс начала Id
                int endIndex = selectedBook.IndexOf(" -"); // Индекс окончания Id

                if (startIndex >= 0 && endIndex > startIndex)
                {
                    string idStr = selectedBook.Substring(startIndex, endIndex - startIndex);
                    int id;
                    if (int.TryParse(idStr, out id))
                    {
                        return id;
                    }
                }
            }

            return -1; // Если не удалось получить Id
        }
        private void RentMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Открываете окно для ввода даты аренды и получаете выбранную дату
            RentalWindow rentalWindow = new RentalWindow();
            bool? result = rentalWindow.ShowDialog();

            if (result == true)
            {
                DateTime rentedDate = rentalWindow.RentedDate;
                int selectedBookId = GetSelectedBookId(); // Получаете Id выбранной книги

                if (selectedBookId != -1)
                {
                    bool success = database.RentBook(selectedBookId, rentedDate);

                    if (success)
                    {
                        MessageBox.Show("Книга успешно арендована");
                        // Здесь вы можете обновить представление книг, чтобы отразить изменения
                        List<string> books = database.GetBooks();
                        View_information.ItemsSource = books;
                    }
                    else
                    {
                        MessageBox.Show("Книга уже арендована или произошла ошибка при аренде книги");
                    }
                }
                else
                {
                    MessageBox.Show("Тест, оно не работает");
                }
            }
        }
    }
}
