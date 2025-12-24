using Microsoft.Data.SqlClient;

namespace GestionScolarite.DataAccessLayer.DataAccess
{
    public static class DatabaseConnection
    {
        private static string connectionString = "Data Source=localhost,14333;Initial Catalog=gestion_scolaire;Persist Security Info=True;User ID=sa;Password=Admin@1234!;Encrypt=True;Trust Server Certificate=True";
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static void SetConnectionString(string cs)
        {
            connectionString = cs;
        }
    }
}


