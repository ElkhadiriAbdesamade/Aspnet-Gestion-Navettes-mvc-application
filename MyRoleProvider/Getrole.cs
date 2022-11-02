using Gestion_Navettes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gestion_Navettes.MyRoleProvider
{
    public static class Getrole
    {
        public static String getrole(string username)
        {
            if (username == "")
            {
                return "";
            }
            Gestion_NavettesEntities db = new Gestion_NavettesEntities();
            string data = db.users.Where(x => x.lgn == username).FirstOrDefault().roles;
            return data;
        }
        public static int getid(string username)
        {
            if (username == "")
            {
                return -1;
            }
            Gestion_NavettesEntities db = new Gestion_NavettesEntities();
            int data = db.users.Where(x => x.lgn == username).FirstOrDefault().id_user;
            return data;
        }
       
    }
}