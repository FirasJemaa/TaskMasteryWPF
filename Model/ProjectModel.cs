using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TaskMastery.Model
{
    internal class ProjectModel
    {
        public BigInteger Id { get; set; }
        public BigInteger IdUser { get; set; }
        public string Projet { get; set; }
        public string Statut { get; set; }
        public int NombreStatut { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
    }
}
