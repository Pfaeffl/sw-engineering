using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LagerverwaltungLib;

namespace LagerverwaltungWebApp.Controllers
{
    public class BaseController : Controller
    {
        protected LagerverwaltungModelContainer db = new LagerverwaltungModelContainer();

        private static String UserIdKey = "UserId";
        public User setUser()
        {
            int? userId = Session[UserIdKey] as int?;
            if (userId == null) return null;
            User res = db.Users.Find(userId);
            if (res == null) return null;
            ViewBag.User = res;
            _LoggedInUser = res;
            return res;
        }

        private User _LoggedInUser = null;
        protected User LoggedInUser => _LoggedInUser ?? setUser();

        protected bool hasUser()
        {
           return LoggedInUser != null;
            
        }

        protected void setUserId(int userId)
        {
            Session[UserIdKey] = userId;
        }

        protected void resetUserId()
        {
            Session[UserIdKey] = null;
        }

        public bool getRoleBesteller()
        {
            if (LoggedInUser != null)
            {
                return LoggedInUser.Besteller;
            }
            return false;
        }

        public bool getRoleLagerist()
        {
            if (LoggedInUser !=null)
            {
                return LoggedInUser.Lagerist;
            }
            return false;
        }

        public bool getRolePersonal()
        {
            if (LoggedInUser != null)
            {
                return LoggedInUser.Personal;
            }
            return false;
        }

        public bool getRoleSystemadm()
        {
            if (LoggedInUser != null)
            {
                return LoggedInUser.Systemadm;
            }
            return false;
        }

        public string getName()
        {
            if(LoggedInUser != null)
            {
                return LoggedInUser.Vorname + " " + LoggedInUser.Name;
            }
            return null;
        }

    }
}
