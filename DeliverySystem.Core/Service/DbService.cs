using DeliverySystem.Core.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DeliverySystem.Core.Services
{
    public class DbService
    {
        private readonly string _connString;

        public DbService(IConfiguration config)
        {
            _connString = config.GetConnectionString("DeliveryDb");
        }

        // 1. Логін
        public User Login(string username, string password)
        {
            using (var con = new SqlConnection(_connString))
            {
                con.Open();
                string query = "SELECT * FROM AppUsers WHERE Username=@u AND Password=@p";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", password);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = (int)reader["UserID"],
                                Username = reader["Username"].ToString(),
                                Role = reader["Role"].ToString(),
                                RelatedEntityId = reader["RelatedEntityID"] == DBNull.Value ? (int?)null : (int)reader["RelatedEntityID"]
                            };
                        }
                    }
                }
            }
            return null;
        }

        // 2. Отримання замовлень
        public DataTable GetOrders(User user)
        {
            using (var con = new SqlConnection(_connString))
            {
                con.Open();
                string query = "SELECT * FROM vw_FullOrderDetails";

                if (user.Role == "Courier" && user.RelatedEntityId.HasValue)
                {
                    string nameQuery = $"SELECT fullname FROM Couriers WHERE courier_id = {user.RelatedEntityId}";
                    using (var nameCmd = new SqlCommand(nameQuery, con))
                    {
                        var courierName = nameCmd.ExecuteScalar()?.ToString();
                        if (!string.IsNullOrEmpty(courierName))
                        {
                            query += $" WHERE CourierName = '{courierName}'";
                        }
                    }
                }

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void AddOrder(int senderId, int receiverId, int? courierId, decimal weight, decimal price, string status)
        {
            using (var con = new SqlConnection(_connString))
            {
                con.Open();
                string query = "INSERT INTO Orders (sender_id, receiver_id, courier_id, weight, delivery_price, status, created_at) " +
                               "VALUES (@s, @r, @c, @w, @p, @st, GETDATE())";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@s", senderId);
                    cmd.Parameters.AddWithValue("@r", receiverId);
                    cmd.Parameters.AddWithValue("@c", courierId.HasValue ? (object)courierId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@w", weight);
                    cmd.Parameters.AddWithValue("@p", price);
                    cmd.Parameters.AddWithValue("@st", status);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 4. Видалення (CRUD)
        public void DeleteOrder(int orderId)
        {
            using (var con = new SqlConnection(_connString))
            {
                con.Open();
                var query = "DELETE FROM Orders WHERE order_id = @id";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", orderId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 5. Оновлення статусу
        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            using (var con = new SqlConnection(_connString))
            {
                con.Open();
                string query = "UPDATE Orders SET status = @s WHERE order_id = @id";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@s", newStatus);
                    cmd.Parameters.AddWithValue("@id", orderId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 6. Звіт по датах
        public decimal GetRevenueByPeriod(DateTime start, DateTime end)
        {
            using (var con = new SqlConnection(_connString))
            {
                con.Open();
                // В SQL BETWEEN включає границі, тому додаємо час до кінця дня
                DateTime endOfDay = end.Date.AddDays(1).AddTicks(-1);

                string query = "SELECT SUM(delivery_price) FROM Orders WHERE created_at BETWEEN @start AND @end AND status != 'Скасовано'";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@start", start);
                    cmd.Parameters.AddWithValue("@end", endOfDay);
                    var result = cmd.ExecuteScalar();
                    return result == DBNull.Value ? 0 : (decimal)result;
                }
            }
        }

        // 7. Перерахунок ціни (процедура)
        public void RecalculatePrice(int orderId)
        {
            using (var con = new SqlConnection(_connString))
            {
                con.Open();
                using (var cmd = new SqlCommand("sp_CalculateOrderPrice", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool CheckUser(string username, string password)
        {
            using (var con = new SqlConnection(_connString))
            {
                con.Open();
                string query = "SELECT COUNT(1) FROM AppUsers WHERE Username=@u AND Password=@p";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", password);
                    var result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result) > 0;
                }
            }
        }

    }
}