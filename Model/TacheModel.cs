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
        public int Id { get; set; }
        public string Titre { get; set; }
        public string Statut { get; set; }
        public string Etiquette { get; set; }
        public int NombreParticipants { get; set; }
    }
}
