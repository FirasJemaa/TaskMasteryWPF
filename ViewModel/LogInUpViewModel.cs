using System.Security;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TaskMastery.Command;
using TaskMastery.DataAccess;
using TaskMastery.View;
using TaskMastery.Model;
using System.Numerics;

namespace TaskMastery.ViewModel
{
    public class LogInUpViewModel : ViewModelBase
    {
        private readonly UserDataTable _userDataTable;
        private readonly Window? _LogWindow;
        private readonly Window? _CurrentWindow;
        private BigInteger _id;
        public BigInteger Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        private string _surname;
        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(_email));
            }
        }
        private string _pseudo;
        public string Pseudo
        {
            get => _pseudo;
            set
            {
                _pseudo = value;
                OnPropertyChanged(nameof(Pseudo));
            }
        }
        // Propriété pour le mot de passe
        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        private string _confirmPassword;
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }
        private string _oldPassword;
        public string OldPassword
        {
            get { return _oldPassword; }
            set
            {
                _oldPassword = value;
                OnPropertyChanged(nameof(OldPassword));
            }
        }
        public ICommand? ConnexionCommand { get; }
        public ICommand? InscriptionCommand { get; }
        public ICommand? UpdatePasswordCommand { get; }
        public ICommand? UpdateCommand { get; }
        public ICommand? DeleteCommand { get; }
        readonly string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d\s]).{11,}$";
        public LogInUpViewModel()
        {
            _surname = "";
            _name = "";
            _email = "";
            _pseudo = "";
            _password = "";
            _confirmPassword = "";
            _oldPassword = "";
            _userDataTable = new UserDataTable();
        }
        public LogInUpViewModel(Window logWindow)
        {
            _surname = "";
            _name = "";
            _email = "";
            _pseudo = "";
            _password = "";
            _confirmPassword = "";
            _oldPassword = "";
            ConnexionCommand = new RelayCommand((param) => LogIn());
            InscriptionCommand = new RelayCommand((param) => LogUp());
            _userDataTable = new UserDataTable();
            _LogWindow = logWindow;
        }
        public LogInUpViewModel(string pseudo, Window currentWindow)
        {
            _surname = "";
            _name = "";
            _email = "";
            _pseudo = "";
            _password = "";
            _confirmPassword = "";
            _oldPassword = "";
            UpdateCommand = new RelayCommand((param) => UpdateUser());
            DeleteCommand = new RelayCommand((param) => DeleteUser());
            UpdatePasswordCommand = new RelayCommand((param) => UpdatePassUser());
            _userDataTable = new UserDataTable();
            GetUser(false, pseudo);
            _CurrentWindow = currentWindow;
        }

        public bool RegexPassword()
        {
            //controle que le mot de passe fait plus de 8 caractères, Une majuscule, un chiffre et un caractère spécial
            if (!Regex.IsMatch(Password, passwordPattern))
            {
                MessageBox.Show("Le mot de passe doit contenir au moins 8 caractères, une majuscule, un chiffre et un caractère spécial");
                return false;
            }
            return true;
        }
        public void DeleteUser()
        {
            if (_userDataTable.DeleteUser(Email))
            {
                MessageBox.Show("Utilisateur supprimé");
                LogView _logInUpWindow = new();
                _logInUpWindow.Show();
                _CurrentWindow?.Close();
            }
        }
        private void LogUp()
        {
            //Vérifier qu'il y a aucun champ vide sauf id
            if (string.IsNullOrEmpty(Surname) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Pseudo))
            {
                MessageBox.Show("Veuillez remplir tous les champs");
                return;
            }
            if (RegexPassword())
            {
                if (Password == ConfirmPassword)
                {
                    //vérifier que l'email et le pseudo n'existent pas déjà
                    if (_userDataTable.CheckFieldsNotExists(Email, Pseudo, Id))
                    {
                        //créer un nouvel utilisateur
                        UserModel _user = new()
                        {
                            Surname = Surname,
                            Name = Name,
                            Email = Email,
                            Pseudo = Pseudo,
                            Password = Password
                        };
                        _userDataTable.AddUser(_user);
                        MainWindow _MainWindowView = new(Pseudo);
                        _MainWindowView.Show();
                        _LogWindow?.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Les mots de passe ne correspondent pas");
                }
            }
        }
        public void LogIn()
        {
            if (NotEmpty() || RegexPassword())
            {
                //vérifier que l'email et le mot de passe sont corrects
                if (_userDataTable.CheckUser(Email, Password))
                {
                    GetUser(true, Email);
                    MainWindow _MainWindowView = new(Pseudo);
                    _MainWindowView.Show();
                    _LogWindow?.Close();
                }
            }
        }
        private bool NotEmpty()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Veuillez remplir tous les champs");
                return false;
            }
            return true;
        }
        private void UpdateUser()
        {
            UserModel _user = new()
            {
                Id = Id,
                Surname = Surname,
                Name = Name,
                Email = Email,
                Pseudo = Pseudo
            };
            if (_userDataTable.UpdateUser(_user))
            {
                MessageBox.Show("Utilisateur modifié");
            }
        }
        private void GetUser(bool bMail, string contenu)
        {
            UserModel? _user = _userDataTable.GetUser(bMail, contenu);
            if (_user != null) { 
                Id = _user.Id;
                Surname = _user.Surname;
                Name = _user.Name;
                Pseudo = _user.Pseudo;
                Email = _user.Email;
            }
        }
        private void UpdatePassUser()
        {
            //Vérifier que l'aien mot de passe est correct
            if (_userDataTable.CheckUser(Email, OldPassword))
            {
                if (RegexPassword())
                {
                    if (Password == ConfirmPassword)
                    {
                        if (_userDataTable.UpdatePassword(Id,Password))
                        {
                            MessageBox.Show("Mot de passe modifié");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Les mots de passe ne correspondent pas");
                    }
                }
            }
            
        }
    }
}
