﻿using System.Data;
using Microsoft.Data.SqlClient;

namespace AspNetCore.Identity.Dapper
{
    public class DefaultSqlConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly string _connectionString;

        public DefaultSqlConnectionFactory(string connectionString, string schema)
        {
            schema = schema.Replace("[", string.Empty).Replace("]", string.Empty);
            _connectionString = connectionString ?? string.Empty;
            DbSchema = schema;
        }

        public async Task<SqlConnection> CreateConnectionAsync() {
            var sqlConnection = new SqlConnection(_connectionString);
            if (sqlConnection.State != ConnectionState.Open)  await sqlConnection.OpenAsync();
            return sqlConnection;
        }

        public string DbSchema { get; set; }
    }
}
