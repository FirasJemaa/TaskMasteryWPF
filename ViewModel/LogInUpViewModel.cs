using System.Security;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TaskMastery.Command;
using TaskMastery.DataAccess;
using TaskMastery.Model;

namespace TaskMastery.ViewModel
{
    public class LogInUpViewModel : ViewModelBase
    {
        private UserModel _user;
        private UserDataTable _userDataTable;
        public ICommand? ConnexionCommand { get; }
        public ICommand? InscriptionCommand { get; }
        string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d\s]).{8,}$";
        public LogInUpViewModel()
        {
            ConnexionCommand = new RelayCommand((param) => Connexion(param));
            InscriptionCommand = new RelayCommand((param) => Inscription(param));
            _user = new UserModel();
            _userDataTable = new UserDataTable();
        }
        public string Surname
        {
            get => _user.Surname;
            set
            {
                _user.Surname = value;
                OnPropertyChanged(nameof(_user.Surname));
            }
        }
        public string Name
        {
            get => _user.Name;
            set
            {
                _user.Name = value;
                OnPropertyChanged(nameof(_user.Name));
            }
        }
        public string Email
        {
            get => _user.Email;
            set
            {
                _user.Email = value;
                OnPropertyChanged(nameof(_user.Email));
            }
        }
        public string Pseudo
        {
            get => _user.Pseudo;
            set
            {
                _user.Pseudo = value;
                OnPropertyChanged(nameof(_user.Pseudo));
            }
        }
        // Mot de passe sécurisé
        private SecureString _password = new SecureString();
        // Propriété pour le mot de passe
        public SecureString Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        // Méthode pour obtenir le mot de passe en clair
        private string GetPasswordAsString()
        {
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(Password);
            try
            {
                return System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
        }
        //Vérifier si le mots de passe est valide
        private bool RegexPassword()
        {
            //controle que le mot de passe fait plus de 8 caractères, Une majuscule, un chiffre et un caractère spécial
            string password = GetPasswordAsString();
            if (!Regex.IsMatch(password, passwordPattern))
            {
                MessageBox.Show("Le mot de passe doit contenir au moins 8 caractères, une majuscule, un chiffre et un caractère spécial");
                return false;
            }
            return true;
        }
        public void DeleteUser()
        {
            if (_userDataTable.DeleteUser(_user.Email))
            {
                MessageBox.Show("User deleted");
            }
            else
            {
                MessageBox.Show("User not found");
            }
        }
        private void Inscription(object param)
        {
            if (_userDataTable.CheckFieldsNotExists(_user)) {
                _userDataTable.AddUser(_user);
                MessageBox.Show("Inscription");
            }
        }
        private void Connexion(object param)
        {
            if (RegexPassword())
            {
                MessageBox.Show("Connexion");
            }
        }
    }
}
