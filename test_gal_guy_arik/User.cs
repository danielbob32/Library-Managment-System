namespace test_gal_guy_arik
{
    public class User
    {
        public string Name { get; set; }
        public string UserId { get; set; }

        public User(string name, string userId)
        {
            Name = name;
            UserId = userId;
        }
    }
}