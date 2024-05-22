using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Windows;
using TaskMastery.Model;

namespace TaskMastery.DataAccess
{
    //cette class va perme de faire les requetes SQL sur la table user
    public class UserDataTable
    {
        //string de connexion à la base de données
        const string _dsn = "Server=localhost;Database=taskmastery;username=root;password=;";
        //MySqlConnection permet de se connecter à une base de données MySQL
        private readonly MySqlConnection _connection;
        //MySqlCommand permet d'exécuter des commandes SQL
        private MySqlCommand _command;
        //cette methode permet de verifier si l'utilisateur existe dans la base de données
        public UserDataTable()
        {
            _command = new();
            _connection = new(_dsn);
        }
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
            }
            catch (Exception e)
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
        public UserModel? Read_Pseudo(string _pseudo)
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
                    if (reader.Read())
                    {

                        //créé un objet User avec les données de la ligne
                        UserModel user = new()
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")).ToString(),
                            Surname = reader.GetString(reader.GetOrdinal("surname")).ToString(),
                            Pseudo = reader.GetString(reader.GetOrdinal("pseudo")).ToString(),
                            Email = reader.GetString(reader.GetOrdinal("email")).ToString(),
                            Password = reader.GetString(reader.GetOrdinal("password")).ToString()
                        };

                        // On ajoute l'objet User à la liste

