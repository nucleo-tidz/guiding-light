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
        private readonly string _connectionString;

        public ChatHistoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("nucleotidz");
        }

        public async Task SaveChatMessageAsync(UserChatHistory userChatHistory)
        {
            const string sql = @"
            INSERT INTO ChatHistory (SessionId, UserId, Role, Message, Timestamp)
            VALUES (@SessionId, @UserId, @Role, @Message, @Timestamp)";

            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(sql, new { SessionId = userChatHistory.SessionId, UserId = userChatHistory.UserId, Role = userChatHistory.Role.ToString(), Message = userChatHistory.Message, Timestamp = DateTime.UtcNow });
        }
        public async Task<ChatHistory> GetChats(string userId, string sessionId)
        {
            const string sql = @"Select SessionId, UserId, Role, Message, Timestamp from ChatHistory  WHERE SessionId = @SessionId and UserId =@UserId ORDER BY Timestamp ASC";
            using var connection = new SqlConnection(_connectionString);

            List<UserChatHistory> userHistory = (await connection.QueryAsync<UserChatHistory>(sql, new { SessionId = sessionId, UserId = userId })).ToList();
            return userHistory.ToChatHistory();
        }
    }

}
