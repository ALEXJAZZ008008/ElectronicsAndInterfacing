using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Sockets;

namespace Game
{
    public class Client
    {
        //This pair of variables holds the IP address of the server and the port of the server globally for easy access
        private static string ipAddress = "localhost";
        private static int port = 43;

        //This is an enum used to differentiate protocols
        private enum Protocol
        {
            whoIs,
            h09,
            h10,
            h11
        }

        //This holds the above enum
        private static Protocol protocol;

        //These are used to store the variables that will bypassed from the client to the server
        private static string username;
        private static string location;
        private static string optionalHeaderLines = string.Empty;

        public string Input(string[] args)
        {
            //These are used to store variables used to verify the arguments given to the client
            int noOfArgs = 0;
            bool argBool = false;

            //This calls the method used to verify the arguments given
            FromUser(args, ref noOfArgs, ref argBool);

            //If the arguments given are valid this calls the method which updates the server
            if (argBool)
            {
                return ToServer(args, ref noOfArgs);
            }
            else
            {
                return null;
            }
        }

        private static void FromUser(string[] args, ref int noOfArgs, ref bool argBool)
        {
            //If there are no arguments the program doesn't bother to execute any further code
            if (args.Length != 0)
            {
                //These are used to track the progress through the assignment process and to pass the correct arguments to the correct variables
                int i = 0;
                bool usernameBool = false;
                bool locationBool = false;

                //This is used to exit the program if the arguments given are invalid
                argBool = true;

                //This ensures the correct number of arguments are initialized
                while (i < args.Length)
                {
                    switch (args[i])
                    {
                        #region H

                        //This is triggured when the IP address is to be changed
                        case "-h":

                            //This ensures that there is an IP address to set
                            if (i + 2 <= args.Length)
                            {
                                try
                                {
                                    //This sets the IP address
                                    ipAddress = args[i + 1];

                                    //This tracks the number of arguments
                                    noOfArgs = noOfArgs + 2;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("ERROR: " + ex.ToString());

                                    //This tracks the number of arguments
                                    noOfArgs++;
                                }
                            }
                            else
                            {
                                //This tracks the number of arguments
                                noOfArgs++;
                            }

                            //This is used to break out of the enclosing while loop
                            i = i + 2;

                            //This breaks out of the case
                            break;

                        #endregion

                        #region P

                        //This is triggered when the port is to be changed
                        case "-p":

                            //This ensures that there is an IP address to set
                            if (i + 2 <= args.Length)
                            {
                                try
                                {
                                    //This sets the port
                                    port = int.Parse(args[i + 1]);

                                    //This tracks the number of arguments
                                    noOfArgs = noOfArgs + 2;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("ERROR: " + ex.ToString());

                                    //This tracks the number of arguments
                                    noOfArgs++;
                                }
                            }
                            else
                            {
                                //This tracks the number of arguments
                                noOfArgs++;
                            }

                            //This is used to break out of the enclosing while loop
                            i = i + 2;

                            //This breaks out of the case
                            break;

                        #endregion

                        #region WhoIs

                        //This detects the type of protocol to be used
                        case "-whoIs":

                            //This sets the protocol to be used
                            protocol = Protocol.whoIs;

                            //This tracks the number of arguments
                            noOfArgs++;

                            //This is used to break out of the enclosing while loop
                            i++;

                            //This breaks out of the case
                            break;

                        #endregion

                        #region H9

                        case "-h9":

                            //This sets the protocol to be used
                            protocol = Protocol.h09;

                            //This tracks the number of arguments
                            noOfArgs++;

                            //This is used to break out of the enclosing while loop
                            i++;

                            //This breaks out of the case
                            break;

                        #endregion

                        #region H0

                        case "-h0":

                            //This sets the protocol to be used
                            protocol = Protocol.h10;

                            //This tracks the number of arguments
                            noOfArgs++;

                            //This is used to break out of the enclosing while loop
                            i++;

                            //This breaks out of the case
                            break;

                        #endregion

                        #region H1

                        case "-h1":

                            //This sets the protocol to be used
                            protocol = Protocol.h11;

                            //This tracks the number of arguments
                            noOfArgs++;

                            //This is used to break out of the enclosing while loop
                            i++;

                            //This breaks out of the case
                            break;

                        #endregion

                        #region Default

                        //This is the default case to be called whenever the argument doesn't match one of the other cases
                        default:

                            //This tests to see if a user name has already been set
                            if (usernameBool == true)
                            {
                                //This sets the location
                                location = args[i];

                                //This ensures the program does not crash by executing the following lines on a empty string
                                if (location != string.Empty)
                                {
                                    //This removes all black space from the beginning and end of the location before putting it into an array of its characters
                                    char[] locationCharArray = location.Trim().ToCharArray();

                                    //if the location begins with a / the location is made invalid so as not to interfere with future code
                                    if ((locationCharArray[0] == '/'))
                                    {
                                        argBool = false;

                                        Console.WriteLine("ERROR: Invalid arguments.");

                                        //This breaks out of the case
                                        break;
                                    }
                                }

                                //If the location is already set that must mean that an invalid set of arguments has been entered as the last argument will always be the location thus this portion of code will exit the program under these circumstances
                                if (locationBool == true)
                                {
                                    argBool = false;

                                    Console.WriteLine("ERROR: Invalid arguments.");

                                    //This breaks out of the case
                                    break;
                                }
                                else
                                {
                                    //This is a boolean to track the status of the location
                                    locationBool = true;
                                }
                            }
                            else
                            {
                                //This sets the user name
                                username = args[i];

                                //This is a boolean to track the status of the user name
                                usernameBool = true;
                            }

                            //This is used to break out of the enclosing while loop
                            i++;

                            //This breaks out of the case
                            break;

                        #endregion
                    }

                    //This breaks out of the while loop if the arguments given are invalid
                    if (!argBool)
                    {
                        break;
                    }
                }
            }
            else
            {
                //This prints to the screen an error message
                Console.WriteLine("ERROR: No arguments given.");
            }
        }

