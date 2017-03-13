using System;
using System.Data.Linq.Mapping;

namespace EntityLayer
{
    [Table(Name = "Users")]
    public class UserEntity
    {
        public UserEntity() { }

        public UserEntity(string _user, string _pw)
        { Username = _user; Password = _pw; }

        [Column(IsPrimaryKey = true)]
        public string Username { get; set; }

        [Column]
        public string Password { get; set; }
    }
}
