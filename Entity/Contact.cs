using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact_CLI.Entity
{
    public class Contact
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }
        public DateTime CreationDate { get; private set; }
        public Contact(int id, string name, string phone, string email)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Email = email;
            CreationDate = DateTime.Now;
        }

        public void Update_name(string name)
        {
            Name = name;
        }
        public void Update_phone(string phone)
        {
            Phone = phone;
        }
        public void Update_email(string email)
        {
            Email = email;
        }
    }
}
