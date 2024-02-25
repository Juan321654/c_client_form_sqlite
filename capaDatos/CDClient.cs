using capaEntidad;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace capaDatos
{
    public class CDClient
    {
        private string dbFilePath = "curso_cs.db";
        private string connectionString = $"Data Source=curso_cs.db;Version=3;";

        public CDClient()
        {
            InitializeDatabase(); // Ensure the database is set up correctly on instantiation
        }

        private void InitializeDatabase()
        {
            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath); // Create the database file if it doesn't exist

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string sql = @"
                    CREATE TABLE IF NOT EXISTS clients (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL,
                        last_name TEXT NOT NULL,
                        photo_path TEXT
                    );";

                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public void checkConnection()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Connected");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connection Failed: " + ex.Message);
                }
            }
        }

        public void Create(CEClients cE)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO clients (name, last_name, photo_path) VALUES (@name, @lastName, @photoPath);";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", cE.Name);
                    command.Parameters.AddWithValue("@lastName", cE.LastName);
                    command.Parameters.AddWithValue("@photoPath", cE.PhotoPath);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Client added successfully.");
                }
            }
        }

        public DataSet GetList()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM clients;";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet, "clientsTable");
                        return dataSet;
                    }
                }
            }
        }

        public void Update(CEClients cE)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE clients SET name = @name, last_name = @lastName, photo_path = @photoPath WHERE id = @id;";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", cE.Name);
                    command.Parameters.AddWithValue("@lastName", cE.LastName);
                    command.Parameters.AddWithValue("@photoPath", cE.PhotoPath);
                    command.Parameters.AddWithValue("@id", cE.Id);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Client updated successfully.");
                }
            }
        }

        public void Delete(CEClients cE)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM clients WHERE id = @id;";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", cE.Id);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Client deleted successfully.");
                }
            }
        }
    }
}
