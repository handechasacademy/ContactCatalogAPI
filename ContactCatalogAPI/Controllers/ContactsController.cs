using Microsoft.AspNetCore.Mvc;
using ContactCatalogAPI.Services;
using ContactCatalogAPI.DTOs;
using ContactCatalogAPI.Validators;

namespace ContactCatalogAPI.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class ContactsController : ControllerBase
    {
        private readonly ContactService _service;

        public ContactsController(ContactService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var contacts = _service.ListContacts();
            return Ok(contacts);
        }

        [HttpPost]
        public IActionResult Add([FromBody] CreateContactDto dto)
        {
            try
            {
                var contact = _service.SaveContact(dto.Name, dto.Email, dto.Tag);
                return CreatedAtAction(nameof(GetAll), new { id = contact.Id }, contact);
            }
            catch (DuplicateEmailException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidInputException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            try
            {
                var result = _service.RemoveContact(id);
                return Ok(result);
            }
            catch (ContactNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateContactDto dto)
        {
            try
            {
                var result = _service.UpdateContact(id, dto.NewName, dto.NewEmail, dto.TagToAdd, dto.TagToRemove);
                return Ok(result);
            }
            catch (ContactNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DuplicateEmailException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidInputException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string name)
        {
            var results = _service.SearchByName(name);
            return Ok(results);
        }

        [HttpGet("filter")]
        public IActionResult Filter([FromQuery] string tag)
        {
            var results = _service.FilterByTag(tag);
            return Ok(results);
        }
    }
}