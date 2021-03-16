using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PriceTracker.Data
{
    
    public class DataAccess
    {
        //private IConfiguration config;

        //public DataAccess(IConfiguration configuration)
        //{
        //    config = configuration;
        //}

        private String ConnectionString()
        {
            return "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=aspnet-PriceTracker-F3296F4B-BC1D-4379-9488-2E1E667F1DDA;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        public void CreateTrackedItem(string userEmail, string itemIdentifier)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(ConnectionString());
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand command = new SqlCommand(
                "INSERT INTO TrackedItems (Email, ItemIdentifier) " +
                "VALUES (@Email, @ItemIdentifier)", con);

            command.Parameters.AddWithValue("@Email", userEmail);
            command.Parameters.AddWithValue("@ItemIdentifier", itemIdentifier);

            con.Open();
            da.InsertCommand = command;
            da.InsertCommand.ExecuteNonQuery();

        }

    }
}
