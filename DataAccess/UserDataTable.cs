using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using TaskMastery.Model;

namespace TaskMastery.DataAccess
{
    //cette class va perme de faire les requetes SQL sur la table user
    class UserDataTable
    {
        //string de connexion à la base de données
        const string _dsn = "Server=localhost;Database=taskmastery;username=root;password=;";
        //MySqlConnection permet de se connecter à une base de données MySQL
        private MySqlConnection _connection = new MySqlConnection(_dsn);
        //MySqlCommand permet d'exécuter des commandes SQL
        private MySqlCommand? _command;
        private string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d\s]).{8,}$";
        //cette methode permet de verifier si l'utilisateur existe dans la base de données
        public bool UserExist(string email, string password)
        {
            OpenConnection();
            //on crée la requete SQL
            _command.CommandText = "SELECT * FROM user WHERE email = @email AND password = @password";
            //on ajoute les parametres à la requete
            _command.Parameters.AddWithValue("email", email);
            _command.Parameters.AddWithValue("password", password);
            //on execute la requete
            MySqlDataReader reader = _command.ExecuteReader();
            //on ferme la connexion
            _connection.Close();
            //on retourne si l'utilisateur existe ou non
            return reader.HasRows;
        }
        //cette methode permet d'ajouter un utilisateur dans la base de données
        public bool AddUser(UserModel _userModel)
        {
            try
            {
                OpenConnection();
                //on crée la requete SQL
                _command.CommandText = "INSERT INTO user (email, password) VALUES (@email, @password)";
                //on ajoute les parametres à la requete
                _command.Parameters.AddWithValue("email", _userModel.Email);
                _command.Parameters.AddWithValue("password", _userModel.Password);
                //on execute la requete
                _command.ExecuteNonQuery();
                //on ferme la connexion
                _connection.Close();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        //cette methode permet de verifier si les champs sont corrects
        public bool CheckFieldsNotExists(UserModel _userModel)
        {
            //controle que le mail et le pseudo n'existe pas déjà dans la base de données
            if (CheckExists(_userModel.Email, "email") || CheckExists(_userModel.Pseudo, "pseudo"))
            {
                return false;
            }
            return true;
        }

        //cette methode permet de verifier si l'élément existe dans la base de données
        public bool CheckExists(string element, string elementToFind)
        {
            OpenConnection();
            //on crée la requete SQL
            _command.CommandText = $"SELECT * FROM user WHERE {elementToFind} = @element";
            //on ajoute les parametres à la requete
            _command.Parameters.AddWithValue("element", element);
            //on execute la requete
            MySqlDataReader reader = _command.ExecuteReader();
            //on ferme la connexion
            _connection.Close();
            //on retourne si le mail existe ou non
            return reader.HasRows;
        }        
        //cette methode permet de suppimer un utilisateur de la base de données
        public bool DeleteUser(string email)
        {
            OpenConnection();
            //on crée la requete SQL
            _command.CommandText = "DELETE FROM user WHERE email = @email";
            //on ajoute les parametres à la requete
            _command.Parameters.AddWithValue("email", email);
            //on execute la requete
            _command.ExecuteNonQuery();
            //on ferme la connexion
            _connection.Close();
            return true;
        }

        //cette methode permet de verifier si la connexion est valide
        public bool CheckUser(string mail,  string password)
        {
            OpenConnection();
            //on crée la requete SQL
            _command.CommandText = "SELECT * FROM user WHERE email = @email AND password = @password";
            //on ajoute les parametres à la requete
            _command.Parameters.AddWithValue("email", mail);
            _command.Parameters.AddWithValue("password", password);
            //on execute la requete
            MySqlDataReader reader = _command.ExecuteReader();
            //on ferme la connexion
            _connection.Close();
            //on retourne si l'utilisateur existe ou non
            return reader.HasRows;
        }

        private void OpenConnection()
        {
            //on ouvre la connexion
            _connection.Open();
            //on crée une commande SQL
            _command = _connection.CreateCommand();
        }
    }
}
