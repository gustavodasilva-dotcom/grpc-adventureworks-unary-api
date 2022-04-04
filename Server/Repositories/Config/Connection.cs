using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Repositories.Config
{
    public class Connection
    {
        #region Properties

        private readonly int _Timeout;

        private readonly SqlConnection _sqlConnection;

        #endregion

        #region Constructor

        public Connection()
        {
            _Timeout = 300;

            _sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
        }

        #endregion

        #region Methods

        private async Task<SqlConnection> ConectarAsync()
        {
            if (_sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                if (string.IsNullOrEmpty(_sqlConnection.ConnectionString))
                    _sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

                await _sqlConnection.OpenAsync();
            }

            return _sqlConnection;
        }

        protected async Task<IEnumerable<T>> ExecutarSelectListaAsync<T>(string query)
        {
            var connection = await ConectarAsync();

            return await connection.QueryAsync<T>(query, commandTimeout: _Timeout);
        }

        protected async Task<T> ExecutarSelectAsync<T>(string query)
        {
            var connection = await ConectarAsync();

            return await connection.QueryFirstOrDefaultAsync<T>(query, commandTimeout: _Timeout);
        }

        protected async Task<bool> ExecutarComandoAsync(string query)
        {
            var connection = await ConectarAsync();

            return await connection.ExecuteAsync(query, commandTimeout: _Timeout) > 0;
        }

        protected async Task<T> ExecutarProcedureAsync<T>(string query)
        {
            var connection = await ConectarAsync();

            return await connection.QueryFirstOrDefaultAsync<T>(query, commandTimeout: _Timeout);
        }

        #endregion
    }
}
