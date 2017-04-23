using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.ComponentModel;

namespace Game
{
    class Game
    {
        public static DateTime watchdogDateTime = DateTime.Now + TimeSpan.FromMilliseconds(3000);

        static void Main(string[] args)
        {
            Process watchdog = new Process();

            int ERROR_FILE_NOT_FOUND = 2;
            int ERROR_ACCESS_DENIED = 5;

            try
            {
                // Get the path that stores user documents.
                string myDocumentsPath = Directory.GetCurrentDirectory();

                string[] myDocumentsPathSplit = Regex.Split(myDocumentsPath, @"Game\\bin\\Debug");

                watchdog.StartInfo.FileName = myDocumentsPathSplit[0] + @"Watchdog\\bin\\Debug\\Watchdog.exe";
                watchdog.Start();
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

            using (SceneManager sceneManager = new SceneManager())
            {
                sceneManager.Run(60.0f);
            }
        }
    };
}
