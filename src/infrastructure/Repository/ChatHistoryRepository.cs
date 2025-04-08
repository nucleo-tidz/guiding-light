using Dapper;
using infrastructure.Helper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.ChatCompletion;
using model;
namespace infrastructure.Repository
{
    public class ChatHistoryRepository : IChatHistoryRepository
    {
        IConfiguration _configuration;
        public ChatHistoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
     
        }

        public async Task SaveChatMessageAsync(UserChatHistory userChatHistory)
        {
            const string sql = @"
            INSERT INTO ChatHistory (SessionId, UserId, Role, Message, Timestamp)
            VALUES (@SessionId, @UserId, @Role, @Message, @Timestamp)";

            using var connection = new SqlConnection(_configuration.GetConnectionString("nucleotidzdb"));
            await connection.ExecuteAsync(sql, new { SessionId = userChatHistory.SessionId, UserId = userChatHistory.UserId, Role = userChatHistory.Role.ToString(), Message = userChatHistory.Message, Timestamp = DateTime.UtcNow });
        }
        public async Task<IEnumerable<UserChatHistory>> GetChats(string userId, string sessionId)
        {
            const string sql = @"Select SessionId, UserId, Role, Message, Timestamp from ChatHistory  WHERE SessionId = @SessionId and UserId =@UserId ORDER BY Timestamp ASC";
            using var connection = new SqlConnection(_configuration.GetConnectionString("nucleotidzdb"));

            List<UserChatHistory> userHistory = (await connection.QueryAsync<UserChatHistory>(sql, new { SessionId = sessionId, UserId = userId })).ToList();
            return userHistory;
        }
    }

}
