using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;

namespace LibraryManager
{
    public partial class MainWindow : Window
    {
        private const string FilePath = "books.json";
        private List<Book> Books { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            LoadBooks();
        }

        // Загрузка книг из файла JSON
        private void LoadBooks()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                Books = JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
            }
            else
            {
                Books = new List<Book>();
            }

            BooksDataGrid.ItemsSource = Books;
        }

        // Поиск книг по названию или автору
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchText = SearchTextBox.Text.ToLower();
            var filteredBooks = Books
                .Where(b => b.Title.ToLower().Contains(searchText) || b.Author.ToLower().Contains(searchText))
                .ToList();

            BooksDataGrid.ItemsSource = filteredBooks;
        }

        // Добавление книги
        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            string title = Prompt("Введите название книги:");
            string author = Prompt("Введите автора книги:");
            string yearString = Prompt("Введите год выпуска книги:");

            if (int.TryParse(yearString, out int year))
            {
                var newBook = new Book { Title = title, Author = author, Year = year };
                Books.Add(newBook);
                SaveBooks();
                LoadBooks(); // Обновляем DataGrid
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректный год.");
            }
        }

        // Метод для отображения диалогов ввода
        private string Prompt(string message)
        {
            return Microsoft.VisualBasic.Interaction.InputBox(message, "Ввод данных", "");
        }

        // Сохранение книг в файл JSON
        private void SaveBooks()
        {
            var json = JsonSerializer.Serialize(Books, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }

    // Модель книги
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
    }
}
