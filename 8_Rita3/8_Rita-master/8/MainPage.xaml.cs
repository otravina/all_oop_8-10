using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media.Imaging;
using System.Data.Entity;

namespace _8
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        bool statement;
        public MainPage()
        {
            InitializeComponent();

           // Create();
        }

        private void Add_Btn_Click(object sender, RoutedEventArgs e)
        {
            var newStudentPage = new NewStudentPage();
            MainFrame.NavigationService.Navigate(newStudentPage);
            Hidden();
        }

        private void Edit_Btn_Click(object sender, RoutedEventArgs e)
        {
            statement = true;

            if (StudentGrid.SelectedItem != null)
            {
                var currentStudent = StudentGrid.SelectedItem as Book;
                var editPage = new EditPage(currentStudent, statement);
                MainFrame.NavigationService.Navigate(editPage);
            }
            else
            {
                MessageBox.Show("Книга не выбрана", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            Hidden();

            //StudentGrid.ItemsSource = AppData.db.Student.ToList();
        }

        private void View_Btn_Click(object sender, RoutedEventArgs e)
        {
            statement = false;
            if (StudentGrid.SelectedItem != null)
            {
                var currentStudent = StudentGrid.SelectedItem as Book;
                var editPage = new EditPage(currentStudent, statement);
                MainFrame.NavigationService.Navigate(editPage);
            }
            else
            {
                MessageBox.Show("Книга не выбрана", "Уведомление",
                MessageBoxButton.OK, MessageBoxImage.Information);
            }

            Hidden();
        }

        private void Delete_Btn_Click(object sender, RoutedEventArgs e)
        {
            using (var transaction = AppData.db.Database.BeginTransaction())
            {
                try
                {
                    if (StudentGrid.SelectedItem != null)
                    {
                        if (MessageBox.Show("Вы действительно хотите удалить книгу (книги)?", "Уведомление",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            List<Book> currentStudent = new List<Book>();

                            foreach (var student in StudentGrid.SelectedItems)
                            {
                                currentStudent.Add(student as Book);
                            }

                            foreach (var student in currentStudent)
                            {
                                if (student.Author != null)
                                {
                                    AppData.db.Author.Remove(student.Author);
                                }
                            }

                            AppData.db.Book.RemoveRange(currentStudent);
                            AppData.db.SaveChanges();
                            transaction.Commit();

                            StudentGrid.ItemsSource = AppData.db.Book.ToList();
                            MessageBox.Show(" Удаление прошло успешно", "Уведомление",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Книга не выбрана, выберите нужную", "Уведомление",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StudentGrid.ItemsSource = AppData.db.Book.ToList();
        }

        private void ToMain_Btn_Click(object sender, RoutedEventArgs e)
        {
            Hidden();
            ToMain_btn.Visibility = Visibility.Collapsed;
            MainFrame.NavigationService.Navigate(new MainPage());
            AppData.db.ChangeTracker.Entries().ToList().ForEach(entity => entity.Reload());
            StudentGrid.ItemsSource = AppData.db.Book.ToList();
        }

        private void Hidden()
        {
            View_btn.Visibility = Visibility.Collapsed;
            Edit_btn.Visibility = Visibility.Collapsed;
            Add_btn.Visibility = Visibility.Collapsed;
            Delete_btn.Visibility = Visibility.Collapsed;
            Sorting_btn.Visibility = Visibility.Collapsed;
        }

        private void Sorting_Btn_Click(object sender, RoutedEventArgs e)
        {
            Hidden();
            var sortingForm = new SortingWindow();
            sortingForm.ShowDialog();

            var books = AppData.db.Book.ToList();

            bool sortBySizeAscending = sortingForm.FileSizeAscending;
            bool sortBySizeDescending = sortingForm.FileSizeDescending;
            bool sortByCountAscending = sortingForm.PageCountAscending;
            bool sortByCountDescending = sortingForm.PageCountDescending;
            bool sortByYearAscending = sortingForm.PublishYearAscending;
            bool sortByYearDescending = sortingForm.PublishYearDescending;
            bool sortByUDCAscending = sortingForm.UDCAscending;
            bool sortByUDCDescending = sortingForm.UDCDescending;

            if (sortBySizeAscending)
            {
                books = books.OrderBy(s => s.FileSize).ToList();
            }
            else if (sortBySizeDescending)
            {
                books = books.OrderByDescending(s => s.FileSize).ToList();
            }
            else if (sortByCountAscending)
            {
                books = books.OrderBy(s => s.PageCount).ToList();
            }
            else if (sortByCountDescending)
            {
                books = books.OrderByDescending(s => s.PageCount).ToList();
            }
            else if (sortByYearAscending)
            {
                books = books.OrderBy(s => s.PublishYear).ToList();
            }
            else if (sortByYearDescending)
            {
                books = books.OrderByDescending(s => s.PublishYear).ToList();
            }
            else if (sortByUDCAscending)
            {
                books = books.OrderBy(s => s.UDC).ToList();
            }
            else if (sortByUDCDescending)
            {
                books = books.OrderByDescending(s => s.UDC).ToList();
            }
            StudentGrid.ItemsSource = books;
        }

        private void Create()
        {
            string connectionString = "Data Source=OTRAVINA\\SQLEXPRESS;Integrated Security=True;";

            string createDatabaseScript = @"
            USE [master]

            IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'Library_2')
            BEGIN
                
                CREATE DATABASE [Library_2]
                    CONTAINMENT = NONE
                    ON  PRIMARY 
                ( NAME = N'Library_2', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Library_2.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
                    LOG ON 
                ( NAME = N'Library_2_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Library_2_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
                    WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
            

                IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
                begin
                EXEC [Library_2].[dbo].[sp_fulltext_database] @action = 'enable'
                end
            

                ALTER DATABASE [Library_2] SET ANSI_NULL_DEFAULT OFF 
            

                ALTER DATABASE [Library_2] SET ANSI_NULLS OFF 
            

                ALTER DATABASE [Library_2] SET ANSI_PADDING OFF 
            

                ALTER DATABASE [Library_2] SET ANSI_WARNINGS OFF 
            

                ALTER DATABASE [Library_2] SET ARITHABORT OFF 
            

                ALTER DATABASE [Library_2] SET AUTO_CLOSE ON 
            

                ALTER DATABASE [Library_2] SET AUTO_SHRINK OFF 
            

                ALTER DATABASE [Library_2] SET AUTO_UPDATE_STATISTICS ON 
            

                ALTER DATABASE [Library_2] SET CURSOR_CLOSE_ON_COMMIT OFF 
            

                ALTER DATABASE [Library_2] SET CURSOR_DEFAULT  GLOBAL 
            

                ALTER DATABASE [Library_2] SET CONCAT_NULL_YIELDS_NULL OFF 
            

                ALTER DATABASE [Library_2] SET NUMERIC_ROUNDABORT OFF 
            

                ALTER DATABASE [Library_2] SET QUOTED_IDENTIFIER OFF 
            

                ALTER DATABASE [Library_2] SET RECURSIVE_TRIGGERS OFF 
            

                ALTER DATABASE [Library_2] SET  ENABLE_BROKER 
            

                ALTER DATABASE [Library_2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
            

                ALTER DATABASE [Library_2] SET DATE_CORRELATION_OPTIMIZATION OFF 
            

                ALTER DATABASE [Library_2] SET TRUSTWORTHY OFF 
            

                ALTER DATABASE [Library_2] SET ALLOW_SNAPSHOT_ISOLATION OFF 
            

                ALTER DATABASE [Library_2] SET PARAMETERIZATION SIMPLE 
            

                ALTER DATABASE [Library_2] SET READ_COMMITTED_SNAPSHOT OFF 
            

                ALTER DATABASE [Library_2] SET HONOR_BROKER_PRIORITY OFF 
            

                ALTER DATABASE [Library_2] SET RECOVERY SIMPLE 
            

                ALTER DATABASE [Library_2] SET  MULTI_USER 
            

                ALTER DATABASE [Library_2] SET PAGE_VERIFY CHECKSUM  
            

                ALTER DATABASE [Library_2] SET DB_CHAINING OFF 
            

                ALTER DATABASE [Library_2] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
            

                ALTER DATABASE [Library_2] SET TARGET_RECOVERY_TIME = 60 SECONDS 
            

                ALTER DATABASE [Library_2] SET DELAYED_DURABILITY = DISABLED 
            

                ALTER DATABASE [Library_2] SET ACCELERATED_DATABASE_RECOVERY = OFF  
            

                ALTER DATABASE [Library_2] SET QUERY_STORE = ON
            

                ALTER DATABASE [Library_2] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
            

                ALTER DATABASE [Library_2] SET  READ_WRITE
            END";

            var createAuthorScript = @"
                USE [Library_2]

                IF NOT EXISTS (SELECT 1 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_TYPE='BASE TABLE' 
                    AND TABLE_NAME='Author')
                BEGIN

                    SET ANSI_NULLS ON
            

                    SET QUOTED_IDENTIFIER ON
           

                    CREATE TABLE [dbo].[Author](
	                    [AuthorID] [int] NOT NULL,
	                    [City] [varchar](50) NOT NULL,
	                    [Country] [varchar](50) NOT NULL,
	                    [FIO] [varchar](150) NOT NULL,
                     CONSTRAINT [PK__Address__091C2A1B87CED139] PRIMARY KEY CLUSTERED 
                    (
	                    [AuthorID] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY]
                END";

            var createBookScript = @"
                USE [Library_2]

                IF NOT EXISTS (SELECT 1 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_TYPE='BASE TABLE' 
                    AND TABLE_NAME='Book')
                BEGIN
                    SET ANSI_NULLS ON
            

                    SET QUOTED_IDENTIFIER ON
            

                   CREATE TABLE [dbo].[Book](
	                    [BookCover] [nvarchar](max) NULL,
	                    [UDC] [int] NOT NULL,
	                    [Title] [varchar](100) NOT NULL,
	                    [PageCount] [int] NOT NULL,
	                    [Publisher] [varchar](100) NOT NULL,
	                    [UploadDate] [date] NULL,
	                    [PublishYear] [int] NULL,
	                    [Format] [varchar](50) NULL,
	                    [FileSize] [decimal](5, 2) NULL,
	                    [ListOfAuthors] [varchar](500) NULL,
	                    [AuthorID] [int] NOT NULL,
	                    [UpdatedAt] [datetime] NULL,
                     CONSTRAINT [PK__Book__32C52A79AC1A9DCF] PRIMARY KEY CLUSTERED 
                    (
	                    [UDC] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

                   ALTER TABLE [dbo].[Book] ADD  DEFAULT (getdate()) FOR [UpdatedAt]

                    ALTER TABLE [dbo].[Book]  WITH CHECK ADD  CONSTRAINT [FK__Book__Author__4D94879B] FOREIGN KEY([AuthorID])
                    REFERENCES [dbo].[Author] ([AuthorID])

                    ALTER TABLE [dbo].[Book] CHECK CONSTRAINT [FK__Book__Author__4D94879B]
                END";

            var createSprocScript = @"
                USE [Library_2]

                IF NOT EXISTS (
                    SELECT 1
                    FROM sys.procedures WITH(NOLOCK)
                    WHERE NAME = 'UpdateBook'
                        AND type = 'P'
                )
                BEGIN
                    EXEC('
                      CREATE PROCEDURE [dbo].[UpdateBook]
                        @UDC INT,
                        @Title varchar(100),
                        @BookCover nvarchar(MAX),
                        @PageCount INT,
                        @Publisher varchar(100),
                        @UploadDate Date,
                        @PublishYear INT,
                        @Format varchar(50),
                        @FileSize decimal(5, 2),
                        @City varchar(50),
                        @Country varchar(50),
                        @FIO varchar(100),   
						@ListOfAuthors varchar(500)
                    AS
                    BEGIN
                        BEGIN TRANSACTION

                        BEGIN TRY
                            UPDATE Book
                            SET UDC = @UDC,
								Title = @Title,
                                BookCover = @BookCover, 
								PublishYear = @PublishYear,  
								Publisher = @Publisher, 
								UploadDate = @UploadDate,
                                PageCount = @PageCount,                             
                                Format = @Format,                              
                                FileSize = @FileSize,
                                ListOfAuthors = @ListOfAuthors

                            FROM Book
                            JOIN Author ON Book.AuthorID = Author.AuthorID
                            WHERE UDC = @UDC;

                            UPDATE Author
                            SET City = @City,
								Country = @Country,
                                FIO = @FIO 
                            FROM Author
                            JOIN Book ON Book.AuthorID = Author.AuthorID
                            WHERE Book.UDC = @UDC;

                            COMMIT TRAN;
                        END TRY
                        BEGIN CATCH
                            ROLLBACK TRAN;
                        END CATCH
                    END')
                END";

            var createTriggerScript = @"
                IF NOT EXISTS (SELECT 1 FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[Book_Update]'))
                BEGIN
                     EXEC('CREATE TRIGGER [dbo].[Book_Update]
                        ON [dbo].[Book]
                        AFTER UPDATE
                        AS
                            UPDATE Book
                            SET UpdatedAt = GETDATE()
                            WHERE UDC = (SELECT UDC FROM inserted)
            

                        ALTER TABLE [dbo].[Book] ENABLE TRIGGER [Book_Update]

                        ALTER TABLE [dbo].[Book] ENABLE TRIGGER [Book_Update]')
                END";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand commandDb = new SqlCommand(createDatabaseScript, connection))
                    using (SqlCommand commandAuthor = new SqlCommand(createAuthorScript, connection))
                    using (SqlCommand commandBook = new SqlCommand(createBookScript, connection))
                    using (SqlCommand commandSproc = new SqlCommand(createSprocScript, connection))
                    using (SqlCommand commandTrigger = new SqlCommand(createTriggerScript, connection))
                    {
                        commandDb.ExecuteNonQuery();
                        commandAuthor.ExecuteNonQuery();
                        commandBook.ExecuteNonQuery();
                        commandSproc.ExecuteNonQuery();
                        commandTrigger.ExecuteNonQuery();

                        Console.WriteLine("База данных была успешно создана");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при создании базы данных: " + ex.Message);
            }
        }
    }
}
