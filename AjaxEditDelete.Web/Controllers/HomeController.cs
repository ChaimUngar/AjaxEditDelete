using AjaxEditDelete.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace AjaxEditDelete.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress; Initial Catalog=PeopleAndCars; Integrated Security=true;";
        //private string _conStr = "Data Source=.\\sqlexpress; Initial Catalog=PeopleAndCars; Integrated Security=true;";


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetPeople()
        {
            var repo = new PeopleRepo(_connectionString);
            List<Person> people = repo.GetAll();
            return Json(people);
        }

        [HttpPost]
        public IActionResult AddPerson(Person p)
        {
            var repo = new PeopleRepo(_connectionString);
            repo.Add(p);
            return Json(p);
        }

        public IActionResult Update(int id)
        {
            var repo = new PeopleRepo(_connectionString);
            Person p = repo.GetPersonById(id);
            return Json(p);
        }

        [HttpPost]
        public IActionResult Update(Person p)
        {
            var repo = new PeopleRepo(_connectionString);
            int id = repo.Edit(p);
            p = repo.GetPersonById(id);
            return Json(p);
        }

        public IActionResult Delete(int id)
        {
            var repo = new PeopleRepo(_connectionString);
            repo.Delete(id);
            return Json(repo.GetAll());
        }
    }
}