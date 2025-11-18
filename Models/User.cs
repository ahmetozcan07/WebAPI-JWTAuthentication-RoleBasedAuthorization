namespace JWTandRoleBasedApp.Models
{
    public class User
    {
        // Id is not used
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
    }
}
