using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ADO
{
    public class DisconnectedMode
    {
        const String conString = @"Persist Security Info = False; 
        Integrated Security = True; Initial Catalog = cinemaDB; Server = WINAPP43MDHLF7O\SQLEXPRESS";

        public static void Disconnected()
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                //costruzione Adapter
                SqlDataAdapter adapter = new SqlDataAdapter();

                //creazione comandi che poi assoceremo all'adapter
                SqlCommand select = new SqlCommand();
                select.Connection = connection;
                select.CommandType = System.Data.CommandType.Text;
                select.CommandText = "select * from movies";

                SqlCommand insert = new SqlCommand();
                insert.Connection = connection;
                insert.CommandType = System.Data.CommandType.Text;
                insert.CommandText = "insert into movies values (@titolo, @genere, @durata)";

                //creazione e definizione dei parametri da passare alla query
                insert.Parameters.Add("@titolo", System.Data.SqlDbType.NVarChar, 255, "titolo");
                insert.Parameters.Add("@genere", System.Data.SqlDbType.NVarChar, 255, "genere");
                insert.Parameters.Add("@durata", System.Data.SqlDbType.Int, 500, "durata");

                //farlo per tutti i metodi CRUD

                //associare i comandi all'adapter
                adapter.SelectCommand = select;
                adapter.InsertCommand = insert;
                

                DataSet dataSet = new DataSet();

                try
                {
                    connection.Open();

                    //scarica la tabella 'movies' e la mette nel DataSet
                    //in automatico esegue l'InsertCommand
                    adapter.Fill(dataSet, "movies");

                    //creazione singolo record
                    DataRow movie = dataSet.Tables["movies"].NewRow();

                    //definiamo i valori della nuova riga
                    movie["titolo"] = "V per Vendetta";
                    movie["genere"] = "Azione";
                    movie["durata"] = 125;

                    //aggiungiamo la nuova riga 
                    dataSet.Tables["movies"].Rows.Add(movie);

                    //manda le modifiche
                    //in automatico esegue gli eventuali InsertCommand, DeleteCommand, UpdateCommand
                    adapter.Update(dataSet, "movies");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
                
            }
        }

    }
}
