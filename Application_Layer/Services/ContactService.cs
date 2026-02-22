using Contact_CLI.Application_Layer.Interfaces;
using Contact_CLI.Entity;

namespace Contact_CLI.Application_Layer.Services
{
    public class ContactService
    {
        private readonly IContact_Repository _repository;
        public ContactService(IContact_Repository repository)
        {
            _repository = repository;
            _repository.LoadContacts();
        }
        public List<Contact> GetAll() => _repository.GetAllContacts();
        public Contact GetById(int id) => _repository.GetContactById(id);
        public void AddContact(int id, string name, string phone, string email)
        {
            var contacts = _repository.GetAllContacts();
            int newId = contacts.Any() ? contacts.Max(c => c.Id) + 1 : 1;
            var newContact = new Contact(newId, name, phone, email);
            _repository.AddContact(newContact);
        }
        public void UpdateContact(int id, string name, string phone, string email)
        {
            var existing = _repository.GetContactById(id);

            if (existing == null)
                throw new Exception("Contact not found");
            existing.Update_name(name);
            existing.Update_phone(phone);
            existing.Update_email(email);

            _repository.UpdateContact(existing);
        }
        public void Delete(int id) => _repository.DeleteContact(id);

        public void Save() => _repository.SaveChanges();
        public void Load() => _repository.LoadContacts();
    }
}
