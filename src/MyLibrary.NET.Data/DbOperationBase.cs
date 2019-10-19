namespace MyLibrary.NET.Data {
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public abstract class DbOperationBase {
        private readonly string _connectionString;

        protected DbOperationBase(string serverNameOrIp, string databaseName) {
            _connectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True", serverNameOrIp, databaseName);
        }

        protected virtual SqlConnection OpenConnection() {
            var sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();
            return sqlConnection;
        }

        public virtual bool ExecuteInsert<T>(string dbTableName, object[] columnsValue) {
            const string insertCommand = "insert into {0} values({1})";

            var formattedValues = new StringBuilder();

            var i = 0;
            foreach (var columnValue in columnsValue) {
                var valueType = columnValue.GetType();

                if (valueType != typeof (string) && valueType != typeof (DateTime))
                    formattedValues.AppendFormat("{0}", columnValue);
                else
                    formattedValues.AppendFormat("N'{0}'", columnValue);

                if (++i < columnsValue.Length)
                    formattedValues.Append(", ");
            }

            var formattedInsertCommand = string.Format(insertCommand, dbTableName, formattedValues);

            return InternalExecuteNonQuery(formattedInsertCommand);
        }

        public virtual bool ExecuteUpdate<T>(string dbTableName, string dbKeyColumnName, T dbKeyColumnValue, IDictionary<string, object> columnsNameValue) {
            const string updateCommand = "update {0} set {1} where {2} = {3}";

            var formattedNameValues = new StringBuilder();

            var i = 0;
            foreach (var columnNameValue in columnsNameValue) {
                var valueType = columnNameValue.Value.GetType();

                if (valueType != typeof (string) && valueType != typeof (DateTime))
                    formattedNameValues.AppendFormat("{0} = {1}", columnNameValue.Key, columnNameValue.Value);
                else
                    formattedNameValues.AppendFormat("{0} = N'{1}'", columnNameValue.Key, columnNameValue.Value);

                if (++i < columnsNameValue.Count)
                    formattedNameValues.Append(", ");
            }

            var formattedUpdateCommad = string.Format(updateCommand, dbTableName, formattedNameValues, dbKeyColumnName, dbKeyColumnValue);

            return InternalExecuteNonQuery(formattedUpdateCommad);
        }

        public virtual bool ExecuteDelete<T>(string dbTableName, string dbColumnName, T dbColumnValue) {
            const string deleteCommand = "delete from {0} where {1} = {2}";

            string formattedDbColumnValue;

            if (typeof (T) == typeof (string))
                formattedDbColumnValue = string.Format("'{0}'", dbColumnValue);
            else
                formattedDbColumnValue = dbColumnValue.ToString();

            var formattedDeleteCommand = string.Format(deleteCommand, dbTableName, dbColumnName, formattedDbColumnValue);

            return InternalExecuteNonQuery(formattedDeleteCommand);
        }

        protected virtual bool InternalExecuteNonQuery(string commandText) {
            bool executeResult;

            try {
                using (var sqlConnection = OpenConnection()) {
                    using (var sqlCommand = sqlConnection.CreateCommand()) {
                        sqlCommand.CommandText = commandText;
                        sqlCommand.CommandType = CommandType.Text;

                        sqlCommand.ExecuteNonQuery();
                    }
                    sqlConnection.Close();
                }

                executeResult = true;
            } catch (Exception) {
                executeResult = false;
            }

            return executeResult;
        }

        protected virtual SqlDataReader InternalGetSqlDataReader(string commandText) {
            SqlDataReader executeResult;

            try {
                var sqlConnection = OpenConnection();
                var sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = commandText;

                executeResult = sqlCommand.ExecuteReader();
            } catch (Exception) {
                executeResult = null;
            }

            return executeResult;
        }
    }
}