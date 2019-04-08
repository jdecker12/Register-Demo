using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;
using Dapper;

namespace AuthDemo.Entities
{
    public class UserStore : IUserStore<User>,  IUserPasswordStore<User>
    {
        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync(
                    "insert into Users([Id],[Username], [NormalizedUsername], [PasswordHash], Values(@id, @userName, @normalizedUsername, @passwordHash)",
                    new
                    {
                        id = user.Id,
                        userName = user.Username,
                        normalizedUsername = user.NormalizedUsername,
                        passwordHash = user.PaswordHash
                    }
               );
            }
            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>(
                    "select * From Users where Id = @id",
                    new { id = userId }
                    );
            }
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>(
                    "select * From Users where NormalizedUsername = @normalizedUsername",
                    new { name = normalizedUserName }
                    );
            }
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            string x = await Task.FromResult(user.NormalizedUsername);
            return x;
        }

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            string x = await Task.FromResult(user.Id);
            return x;
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            string x = await Task.FromResult(user.Username);
            return x;
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUsername = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.Username = userName;
            return Task.CompletedTask;
        }

        public static DbConnection GetOpenConnection()
        {
            var connection = new SqlConnection("(localdb)\\ProjectsV13;Database=AuthDemo;trusted connection=yes");
            connection.Open();
            return connection;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync(
                    "update Users set [Id] = @id, [Username] = @username, [NormalizedUsername] = @normalizedUsername, [PasswordHash] = @passwordHash, where [Id] = @id",
                    new
                    {
                        id = user.Id,
                        userName = user.Username,
                        normalizedUsername = user.NormalizedUsername,
                        passwordHash = user.PaswordHash
                    }
               );
            }
            return IdentityResult.Success;
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PaswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PaswordHash);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PaswordHash != null);
        }
    }
}
