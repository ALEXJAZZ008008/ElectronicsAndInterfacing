using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;

class Server
{
    #region OnServerShutdown Hook

    //This is used to end the server
    static bool serverOffline = false;

    //These are used to initiaize an event handler used to detect when the server has exited
    private delegate bool EventHandler();
    static EventHandler eventHandler;

    [DllImport("Kernel32")]
    private static extern bool SetConsoleCtrlHandler(EventHandler eventHandler, bool add);

    #endregion

    //This is the dictionary used to store the usernames and associated locations
    public static Dictionary<string, string> dictionary = new Dictionary<string, string>();

    //This is used to differentiate from code associated with each protocol
    public enum Protocol
    {
        whoIs,
        h09,
        h10,
        h11
    }

    //These are the default file paths to the server backups and logs
    public static string dictionaryBackupPath = "DictionaryBackup.txt";
    public static string logPath = "ServerLog.txt";

    public static Mutex backupMutex = new Mutex();
    public static Mutex loggingMutex = new Mutex();

    public static DateTime dateTime;

    public static bool logBool;
    public static string logMessage = string.Empty;

    private static void Main(string[] args)
    {
        //This initializes an instance of the above event handler
        eventHandler += new EventHandler(OnServerShutdown);
        SetConsoleCtrlHandler(eventHandler, true);

        //This initializes files for backups etc
        OnServerStartup(args);

        //This starts a listener
        TcpListener listener = new TcpListener(IPAddress.Any, 43);
        listener.Start();

        #region To Threads

        try
        {
            //This is the main loop of the program
            while (!serverOffline)
            {
                //This creates new sockets
                Socket socket = listener.AcceptSocket();

                //This calls the thread method
                Threads(socket);
            }
        }
        catch (Exception ex)
        {
            //This prints to the screen an error message
            Console.WriteLine("ERROR: " + ex.ToString());

            //This ensures the server is shutdown correctly under unusual circumstances
            OnServerShutdown();
        }

        #endregion
    }

    private static void OnServerStartup(string[] args)
    {
        dateTime = DateTime.MinValue;

        //If there are no arguments the program doesn't bother to execute any further code
        if (args.Length != 0)
        {
            //These are used to track the progress through the assignment process and to pass the correct arguments to the correct variables
            int i = 0;

            //This ensures the correct number of arguments are initialized
            while (i < args.Length)
            {
                switch (args[i])
                {
                    #region L

                    //This is triggured when the server log file path is to be changed
                    case "-l":

                        //This ensures that there is an server log file path to set
                        if (i + 2 <= args.Length)
                        {
                            //This sets the server log file path
                            logPath = args[i + 1];
                        }

                        //This is used to break out of the enclosing while loop
                        i = i + 2;

                        break;

                    #endregion

                    #region F

                    //This is triggured when the dictionary backup file path is to be changed
                    case "-f":

                        //This ensures that there is an dictionary backup file path to set
                        if (i + 2 <= args.Length)
                        {
                            //This sets the dictionary backup file path
                            dictionaryBackupPath = args[i + 1];
                        }

                        //This is used to break out of the enclosing while loop
                        i = i + 2;

                        //This breaks out of the case
                        break;

                    #endregion
                }
            }
        }

        //If the dictionary backup file exists it is loaded here
        if (File.Exists(dictionaryBackupPath))
        {
            //This initializes a new reader for the dictionary backup file
            StreamReader streamReader = new StreamReader(dictionaryBackupPath);

            //This continues while there are lines to read
            while ((!streamReader.EndOfStream) && (streamReader.Peek() != -1))
            {
                //This reads the next line from the dictionary backup file
                string nextLine = streamReader.ReadLine();

                //This splits the username from the location
                string[] toDictionary = Regex.Split(nextLine, "\",\"");
                toDictionary[0] = toDictionary[0].Split('"')[1];
                toDictionary[1] = toDictionary[1].Split('"')[0];

                //This addes all the necessary information to the dictionary
                dictionary.Add(toDictionary[0], toDictionary[1]);
            }

            //This closes the stream reader
            streamReader.Close();
        }
        else
        {
            //This creates the dictionary backup file if it does not exist
            File.Create(dictionaryBackupPath).Close();
        }
    }

    private static bool OnServerShutdown()
    {
        dateTime = DateTime.MinValue;

        //This initializes a new instance of the Update class
        Update update = new Update();

        //This calls the dictionary backup file update method
        update.UpdateDictionaryBackup();

        //This ends the main loop
        serverOffline = true;

        return true;
    }

