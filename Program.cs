using System;

namespace ADO
{
    class Program
    {
        static void Main(string[] args)
        {
            DisconnectedMode.Disconnected();

            ConnectedMode.Connected();

            //ConnectedMode.ConnectedWithParameters();

            //ConnectedMode.ConnectedStoredProcedure();

            //ConnectedMode.ConnectedScalar();

        }
    }
}
