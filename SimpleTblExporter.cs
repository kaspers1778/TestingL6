using System.Data;
using System.Data.SqlClient;
using System.IO;
using kpi_lab6.Comparers;

namespace kpi_lab6
{
    public class SimpleTblExporter
    {
        public static void ExportWhereOrderBy()
        {
            var csvFilePath = @$"{Settings.Path}\Export.csv";
            var connectionString = "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = HouseCompanyDB;Integrated Security = True;";
            var sqlExpression = "SELECT * FROM Citzens wHERE IsAdult=1 ORDER BY	Flat_FlatId";

            using var connection = new SqlConnection(connectionString);
            using var writer = new StreamWriter(csvFilePath);

            connection.Open();
            var command = new SqlCommand(sqlExpression, connection);
            var reader = command.ExecuteReader();
                

            if (reader.HasRows)
            {
                writer.WriteLine("id,name,secondName,isAdult,FlatId");
                while (reader.Read())
                {
                    var id = reader.GetInt32("CitzenId");
                    var name = reader.GetString("FirstName");
                    var secondName = reader.GetString("SecondName");
                    var isAdult = reader.GetBoolean("IsAdult");
                    var flatId = reader.GetInt32("Flat_FlatId");
                    writer.WriteLine($"{id},{name},{secondName},{isAdult},{flatId}");
                }
                reader.Close();
            }
        }

        public static void ExportGroupBy()
        {
            var csvFilePath = @$"{Settings.Path}\Export2.csv";
            var connectionString = "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = HouseCompanyDB;Integrated Security = True;";
            var sqlExpression = "SELECT COUNT(Houses.HouseId) as Adults ,Houses.HouseId FROM Citzens JOIN Flats ON Citzens.Flat_FlatId = Flats.FlatId JOIN Houses ON Flats.House_HouseId = Houses.HouseId " +
                            "WHERE Citzens.IsAdult = 1 " +
                            "GROUP BY Houses.HouseId";

            using var connection = new SqlConnection(connectionString);
            using var writer = new StreamWriter(csvFilePath);

            connection.Open();
            var command = new SqlCommand(sqlExpression, connection);
            var reader = command.ExecuteReader();


            if (reader.HasRows)
            {
                writer.WriteLine("id,Adults");
                while (reader.Read())
                {
                    var id = reader.GetInt32("HouseId");
                    var amount = reader.GetInt32("Adults");
                    writer.WriteLine($"{id},{amount}");
                }
                reader.Close();
            }
        }
    }
}