        private static string ToServer(string[] args, ref int noOfArgs)
        {
            //These variables are used to store messages to be sent to the server and screen
            string toServerInput = string.Empty;
            string toScreenOutput = string.Empty;

            //This variable dictates if the output from the server needs to be read or not
            bool serverOutput = false;

            //This ensures that a correct set of arguments has been received
            if ((args.Length - noOfArgs == 1) || (args.Length - noOfArgs == 2))
            {
                //If the number of arguments unaccounted for is one then there must only be arguments for a user name
                if (args.Length - noOfArgs == 1)
                {
                    #region One Arg

                    //This indicates that the server will return something of value
                    serverOutput = true;

                    //This addes things to be outputted to the screen
                    toScreenOutput = username + " is ";

                    switch (protocol)
                    {
                        #region WhoIs

                        //This deals with whois server requests
                        case Protocol.whoIs:

                            //This sets the string to query the server
                            toServerInput = username;

                            //This breaks out of the case
                            break;

                        #endregion

                        #region H09

                        //This deals with HTTP 0.9 server requests
                        case Protocol.h09:

                            //This sets the string to query the server
                            toServerInput = "GET" + " /" + username;

                            //This breaks out of the case
                            break;

                        #endregion

                        #region H10

                        //This deals with HTTP 1.0 server requests
                        case Protocol.h10:

                            //This sets the string to query the server
                            toServerInput = "GET" + " /" + username + " HTTP/1.0" + "\r\n" + optionalHeaderLines;

                            //This breaks out of the case
                            break;

                        #endregion

                        #region H11

                        //This deals with HTTP 1.0 server requests
                        case Protocol.h11:

                            //This sets the string to query the server
                            toServerInput = "GET" + " /" + username + " HTTP/1.1" + "\r\n" + "Host: " + ipAddress + "\r\n" + optionalHeaderLines;

                            //This breaks out of the case
                            break;

                        #endregion
                    }

                    #endregion
                }

                //If the number of arguments unaccounted for is one then there must be arguments for a user name and a location
                else
                {
                    #region Two Arg

                    //This addes things to be outputted to the screen
                    toScreenOutput = username + " location changed to be " + location + "\r\n";

                    switch (protocol)
                    {
                        #region WhoIs

                        //This deals with whois server requests
                        case Protocol.whoIs:

                            //This sets the string to query the server
                            toServerInput = username + " " + location;

                            //This breaks out of the case
                            break;

                        #endregion

                        #region H09

                        //This deals with HTTP 0.9 server requests
                        case Protocol.h09:

                            //This sets the string to query the server
                            toServerInput = "PUT" + " /" + username + "\r\n" + "\r\n" + location;

                            //This breaks out of the case
                            break;

                        #endregion

                        #region H10

                        //This deals with HTTP 1.0 server requests
                        case Protocol.h10:

                            //This sets the string to query the server
                            toServerInput = "POST" + " /" + username + " HTTP/1.0" + "\r\n" + "Content-Length: " + location.Length + "\r\n" + optionalHeaderLines + "\r\n" + location;

                            //This breaks out of the case
                            break;

                        #endregion

                        #region H11

                        //This deals with HTTP 1.0 server requests
                        case Protocol.h11:

                            //This sets the string to query the server
                            toServerInput = "POST" + " /" + username + " HTTP/1.1" + "\r\n" + "Host: " + ipAddress + "\r\n" + "Content-Length: " + location.Length + "\r\n" + optionalHeaderLines + "\r\n" + location;

                            //This breaks out of the case
                            break;

                        #endregion
                    }

                    #endregion
                }
            }

            try
            {
                #region Client

                //This initializes a new Tcp client to handle the requests to the server
                TcpClient client = new TcpClient();

                client.Connect(ipAddress, port);

                //These are the variables used to pass data too and from the server
                StreamWriter streamWriter = new StreamWriter(client.GetStream());
                StreamReader streamReader = new StreamReader(client.GetStream());

                //This ensures that if the server hangs for too long the client doesn't wait for a response
                client.SendTimeout = 1000;

                //This sends the queries to the database
                streamWriter.WriteLine(toServerInput);
                streamWriter.Flush();

                //This ensures that if the server hangs for too long the client doesn't wait for a response
                client.ReceiveTimeout = 1000;

                #endregion

                #region ToScreenOutput

                //This will skip the response code if the response from the server is not needed
                if (serverOutput)
                {
                    //This continues while there are lines to read
                    while ((client.Connected) && (!streamReader.EndOfStream) && (streamReader.Peek() != -1))
                    {
                        //This reads the next line from the server
                        string nextLine = streamReader.ReadLine();

                        //This adds it to the variable to be printed to the screen
                        toScreenOutput = toScreenOutput + nextLine + "\r\n";

                        //This is used to ensure a stack overflow is not caused and to help with HTTP 1.1 requests
                        if (nextLine == null || nextLine == "</html>")
                        {
                            break;
                        }
                    }

                    #region Split

                    //This is used to remove OK from the end of whois returns
                    string[] toScreenOutputSectionsWhoIs = Regex.Split(toScreenOutput, "\r\nOK");

                    if ((toScreenOutputSectionsWhoIs.Length == 2))
                    {
                        toScreenOutput = toScreenOutputSectionsWhoIs[0] + "\r\n";
                    }

                    //This is used to remove none essential lines from HTTP responses
                    string[] toScreenOutputSectionsHTML = Regex.Split(toScreenOutput, "<!DOCTYPE HTML");

                    if ((toScreenOutputSectionsHTML.Length == 2))
                    {
                        toScreenOutput = username + " is <!DOCTYPE HTML" + toScreenOutputSectionsHTML[1];
                    }

                    //This is used to remove none essential lines from HTTP responses
                    string[] toScreenOutputSectionsLocation1 = Regex.Split(toScreenOutput, "text/plain\r\n");

                    if ((toScreenOutputSectionsLocation1.Length == 2))
                    {
                        toScreenOutput = username + toScreenOutputSectionsLocation1[1];
                    }

                    //This is used to remove none essential lines
                    if (username != string.Empty)
                    {
                        string[] toScreenOutputSectionsUsername = Regex.Split(toScreenOutput, username + "\r\n");

                        if ((toScreenOutputSectionsUsername.Length == 2))
                        {
                            toScreenOutput = username + " is " + toScreenOutputSectionsUsername[1];
                        }
                    }

                    //This is used to remove none essential lines
                    string[] toScreenOutputSectionsLocation2 = Regex.Split(toScreenOutput, " is \r\n");

                    if ((toScreenOutputSectionsLocation2.Length == 2))
                    {
                        toScreenOutput = toScreenOutputSectionsLocation2[0] + " is " + toScreenOutputSectionsLocation2[1];
                    }

                    //This is used to remove none essential lines
                    string[] toScreenOutputSectionsRN = Regex.Split(toScreenOutput, "\r\n");

                    if ((toScreenOutputSectionsRN.Length == 1))
                    {
                        toScreenOutput = toScreenOutput + "\r\n";
                    }

                    #endregion
                }
                else
                {
                    //This ensures the client doesn't crash when the server is trying to force a response that is unneeded
                    while ((client.Connected) && (!streamReader.EndOfStream) && (streamReader.Peek() != -1))
                    {
                        streamReader.ReadLine();
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                //This prints to the screen an error message
                toScreenOutput = "ERROR: " + ex.ToString();
            }

            //This prints the server response to the screen
            Console.WriteLine(toScreenOutput);

            return toScreenOutput;
        }
    }
}