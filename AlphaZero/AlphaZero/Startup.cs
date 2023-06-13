using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using Hangfire;
using Hangfire.SqlServer;

[assembly: OwinStartupAttribute(typeof(AlphaZero.Startup))]
namespace AlphaZero
{
    public partial class Startup
    {

        private string connectionString = "Server=.\\SQLEXPRESS; Database=db_roomrental; Integrated Security=True;"; // Define connection string

        private IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString);

            yield return new BackgroundJobServer();
        }

        public void Configuration(IAppBuilder app)
        {
            app.UseHangfireAspNet(GetHangfireServers);
            app.UseHangfireDashboard();

            // Let's also create a sample background job
            //BackgroundJob.Enqueue(() => Debug.WriteLine("Hello world from Hangfire!"));
            RecurringJob.AddOrUpdate("Run every day", () => CreateReminder(), Cron.Minutely);
            //"* * * * *" for minutely [strictly testing]
            //Cron.Daily for daily
            // ...other configuration logic

            ConfigureAuth(app);

        }

        public void CreateReminder()
        {
            // Your logic for creating reminders goes here
            // You can perform the necessary database operations or any other tasks

            // For example, let's print a simple message to the console

            // Generate a dummy record
            //string userPassword = "dummyPassword";
            //string userName = "Dummy User";
            //string userEmail = "dummy@example.com";
            //string userType = "Regular";

            // Construct the SQL query
            //string query = $"INSERT INTO [user] (user_password, user_name, user_email, user_type) " +
            //$"VALUES ('{userPassword}', '{userName}', '{userEmail}', '{userType}')";


            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check for landlords who need to be paid today
                string landlordQuery = "INSERT INTO reminds (landlord_id, reminder_date, reminder_title, reminder_desc, reminder_status) " +
                       "SELECT l.landlord_id, l.landlord_due, " +
                       "'Pay Landlord', " +
                       "'Pay ' + CONVERT(varchar, l.landlord_fee) + ' to ' + l.landlord_name + ' today! For Floors ' + " +
                       "(SELECT STUFF((SELECT ', ' + CAST(f.floor_id AS varchar) " +
                       "FROM floor f " +
                       "WHERE f.landlord_id = l.landlord_id " +
                       "FOR XML PATH('')), 1, 2, '') AS floor_ids), " +
                       "0 AS reminder_status " +
                       "FROM landlord l " +
                       "WHERE l.landlord_due = CONVERT(date, GETDATE());";

                SqlCommand landlordCommand = new SqlCommand(landlordQuery, connection);
                landlordCommand.ExecuteNonQuery();



                // Check for tenants who need to be paid today
                // Tenant query
                string tenantQuery = "INSERT INTO reminds (tenant_id, reminder_date, reminder_title, reminder_desc, reminder_status) " +
                     "SELECT t.tenant_id, t.tenant_outDate, " +
                     "'Collect Rent from Room ' + CAST(r.room_number AS varchar) + ' at ' + CAST(f.floor_id AS varchar) + ' today!' AS reminder_title, " +
                     "'Collect Rent from ' + t.tenant_name + ' by contacting ' + t.tenant_phoneNo + ' today!' AS reminder_desc, " +
                     "0 AS reminder_status " +
                     "FROM tenant t " +
                     "JOIN room r ON t.room_id = r.room_id " +
                     "JOIN floor f ON r.floor_id = f.floor_id " +
                     "WHERE t.tenant_outDate = CONVERT(date, GETDATE());";


                SqlCommand tenantCommand = new SqlCommand(tenantQuery, connection);
                tenantCommand.ExecuteNonQuery();

                string deleteDuplicateQuery = @"WITH cte AS (
                                                    SELECT *,
                                                        ROW_NUMBER() OVER (
                                                            PARTITION BY ISNULL(tenant_id, landlord_id), reminder_desc, reminder_title, reminder_date
                                                            ORDER BY (SELECT NULL)
                                                        ) AS RowNum
                                                    FROM reminds
                                                )
                                                DELETE FROM cte WHERE RowNum > 1;";

                SqlCommand deleteDuplicateCommand = new SqlCommand(deleteDuplicateQuery, connection);
                deleteDuplicateCommand.ExecuteNonQuery();
            }

            //Debug.WriteLine("Final SQL job executed!");
        }
    }
}
