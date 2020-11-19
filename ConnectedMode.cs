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
                reader.Close();
                connection.Close();
            }
        }

        public static void ConnectedWithParameters()
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                Console.WriteLine("Inserisci un genere");
                String genere;
                genere = Console.ReadLine();

                //2. aprire una connessione

                connection.Open();

                //3. creare un command (ho usato il costruttore che vuole già un command tipo testo e 
                // l'oggetto connessione)
                SqlCommand command = new SqlCommand("select * from movies where genere = @genere", connection);

                //3b. creare parametro
                SqlParameter par = new SqlParameter();
                par.ParameterName = "@genere"; //il nome del parametro che troverò nel comando

                par.Value = genere; //il valore del parametro
                command.Parameters.Add(par); // aggiunge il comando creato a riga 91 al comando di riga 88

                // command.Parameters.AddWithValue("@genere", genere); 
                //fa la stessa cosa da riga 91 a 95 ma in una sola riga

                //4. eseguire il command --> datareader

                SqlDataReader reader = command.ExecuteReader();

                //5. leggere i dati
                while (reader.Read())
                {
                    Console.WriteLine("{0} - {1} - {2}",
                        reader["ID"],
                        reader["titolo"],
                        reader["genere"]);
                }

                //6. chiudere la connessione
                reader.Close();

                connection.Close();
            }
        }

        public static void ConnectedStoredProcedure()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
              
                //APRO LA CONNESSIONE
                connection.Open();

                //CREO IL COMANDO
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "stpActorsByCachetRange";

                // questa procedure vuole dei parametri! quindi dobbiamo creare dei param qui
                //CREO I PARAMETRI DEL COMANDO E LI AGGIUNGO

                cmd.Parameters.AddWithValue("@min", 5000);
                cmd.Parameters.AddWithValue("@max", 9000);

                //QUI C'E' UN PARAMETRO OUTPUT. Uso il modo lungo perché è output.
                SqlParameter returnValue = new SqlParameter();
                returnValue.ParameterName = "@returnedCount";

                //definisco il tipo del valore di ritorno.
                //non è necessario tranne che per nvarchar, in quanto dobbiamo specificare la lunghezza
                returnValue.SqlDbType = System.Data.SqlDbType.Int;

                //specifico che il parametro è in direzione output.
                //se non si specifica, il programma dà per scontato che sia input
                returnValue.Direction = System.Data.ParameterDirection.Output;

                //AGGIUNGO IL PARAMETRO AL COMANDO
                cmd.Parameters.Add(returnValue);

                //ESEGUO IL COMANDO
                SqlDataReader reader = cmd.ExecuteReader();

                //VISUALIZZO I DATI
                while (reader.Read())
                {
                    Console.WriteLine("{0} - {1} - {2} - {3}",
                                      reader["ID"],
                                      reader["FirstName"],
                                      reader["LastName"],
                                      reader["Cachet"]);
                }

                //CHIUDO L'ESECUZIONE DEL COMANDO
                reader.Close();

                //se non voglio leggere la tabella ma avere solo il parametro di ritorno
                cmd.ExecuteNonQuery();

                Console.WriteLine("Numero attori: {0}", cmd.Parameters["@returnedCount"].Value);

                //CHIUDO LA CONNESSIONE
                connection.Close();
            }
        }
    }


}
