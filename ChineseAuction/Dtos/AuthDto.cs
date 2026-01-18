namespace ChineseAuction.Dtos
{
    public class loginRequestDto
    {
        public string Email { get; set; }=string.Empty;
        public string Password { get; set; }=string.Empty;
    }
    public class loginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string TokenType { get; set; } = "Bearer";
        public int ExpiresInMinutes { get; set; }
        public GetUserDto User { get; set; }=new GetUserDto();
    }
}
