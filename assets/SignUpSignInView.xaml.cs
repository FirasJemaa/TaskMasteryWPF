using System.Windows;
using BCrypt.Net;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Data;

namespace TaskMastery.Assets
{
    /// <summary>
    /// Logique d'interaction pour SignUpSignInView.xaml
    /// </summary>
    public partial class SignUpSignInView : Window
    {
        //string de connexion à la base de données
        const string _dsn = "Server=localhost;Database=taskmastery;username=root;password=;";
        //MySqlConnection permet de se connecter à une base de données MySQL
        private MySqlConnection _connection = new MySqlConnection(_dsn);
        //MySqlCommand permet d'exécuter des commandes SQL
        private MySqlCommand? _command;
        Components.InscriptionUserControl _inscriptionUserControl = new Components.InscriptionUserControl();
        Components.ConnexionUserControl _connexionUserControl = new Components.ConnexionUserControl();
        private string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d\s]).{8,}$";

        public SignUpSignInView()
        {
            InitializeComponent();
            Btn_Changed_Window.Content = "Inscription";
            Btn_Validate.Content = "Connexion";
            this.Title = "Connexion";
            
            GR_Champs.Children.Add(_connexionUserControl);
        }

        private void Btn_Changed_Window_Click(object sender, RoutedEventArgs e)
        {
            GR_Champs.Children.Clear();
            if (Btn_Changed_Window.Content.ToString() == "Connexion")
            {
                Btn_Changed_Window.Content = "Inscription";
                Btn_Validate.Content = "Connexion";
                this.Title = "Connexion";                
                GR_Champs.Children.Add(_connexionUserControl);
            }
            else
            {
                Btn_Changed_Window.Content = "Connexion";
                Btn_Validate.Content = "Inscription";
                this.Title = "Inscription";
                GR_Champs.Children.Add(_inscriptionUserControl);
            }   
        }

        private void Btn_Validate_Click(object sender, RoutedEventArgs e)
        {
            //condition pour voir si la fonction VerifierChamps est vrai
            if (!VerifierChamps())
            {
                return;
            }
            /*if (Btn_Validate.Content.ToString() == "Inscription")
            {
            }*/
            else
            {
                LogInUp();
            }
        }
        private bool VerifierChamps()
        {
            if (Btn_Validate.Content.ToString() == "Inscription")
            {
                if (_inscriptionUserControl.SAI_Name.Text == "" || _inscriptionUserControl.SAI_Surname.Text == "" || _inscriptionUserControl.SAI_Pseudo.Text == "" || _inscriptionUserControl.SAI_Password.Password == "" || _inscriptionUserControl.SAI_ConfirmPassword.Password == "")
                {
                    MessageBox.Show("Veuillez remplir tous les champs");
                    return false;
                }

                //controle que les mots de passe sont identiques
                if (_inscriptionUserControl.SAI_Password.Password != _inscriptionUserControl.SAI_ConfirmPassword.Password)
                {
                    MessageBox.Show("Les mots de passe ne sont pas identiques");
                    return false;
                }

                return RegexPassword(_inscriptionUserControl.SAI_Password.Password);
            }
            else{
                if (_connexionUserControl.SAI_Mail.Text == "" || _connexionUserControl.SAI_Password.Password == "")
                {
                    MessageBox.Show("Veuillez remplir tous les champs");
                    return false;
                }
                return RegexPassword(_connexionUserControl.SAI_Password.Password);
            }
        } 
        
        private bool RegexPassword(string password)
        {
            //controle que le mot de passe fait plus de 8 caractères, Une majuscule, un chiffre et un caractère spécial
            if (!Regex.IsMatch(password, passwordPattern))
            {
                MessageBox.Show("Le mot de passe doit contenir au moins 8 caractères, une lettre majuscule, une lettre miniscule et un caractère spécial");
                return false;
            }
            return true;
        }
        private void LogInUp()
        {
            if (Btn_Validate.Content.ToString() == "Inscription")
            {
                try
                {
                    //vérifier que le pseudo ou le mail n'existe pas déjà dans la base de données
                    _command = new MySqlCommand("SELECT * FROM users WHERE pseudo = @pseudo OR email = @mail", _connection);
                    _command.Parameters.AddWithValue("@pseudo", _inscriptionUserControl.SAI_Pseudo.Text);
                    _command.Parameters.AddWithValue("@mail", _inscriptionUserControl.SAI_Mail.Text);
                    _connection.Open();
                    MySqlDataReader reader = _command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        MessageBox.Show("Ce pseudo ou cet email existe déjà");
                        return;
                    }

                    _connection.Close();

                    //on crée une commande SQL pour ajouter une ligne dans la table
                    _command = new MySqlCommand("INSERT INTO users (name, surname, pseudo, email, password) VALUES (@nom, @prenom, @pseudo, @email, @password)", _connection);
                    //on ajoute les paramètres de la commande SQL
                    _command.Parameters.AddWithValue("@nom", _inscriptionUserControl.SAI_Name.Text);
                    _command.Parameters.AddWithValue("@prenom", _inscriptionUserControl.SAI_Surname.Text);
                    _command.Parameters.AddWithValue("@email", _inscriptionUserControl.SAI_Mail.Text);
                    _command.Parameters.AddWithValue("@pseudo", _inscriptionUserControl.SAI_Pseudo.Text);
                    _command.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(_inscriptionUserControl.SAI_Password.Password));
                    //on ouvre la connexion
                    _connection.Open();
                    //on exécute la commande SQL, nonQuery car on ne récupère pas de données
                    _command.ExecuteNonQuery();
                    //on actualise le DataGrid
                    
                    MessageBox.Show("Utilisateur ajouté avec succès", "Nouvel utilisateur", MessageBoxButton.OK, MessageBoxImage.Information);
                    //fermer la fenêtre et ouvrir la fenêtre principale en lui donnant le pseudo de l'utilisateur   
                    MainWindow mainWindow = new MainWindow(_inscriptionUserControl.SAI_Pseudo.Text);
                    mainWindow.Show();
                    this.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    //Dispose permet de libérer les ressources utilisées par l'objet
                    _command.Dispose();
                    //on ferme la connexion
                    _connection.Close();
                }
            }
            else
            {
                try
                {
                    //on crée une commande SQL pour ajouter une ligne dans la table
                    _command = new MySqlCommand("SELECT * FROM users WHERE email = @mail", _connection);
                    //on ajoute les paramètres de la commande SQL
                    _command.Parameters.AddWithValue("@mail", _connexionUserControl.SAI_Mail.Text);
                    //on ouvre la connexion
                    _connection.Open();
                    //on exécute la commande SQL, nonQuery car on ne récupère pas de données
                    MySqlDataReader reader = _command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (BCrypt.Net.BCrypt.Verify(_connexionUserControl.SAI_Password.Password, reader["password"].ToString()))
                        {
                            //MessageBox.Show("Connexion réussie", "Connexion", MessageBoxButton.OK, MessageBoxImage.Information);
                            //fermer la fenêtre et ouvrir la fenêtre principale en lui donnant le pseudo de l'utilisateur
                            MainWindow mainWindow = new MainWindow(_connexionUserControl.SAI_Mail.Text);
                            mainWindow.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Mot de passe incorrect", "Connexion", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Email incorrect", "Connexion", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    //Dispose permet de libérer les ressources utilisées par l'objet
                    _command.Dispose();
                    //on ferme la connexion
                    _connection.Close();
                }   
            }
        }        
    }
}
