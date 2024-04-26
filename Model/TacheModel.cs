using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMastery.Model
{
    internal class TacheModel
    {
        public BigInteger Id { get; set; }
        public int Priorite { get; set; }
        public string Titre { get; set; }
        public string Designation { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateCloture { get; set; }
        public bool Etat { get; set; }
        public BigInteger IdProjet { get; set; }
        public BigInteger IdCouleur { get; set; }
        public BigInteger IdEtiquette { get; set; }
        public BigInteger IdStatut { get; set; }
    }
}
