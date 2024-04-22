using System.Security;
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
        public ICommand? LogInUpCommand { get; }
        // Enumération pour représenter l'action à effectuer
        public enum Action
        {
            Inscription,
            Connexion,
            Suppression
        }
        public LogInUpViewModel()
        {
            Action action = Action.Inscription;
            _user = new UserModel();
            _userDataTable = new UserDataTable();
            if (action == Action.Inscription)
            {
                LogInUpCommand = new RelayCommand((param) => Inscription(param));
            }
            else if (action == Action.Connexion)
            {
                LogInUpCommand = new RelayCommand((param) => Connexion(param));
            }
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

        private SecureString? _password;
        public SecureString? Password
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

        private void LoggedIn(object param)
        {
            if (param == null)
            {
                MessageBox.Show("Veuillez remplir tous les champs");
                return;
            }
            // Tester si l'utilisateur est déjà inscrit
            if (_userDataTable.CheckUser(_user.Email, GetPasswordAsString()))
            {
                MessageBox.Show($"Connexion réussi {param}");
            }
            else
            {
                MessageBox.Show("Connexion échouée");
            }
        }

        public void AddUser()
        {
            _userDataTable.AddUser(_user);
        }
        public bool CheckFields()
        {
            return _userDataTable.CheckFieldsNotExists(_user);
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
            };
        }
        private void Inscription(object param)
        {
            // Logique pour l'inscription
        }
        private void Connexion(object param)
        {
            // Logique pour la connexion
        }
    }
}
