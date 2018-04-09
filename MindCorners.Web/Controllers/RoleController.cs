using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using MindCorners.Authentication;

namespace MindCorners.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        ApplicationDbContext context;

        public RoleController()
        {
            context = new ApplicationDbContext();
        }
        /// <summary>
        /// Get All Roles
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var Roles = context.Roles.ToList();
            return View(Roles);
        }

        /// <summary>
        /// Create  a New role
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var role = new IdentityRole();
            return View("Form", role);
        }

        /// <summary>
        /// Create a New Role
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(IdentityRole role)
        {
            var roleDb = context.Roles.FirstOrDefault(p => p.Id == role.Id);
            if (roleDb == null)
            {
                context.Roles.Add(role);
            }
            else
            {
                roleDb.Name = role.Name;
            }

            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(string id)
        {
            var role = context.Roles.FirstOrDefault(p => p.Id == id);
            return View("Form",role);
        }

    }
}