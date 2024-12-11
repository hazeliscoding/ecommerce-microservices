using Dapper;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;

namespace eCommerce.Infrastructure.Repositories;

public class UsersRepository(DapperDbContext dbContext) : IUsersRepository
{
    public async Task<ApplicationUser?> AddUser(ApplicationUser user)
    {
        // Generate a new unique user id
        user.UserId = Guid.NewGuid();

        const string query =
            "INSERT INTO users (user_id, email, person_name, gender, password) VALUES (@UserId, @Email, @PersonName, @Gender, @Password)";

        var rowCountAffected = await dbContext.DbConnection.ExecuteAsync(query, user);
        return rowCountAffected == 0 ? null : user;
    }

    public async Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password)
    {
        const string query = @"
            SELECT 
                user_id AS UserId, 
                email, 
                person_name AS PersonName, 
                gender, 
                password 
            FROM users 
            WHERE email = @Email AND password = @Password;";

        var user = await dbContext.DbConnection.QueryFirstOrDefaultAsync<ApplicationUser>(query, new { Email = email, Password = password });
        return user;
    }
}