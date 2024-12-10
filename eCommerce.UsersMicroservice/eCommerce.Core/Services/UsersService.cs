using eCommerce.Core.Dto;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.ServiceContracts;

namespace eCommerce.Core.Services;

internal class UsersService(IUsersRepository usersRepository) : IUsersService
{
    public async Task<AuthenticationResponse?> Login(LoginRequest loginRequest)
    {
        var user = await usersRepository.GetUserByEmailAndPassword(loginRequest.Email, loginRequest.Password);

        return user == null
            ? null
            : new AuthenticationResponse(user.UserId, user.Email, user.PersonName, user.Gender, "token", true);
    }

    public async Task<AuthenticationResponse?> Register(RegisterRequest registerRequest)
    {
        var existingUser =
            await usersRepository.GetUserByEmailAndPassword(registerRequest.Email, registerRequest.Password);

        if (existingUser != null)
            return null;

        var user = new ApplicationUser
        {
            Email = registerRequest.Email,
            Password = registerRequest.Password,
            PersonName = registerRequest.PersonName,
            Gender = registerRequest.Gender.ToString(),
        };

        var newUser = await usersRepository.AddUser(user);

        if (newUser != null)
            return new AuthenticationResponse(newUser.UserId, newUser.Email, newUser.PersonName, newUser.Gender,
                "token",
                true);

        return null;
    }
}