    private static void Threads(Socket socket)
    {
        //This initializes a new instance of the Update class
        Update update = new Update();

        //This initializes a new thread with a max memory size of 250Kb and a loction to work of Initialisation in the Update class
        Thread thread = new Thread(() => update.Initialisation(socket), 1);

        //This starts a new thread
        thread.Start();
    }
}

class Update
{
    //This creates a new instance of the Logging class
    Logging logging = new Logging();

    public void Initialisation(Socket socket)
    {
        //This opens a new network stream on the given socket
        NetworkStream networkStream = new NetworkStream(socket);

        //This acquires the IP address of the host
        string ipAddress = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();

        #region To UpdateProtocol

        try
        {
            //This calls the update protocol method
            UpdateProtocol(networkStream, ipAddress);
        }
        catch (Exception ex)
        {
            //This prints to the screen an error message
            Console.WriteLine("ERROR: " + ex.ToString());
        }
        finally
        {
            //This cleans up before exiting
            networkStream.Close();

            socket.Close();
        }

        #endregion
    }

    private void UpdateProtocol(NetworkStream networkStream, string ipAddress)
    {
        //These are the variables used to pass data too and from the server
        StreamWriter streamWriter = new StreamWriter(networkStream);
        StreamReader streamReader = new StreamReader(networkStream);

        //This ensures that if the client hangs for too long the server doens't wait for a responce
        networkStream.ReadTimeout = 1000;

        //This reads the next line from the client
        string nextLine = streamReader.ReadLine();

        #region Protocol

        //This holds the above Protocol enum
        Server.Protocol protocol;

        //This selects the correct protocol
        if ((nextLine.Contains("GET /")) || (nextLine.Contains("PUT /")) || (nextLine.Contains("POST /")))
        {
            if (nextLine.Contains(" HTTP/1.0") || nextLine.Contains(" HTTP/1.1"))
            {
                if (nextLine.Contains(" HTTP/1.0"))
                {
                    protocol = Server.Protocol.h10;
                }
                else
                {
                    protocol = Server.Protocol.h11;
                }
            }
            else
            {
                protocol = Server.Protocol.h09;
            }
        }
        else
        {
            protocol = Server.Protocol.whoIs;
        }

        #endregion

        //This ensures that if the client hangs for too long the server doens't wait for a responce
        networkStream.WriteTimeout = 1000;

        //This calls the update dictionary class
        UpdateDictionary(streamWriter, streamReader, nextLine, protocol, ipAddress);
    }

