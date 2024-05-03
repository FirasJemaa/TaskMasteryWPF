using System.Numerics;
using System.Windows;
using TaskMastery.DataAccess;

namespace TaskMastery.Model
{
    public class EtiquetteModel
    {
        public BigInteger id_userCurrent;
        private BigInteger _id;
        public BigInteger Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        private string _designation;
        public string Designation { 
            get
            {
                return _designation;
            } 
            set
            {
                _designation = value;
                updateField();
            }
        }
        private BigInteger _id_User;
        public BigInteger Id_User {
            get
            {
                return _id_User;
            }
            set
            {
                _id_User = value;
                
            }
        }
        UserDataTable dataAccess;

        public EtiquetteModel(string pseudo)
        {
            dataAccess = new UserDataTable();
            id_userCurrent = dataAccess.GetId(pseudo);
            _designation = "";
        }
        public EtiquetteModel(BigInteger _id_userCurrent)
        {
            dataAccess = new UserDataTable();
            this.id_userCurrent = _id_userCurrent;
        }
        private void updateField()
        {
            //Mise à jour de l'étiquette sinon création
            if (_id == 0)
            {
                _id = dataAccess.InsertEtiquette(_designation, id_userCurrent);
            }
            else
            {
                dataAccess.UpdateEtiquette(_id, _designation);
            }

        }
    }
}
