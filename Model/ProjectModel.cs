using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TaskMastery.Model
{
    public class ProjectModel
    {
        public BigInteger Id { get; set; }
        public BigInteger IdUser { get; set; }
        public string Projet { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        public ProjectModel()
        {
            Id = 0;
            IdUser = 0;
            Projet = "";
        }
    }
}
