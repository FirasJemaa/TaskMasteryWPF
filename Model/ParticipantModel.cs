using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TaskMastery.Model
{
    class ParticipantModel
    {
        public BigInteger Id { get; set; }
        public string Pseudo { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public ParticipantModel()
        {
            Id = 0;
            Pseudo = "";
            Nom = "";
            Prenom = "";
        }
    }
}