    private void UpdateDictionary(StreamWriter streamWriter, StreamReader streamReader, String nextLine, Server.Protocol protocol, string ipAddress)
    {
        //These variables are used to hold output stings
        string output = string.Empty;
        string protocolStatus = "OK";

        switch (protocol)
        {
            #region WhoIs

            //This deals with whois server requests
            case Server.Protocol.whoIs:

                //This splits the input into a array containing the username and location
                string[] nextLineSectionsWhoIs = nextLine.Split(new char[] { ' ' }, 2);

                #region 1

                //This checks to see if the array contains both a username and location
                if (nextLineSectionsWhoIs.Length == 1)
                {
                    //This checks to see if the dictionary already containes an instance of the given username
                    if (Server.dictionary.ContainsKey(nextLine))
                    {
                        //This adds the username to a string
                        string username = Server.dictionary[nextLine];

                        //This adds the username to a string
                        output = username + "\r\n";
                    }
                    else
                    {
                        //This outputs an error if the dictionary does not contain the username
                        output = "ERROR: no entries found" + "\r\n";
                    }
                }

                #endregion

                #region 2

                //This checks to see if the array contains both a username and location
                if (nextLineSectionsWhoIs.Length == 2)
                {
                    //This checks to see if the dictionary already containes an instance of the given username
                    if (Server.dictionary.ContainsKey(nextLineSectionsWhoIs[0]))
                    {
                        //This adds the location to the dictionary
                        Server.dictionary[nextLineSectionsWhoIs[0]] = nextLineSectionsWhoIs[1];

                        //This an output to a string
                        output = "OK" + "\r\n";
                    }
                    else
                    {
                        //This adds the username and location to the dictionary
                        Server.dictionary.Add(nextLineSectionsWhoIs[0], nextLineSectionsWhoIs[1]);

                        //This an output to a string
                        output = "OK" + "\r\n";
                    }
                }

                #endregion

                //This breaks out of the case
                break;

            #endregion

            #region H09

            //This deals with whois server requests
            case Server.Protocol.h09:

                //This splits the input into a array containing the username and location
                string[] nextLineSectionsH09 = nextLine.Split(new char[] { ' ' }, 2);

                #region GET

                //This checks to see if the array contains both a username and location
                if (nextLineSectionsH09[0] == "GET")
                {
                    //This splits the input into a array containing the username and GET
                    string nextLineLocation = nextLine.Split('/')[1];

                    //This checks to see if the dictionary already containes an instance of the given username
                    if (Server.dictionary.ContainsKey(nextLineLocation))
                    {
                        //This adds the username to a string
                        string username = Server.dictionary[nextLineLocation];

                        //This adds the username to a string
                        output = "HTTP/0.9 200 OK" + "\r\n" + "Content-Type: text/plain" + "\r\n\r\n" + username + "\r\n";
                    }
                    else
                    {
                        //This outputs an error if the dictionary does not contain the username
                        output = "HTTP/0.9 404 Not Found" + "\r\n" + "Content-Type: text/plain" + "\r\n\r\n";
                    }
                }

                #endregion

                #region PUT

                //This checks to see if the array contains both a username and location
                if (nextLineSectionsH09[0] == "PUT")
                {
                    //This splits the input into a array containing the username and GET
                    string username = nextLine.Split('/')[1];

                    streamReader.ReadLine();

                    //This adds the location to a string
                    string locationOfUsername = streamReader.ReadLine(); ;

                    //This checks to see if the dictionary already containes an instance of the given username
                    if (Server.dictionary.ContainsKey(username))
                    {
                        //This adds the location to the dictionary
                        Server.dictionary[username] = locationOfUsername;

                        //This an output to a string
                        output = "HTTP/0.9 200 OK" + "\r\n" + "Content-Type: text/plain" + "\r\n\r\n";
                    }
                    else
                    {
                        //This adds the username and location to the dictionary
                        Server.dictionary.Add(username, locationOfUsername);

                        //This an output to a string
                        output = "HTTP/0.9 200 OK" + "\r\n" + "Content-Type: text/plain" + "\r\n\r\n";
                    }
                }

                #endregion

                //This breaks out of the case
                break;

            #endregion

            #region H10

            case Server.Protocol.h10:

                //This splits the input into a array containing the username and location
                string[] nextLineSectionsH10 = nextLine.Split(new char[] { ' ' }, 3);

                #region GET

                //This checks to see if the array contains both a username and location
                if (nextLineSectionsH10[0] == "GET")
                {
                    //This splits the input into a array containing the username and GET
                    string nextLineLocation = nextLineSectionsH10[1].Split('/')[1];

                    //This checks to see if the dictionary already containes an instance of the given username
                    if (Server.dictionary.ContainsKey(nextLineLocation))
                    {
                        //This adds the username to a string
                        string username = Server.dictionary[nextLineLocation];

                        //This adds the username to a string
                        output = "HTTP/1.0 200 OK" + "\r\n" + "Content-Type: text/plain" + "\r\n\r\n" + username + "\r\n";
                    }
                    else
                    {
                        //This outputs an error if the dictionary does not contain the username
                        output = "HTTP/1.0 404 Not Found" + "\r\n" + "Content-Type: text/plain" + "\r\n\r\n";
                    }
                }

                #endregion

                #region POST

                //This checks to see if the array contains both a username and location
                if (nextLineSectionsH10[0] == "POST")
                {
                    //This splits the input into a array containing the username and POST
                    string username = nextLineSectionsH10[1].Split('/')[1];

                    streamReader.ReadLine();
                    streamReader.ReadLine();

                    //This adds the location to a string
                    string locationOfUsername = streamReader.ReadLine();

                    //This checks to see if the dictionary already containes an instance of the given username
                    if (Server.dictionary.ContainsKey(username))
                    {
                        //This adds the username and location to the dictionary
                        Server.dictionary[username] = locationOfUsername;

                        //This an output to a string
                        output = "HTTP/1.0 200 OK" + "\r\n" + "Content-Type: text/plain" + "\r\n\r\n";
                    }
                    else
                    {
                        //This adds the username and location to the dictionary
                        Server.dictionary.Add(username, locationOfUsername);

                        //This an output to a string
                        output = "HTTP/1.0 200 OK" + "\r\n" + "Content-Type: text/plain" + "\r\n\r\n";
                    }
                }

                #endregion

                //This breaks out of the case
                break;

            #endregion

            #region H11

            case Server.Protocol.h11:

                //This splits the input into a array containing the username and location
                string[] nextLineSectionsH11 = nextLine.Split(new char[] { ' ' }, 3);

                #region GET

                //This checks to see if the array contains both a username and location
                if (nextLineSectionsH11[0] == "GET")
                {
                    //This splits the input into a array containing the username and GET
                    string nextLineLocation = nextLineSectionsH11[1].Split('/')[1];

                    streamReader.ReadLine();

                    //This adds the optional header lines to a string
                    string optionalHeaderLines = streamReader.ReadLine();

                    //This checks to see if the dictionary already containes an instance of the given username
                    if (Server.dictionary.ContainsKey(nextLineLocation))
                    {
                        //This adds the username to a string
                        string username = Server.dictionary[nextLineLocation];

                        //This adds the username to a string
                        output = "HTTP/1.1 200 OK" + "\r\n" + "Content-Type: text/plain" + "\r\n\r\n" + optionalHeaderLines + "\r\n" + username + "\r\n";
                    }
                    else
                    {
                        //This outputs an error if the dictionary does not contain the username
                        output = "HTTP/1.1 404 Not Found" + "\r\n" + "Content-Type: text/plain" + "\r\n\r\n" + optionalHeaderLines + "\r\n";
                    }
                }

                #endregion

                #region POST

                //This checks to see if the array contains both a username and location
                if (nextLineSectionsH11[0] == "POST")
                {
                    //This splits the input into a array containing the username and POST
                    string username = nextLineSectionsH11[1].Split('/')[1];

                    streamReader.ReadLine();
                    streamReader.ReadLine();

                    //This adds the optional header lines to a string
                    string OptionalHeaderLines = streamReader.ReadLine();

                    //This adds the location to a string
                    string locationOfUsername = streamReader.ReadLine(); ;

                    //This checks to see if the dictionary already containes an instance of the given username
                    if (Server.dictionary.ContainsKey(username))
                    {
                        //This adds the username and location to the dictionary
                        Server.dictionary[username] = locationOfUsername;

                        //This an output to a string
                        output = "HTTP/1.1 200 OK" + "\r\n" + "Content-Type: text/plain" + "\r\n\r\n" + OptionalHeaderLines + "\r\n";
                    }
                    else
                    {
                        //This adds the username and location to the dictionary
                        Server.dictionary.Add(username, locationOfUsername);

                        //This an output to a string
                        output = "HTTP/1.1 200 OK" + "\r\n" + "Content-Type: text/plain" + "\r\n\r\n" + OptionalHeaderLines + "\r\n";
                    }
                }
                #endregion

                //This breaks out of the case
                break;

            #endregion

            #region Default

            default:

                protocolStatus = "UNKNOWN";

                break;

            #endregion
        }

        streamWriter.WriteLine(output);
        streamWriter.Close();

        //This updates the dictionary backup
        UpdateDictionaryBackup();

        //This runs the logging scripts
        logging.ToLog(output, ipAddress, protocolStatus);
    }

