using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows;

namespace WpfApp7
{
    class Database
    {
        static readonly string connectionStr = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        private Database()
        {

        }
        public static bool createTable()
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                SqlCommand query = new SqlCommand(@"CREATE TABLE Images(
                                ID int IDENTITY(1, 1) NOT NULL,
                                Lable varchar(MAX) NOT NULL,
                                Data IMAGE NOT NULL,
                                CONSTRAINT pk_id PRIMARY KEY(ID))", connection);
                try
                {
                    query.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
        public static List<Models.ImageMinData> getImagesMinData()
        {
            List<Models.ImageMinData> res = new List<Models.ImageMinData>();
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                SqlCommand query = new SqlCommand("SELECT ID, Lable FROM [Images]", connection);
                SqlDataReader reader = query.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        res.Add(new Models.ImageMinData(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
                catch
                {
                    return null;
                }
            }
            return res;
        }
        public static Image getImageByID(int id)
        {
            byte[] imageBytes;
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                SqlCommand query = new SqlCommand("SELECT Data FROM [Images] WHERE ID = @ID", connection);
                query.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = query.ExecuteReader();
                try
                {
                    reader.Read();
                    imageBytes = (byte[])reader[0];
                    using (MemoryStream stream = new MemoryStream(imageBytes, false))
                        return Image.FromStream(stream);
                }
                catch
                {
                    return null;
                }
            }

        }

        static public bool AddImage(byte[] imageBytes, string lable = "nolabled")
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                string query = @"INSERT INTO Images(Lable, Data) VALUES(@Lable, @Data)";
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                sqlCommand.Parameters.AddWithValue("@Lable", lable);
                sqlCommand.Parameters.AddWithValue("@Data", imageBytes);
                try
                {
                    sqlCommand.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }
        static public bool DeleteImageById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                SqlCommand query = new SqlCommand("DELETE FROM [Images] WHERE ID = @ID", connection);
                query.Parameters.AddWithValue("@ID", id);
                try
                {
                    query.ExecuteNonQuery();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
