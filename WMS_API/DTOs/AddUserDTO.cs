namespace WEB_API.DTOs
{
    public class AddUserDTO
    {
        public int RoleId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName {  get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Address {  get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
