using System.Data;
using Azure;
using Microsoft.Data.SqlClient;

namespace Skyway2.Common;

public class SqlCommandExecuter
    {
        private readonly string connectionString;
        //private readonly LoggerException _loggerException;

        public SqlCommandExecuter(string connectionString) // LoggerException loggerExtensions
        {
            this.connectionString = connectionString;
         //   _loggerException = loggerException;
        }

        public async Task<Response<sqlExecuterCommandResult<T>>> ExecuteStoredProcedure<T>(
            string spName, List<ExecuteSpInput> parameters = default, T result = default)
        {
            using var connection = new SqlConnection(connectionString);
            using SqlCommand command = connection.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = spName;

            command.Parameters.Add("@Result", SqlDbType.Int);
            command.Parameters["@Result"].Direction = ParameterDirection.Output;

            foreach (var parameter in parameters ?? new List<ExecuteSpInput>())
            {
                command.Parameters.Add(parameter.Name, parameter.DbType).Value = parameter.Value;
            }

            var response = new Response<sqlExecuterCommandResult<T>>();

            try
            {
                await connection.OpenAsync();
            }
            catch (Exception ex)
            {
 //               await _loggerException.Invoke(ex);
                response.devMessage = ex.Message;
                response.message = "عدم ارتباط با سرور";
                response.statusCode = -1;
                response.title = spName;
                response.payload = null;
                return response;
            }

            try
            {
                DataTable tbl = new DataTable();
                tbl.Load(await command.ExecuteReaderAsync());
                //return tbl;
                response.payload = new sqlExecuterCommandResult<T>(tbl, (T)command.Parameters["@Result"].Value);
                response.statusCode = 0;
                response.title = spName;
                response.devMessage = "Done successful";
                return response;
            }
            catch (Exception ex)
            {
               // await _loggerException.Invoke(ex);
                response.devMessage = ex.Message;
                response.message = "خطای دستوری";
                response.statusCode = -2;
                response.title = spName;
                response.payload = null;
                return response;
            }

            finally
            {
                await connection.CloseAsync();
            }

            return response;
        }

        public async Task<Response<sqlExecuterCommandResult<T>>> ExecuteStoredProcedureLong<T>(
            string spName, List<ExecuteSpInput> parameters = default, T result = default)
        {
            using var connection = new SqlConnection(connectionString);
            using SqlCommand command = connection.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = spName;

            command.Parameters.Add("@Result", SqlDbType.BigInt);
            command.Parameters["@Result"].Direction = ParameterDirection.Output;

            foreach (var parameter in parameters ?? [])
            {
                command.Parameters.Add(parameter.Name, parameter.DbType).Value = parameter.Value;
            }

            var response = new Response<sqlExecuterCommandResult<T>>();

            try
            {
                await connection.OpenAsync();
            }
            catch (Exception ex)
            {
               // await _loggerException.Invoke(ex);
                response.devMessage = ex.Message;
                response.message = "عدم ارتباط با سرور";
                response.statusCode = -1;
                response.title = spName;
                response.payload = null;
                return response;
            }

            try
            {
                DataTable tbl = new DataTable();
                tbl.Load(await command.ExecuteReaderAsync());
                //return tbl;
                response.payload = new sqlExecuterCommandResult<T>(tbl, (T)command.Parameters["@Result"].Value);
                response.statusCode = 0;
                response.title = spName;
                response.devMessage = "Done successful";
                return response;
            }
            catch (Exception ex)
            {
              //  await _loggerException.Invoke(ex);
                response.devMessage = ex.Message;
                response.message = "خطای دستوری";
                response.statusCode = -2;
                response.title = spName;
                response.payload = null;
                return response;
            }

            finally
            {
                await connection.CloseAsync();
            }

            return response;
        }

        public async Task<Response<DataTable>> ExecuteStoredProcedure(
            string spName, List<ExecuteSpInput> parameters = default)
        {
            Response<DataTable> response = new Response<DataTable>();
            using var connection = new SqlConnection(connectionString);

            using SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = spName;

            foreach (var parameter in parameters ?? new List<ExecuteSpInput>())
            {
                command.Parameters.Add(parameter.Name, parameter.DbType).Value = parameter.Value;
            }

            try
            {
                await connection.OpenAsync();
            }
            catch (Exception ex)
            {
            //    await _loggerException.Invoke(ex);
                response.devMessage = ex.Message;
                response.message = "عدم ارتباط با سرور";
                response.statusCode = -1;
                response.title = spName;
                response.payload = null;
                return response;
            }

            try
            {
                response.payload = new DataTable("tbl");
                response.payload.Load(await command.ExecuteReaderAsync());
                response.devMessage = "Done successful";
                response.message = "";
                response.statusCode = 0;
                response.title = spName;
                return response;
            }
            catch (Exception ex)
            {
              //  await _loggerException.Invoke(ex);
                response.devMessage = ex.Message;
                response.message = "خطای دستوری";
                response.statusCode = -2;
                response.title = spName;
                response.payload = null;
                return response;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }

    public class sqlExecuterCommandResult<T>
    {
        public sqlExecuterCommandResult(DataTable dataTable, T result)
        {
            this.dataTable = dataTable;
            this.result = result;
        }

        public DataTable dataTable { get; set; }
        public T result { get; set; }
    }