    public void UpdateDictionaryBackup()
    {
        if (DateTime.Now > Server.dateTime)
        {
                //This locks this part of the code univerally
                Server.backupMutex.WaitOne();

                try
                {
                    Server.logBool = true;

                    //This creates a new stream writer that will erase all contentes of the dictionary backup before writing
                    StreamWriter streamWriter = new StreamWriter(Server.dictionaryBackupPath, false);

                    //This writes each line from the dictionary into the dictionary backup file in a CSV style
                    foreach (var entry in Server.dictionary)
                    {
                        streamWriter.WriteLine("\"{0}\",\"{1}\"", entry.Key, entry.Value);
                    }

                    //This closes the current stream writer
                    streamWriter.Close();
                }
                catch
                {

                }

                //This allowes another thread to enter the previous code
                Server.backupMutex.ReleaseMutex();

            Server.dateTime = DateTime.Now + TimeSpan.FromMilliseconds(1000);
        }
        else
        {
            Server.logBool = false;
        }
    }
}

class Logging
{
    //This allows external access to the private log method
    public void ToLog(string output, string ipAddress, string protocolStatus)
    {
        Log(output, ipAddress, protocolStatus);
    }

    private void Log(string output, string ipAddress, string protocolStatus)
    {
        //This adds the correct log message to a string
        Server.logMessage = Server.logMessage + ipAddress + " - - " + DateTime.Now.ToString("'['dd'/'MM'/'yyyy':'HH':'mm':'ss zz00']'") + " \"" + Regex.Split(output, "\r\n")[0] + "\" " + protocolStatus + "\r\n";

        if (Server.logBool)
        {
            //This locks this part of the code univerally
            Server.loggingMutex.WaitOne();

            try
            {
                //This creates if needed a file that is then written to with the log message
                StreamWriter streamWriter = File.AppendText(Server.logPath);

                //This writes the message
                streamWriter.Write(Server.logMessage);
                streamWriter.Close();

                //This outputs the log message to the screen
                Console.Write(Server.logMessage);

                Server.logMessage = string.Empty;
            }
            catch
            {

            }

            //This allowes another thread to enter the previous code
            Server.loggingMutex.ReleaseMutex();
        }
    }
}