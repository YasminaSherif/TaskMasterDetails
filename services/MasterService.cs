using System.Data;
using System.Data.SqlClient;
using TaskMasterDetails.DTO;
using TaskMasterDetails.services.contarcts;

namespace TaskMasterDetails.services
{
    public class MasterService : IMasterService
    {
        private readonly SqlConnection _connection;
        private readonly ILogger<MasterService> _logger;

        public MasterService(SqlConnection connection, ILogger<MasterService> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<int> CreateMasterDetail(CreateMasterDetailDTO dto)
        {
            try
            {
                await _connection.OpenAsync();

                var command = new SqlCommand("insertData", _connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };
                command.Parameters.AddWithValue("@name", dto.MasterName);
                command.Parameters.AddWithValue("@DetailDescription", dto.DetailDescription);

                var createdId = new SqlParameter("@MasterId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(createdId);
                await command.ExecuteNonQueryAsync();
                return (int)createdId.Value;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while creating master detail record");
                throw new Exception("Error while creating master detail record.", e);
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> DeleteMasterDetail(int id)
        {
            try
            {
                await _connection.OpenAsync();

                var command = new SqlCommand("deleteData", _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@id", id);

                var rowsAffected = await command.ExecuteNonQueryAsync();

                return rowsAffected > 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while creating master detail record");
                throw new Exception("Error while creating master detail record.", e);

            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> UpdateMasterDetail(UpdateMasterDetailDTO dto)
        {
            try
            {
                await _connection.OpenAsync();
                var command = new SqlCommand("UpdateNameAndDetails", _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@id", dto.Id);
                command.Parameters.AddWithValue("@name", dto.MasterName);
                command.Parameters.AddWithValue("@details", dto.DetailDescription);

                var rowsAffected = await command.ExecuteNonQueryAsync();


                return rowsAffected > 0;
            }
            catch (Exception e)
            {

                _logger.LogError(e, "Error while Updating master detail record");
                throw new Exception("Error while Updating master detail record.", e);
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<IEnumerable<RetrieveMasterDetailDTO>> GetAllMasterDetails()
        {
            try
            {
                await _connection.OpenAsync();

                var command = new SqlCommand("retriveAllData", _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                var reader = await command.ExecuteReaderAsync();

                var result = new List<RetrieveMasterDetailDTO>();

                while (await reader.ReadAsync())
                {
                    result.Add(new RetrieveMasterDetailDTO
                    {
                        Id = (int)reader["MasterId"],
                        MasterName = (string)reader["MasterName"],
                        CreatedDate = (DateTime)reader["CreatedDate"],
                        DetailDescription = (string)reader["DetailDescription"]
                    });
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while Retrieving master detail records");
                throw new Exception("Error while Retrieving master detail records.", e);

            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<RetrieveMasterDetailDTO> GetMasterDetailById(int id)
        {
            try
            {
                await _connection.OpenAsync();

                var command = new SqlCommand("retrieveData", _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@id", id);

                var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new RetrieveMasterDetailDTO
                    {
                        Id = (int)reader["MasterId"],
                        MasterName = (string)reader["MasterName"],
                        CreatedDate = (DateTime)reader["CreatedDate"],
                        DetailDescription = (string)reader["DetailDescription"]
                    };
                }

                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while retrieving master detail record");
                throw new Exception("Error while retrieving master detail record.", e);
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }


    }
}

