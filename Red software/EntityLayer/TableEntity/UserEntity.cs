using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer
{
    [Table("Users")]
    public class UserEntity
    {
        public UserEntity() { }

        public UserEntity(string user, string pw) { Username = user; Password = pw; }

        [Key]
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
