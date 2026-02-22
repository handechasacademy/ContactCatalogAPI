using ContactCatalogAPI.Models;
using ContactCatalogAPI.Repositories;

namespace ContactCatalogAPI.Services
{
    public class ContactService
    {
        private readonly IContactRepository _repository;

        public ContactService(IContactRepository repository)
        {
            _repository = repository;
        }

        public Contact SaveContact(int id, string name, string email, string tag)
        {
            return _repository.SaveContact(id, name, email, tag);
        }

        public string RemoveContact(int id)
        {
            return _repository.RemoveContact(id);
        }

        public string UpdateContact(int id, string newName, string newEmail, string tagToAdd, string tagToRemove)
        {
            return _repository.UpdateContact(id, newName, newEmail, tagToAdd, tagToRemove);
        }

        public List<Contact> ListContacts()
        {
            return _repository.ListContacts();
        }

        public List<Contact> SearchByName(string name)
        {
            return _repository.SearchByName(name);
        }

        public List<Contact> FilterByTag(string tag)
        {
            return _repository.FilterByTag(tag);
        }
    }
}