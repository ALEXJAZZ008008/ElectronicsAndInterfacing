using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using Game;

namespace Watchdog
{
    class Watchdog
    {
        static void Main(string[] args)
        {
            Client client = new Client();

            Process server = new Process();
            
            int ERROR_FILE_NOT_FOUND = 2;
            int ERROR_ACCESS_DENIED = 5;

            try
            {
                // Get the path that stores user documents.
                string myDocumentsPath = Directory.GetCurrentDirectory();

                string[] myDocumentsPathSplit = Regex.Split(myDocumentsPath, @"Game\\bin\\Debug");

                server.StartInfo.FileName = myDocumentsPathSplit[0] + @"Server\\bin\\Debug\\Server.exe";
                server.Start();
            }
            catch (Win32Exception e)
            {
                if (e.NativeErrorCode == ERROR_FILE_NOT_FOUND)
                {
                    Console.WriteLine(e.Message + ". Check the path.");
                }
                else
                {
                    if (e.NativeErrorCode == ERROR_ACCESS_DENIED)
                    {
                        Console.WriteLine(e.Message + ". You do not have permission to access this file.");
                    }
                }
            }

            client.Input(new string[] { "-h", "localhost", "dateTime", (DateTime.Now + TimeSpan.FromMilliseconds(10000)).ToString() });

            DateTime serverDateTime = DateTime.Parse(Regex.Split(client.Input(new string[] { "-h", "localhost", "dateTime" }), " is ")[1]);

            while (serverDateTime >= DateTime.Now)
            {
                Thread.Sleep(3000);

                try
                {
                    serverDateTime = DateTime.Parse(Regex.Split(client.Input(new string[] { "-h", "localhost", "dateTime" }), " is ")[1]);
                }
                catch
                {

                }
            }

            server.Kill();
        }
    }
}
