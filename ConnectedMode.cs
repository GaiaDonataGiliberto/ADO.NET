using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ADO
{
    public class ConnectedMode
    {
        const String connectionString = @"Persist Security Info = False; 
        Integrated Security = True; Initial Catalog = cinemaDB; Server = WINAPP43MDHLF7O\SQLEXPRESS";

        //Persist Security Info = False -->  non salva la ps (perché gli stiamo passando quella dell'account)
        //Integrated Security = True --> stiamo dicendo di utilizzare user e ps dell'account
        //Initial Catalog = CinemaDB --> nome del DB
        //Server = WINAPP43MDHLF7O\SQLEXPRESS --> nome del server

        public static void Connected()
        {
            /*1. creare una connessione
             *2. aprire la connessione
             *3. creare un command
             *3b. creare parametri
             *4. eseguire command
             *5. leggere i dati
             *6. chiudere la connessione
             */

            //1. creare una connessione
            //METODO 1  (ricordarsi di importare il pacchetto SqlConnection, va fatto per ogni progetto
            //che usa ado.net)

            // SqlConnection connection = new SqlConnection();
            // connection.ConnectionString = connectionString;

            //METODO 2
            // SqlConnection connection = new SqlConnection(connectionString);

            //METODO 3 (migliore)
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //2. aprire una connessione

                connection.Open();

                //3. creare un command
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "select * from movies";

                //4. eseguire il command --> datareader

                SqlDataReader reader = command.ExecuteReader();

                //5. leggere i dati
                while (reader.Read())
                {
                    Console.WriteLine("{0} - {1} - {2} - {3}",
                        reader["ID"],
                        reader["titolo"],
                        reader["genere"],
                        reader["durata"]);
                }

                //6. chiudere la connessione

                connection.Close();
            }
        }
    }
}
