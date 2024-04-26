namespace TaskMastery.Assets.Components
{
    public class PasswordParameters
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Password { get; set; }
        public PasswordParameters (string newPassword, string confirmPassword, string password)
        {
            NewPassword = newPassword;
            ConfirmPassword = confirmPassword;
            Password = password;
        }
        public PasswordParameters(){
            NewPassword = "";
            ConfirmPassword = "";
            Password = "";
        }
    }
}