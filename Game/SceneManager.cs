using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Game
{
    class SceneManager : GameWindow
    {
        public static Client client = new Client();

        public static string ipAddress = "localhost";
        public static string tempIPAddress;

        Scene scene;

        public delegate string SceneDelegate();
        public SceneDelegate sceneTypeChecker;

        public delegate void FrameSceneDelegate(FrameEventArgs e);
        public FrameSceneDelegate updater, renderer;

        System.Timers.Timer timer;
        public static int t;
        bool timeOut;

        public static DateTime[] dateTimeArray = new DateTime[4];

        public static int originalWidth = 1024;
        public static int originalHeight = 576;
        static int width;
        static int height;

        public static Color backgroundColour;

        public static Brush[] textColourArray = new Brush[5];

        public static string textFont = "Arial";
        public static int textFontSize = 20;

        public static string[,] highscoreArray = new string[5, 2];

        public static string[] playerNameArray = new string[2];
        public static int[] playerScoreArray = new int[2];

        public static bool ai, listening, clientNetworked, serverNetworked, playerNameBool, eventUpdate, update, updateComplete;

        public static int networkedPlayerNumber;

        public SceneManager()
        {
            Keyboard.KeyDown += Keyboard_KeyDown;
            Mouse.ButtonDown += Mouse_ButtonDown;
        }

        public void KickTheDog()
        {
            try
            {
                client.Input(new string[] { "-h", "localhost", "dateTime", (DateTime.Now + TimeSpan.FromMilliseconds(10000)).ToString() });
            }
            catch
            {

            }
        }

        private void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButton.Left:

                    switch (sceneTypeChecker())
                    {
                        case "MainMenuScene":

                            var mouse = Mouse;
                            int x = mouse.X;
                            int y = mouse.Y;

                            Font menuItemFont = new Font(textFont, textFontSize);

                            if (!listening)
                            {
                                if ((x >= ((WindowWidth / 2) - (MainMenuScene.textGFX.MeasureString(MainMenuScene.menuItemArray[0], menuItemFont).Width / 2))) && (x <= ((WindowWidth / 2) + (MainMenuScene.textGFX.MeasureString(MainMenuScene.menuItemArray[0], menuItemFont).Width / 2))) && (y >= ((-(WindowHeight / 8)) + (WindowHeight / 2))) && (y <= ((-(WindowHeight / 8)) + ((WindowHeight / 2) + (textFontSize)))))
                                {
                                    StartDisplayHighscoresScene();
                                }

                                if ((x >= ((WindowWidth / 2) - (MainMenuScene.textGFX.MeasureString(MainMenuScene.menuItemArray[1], menuItemFont).Width / 2))) && (x <= ((WindowWidth / 2) + (MainMenuScene.textGFX.MeasureString(MainMenuScene.menuItemArray[1], menuItemFont).Width / 2))) && (y >= ((-(WindowHeight / 24)) + (WindowHeight / 2))) && (y <= ((-(WindowHeight / 24)) + ((WindowHeight / 2) + (textFontSize)))))
                                {
                                    ai = true;
                                    clientNetworked = false;
                                    serverNetworked = false;

                                    StartGameScene();
                                }

                                if ((x >= ((WindowWidth / 2) - (MainMenuScene.textGFX.MeasureString(MainMenuScene.menuItemArray[2], menuItemFont).Width / 2))) && (x <= ((WindowWidth / 2) + (MainMenuScene.textGFX.MeasureString(MainMenuScene.menuItemArray[2], menuItemFont).Width / 2))) && (y >= ((WindowHeight / 24) + (WindowHeight / 2))) && (y <= ((WindowHeight / 24) + ((WindowHeight / 2) + (textFontSize)))))
                                {
                                    ai = false;
                                    clientNetworked = false;
                                    serverNetworked = false;

                                    StartGameScene();
                                }

                                if ((x >= ((WindowWidth / 2) - (MainMenuScene.textGFX.MeasureString(MainMenuScene.menuItemArray[3], menuItemFont).Width / 2))) && (x <= ((WindowWidth / 2) + (MainMenuScene.textGFX.MeasureString(MainMenuScene.menuItemArray[3], menuItemFont).Width / 2))) && (y >= ((WindowHeight / 8) + (WindowHeight / 2))) && (y <= ((WindowHeight / 8) + ((WindowHeight / 2) + (textFontSize)))))
                                {
                                    string clientOutput = client.Input(new string[] { "-h", ipAddress, "players" });

                                    if (clientOutput == "players is false,false\r\n\r\n")
                                    {
                                        client.Input(new string[] { "-h", ipAddress, "players", "true,false" });

                                        networkedPlayerNumber = 0;
                                    }
                                    else
                                    {
                                        if (clientOutput == "players is true,false\r\n\r\n")
                                        {
                                            client.Input(new string[] { "-h", ipAddress, "players", "true,true" });

                                            networkedPlayerNumber = 1;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Game already in progress, please wait.");

                                            networkedPlayerNumber = 2;
                                        }
                                    }

                                    if (networkedPlayerNumber != 2)
                                    {
                                        dateTimeArray[2] = DateTime.Now + TimeSpan.FromMilliseconds(3000);

                                        while (client.Input(new string[] { "-h", ipAddress, "players" }) == "players is true,false\r\n\r\n" && DateTime.Now <= dateTimeArray[2])
                                        {
                                            System.Threading.Thread.Sleep(1000);

                                            Console.WriteLine("Waiting for opponent");

                                            KickTheDog();
                                        }

                                        if (client.Input(new string[] { "-h", ipAddress, "players" }) == "players is true,false\r\n\r\n" || DateTime.Now >= dateTimeArray[2])
                                        {
                                            client.Input(new string[] { "-h", ipAddress, "players", "false,false" });

                                            Console.WriteLine("No opponent available");
                                        }
                                        else
                                        {
                                            ai = false;
                                            clientNetworked = true;
                                            serverNetworked = false;

                                            StartGameScene();
                                        }
                                    }
                                }
                            }

                            break;

                        case "DisplayHighscores":

                            break;

                        case "GameScene":

                            break;

                        case "GameOverScene":

                            break;
                    }

                    break;

                case MouseButton.Right:

                    switch (sceneTypeChecker())
                    {
                        case "MainMenuScene":

                            break;

                        case "DisplayHighscores":

                            InitialiseMainMenuScene();

                            break;

                        case "GameScene":

                            break;

                        case "GameOverScene":

                            InitialiseMainMenuScene();

                            break;
                    }

                    break;
            }
        }

        private void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:

                    ExitGame();

                    break;

                case Key.F:

                    if (sceneTypeChecker() != "GameScene" || sceneTypeChecker() != "GameOverScene")
                    {
                        Fullscreen();
                    }

                    break;

                case Key.N:

                    if (sceneTypeChecker() != "GameOverScene")
                    {
                        Normal();
                    }

                    break;

                case Key.L:

                    if (sceneTypeChecker() == "MainMenuScene")
                    {
                        ipAddress = "localhost";
                        tempIPAddress = ipAddress;
                    }

                    break;

                case Key.S:

                    if (sceneTypeChecker() == "MainMenuScene")
                    {
                        if (listening)
                        {
                            listening = false;

                            for (int i = 0; i < textColourArray.Length; i++)
                            {
                                textColourArray[i] = Brushes.White;
                            }
                        }
                        else
                        {
                            listening = true;

                            for (int i = 0; i < textColourArray.Length; i++)
                            {
                                textColourArray[i] = Brushes.CornflowerBlue;
                            }
                        }
                    }

                    break;
            }
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
            {
                return "zero";
            }

            if (number < 0)
            {
                return "minus " + NumberToWords(Math.Abs(number));
            }

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                {
                    words += "and ";
                }

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                {
                    words += unitsMap[number];
                }
                else
                {
                    words += tensMap[number / 10];

                    if ((number % 10) > 0)
                    {
                        words += "-" + unitsMap[number % 10];
                    }
                }
            }

            return words;
        }

        public static int WindowWidth
        {
            get { return width; }
        }

        public static int WindowHeight
        {
            get { return height; }
        }

        private void ExitGame()
        {
            Exit();
        }

        private void Fullscreen()
        {
            WindowState = WindowState.Fullscreen;
        }

        private void Normal()
        {
            WindowState = WindowState.Normal;
        }

        private void Fixed()
        {
            WindowBorder = WindowBorder.Fixed;
        }

        private void Resizable()
        {
            WindowBorder = WindowBorder.Resizable;
        }

        private void InitialiseMainMenuScene()
        {
            backgroundColour = Color.CornflowerBlue;

            eventUpdate = true;
            update = false;
            updateComplete = false;

            if (!listening)
            {
                for (int i = 0; i < textColourArray.Length; i++)
                {
                    textColourArray[i] = Brushes.White;
                }

                listening = false;
            }
            else
            {
                for (int i = 0; i < textColourArray.Length; i++)
                {
                    textColourArray[i] = Brushes.CornflowerBlue;
                }
            }

            if (WindowBorder != WindowBorder.Resizable)
            {
                Resizable();
            }

            tempIPAddress = ipAddress;

            for (int i = 0; i < (dateTimeArray.Length - 1); i++)
            {
                dateTimeArray[i] = DateTime.MinValue;
            }

            Resizable();

            StartMainMenuScene();
        }

        private void StartMainMenuScene()
        {
            scene = new MainMenuScene(this);
        }

        private void UpdateHighScores()
        {
            if (clientNetworked)
            {
                if (networkedPlayerNumber == 0)
                {
                    playerNameArray[1] = "N/A";
                    playerScoreArray[1] = -1;
                }
                else
                {
                    playerNameArray[0] = "N/A";
                    playerScoreArray[0] = -1;
                }
            }

            int length = highscoreArray.Length / 2;

            string[] test = new string[2];

            if (ipAddress != "localhost")
            {
                string highscoreString = client.Input(new string[] { "-h", ipAddress, "highscore" });

                if (highscoreString != "highscore is ERROR: no entries found\r\n\r\n")
                {
                    string[] highscoreStringArray = Regex.Split(highscoreString, " is ")[1].Split(',');

                    int i = 0;

                    string[,] tempHighscoreArray = new string[5, 2];

                    for (int j = 0; j < length; j++)
                    {
                        tempHighscoreArray[j, 0] = highscoreStringArray[i++];
                        tempHighscoreArray[j, 1] = highscoreStringArray[i++];
                    }

                    for (int k = 0; k < tempHighscoreArray.Length / 2; k++)
                    {
                        test[0] = tempHighscoreArray[k, 0];
                        test[1] = tempHighscoreArray[k, 1];

                        if (test[0] == string.Empty)
                        {
                            test[0] = NumberToWords(int.Parse(test[1]));
                        }

                        for (int l = 0; l < length; l++)
                        {
                            if (test[0] == highscoreArray[l, 0] && test[1] == highscoreArray[l, 1])
                            {
                                break;
                            }
                            else
                            {
                                if (int.Parse(test[1]) > int.Parse(highscoreArray[l, 1]))
                                {
                                    string[] temp = new string[2];

                                    temp[0] = highscoreArray[l, 0];
                                    temp[1] = highscoreArray[l, 1];

                                    highscoreArray[l, 0] = test[0];
                                    highscoreArray[l, 1] = test[1];

                                    test[0] = temp[0];
                                    test[1] = temp[1];
                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < playerScoreArray.Length; i++)
            {
                test[0] = playerNameArray[i];
                test[1] = playerScoreArray[i].ToString();

                if (test[0] == string.Empty)
                {
                    test[0] = NumberToWords(int.Parse(test[1]));
                }

                for (int j = 0; j < length; j++)
                {
                    if (test[0] == highscoreArray[j, 0] && test[1] == highscoreArray[j, 1])
                    {
                        break;
                    }
                    else
                    {
                        if (int.Parse(test[1]) > int.Parse(highscoreArray[j, 1]))
                        {
                            string[] temp = new string[2];

                            temp[0] = highscoreArray[j, 0];
                            temp[1] = highscoreArray[j, 1];

                            highscoreArray[j, 0] = test[0];
                            highscoreArray[j, 1] = test[1];

                            test[0] = temp[0];
                            test[1] = temp[1];
                        }
                    }
                }
            }

            string output = string.Empty;

            for (int i = 0; i < length; i++)
            {
                output = output + highscoreArray[i, 0] + "," + highscoreArray[i, 1] + ",";
            }

            output = output.Remove(output.Length - 1);

            if (ipAddress != "localhost")
            {
                client.Input(new string[] { "-h", ipAddress, "highscore", output });
            }

            client.Input(new string[] { "-h", "localhost", "highscore", output });
        }

        private void StartDisplayHighscoresScene()
        {
            backgroundColour = Color.CornflowerBlue;

            textColourArray[0] = Brushes.White;

            UpdateHighScores();

            scene = new DisplayHighscores(this);
        }

        public void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (t >= 30)
            {
                timeOut = true;

                timer.Stop();
                timer.Close();
            }
            else
            {
                t++;

                timer.Start();
            }
        }

        private void StartGameScene()
        {
            backgroundColour = Color.Black;

            for (int i = 0; i < playerScoreArray.Length; i++)
            {
                playerNameArray[i] = string.Empty;
                playerScoreArray[i] = 0;
            }

            Normal();
            Fixed();

            t = 0;
            timeOut = false;

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(TimerElapsed);
            timer.Start();

            scene = new GameScene(this);
        }

        private void InitialiseGameOverScene()
        {
            backgroundColour = Color.CornflowerBlue;

            textColourArray[0] = Brushes.White;

            for (int i = 0; i < playerNameArray.Length; i++)
            {
                playerNameArray[i] = string.Empty;
            }

            if (ai)
            {
                SceneManager.playerNameArray[1] = "AI";
            }

            if (clientNetworked)
            {
                if (networkedPlayerNumber == 0)
                {
                    playerNameArray[1] = "N/A";
                }
                else
                {
                    playerNameArray[0] = "N/A";
                }
            }

            playerNameBool = true;
            eventUpdate = true;
            update = false;
            updateComplete = false;

            dateTimeArray[3] = DateTime.MinValue;

            Resizable();

            StartGameOverScene();
        }

        private void StartGameOverScene()
        {
            scene = new GameOverScene(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.Enable(EnableCap.DepthTest);

            Width = originalWidth;
            Height = originalHeight;

            width = Width;
            height = Height;

            client.Input(new string[] { "-h", ipAddress, "players", "false,false" });

            string highscoreString = client.Input(new string[] { "-h", ipAddress, "highscore" });

            int length = highscoreArray.Length / 2;

            if (highscoreString == "highscore is ERROR: no entries found\r\n\r\n")
            {
                for (int i = 0; i < length; i++)
                {
                    highscoreArray[length - (i + 1), 0] = NumberToWords(i);
                    highscoreArray[length - (i + 1), 1] = i.ToString();
                }

                string output = string.Empty;

                for (int i = 0; i < length; i++)
                {
                    output = output + highscoreArray[i, 0] + "," + highscoreArray[i, 1] + ",";
                }

                output = output.Remove(output.Length - 1);

                client.Input(new string[] { "-h", ipAddress, "highscore", output });
            }
            else
            {
                string[] highscoreStringArray = Regex.Split(highscoreString, " is ")[1].Split(',');

                int i = 0;

                for (int j = 0; j < length; j++)
                {
                    highscoreArray[j, 0] = highscoreStringArray[i++];
                    highscoreArray[j, 1] = highscoreStringArray[i++];
                }
            }

            Normal();

            InitialiseMainMenuScene();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            SceneManager.width = Width;
            SceneManager.height = Height;

            switch (sceneTypeChecker())
            {
                case "MainMenuScene":

                    scene = new MainMenuScene(this);

                    break;

                case "DisplayHighscores":

                    scene = new DisplayHighscores(this);

                    break;

                case "GameOverScene":

                    scene = new GameOverScene(this);

                    break;

                case "GameScene":

                    break;
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (DateTime.Now >= Game.watchdogDateTime)
            {
                KickTheDog();

                Game.watchdogDateTime = DateTime.Now + TimeSpan.FromMilliseconds(3000);
            }

            switch (sceneTypeChecker())
            {
                case "MainMenuScene":

                    if (listening)
                    {
                        if (DateTime.Now >= dateTimeArray[0])
                        {
                            if (client.Input(new string[] { "-h", ipAddress, "players" }) == "players is true,true\r\n\r\n")
                            {
                                ai = false;
                                clientNetworked = false;
                                serverNetworked = true;

                                StartGameScene();
                            }

                            dateTimeArray[0] = DateTime.Now + TimeSpan.FromMilliseconds(100);
                        }
                    }
                    else
                    {
                        if (update)
                        {
                            update = false;

                            StartMainMenuScene();
                        }

                        if (updateComplete)
                        {
                            updateComplete = false;

                            ipAddress = tempIPAddress;
                        }

                        Font menuItemFont = new Font(textFont, textFontSize);

                        var mouse = Mouse;
                        int x = mouse.X;
                        int y = mouse.Y;

                        if ((x >= ((WindowWidth / 2) - (MainMenuScene.textGFX.MeasureString(MainMenuScene.menuItemArray[0], menuItemFont).Width / 2))) && (x <= ((WindowWidth / 2) + (MainMenuScene.textGFX.MeasureString(MainMenuScene.menuItemArray[0], menuItemFont).Width / 2))) && (y >= ((-(WindowHeight / 8)) + (WindowHeight / 2))) && (y <= ((-(WindowHeight / 8)) + ((WindowHeight / 2) + (textFontSize)))))
                        {
                            textColourArray[0] = Brushes.Yellow;
                        }
                        else
                        {
                            textColourArray[0] = Brushes.White;
                        }

                        if ((x >= ((WindowWidth / 2) - (MainMenuScene.textGFX.MeasureString(MainMenuScene.menuItemArray[1], menuItemFont).Width / 2))) && (x <= ((WindowWidth / 2) + (MainMenuScene.textGFX.MeasureString(MainMenuScene.menuItemArray[1], menuItemFont).Width / 2))) && (y >= ((-(WindowHeight / 24)) + (WindowHeight / 2))) && (y <= ((-(WindowHeight / 24)) + ((WindowHeight / 2) + (textFontSize)))))
                        {
                            textColourArray[1] = Brushes.Yellow;
                        }
                        else
                        {
                            textColourArray[1] = Brushes.White;
                        }

                        if ((x >= ((WindowWidth / 2) - (MainMenuScene.textGFX.MeasureString(MainMenuScene.menuItemArray[2], menuItemFont).Width / 2))) && (x <= ((WindowWidth / 2) + (MainMenuScene.textGFX.MeasureString(MainMenuScene.menuItemArray[2], menuItemFont).Width / 2))) && (y >= ((WindowHeight / 24) + (WindowHeight / 2))) && (y <= ((WindowHeight / 24) + ((WindowHeight / 2) + (textFontSize)))))
                        {
                            textColourArray[2] = Brushes.Yellow;
                        }
                        else
                        {
                            textColourArray[2] = Brushes.White;
                        }

                        if ((x >= ((WindowWidth / 2) - (MainMenuScene.textGFX.MeasureString(MainMenuScene.menuItemArray[3], menuItemFont).Width / 2))) && (x <= ((WindowWidth / 2) + (MainMenuScene.textGFX.MeasureString(MainMenuScene.menuItemArray[3], menuItemFont).Width / 2))) && (y >= ((WindowHeight / 8) + (WindowHeight / 2))) && (y <= ((WindowHeight / 8) + ((WindowHeight / 2) + (textFontSize)))))
                        {
                            textColourArray[3] = Brushes.Yellow;
                        }
                        else
                        {
                            textColourArray[3] = Brushes.White;
                        }
                    }

                    break;

                case "DisplayHighscores":

                    break;

                case "GameScene":

                    if (timeOut)
                    {
                        if (serverNetworked)
                        {
                            client.Input(new string[] { "-h", ipAddress, "players", "false,false" });

                            InitialiseMainMenuScene();
                        }
                        else
                        {
                            InitialiseGameOverScene();
                        }
                    }

                    break;

                case "GameOverScene":

                    if (update)
                    {
                        update = false;

                        StartGameOverScene();
                    }

                    if (updateComplete)
                    {
                        updateComplete = false;

                        StartDisplayHighscoresScene();
                    }

                    break;
            }

            updater(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            renderer(e);

            GL.Flush();
            SwapBuffers();
        }
    }
}