                        // On retourne la liste
                        return user;

                    }
                    else
                    {
                        return null;
                    }


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
                List<ProjectModel> projects = [];
                //on lit les données
                while (reader.Read())
                {
                    ProjectModel project = new()
                    {
                        Id = reader.GetInt64(reader.GetOrdinal("id")),
                        Projet = reader.GetString(reader.GetOrdinal("designation")).ToString()
                    };
                    projects.Add(project);
                }
                return projects;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return [];
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
                _command = new MySqlCommand("SELECT E.id, designation, U.id as idUser, id_user FROM etiquettes as E " +
                                       "INNER JOIN users as U ON U.id = E.id_user " +
                                       "WHERE pseudo = @pseudo " +
                                       "ORDER BY designation;", _connection);
                //on ajoute les paramètres de la commande SQL
                _command.Parameters.AddWithValue("@pseudo", pseudo);
                //on execute la requete
                MySqlDataReader reader = _command.ExecuteReader();
                //on crée une liste d'étiquettes
                ObservableCollection<EtiquetteModel> etiquettes = [];
                //on lit les données
                while (reader.Read())
                {
                    EtiquetteModel etiquette = new(reader.GetInt64(reader.GetOrdinal("idUser")))
                    {
                        Id = reader.GetInt64(reader.GetOrdinal("id")),
                        Designation = reader.GetString(reader.GetOrdinal("designation")).ToString(),
                        Id_User = reader.GetInt64(reader.GetOrdinal("id_user"))
                    };
                    etiquettes.Add(etiquette);
                }
                return etiquettes;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return [];
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
                _command = new MySqlCommand("SELECT T.id, T.titre as T_designation, S.designation as S_designation, E.designation as E_designation," +
                    "count(A.id) as NbrParticipant, COUNT(C.designation) as TotalResult, SUM(C.checked) as TotalResultChecked " +
                    "FROM taches as T " +
                    "LEFT JOIN attributions as A ON A.id_tache = T.id " +
                    "INNER JOIN projets as P ON P.id = T.id_projet " +
                    "LEFT JOIN statuts as S ON S.id = T.id_statut " +
                    "LEFT JOIN etiquettes as E ON E.id = T.id_etiquette " +
                    "LEFT JOIN checklists as C ON C.id_tache = T.id " +
                    "WHERE P.id = @id_projet_sys " +
                    "GROUP BY T.id, T.designation, S.designation, E.designation " +
                    "ORDER BY T_designation, T.id;", _connection);
                //on ajoute les paramètres de la commande SQL
                _command.Parameters.AddWithValue("@id_projet_sys", id_Projet);
                //on execute la requete
                MySqlDataReader reader = _command.ExecuteReader();
                //on crée une liste de tâches
                ObservableCollection<TacheModel> taches = [];
                //on lit les données
                while (reader.Read())
                {
                    TacheModel tache = new()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Titre = reader.GetString(reader.GetOrdinal("T_designation")).ToString(),
                        Statut = reader.GetString(reader.GetOrdinal("S_designation")).ToString(),
                        Etiquette = reader.GetString(reader.GetOrdinal("E_designation")).ToString(),
                        NombreParticipants = reader.GetInt32(reader.GetOrdinal("NbrParticipant"))
                    };
                    if (reader["TotalResultChecked"] == DBNull.Value)
                    {
                        tache.Cheklist = "0/0";
                    }
                    else
                    {
                        tache.Cheklist = reader["TotalResultChecked"].ToString() + "/" + reader["TotalResult"].ToString();
                    }
                    taches.Add(tache);
                }
                return taches;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return [];
            }
            finally
            {
                _connection.Close();
            }
        }
        public UserModel? GetUser(bool bEmail, string element)
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
                    UserModel user = new()
                    {
                        Id = reader.GetInt64(reader.GetOrdinal("id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Surname = reader.GetString(reader.GetOrdinal("surname")),
                        Email = reader.GetString(reader.GetOrdinal("email")),
                        Pseudo = reader.GetString(reader.GetOrdinal("pseudo")),
                        Password = reader.GetString(reader.GetOrdinal("password")),
                    };
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

        public BigInteger GetId(string pseudo)
        {
            try
            {
                OpenConnection();
                //on crée la requete SQL
                _command.CommandText = "SELECT id FROM users WHERE pseudo = @pseudo";
                //on ajoute les parametres à la requete
                _command.Parameters.AddWithValue("pseudo", pseudo);
                //on execute la requete
                MySqlDataReader reader = _command.ExecuteReader();
                //on retourne si le mail existe ou non
                if (reader.Read())
                {
                    return reader.GetInt64(reader.GetOrdinal("id"));
                }
                return 0;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;
            }
            finally
            {
                _connection.Close();
            }
        }
        public BigInteger InsertEtiquette(string designation, BigInteger id_User)
        {
            //cette classe permet de mettre à jour un utilisateur dans la base de données
            try
            {
                OpenConnection();
                //on crée une commande SQL pour mettre à jour une ligne dans la table
                _command = new MySqlCommand("INSERT INTO etiquettes (designation, id_user, created_at) VALUES (@designation, @id_User, @data_Day);", _connection);
                //on ajoute les paramètres de la commande SQL
                _command.Parameters.AddWithValue("@designation", designation);
                _command.Parameters.AddWithValue("@id_user", id_User);
                _command.Parameters.AddWithValue("@data_Day", DateTime.Now);
                //on execute la requete
                _command.ExecuteNonQuery();
                //on retourne l'id de l'etiquette
                return (BigInteger)_command.LastInsertedId;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;
            }
            finally
            {
                //on ferme la connexion
                _connection.Close();
            }
        }
        public bool DeleteEtiquette(BigInteger id)
        {
            //cette classe permet de mettre à jour un utilisateur dans la base de données
            try
            {
                OpenConnection();
                //on crée une commande SQL pour mettre à jour une ligne dans la table
                _command = new MySqlCommand("DELETE FROM etiquettes WHERE id = @id", _connection);
                //on ajoute les paramètres de la commande SQL
                _command.Parameters.AddWithValue("@id", id);
                //on execute la requete
                _command.ExecuteNonQuery();
                //on ferme la connexion
                _connection.Close();
                return true;
            }
            catch
            {
                MessageBox.Show("Cette étiquette ne peut pas être effacer car elle est utiliser comme élément dans une ou plusieur tâche(s)");
                return false;
            }
        }

        public void UpdateEtiquette(BigInteger id, string designation)
        {
            //cette classe permet de mettre à jour un utilisateur dans la base de données
            try
            {
                OpenConnection();
                //on crée une commande SQL pour mettre à jour une ligne dans la table
                _command = new MySqlCommand("UPDATE etiquettes SET designation = @designation, updated_at = @dataDay WHERE id = @id", _connection);
                //on ajoute les paramètres de la commande SQL
                _command.Parameters.AddWithValue("@designation", designation);
                _command.Parameters.AddWithValue("@dataDay", DateTime.Now);
                _command.Parameters.AddWithValue("@id", id);
                //on execute la requete
                _command.ExecuteNonQuery();
                //on ferme la connexion
                _connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        //liste des participants
        public ObservableCollection<ParticipantModel> LoadParticipantsFromDatabase(BigInteger id_Tache)
        {
            // Retourner la liste des participants chargées selon le pseudo de l'utilisateur
            try
            {
                OpenConnection();
                //on crée une commande SQL pour mettre à jour une ligne dans la table
                _command = new MySqlCommand("SELECT A.id, pseudo, name, surname " +
                    "FROM attributions as A " +
                    "INNER JOIN users as U ON U.id = A.id_inviter " +
                    "WHERE id_tache = @id_tache_sys " +
                    "ORDER BY pseudo, name, surname; ", _connection);
                //on ajoute les paramètres de la commande SQL
                _command.Parameters.AddWithValue("@id_tache_sys", id_Tache);
                //on execute la requete
                MySqlDataReader reader = _command.ExecuteReader();
                //on crée une liste de tâches
                ObservableCollection<ParticipantModel> participants = [];
                //on lit les données
                while (reader.Read())
                {
                    ParticipantModel participant = new()
                    {
                        Id = reader.GetInt64(reader.GetOrdinal("id")),
                        Pseudo = reader.GetString(reader.GetOrdinal("pseudo")),
                        Nom = reader.GetString(reader.GetOrdinal("name")),
                        Prenom = reader.GetString(reader.GetOrdinal("surname"))
                    };
                    participants.Add(participant);
                }
                return participants;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return [];
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
