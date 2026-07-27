namespace MyMvcApp.Models
{
    public static class UserService
    {
        
        public static List<User> Users = new List<User>();

        public static bool Register(User user)
        {
            
            if (Users.Any(u => u.Username == user.Username))
                return false;

            Users.Add(user);
            return true;
        }

        public static User Login(string username, string password)
        {
            return Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}