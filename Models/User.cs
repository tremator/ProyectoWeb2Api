namespace ProyectoWeb2.Models
{
    public class User
    {
        public long id {get; set;}
        public string email { get; set; }
        public string password { get; set; }    
        public string firstName { get; set; }
        public string LastName { get; set; }
        public long roleId { get; set; }
        public Role role { get; set; }
    }
}