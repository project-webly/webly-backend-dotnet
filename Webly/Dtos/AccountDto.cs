namespace Webly.Dtos;

public class AccountDto
{
    public string UserName { get; init; }
}

public class RegisterDto
{
    public string UserName { get; init; }

    public string Password { get; init; }
}

public class LoginDto
{
    public string UserName { get; init; }
    public string Password { get; init; }
}