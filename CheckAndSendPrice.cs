using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace web_app.AppHost
{
    internal class CheckAndSendPrice
    {
    
        private static string smtpServer = "Task_1.gmail.com";
        private static int smtpPort = 587;
        private static string smtpLogin = "Task_1@gmail.com";
        private static string smtpPassword = "Task_1";
        private const string ConnString = "Data Source=app.db";
        static async Task Main()
        {
            using (var conn = new SqliteConnection($"Data Source={ConnString}"))
            {
                conn.Open();
                string query = "SELECT url, mail, price FROM Requests";
                using (var cmd = new SqliteCommand(query, conn))
                using (var read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        string url = read.GetString(0);
                        string email = read.GetString(1);
                        int price = read.GetInt32(2);

                        int newPrice = await GetPrice(url);
                        if (newPrice == 0)
                        {
                            UpdatePrice(conn, url, newPrice);
                        }

                        if (price != newPrice)
                        {
                            SendEmail(email, price, newPrice, url);
                            UpdatePrice(conn, url, newPrice);
                        }
                    }
                }
            }
        }
        private static async Task<int> GetPrice(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

                var data = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("ajax", "1")
                });

                HttpResponseMessage response = await client.PostAsync(url, data);

                string content = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(content);
                JsonElement json = doc.RootElement;

                return json.GetProperty("price").GetInt32();
            }
        }

        private static void SendEmail(string toEmail, int oldPrice, int newPrice, string url)
        {

            using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
            {
                client.Credentials = new System.Net.NetworkCredential(smtpLogin, smtpPassword);
                client.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(smtpLogin);
                mail.To.Add(toEmail);
                mail.Subject = "Цена изменилась";
                mail.Body = $"Старая цена{oldPrice}, новая цена {newPrice}";

                client.Send(mail);
            }
        }

        private static void UpdatePrice(SqliteConnection conn, string url,int newPrice)
        {
            string update = "UPDATE Requests SET price = @price WHERE url = @url";
            using (var cmd = new SqliteCommand(update, conn))
            {
                cmd.Parameters.AddWithValue("@price", newPrice);
                cmd.Parameters.AddWithValue("@url", url);
                cmd.ExecuteNonQuery();
            }
        }
    }

}

