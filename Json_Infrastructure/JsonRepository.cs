using System.Text.Json;
using Contact_CLI.Application_Layer.Interfaces;
using Contact_CLI.Entity;

namespace Contact_CLI.Json_Infrastructure
{
    public class JsonRepository : IContact_Repository
    {
        private readonly string _filePath = "contacts.json";
        private List<Contact> _contacts = new();
        public void LoadContacts()
        {
            if (!File.Exists(_filePath))
            {
                _contacts = new List<Contact>();
                return;
            }

            var json = File.ReadAllText(_filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                _contacts = new List<Contact>();
                return;
            }

            var data = JsonSerializer.Deserialize<List<Contact>>(json);

            _contacts = data ?? new List<Contact>();
        }
        public void SaveChanges()
        {
            var json = JsonSerializer.Serialize(_contacts,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(_filePath, json);
        }
        public void AddContact(Contact contact)
        {
            _contacts.Add(contact);
        }

        public void DeleteContact(int id)
        {
            var contact = GetContactById(id);
            if (contact != null)
                _contacts.Remove(contact);
        }
        public Contact GetContactById(int id)
        {
            return _contacts.FirstOrDefault(c => c.Id == id);
        }

        public void UpdateContact(Contact contact)
        {
            var existing = GetContactById(contact.Id);
            if (existing == null) return;

            existing.GetType().GetProperty("Name")?.SetValue(existing, contact.Name);
            existing.GetType().GetProperty("Phone")?.SetValue(existing, contact.Phone);
            existing.GetType().GetProperty("Email")?.SetValue(existing, contact.Email);
        }

        List<Contact> IContact_Repository.GetAllContacts()
        {
            return _contacts;
        }
    }
}