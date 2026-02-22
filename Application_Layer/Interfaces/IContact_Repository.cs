using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contact_CLI.Entity;

namespace Contact_CLI.Application_Layer.Interfaces
{
    public interface IContact_Repository
    {
        void AddContact(Contact contact);
        void UpdateContact(Contact contact);
        void DeleteContact(int id);
        Contact GetContactById(int id);
        List<Contact> GetAllContacts();
        void SaveChanges();
        void LoadContacts();
    }
}
