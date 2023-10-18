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
    }
}
