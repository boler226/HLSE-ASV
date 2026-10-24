namespace UsersService.Application.DTOs {
    public class UserDto {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; }
        public string Role { get; set; } = string.Empty;
        public Guid? CompanyId { get; set; }
    }
}
