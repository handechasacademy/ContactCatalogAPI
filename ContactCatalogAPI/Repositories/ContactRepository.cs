using System;
using System.Collections.Generic;
using System.Linq;
using ContactCatalogAPI.Models;
using ContactCatalogAPI.Validators;
using Microsoft.Extensions.Logging;

namespace ContactCatalogAPI.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private Dictionary<int, Contact> _contacts = new Dictionary<int, Contact>();
        private HashSet<string> _emails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private ILogger<ContactRepository> _logger;


        public ContactRepository(Dictionary<int, Contact> contacts,
                         HashSet<string> emails,
                         ILogger<ContactRepository> logger)
        {
            _contacts = contacts;
            _emails = emails;
            _logger = logger;
        }

        public Contact SaveContact(int id, string name, string email, string tag)
        {
            if (_contacts.ContainsKey(id))
                throw new DuplicateIdException(id);

            if (_emails.Contains(email))
                throw new DuplicateEmailException(email);

            var tags = new List<string> { tag };
            var contact = new Contact(id, name, email, tags);

            _contacts.Add(contact.Id, contact);
            _emails.Add(contact.Email);

            return contact;
        }

        public string UpdateContact(int id, string newName, string newEmail, string tagToAdd, string tagToRemove)
        {
            if (_contacts.ContainsKey(id))
            {
                var contact = _contacts[id];

                if (!string.IsNullOrWhiteSpace(newName))
                {
                    contact.Name = newName;
                }

                if (!string.IsNullOrWhiteSpace(newEmail))
                {
                    if (_emails.Contains(newEmail))
                        throw new DuplicateEmailException(newEmail);
                    else if (!EmailValidator.IsValidEmail(newEmail))
                        throw new InvalidInputException("Invalid email format.");
                    else
                    {
                        _emails.Remove(contact.Email);
                        contact.Email = newEmail;
                        _emails.Add(newEmail);
                    }
                }

                if (!string.IsNullOrWhiteSpace(tagToAdd))
                {
                    if (contact.Tags.Contains(tagToAdd, StringComparer.OrdinalIgnoreCase))
                        throw new InvalidInputException($"Tag '{tagToAdd}' already exists.");
                    else
                        contact.Tags.Add(tagToAdd);
                }

                if (!string.IsNullOrWhiteSpace(tagToRemove))
                {
                    if (contact.Tags.Contains(tagToRemove, StringComparer.OrdinalIgnoreCase))
                        contact.Tags.Remove(tagToRemove);
                    else
                        throw new InvalidInputException($"Tag '{tagToRemove}' not found.");
                }

                return $"Contact with ID {id} has been updated.";
            }
            else
            {
                throw new ContactNotFoundException(id);
            }
        }

        public List<Contact> SearchByName(string namePart)
        {
            return _contacts.Values
                .Where(s => s.Name.Contains(namePart, StringComparison.OrdinalIgnoreCase))
                .OrderBy(s => s.Name)
                .ToList();
        }

        public List<Contact> FilterByTag(string tag)
        {
            return _contacts.Values
                .Where(f => f.Tags.Contains(tag, StringComparer.OrdinalIgnoreCase))
                .OrderBy(f => f.Name)
                .ToList();
        }

        public string RemoveContact(int id)
        {
            if (_contacts.ContainsKey(id))
            {
                _emails.Remove(_contacts[id].Email);
                _contacts.Remove(id);
                return $"Contact with ID {id} has been removed.";
            }
            else
            {
                throw new ContactNotFoundException(id);
            }
        }

        public List<Contact> ListContacts()
        {
            return _contacts.Values.OrderBy(l => l.Id).ToList();
        }
    }
}
