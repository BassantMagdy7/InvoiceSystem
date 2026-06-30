namespace WebAPI.Services.Auth
{
    public class AuthResult
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public string? Email { get; set; }
        public object? Errors { get; set; }
    }
}
