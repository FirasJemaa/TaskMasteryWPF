using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TaskMastery.Model
{
    internal class UserModel
    {
        public BigInteger Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Pseudo { get; set; }

        //Liste des projets de l'utilisateur
        public List<ProjectModel> Projects { get; set; }
        public UserModel() { 
            Name = "";
            Surname = "";
            Email = "";
            Password = "";
            Pseudo = "";
            Projects = new List<ProjectModel>();
        }

    }
}
