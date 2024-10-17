using DocumentFormat.OpenXml.Spreadsheet;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using GTERP.Interfaces.HR;
using GTERP.Interfaces.HRVariables;
using GTERP.Models.Common;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Syncfusion.DataSource.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace GTERP.Models
{
    public class BackgroundServiceQuotation : BackgroundService
    {
        private readonly ILogger<BackgroundServiceQuotation> _logger;
        private readonly GTRDBContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public BackgroundServiceQuotation(ILogger<BackgroundServiceQuotation> logger, GTRDBContext context,
            IWebHostEnvironment env, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _env = env;
            _configuration = configuration;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Service Started");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    string connectionString = _configuration.GetConnectionString("DefaultConnection");
                    string procedureName = "HR_getautoNotification"; // Replace with your actual stored procedure name

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(procedureName, connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@ComId", ""); 

                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            foreach(var item in reader)
                            {
                                var token = reader["Token"].ToString();
                                var title = reader["Title"].ToString();
                                var body = reader["Body"].ToString();
                                var empid = reader["EmpId"].ToString();
                                await SendNotificationAsync(token, title, body);

                                using (var context = new GTRDBContext())
                                {
                                    var entityToUpdate = context.HR_Notify.Find(empid);
                                    if (entityToUpdate != null)
                                    {
                                        entityToUpdate.IsMobileApp = 1;

                                        context.SaveChanges();
                                    }
                                }

                            }

                            connection.Close();

                        }
                    }

                    //SqlParameter[] parameter = new SqlParameter[1];
                    //parameter[0] = new SqlParameter("@ComId", comid);

                    //string query = $"Exec HR_getautoNotification '{comid}'";

                    //var data = Helper.ExecProcMapTList<NotificationPeram>("HR_getautoNotification", parameter);
                    //if(data!= null)
                    //{
                    //    foreach (var item in data)
                    //    {
                    //        await SendNotificationAsync(item.Token, item.Title, item.Body);
                    //    }
                    //}
                    
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while checking work orders for notifications");
                }
                Console.Write("Give");
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);


            }
        }

        public async Task SendNotificationAsync(string tkn, string title, string body)
        {
            // Initialize the Firebase Admin SDK
            var path = _env.ContentRootPath;
            path = path + "\\auth.json";
            FirebaseApp app = null;
            try
            {
                app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(path)
                }, "myApp");
            }
            catch (Exception ex)
            {
                app = FirebaseApp.GetInstance("myApp");
            }

            var fcm = FirebaseAdmin.Messaging.FirebaseMessaging.GetMessaging(app);

            var token = tkn;
            Message message = new Message()
            {
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = title,
                    Body = body
                },
                Data = new Dictionary<string, string>()
                 {
                     { "AdditionalData1", "This is Additional data" },
                     { "AdditionalData2", "data 2" },
                     //{ "image", "https://images.unsplash.com/photo-1555939594-58d7cb561ad1?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=387&q=80"},
                 },
                Token = token
            };
            var result = await fcm.SendAsync(message);
        }
    }
}
