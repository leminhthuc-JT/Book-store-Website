using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LTW_Ban_Sach.Models;

namespace LTW_Ban_Sach.ApiControllers
{
    public class BooksController : ApiController
    {
        private DBContext db = new DBContext();
        public List<Books> GetBooks()
        {
            List<Books> books = db.Books.ToList();
            return books;
        }
    }
}
