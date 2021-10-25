using MySqlConnector;
using System.Data;
namespace meria.Services
{
    public class DataBaseOnly
    {
        public MySqlConnection connection = new MySqlConnection("server=localhost; port=3306;username=meria;password=123;database=gorkenesh;allow zero datetime=yes;Allow User Variables=True");
        //public MySqlConnection connection = new MySqlConnection("server=localhost; port=3306;username=meria;password=123;database=gorkenesh;allow zero datetime=yes;Allow User Variables=True");

        public delegate void SendData(DataTable data);
        public event SendData del;
        public void SoursData(string s)
        {
            connection.Close();
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
            DataTable dta1 = new DataTable();
            MySqlDataAdapter dataadap = new MySqlDataAdapter(cmd);
            dataadap.Fill(dta1);
            del(dta1);
            connection.Close();
        }
        public void InsertData(string s)
        {
            
            connection.Close();
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
