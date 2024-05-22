using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMastery.Model
{
    public class TacheModel
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public string Statut { get; set; }
        public string Etiquette { get; set; }
        public int NombreParticipants { get; set; }
        public string Cheklist { get; set; }
        public TacheModel()
        {
            Id = 0;
            Titre = "";
            Statut = "";
            Etiquette = "";
            NombreParticipants = 0;
            Cheklist = "";
        }
    }
}
