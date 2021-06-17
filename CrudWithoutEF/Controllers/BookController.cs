using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrudWithoutEF.Data;
using CrudWithoutEF.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CrudWithoutEF.Controllers
{
    /* This Controller uses ADO.NET and Stored Procedures in the SQL Database 
     to perform its CRUD operations.  This is the "old school" and classic way to 
     perform database operation in C#/.NET.  It's fast, but can look a little bit 
     muddy compared to using Entity Framework or another ORM.
    
     In order for this project to work on your machine, please see the SQL 
     scripts included for creating the database (and Books table) under the README, 
     and change the connection string in AppSettings.json to be your connection string. */
    public class BookController : Controller
    {
        private readonly IConfiguration _configuration;

        public BookController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: Book
        public IActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("BookViewAll", conn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(dtbl);

            }
            return View(dtbl);
        }

        // GET: Book/AddOrEdit/
        public IActionResult AddOrEdit(int? id)
        {
            BookViewModel bookViewModel = new BookViewModel();
            if(id > 0)
            {
                bookViewModel = fetchBookByID(id);
            }
            return View(bookViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("BookID,Title,Author,Price")] BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("BookAddOrEdit", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("BookID",bookViewModel.BookID);
                    command.Parameters.AddWithValue("Title", bookViewModel.Title);
                    command.Parameters.AddWithValue("Author", bookViewModel.Author);
                    command.Parameters.AddWithValue("Price", bookViewModel.Price);
                    command.ExecuteNonQuery();

                }
                    return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Book/Delete/5
        public IActionResult Delete(int? id)
        {
            BookViewModel book = fetchBookByID(id);
            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("BookDeleteByID", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("BookID", id);
                command.ExecuteNonQuery();

            }
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public BookViewModel fetchBookByID(int? id)
        {
            BookViewModel book = new BookViewModel();
            using(SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dtbl = new DataTable();
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("BookViewByID", conn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("BookID", id);
                da.Fill(dtbl);
                if (dtbl.Rows.Count == 1)
                {
                    book.BookID = Convert.ToInt32(dtbl.Rows[0]["BookID"].ToString());
                    book.Title = dtbl.Rows[0]["Title"].ToString();
                    book.Author = dtbl.Rows[0]["Author"].ToString();
                    book.Price = Convert.ToInt32(dtbl.Rows[0]["Price"].ToString());
                }
                return book;
            }
        }
    }
}
