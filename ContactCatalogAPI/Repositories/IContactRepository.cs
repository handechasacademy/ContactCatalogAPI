using System.Collections.Generic;
using ContactCatalogAPI.Models;

namespace ContactCatalogAPI.Repositories
{
    public interface IContactRepository
    {
        Contact SaveContact(string name, string email, string tag);
        string RemoveContact(int id);
        List<Contact> SearchByName(string namePart);
        List<Contact> FilterByTag(string tag);
        List<Contact> ListContacts();
        string UpdateContact(int id, string newName, string newEmail, string tagToAdd, string tagToRemove);
    }
}