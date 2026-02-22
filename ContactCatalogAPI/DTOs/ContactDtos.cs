namespace ContactCatalogAPI.DTOs
{
    public class CreateContactDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Tag { get; set; }
    }

    public class UpdateContactDto
    {
        public string? NewName { get; set; }
        public string? NewEmail { get; set; }
        public string? TagToAdd { get; set; }
        public string? TagToRemove { get; set; }
    }
}