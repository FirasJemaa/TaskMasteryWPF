using System.Security;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TaskMastery.Command;
using TaskMastery.DataAccess;
using TaskMastery.View;
using TaskMastery.Model;

namespace TaskMastery.ViewModel
{
    public class LogInUpViewModel : ViewModelBase
    {
        private readonly UserModel _user;
        private readonly UserDataTable _userDataTable;
        private readonly Window _LogWindow;
        private readonly Window _CurrentWindow;
        public ICommand? ConnexionCommand { get; }
        public ICommand? InscriptionCommand { get; }
        public ICommand? UpdatePassCommand { get; }
        public ICommand? UpdateCommand { get; }
        public ICommand? DeleteCommand { get; }

        string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d\s]).{8,}$";
        public LogInUpViewModel(Window logWindow)
        {
            ConnexionCommand = new RelayCommand((param) => Connexion(param));
            InscriptionCommand = new RelayCommand((param) => Inscription(param));
            _user = new UserModel();
            _user.Email = "soufiane@gmail.com";
            _userDataTable = new UserDataTable();
            _LogWindow = logWindow;
        }
        public LogInUpViewModel(string _pseudo, Window currentWindow)
        {
            _CurrentWindow = currentWindow;
            _user = new UserModel();
            _userDataTable = new UserDataTable();
            _user = _userDataTable.xRead_Pseudo(_pseudo);
            UpdateCommand = new RelayCommand((param) => UpdateUser());
            DeleteCommand = new RelayCommand((param) => DeleteUser());
            //UpdatePassCommand = new RelayCommand((param) => UpdatePassUser());
        }
        public string Id
        {
            get => _user.Id.ToString();
            set
            {
                _user.Id = int.Parse(value);
                OnPropertyChanged(nameof(_user.Id));
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
        // Propriété pour le mot de passe
        public SecureString Password
        {
            get { return Password; }
            set
            {
                Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public SecureString ConfirmPassword
        {
            get { return ConfirmPassword; }
            set
            {
                ConfirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }
        //Vérifier si le mots de passe est valide
        private bool RegexPassword()
        {
            //controle que le mot de passe fait plus de 8 caractères, Une majuscule, un chiffre et un caractère spécial
            if (!Regex.IsMatch(_user.Password, passwordPattern))
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
                MessageBox.Show("Utilisateur supprimé");
                LogView _logInUpWindow = new LogView();
                _logInUpWindow.Show();
                _CurrentWindow.Close();
            }
        }
        private void Inscription(object param)
        {
            //récupérer le mot de passe et le mettre dans la variable _password
            _user.Password = ((System.Windows.Controls.PasswordBox)param).Password;
            if (_user.Password.Length > 0)
            {
                //vérifier que l'email et le pseudo n'existent pas déjà
                if (_userDataTable.CheckFieldsNotExists(_user))
                {
                    _userDataTable.AddUser(_user);
                    //fermer la view de connexion
                    _LogWindow.Close();
                }
            }
        }
        private void Connexion(object param)
        {
            //récupérer le mot de passe et le mettre dans la variable _password
            _user.Password = ((System.Windows.Controls.PasswordBox)param).Password;
            if (NotEmpty() || RegexPassword())
            {
                //vérifier que l'email et le mot de passe sont corrects
                if (_userDataTable.CheckUser(_user.Email, _user.Password))
                {
                    //fermer la view de connexion
                    _LogWindow.Close();
                }
            }
        }
        private bool NotEmpty()
        {
            if (string.IsNullOrEmpty(_user.Email) || string.IsNullOrEmpty(_user.Password))
            {
                MessageBox.Show("Veuillez remplir tous les champs");
                return false;
            }
            return true;
        }
        private void UpdateUser()
        {
            if (_userDataTable.UpdateUser(_user))
            {
                MessageBox.Show("Utilisateur modifié");
            }
        }   
        private void UpdatePassUser(object param)
        {
            MessageBox.Show(param.ToString());
        }
    }
}
