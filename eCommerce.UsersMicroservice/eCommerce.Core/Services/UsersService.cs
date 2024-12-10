using AutoMapper;
using eCommerce.Core.Dto;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.ServiceContracts;

namespace eCommerce.Core.Services;

internal class UsersService(IUsersRepository usersRepository, IMapper mapper) : IUsersService
{
    public async Task<AuthenticationResponse?> Login(LoginRequest loginRequest)
    {
        var user = await usersRepository.GetUserByEmailAndPassword(loginRequest.Email, loginRequest.Password);

        return user == null
            ? null
            : mapper.Map<AuthenticationResponse>(user) with { Success = true, Token = "token" };
    }

    public async Task<AuthenticationResponse?> Register(RegisterRequest registerRequest)
    {
        var user = mapper.Map<ApplicationUser>(registerRequest);

        var newUser = await usersRepository.AddUser(user);

        return newUser != null
            ? mapper.Map<AuthenticationResponse>(newUser) with { Success = true, Token = "token" }
            : null;
    }
}