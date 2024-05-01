using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Org.BouncyCastle.Tls.Crypto;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.IO.Packaging;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Windows;
using TaskMastery.Assets.Components;
using TaskMastery.Model;
using TaskMastery.View;

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
        //cette methode permet de verifier si l'utilisateur existe dans la base de données
        public bool UserExistBeforeToDelete(string email, string password)
        {
            OpenConnection();
            //on crée la requete SQL
            _command.CommandText = "SELECT * FROM users WHERE email = @email";
            //on ajoute les parametres à la requete
            _command.Parameters.AddWithValue("email", email);
            //on execute la requete
            MySqlDataReader reader = _command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                if (BCrypt.Net.BCrypt.Verify(password, reader["password"].ToString()))
                {
                    //on ferme la connexion
                    _connection.Close();
                    return (true);
                }
                else
                {
                    MessageBox.Show("Mot de passe incorrect", "Connexion", MessageBoxButton.OK, MessageBoxImage.Error);
                    //on ferme la connexion
                    _connection.Close();
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Email incorrect", "Connexion", MessageBoxButton.OK, MessageBoxImage.Error);
                //on ferme la connexion
                _connection.Close();
                return false;
            }

        }
        //cette methode permet d'ajouter un utilisateur dans la base de données
        public bool AddUser(UserModel _userModel)
        {
            try
            {
                OpenConnection();
                //on crée une commande SQL pour ajouter une ligne dans la table
                _command = new MySqlCommand("INSERT INTO users (name, surname, pseudo, email, password, created_at) VALUES (@nom, @prenom, @pseudo, @email, @password, @created_at)", _connection);
                //on ajoute les paramètres de la commande SQL
                _command.Parameters.AddWithValue("@nom", _userModel.Name);
                _command.Parameters.AddWithValue("@prenom", _userModel.Surname);
                _command.Parameters.AddWithValue("@email", _userModel.Email);
                _command.Parameters.AddWithValue("@pseudo", _userModel.Pseudo);
                _command.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(_userModel.Password));
                _command.Parameters.AddWithValue("@created_at", DateTime.Now);
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
        public bool CheckFieldsNotExists(string Email, string Pseudo, BigInteger Id)
        {
            //controle que le mail et le pseudo n'existe pas déjà dans la base de données
            if (CheckExists(Email, "email", Id) || CheckExists(Pseudo, "pseudo", Id))
            {
                return false;
            }
            return true;
        }

        //cette methode permet de verifier si l'élément existe dans la base de données
        public bool CheckExists(string element, string elementToFind, BigInteger Id)
        {
            try
            {
                OpenConnection();
                //on crée la requete SQL
                _command.CommandText = $"SELECT * FROM users WHERE {elementToFind} = @element AND id != @id";
                //on ajoute les parametres à la requete
                _command.Parameters.AddWithValue("element", element);
                _command.Parameters.AddWithValue("id", Id);
                //on execute la requete
                MySqlDataReader reader = _command.ExecuteReader();
                //on retourne si le mail existe ou non
                return reader.HasRows;
            }catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            finally
            {
                   _connection.Close();
            }
        }
        //cette methode permet de suppimer un utilisateur de la base de données
        public bool DeleteUser(string email)
        {
            //On demande à l'utilisateur de confirmer la suppression avec le mot de passe à l'aide d'une MessageBox qu'il faut valider
            MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer votre compte ?", "Suppression", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return false;
            }
            else
            {
                //on demande le mot de passe
                string password = Microsoft.VisualBasic.Interaction.InputBox("Entrez votre mot de passe", "Suppression", "", -1, -1);
                //on vérifie que le mot de passe est correct
                if (!UserExistBeforeToDelete(email, password))
                {
                    MessageBox.Show("Mot de passe incorrect", "Suppression", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

            }
            OpenConnection();
            //on crée la requete SQL
            _command.CommandText = "DELETE FROM users WHERE email = @email";
            //on ajoute les parametres à la requete
            _command.Parameters.AddWithValue("email", email);
            //on execute la requete
            _command.ExecuteNonQuery();
            //on ferme la connexion
            _connection.Close();

            return true;
        }

        //cette methode permet de verifier si la connexion est valide
        public bool CheckUser(string mail, string password)
        {
            try
            {
                //on crée une commande SQL pour ajouter une ligne dans la table
                _command = new MySqlCommand("SELECT * FROM users WHERE email = @mail", _connection);
                //on ajoute les paramètres de la commande SQL
                _command.Parameters.AddWithValue("@mail", mail);
                //on ouvre la connexion
                _connection.Open();
                //on exécute la commande SQL, nonQuery car on ne récupère pas de données
                MySqlDataReader reader = _command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    if (BCrypt.Net.BCrypt.Verify(password, reader["password"].ToString()))
                    {
                        return (true);
                    }
                    else
                    {
                        MessageBox.Show("Mot de passe incorrect", "Connexion", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Email incorrect", "Connexion", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                //Dispose permet de libérer les ressources utilisées par l'objet
                _command.Dispose();
                //on ferme la connexion
                _connection.Close();
            }
        }

        private void OpenConnection()
        {
            //on ouvre la connexion
            _connection.Open();
            //on crée une commande SQL
            _command = _connection.CreateCommand();
        }
        public UserModel xRead_Pseudo(string _pseudo)
        {
            try
            {
                // Cette méthode va lire la table user et retourner la ligne correspondant au pseudo
                OpenConnection();
                // On crée la requête SQL
                _command.CommandText = "SELECT * FROM users WHERE pseudo = @pseudo";
                // On ajoute le paramètre à la requête
                _command.Parameters.AddWithValue("@pseudo", _pseudo);
                // On exécute la requête
                MySqlDataReader reader = _command.ExecuteReader();
                // Vérifier si le reader contient une ligne
                if (reader.HasRows)
                {
                    // On lit la première ligne
                    reader.Read();
                    //créé un objet User avec les données de la ligne
                    UserModel user = new UserModel();
                    user.Id = int.Parse(reader["id"].ToString());
                    user.Name = reader["name"].ToString();
                    user.Surname = reader["surname"].ToString();
                    user.Pseudo = reader["pseudo"].ToString();
                    user.Email = reader["email"].ToString();
                    user.Password = reader["password"].ToString();

                    // On ajoute l'objet User à la liste

                    // On retourne la liste
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            finally
            {
                // Assurez-vous de fermer la connexion même en cas d'erreur
                _connection.Close();
            }
        }
        public bool UpdateUser(UserModel _userUpdate)
        {
            //cette classe permet de mettre à jour un utilisateur dans la base de données
            try
            {
                //Vérifier que le nouveau pseudo ou mail n'existe pas déjà
                if (!CheckFieldsNotExists(_userUpdate.Email, _userUpdate.Pseudo, _userUpdate.Id))
                {
                    MessageBox.Show("Le pseudo ou l'email existe déjà", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                OpenConnection();
                //on crée une commande SQL pour mettre à jour une ligne dans la table
                _command = new MySqlCommand("UPDATE users SET name = @name, surname = @surname, email = @email, updated_at = @update_at, pseudo = @pseudo WHERE id = @id", _connection);
                //on ajoute les paramètres de la commande SQL
                _command.Parameters.AddWithValue("@id", _userUpdate.Id);
                _command.Parameters.AddWithValue("@name", _userUpdate.Name);
                _command.Parameters.AddWithValue("@surname", _userUpdate.Surname);
                _command.Parameters.AddWithValue("@email", _userUpdate.Email);
                _command.Parameters.AddWithValue("@pseudo", _userUpdate.Pseudo);
                _command.Parameters.AddWithValue("@update_at", DateTime.Now);
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

        public List<ProjectModel> LoadProjectsFromDatabase(string pseudo)
        {
            // Retourner la liste des projets chargés selon le pseudo de l'utilisateur
            try
            {
                OpenConnection();
                //on crée une commande SQL pour mettre à jour une ligne dans la table
                _command = new MySqlCommand("SELECT P.id, designation FROM projets as P " +
                    "INNER JOIN users as U ON U.id = P.id_user " +
                    "WHERE pseudo = @pseudo " +
                    "ORDER BY designation;", _connection);
                //on ajoute les paramètres de la commande SQL
                _command.Parameters.AddWithValue("@pseudo", pseudo);
                //on execute la requete
                MySqlDataReader reader = _command.ExecuteReader();
                //on crée une liste de projets
                List<ProjectModel> projects = new List<ProjectModel>();
                //on lit les données
                while (reader.Read())
                {
                    ProjectModel project = new ProjectModel();
                    project.Id = int.Parse(reader["id"].ToString());
                    project.Projet = reader["designation"].ToString();
                    projects.Add(project);
                }
                return projects;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return new List<ProjectModel>();
            }
            finally
            {
                _connection.Close();
            }            
        }
        public ObservableCollection<EtiquetteModel> LoadEtiquettesFromDatabase(string pseudo)
        {
            // Retourner la liste des étiquettes chargées selon le pseudo de l'utilisateur
            try
            {
                OpenConnection();
                //on crée une commande SQL pour mettre à jour une ligne dans la table
                _command = new MySqlCommand("SELECT E.id, designation FROM etiquettes as E " +
                                       "INNER JOIN users as U ON U.id = E.id_user " +
                                       "WHERE pseudo = @pseudo " +
                                       "ORDER BY designation;", _connection);
                //on ajoute les paramètres de la commande SQL
                _command.Parameters.AddWithValue("@pseudo", pseudo);
                //on execute la requete
                MySqlDataReader reader = _command.ExecuteReader();
                //on crée une liste d'étiquettes
                ObservableCollection<EtiquetteModel> etiquettes = new ObservableCollection<EtiquetteModel>();
                //on lit les données
                while (reader.Read())
                {
                    EtiquetteModel etiquette = new EtiquetteModel();
                    etiquette.Id = int.Parse(reader["id"].ToString());
                    etiquette.Designation = reader["designation"].ToString();
                    etiquettes.Add(etiquette);
                }
                return etiquettes;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return new ObservableCollection<EtiquetteModel>();
            }
            finally
            {
                _connection.Close();
            }
        }
        public ObservableCollection<TacheModel> LoadTachesFromDatabase(BigInteger id_Projet)
        {
            // Retourner la liste des tâches chargées selon le pseudo de l'utilisateur
            try
            {
                OpenConnection();
                //on crée une commande SQL pour mettre à jour une ligne dans la table
                _command = new MySqlCommand("SELECT T.id, T.titre as T_designation, S.designation as S_designation, E.designation as " +
                    "E_designation, count(A.id) as NbrParticipant " +
                    "FROM taches as T " +
                    "LEFT JOIN attributions as A ON A.id_tache = T.id " +
                    "INNER JOIN projets as P ON P.id = T.id_projet " +
                    "LEFT JOIN statuts as S ON S.id = T.id_statut " +
                    "LEFT JOIN etiquettes as E ON E.id = T.id_etiquette " +
                    "WHERE P.id = @id_projet_sys " +
                    "GROUP BY T.id, T.designation, S.designation, E.designation;", _connection);
                //on ajoute les paramètres de la commande SQL
                _command.Parameters.AddWithValue("@id_projet_sys", id_Projet);
                //on execute la requete
                MySqlDataReader reader = _command.ExecuteReader();
                //on crée une liste de tâches
                ObservableCollection<TacheModel> taches = new ObservableCollection<TacheModel>();
                //on lit les données
                while (reader.Read())
                {
                    TacheModel tache = new TacheModel();
                    tache.Id = int.Parse(reader["id"].ToString());
                    tache.Titre = reader["T_designation"].ToString();
                    tache.Statut = reader["S_designation"].ToString();
                    tache.Etiquette = reader["E_designation"].ToString();
                    tache.NombreParticipants = int.Parse(reader["NbrParticipant"].ToString());
                    taches.Add(tache);
                }
                return taches;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return new ObservableCollection<TacheModel>();
            }
            finally
            {
                _connection.Close();
            }
        }
        public UserModel GetUser(bool bEmail, string element)
        {
            //récupérer les informations de l'utilisateur et les renvoyer dans une ObservableCollection
            try
            {
                OpenConnection();
                //on crée la requete SQL
                _command.CommandText = "SELECT * FROM users WHERE ";
                //on ajoute les parametres à la requete
                if (bEmail)
                {
                    _command.CommandText += "email = @email";
                    _command.Parameters.AddWithValue("email", element);
                }
                else
                {
                    _command.CommandText += "pseudo = @pseudo";
                    _command.Parameters.AddWithValue("pseudo", element);
                }
                //on execute la requete
                MySqlDataReader reader = _command.ExecuteReader();
                //on lit les données
                if (reader.Read())
                {
                    UserModel user = new UserModel();
                    user.Id = Int64.Parse(reader["id"].ToString());
                    user.Name = reader["name"].ToString();
                    user.Surname = reader["surname"].ToString();
                    user.Email = reader["email"].ToString();
                    user.Pseudo = reader["pseudo"].ToString();
                    user.Password = reader["password"].ToString();
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            finally
            {
                _connection.Close();
            }
        }
        public bool UpdatePassword(BigInteger id, string password)
        {
            //cette classe permet de mettre à jour un utilisateur dans la base de données
            try
            {
                OpenConnection();
                //on crée une commande SQL pour mettre à jour une ligne dans la table
                _command = new MySqlCommand("UPDATE users SET password = @password WHERE id = @id", _connection);
                //on ajoute les paramètres de la commande SQL
                _command.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(password));
                _command.Parameters.AddWithValue("@id", id);
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
    }
}
