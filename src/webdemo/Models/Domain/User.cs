namespace webdemo.Models.Domain
{
    public class User : Entity
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public int Age { get; set; }
    }
}
