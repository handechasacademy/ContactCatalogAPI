using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactCatalogAPI.Validators
{
    public class DuplicateEmailException : Exception
    {
        public DuplicateEmailException(string email) : base($"A contact with the email '{email}' already exists.") { }
    }

    public class ContactNotFoundException : Exception
    {
        public ContactNotFoundException(int id) : base($"No contact found with ID: {id}.") { }
    }

    public class InvalidInputException : Exception
    {
        public InvalidInputException(string message) : base(message) { }
    }
}
