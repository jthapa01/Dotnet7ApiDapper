﻿using System.Security.Claims;
using AspNetCore.Identity.Dapper.Stores;
using Dapper;

namespace AspNetCore.Identity.Dapper.Providers
{
    internal class RoleClaimsProvider
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

        public RoleClaimsProvider(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _databaseConnectionFactory = databaseConnectionFactory;
        }

        public async Task<IList<Claim>> GetClaimsAsync(string roleId)
        {
            var command = "SELECT * " +
                                   $"FROM [{_databaseConnectionFactory.DbSchema}].[AspNetRoleClaims] " +
                                   "WHERE RoleId = @RoleId;";

            IEnumerable<RoleClaim> roleClaims = new List<RoleClaim>();

            await using var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync();
            return (
                    await sqlConnection.QueryAsync<RoleClaim>(command, new { RoleId = roleId })
                )
                .Select(x => new Claim(x.ClaimType, x.ClaimValue))
                .ToList();
        }
    }
}
