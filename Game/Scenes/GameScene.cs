using System;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Game
{
    class GameScene : Scene, IScene
    {
        Matrix4 projectionMatrix;

        PlayerPaddle player1Paddle;
        PlayerPaddle player2Paddle;
        AIPaddle aiPaddle;
        Ball ball;

        Bitmap textBMP;
        Graphics textGFX;
        int textTexture;

        bool[] PlayerMoveUpArray = new bool[2] {false, false};
        bool[] PlayerMoveDownArray = new bool[2] {false, false};
        bool[] PlayerMoveRightArray = new bool[2] { false, false };
        bool[] PlayerMoveLeftArray = new bool[2] { false, false };

        public string SceneTypeCheck()
        {
            return "GameScene";
        }

        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            // Set the SceneTypeCheck, Update and Render delegates to the SceneTypeCheck, Update and Render methods of this class
            sceneManager.sceneTypeChecker = SceneTypeCheck;
            sceneManager.updater = Update;
            sceneManager.renderer = Render;

            // Set Keyboard events to go to a method in this class
            sceneManager.Keyboard.KeyDown += Keyboard_KeyDown;
            sceneManager.Keyboard.KeyUp += Keyboard_KeyUp;

            ResetGame();

            // Create Bitmap and OpenGL texture for rendering text
            textBMP = new Bitmap(sceneManager.Width, sceneManager.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb); // match window size
            textGFX = Graphics.FromImage(textBMP);
            textGFX.Clear(SceneManager.backgroundColour);
            textTexture = GL.GenTexture();

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, textTexture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, textBMP.Width, textBMP.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);
        }

        public void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:

                    if (SceneManager.clientNetworked)
                    {
                        if (SceneManager.networkedPlayerNumber == 0)
                        {
                            PlayerMoveUpArray[0] = true;
                        }
                    }
                    else
                    {
                        PlayerMoveUpArray[0] = true;
                    }

                    break;

                case Key.D:

                    if (SceneManager.clientNetworked)
                    {
                        if (SceneManager.networkedPlayerNumber == 0)
                        {
                            PlayerMoveRightArray[0] = true;
                        }
                    }
                    else
                    {
                        PlayerMoveRightArray[0] = true;
                    }

                    break;

                case Key.S:

                    if (SceneManager.clientNetworked)
                    {
                        if (SceneManager.networkedPlayerNumber == 0)
                        {
                            PlayerMoveDownArray[0] = true;
                        }
                    }
                    else
                    {
                        PlayerMoveDownArray[0] = true;
                    }

                    break;

                case Key.A:

                    if (SceneManager.clientNetworked)
                    {
                        if (SceneManager.networkedPlayerNumber == 0)
                        {
                            PlayerMoveLeftArray[0] = true;
                        }
                    }
                    else
                    {
                        PlayerMoveLeftArray[0] = true;
                    }

                    break;

                case Key.Up:

                    if (SceneManager.clientNetworked)
                    {
                        if (SceneManager.networkedPlayerNumber == 1)
                        {
                            PlayerMoveUpArray[1] = true;
                        }
                    }
                    else
                    {
                        PlayerMoveUpArray[1] = true;
                    }

                    break;

                case Key.Right:

                    if (SceneManager.clientNetworked)
                    {
                        if (SceneManager.networkedPlayerNumber == 1)
                        {
                            PlayerMoveRightArray[1] = true;
                        }
                    }
                    else
                    {
                        PlayerMoveRightArray[1] = true;
                    }

                    break;

                case Key.Down:

                    if (SceneManager.clientNetworked)
                    {
                        if (SceneManager.networkedPlayerNumber == 1)
                        {
                            PlayerMoveDownArray[1] = true;
                        }
                    }
                    else
                    {
                        PlayerMoveDownArray[1] = true;
                    }

                    break;

                case Key.Left:

                    if (SceneManager.clientNetworked)
                    {
                        if (SceneManager.networkedPlayerNumber == 1)
                        {
                            PlayerMoveLeftArray[1] = true;
                        }
                    }
                    else
                    {
                        PlayerMoveLeftArray[1] = true;
                    }

                    break;
            }
        }

        public void Keyboard_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:

                    if (SceneManager.clientNetworked)
                    {
                        if (SceneManager.networkedPlayerNumber == 0)
                        {
                            PlayerMoveUpArray[0] = false;
                        }
                    }
                    else
                    {
                        PlayerMoveUpArray[0] = false;
                    }

                    break;

                case Key.D:

                    if (SceneManager.clientNetworked)
                    {
                        if (SceneManager.networkedPlayerNumber == 0)
                        {
                            PlayerMoveRightArray[0] = false;
                        }
                    }
                    else
                    {
                        PlayerMoveRightArray[0] = false;
                    }

                    break;

                case Key.S:

                    if (SceneManager.clientNetworked)
                    {
                        if (SceneManager.networkedPlayerNumber == 0)
                        {
                            PlayerMoveDownArray[0] = false;
                        }
                    }
                    else
                    {
                        PlayerMoveDownArray[0] = false;
                    }

                    break;

                case Key.A:

                    if (SceneManager.clientNetworked)
                    {
                        if (SceneManager.networkedPlayerNumber == 0)
                        {
                            PlayerMoveLeftArray[0] = false;
                        }
                    }
                    else
                    {
                        PlayerMoveLeftArray[0] = false;
                    }

                    break;

                case Key.Up:

                    if (SceneManager.clientNetworked)
                    {
                        if (SceneManager.networkedPlayerNumber == 1)
                        {
                            PlayerMoveUpArray[1] = false;
                        }
                    }
                    else
                    {
                        PlayerMoveUpArray[1] = false;
                    }

                    break;

                case Key.Right:

                    if (SceneManager.clientNetworked)
                    {
                        if (SceneManager.networkedPlayerNumber == 1)
                        {
                            PlayerMoveRightArray[1] = false;
                        }
                    }
                    else
                    {
                        PlayerMoveRightArray[1] = false;
                    }

                    break;

                case Key.Down:

                    if (SceneManager.clientNetworked)
                    {
                        if (SceneManager.networkedPlayerNumber == 1)
                        {
                            PlayerMoveDownArray[1] = false;
                        }
                    }
                    else
                    {
                        PlayerMoveDownArray[1] = false;
                    }

                    break;

                case Key.Left:

                    if (SceneManager.clientNetworked)
                    {
                        if (SceneManager.networkedPlayerNumber == 1)
                        {
                            PlayerMoveLeftArray[1] = false;
                        }
                    }
                    else
                    {
                        PlayerMoveLeftArray[1] = false;
                    }

                    break;
            }
        }

        private void ResetGame()
        {
            player1Paddle = new PlayerPaddle((SceneManager.WindowWidth / 2) - ((SceneManager.originalWidth / 2) - 50), (int)(SceneManager.WindowHeight / 2));
            player1Paddle.Init();

            if (SceneManager.ai)
            {
                aiPaddle = new AIPaddle((SceneManager.WindowWidth / 2) + ((SceneManager.originalWidth / 2) - 50), (int)(SceneManager.WindowHeight / 2));
                aiPaddle.Init();
            }
            else
            {
                player2Paddle = new PlayerPaddle((SceneManager.WindowWidth / 2) + ((SceneManager.originalWidth / 2) - 50), (int)(SceneManager.WindowHeight / 2));
                player2Paddle.Init();
            }

            ball = new Ball((int)(SceneManager.WindowWidth / 2), (int)(SceneManager.WindowHeight / 2));
            ball.Init();
        }

        private void CollisionDetection()
        {
            // Player1
            if ((ball.Position.X > (player1Paddle.Position.X - 15.0f) && ball.Position.X < (player1Paddle.Position.X + 15.0f)) && (ball.Position.Y > (player1Paddle.Position.Y - 35.0f) && ball.Position.Y < (player1Paddle.Position.Y + 35.0f)))
            {
                
                ball.Position = new Vector2((player1Paddle.Position.X + ball.Radius) + 5, ball.Position.Y);

                if (ball.Velocity.X <= 0)
                {
                    ball.Velocity = new Vector2(ball.Velocity.X * -1.0f, ball.Velocity.Y) * 1.25f;
                }
            }

            if (SceneManager.ai)
            {
                // AI
                if ((aiPaddle.Position.X - ball.Position.X) < ball.Radius && (ball.Position.Y > (aiPaddle.Position.Y - 35.0f) && ball.Position.Y < (aiPaddle.Position.Y + 35.0f)))
                {
                    ball.Position = new Vector2(aiPaddle.Position.X - ball.Radius, ball.Position.Y);
                    ball.Velocity = new Vector2(ball.Velocity.X * -1.0f, ball.Velocity.Y) * 1.25f;
                }
            }
            else
            {
                //Player2
                if ((ball.Position.X > (player2Paddle.Position.X - 15.0f) && ball.Position.X < (player2Paddle.Position.X + 15.0f)) && (ball.Position.Y > (player2Paddle.Position.Y - 35.0f) && ball.Position.Y < (player2Paddle.Position.Y + 35.0f)))
                {

                    ball.Position = new Vector2((player2Paddle.Position.X - ball.Radius) - 5, ball.Position.Y);

                    if (ball.Velocity.X >= 0)
                    {
                        ball.Velocity = new Vector2(ball.Velocity.X * -1.0f, ball.Velocity.Y) * 1.25f;
                    }
                }
            }
        }

        private bool GoalDetection()
        {
            if (ball.Position.X > (SceneManager.WindowWidth / 2) + (SceneManager.originalWidth / 2))
            {
                if (!SceneManager.clientNetworked)
                {
                    SceneManager.playerScoreArray[0]++;
                }

                return true;
            }
            else if (ball.Position.X < (SceneManager.WindowWidth / 2) - (SceneManager.originalWidth / 2))
            {
                if (!SceneManager.clientNetworked)
                {
                    SceneManager.playerScoreArray[1]++;
                }

                return true;
            }

            return false;
        }

        public void Update(FrameEventArgs e)
        {
            // Set the title of the window
            sceneManager.Title = "Pong - Player 1 Score: " + SceneManager.playerScoreArray[0] + " - Player 2 Score: " + SceneManager.playerScoreArray[1] + " - Time remaining: " + (30 - SceneManager.t);

            if (PlayerMoveUpArray[0] == true)
            {
                player1Paddle.MoveY(8);
            }

            if (PlayerMoveRightArray[0] == true)
            {
                player1Paddle.MoveX(8);
            }

            if (PlayerMoveDownArray[0] == true)
            {
                player1Paddle.MoveY(- 8);
            }

            if (PlayerMoveLeftArray[0] == true)
            {
                player1Paddle.MoveX(- 8);
            }

            if (SceneManager.ai)
            {
                aiPaddle.Move(ball.Position);
                aiPaddle.Update((float)e.Time);
            }
            else
            {
                if (PlayerMoveUpArray[1] == true)
                {
                    player2Paddle.MoveY(8);
                }

                if (PlayerMoveRightArray[1] == true)
                {
                    player2Paddle.MoveX(8);
                }

                if (PlayerMoveDownArray[1] == true)
                {
                    player2Paddle.MoveY(- 8);
                }

                if (PlayerMoveLeftArray[1] == true)
                {
                    player2Paddle.MoveX(- 8);
                }
            }

            if (!SceneManager.clientNetworked)
            {
                try
                {
                    ball.Update((float)e.Time);

                    string playerPositionString;
                    string[] playerPositionStringArray;

                    if (SceneManager.serverNetworked)
                    {
                        playerPositionString = SceneManager.client.Input(new string[] { "-h", SceneManager.ipAddress, "player1Client" });

                        if (playerPositionString != "player1Client is ERROR: no entries found\r\n\r\n")
                        {
                            playerPositionStringArray = Regex.Split(playerPositionString, " is ")[1].Split(',');

                            player1Paddle.Position = new Vector2(int.Parse(playerPositionStringArray[0]), int.Parse(playerPositionStringArray[1]));
                        }

                        playerPositionString = SceneManager.client.Input(new string[] { "-h", SceneManager.ipAddress, "player2Client" });

                        if (playerPositionString != "player2Client is ERROR: no entries found\r\n\r\n")
                        {
                            playerPositionStringArray = Regex.Split(playerPositionString, " is ")[1].Split(',');

                            player2Paddle.Position = new Vector2(int.Parse(playerPositionStringArray[0]), int.Parse(playerPositionStringArray[1]));
                        }
                    }

                    CollisionDetection();

                    if (SceneManager.serverNetworked)
                    {
                        string position = player1Paddle.Position.X.ToString() + "," + player1Paddle.Position.Y.ToString() + "," + player2Paddle.Position.X.ToString() + "," + player2Paddle.Position.Y.ToString() + "," + ball.Position.X.ToString() + "," + ball.Position.Y.ToString() + "," + SceneManager.playerScoreArray[0] + "," + SceneManager.playerScoreArray[1];

                        SceneManager.client.Input(new string[] { "-h", SceneManager.ipAddress, "playerServer", position });
                    }
                }
                catch
                {

                }
            }
            else
            {
                try
                {
                    string position;
                    string positionString;
                    string[] positionStringArray;

                    if (SceneManager.networkedPlayerNumber == 0)
                    {
                        position = player1Paddle.Position.X.ToString() + "," + player1Paddle.Position.Y.ToString();

                        SceneManager.client.Input(new string[] { "-h", SceneManager.ipAddress, "player1Client", position });
                        positionString = SceneManager.client.Input(new string[] { "-h", SceneManager.ipAddress, "playerServer" });

                        if (positionString != "playerServer is ERROR: no entries found\r\n\r\n")
                        {
                            positionStringArray = Regex.Split(positionString, " is ")[1].Split(',');

                            player2Paddle.Position = new Vector2(int.Parse(positionStringArray[2]), int.Parse(positionStringArray[3]));
                            ball.Position = new Vector2(float.Parse(positionStringArray[4]), float.Parse(positionStringArray[5]));
                            SceneManager.playerScoreArray[0] = int.Parse(positionStringArray[6]);
                            SceneManager.playerScoreArray[1] = int.Parse(positionStringArray[7]);
                        }
                    }
                    else
                    {
                        position = player2Paddle.Position.X.ToString() + "," + player2Paddle.Position.Y.ToString();

                        SceneManager.client.Input(new string[] { "-h", SceneManager.ipAddress, "player2Client", position });
                        positionString = SceneManager.client.Input(new string[] { "-h", SceneManager.ipAddress, "playerServer" });

                        if (positionString != "playerServer is ERROR: no entries found\r\n\r\n")
                        {
                            positionStringArray = Regex.Split(positionString, " is ")[1].Split(',');

                            player1Paddle.Position = new Vector2(int.Parse(positionStringArray[0]), int.Parse(positionStringArray[1]));
                            ball.Position = new Vector2(float.Parse(positionStringArray[4]), float.Parse(positionStringArray[5]));
                            SceneManager.playerScoreArray[0] = int.Parse(positionStringArray[6]);
                            SceneManager.playerScoreArray[1] = int.Parse(positionStringArray[7]);
                        }
                    }
                }
                catch
                {

                }
            }

            if (GoalDetection())
            {
                ResetGame();
            }
        }

        private void RenderText(string text, Brush textColour, float x, float y)
        {
            Font textFont = new Font(SceneManager.textFont, SceneManager.textFontSize);

            textGFX.DrawString(text, textFont, textColour, x, y);

            // Enable the texture
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, textTexture);

            BitmapData data = textBMP.LockBits(new Rectangle(0, 0, textBMP.Width, textBMP.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, (int)textBMP.Width, (int)textBMP.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            textBMP.UnlockBits(data);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0f, 1f); GL.Vertex2(0f, 0f);
            GL.TexCoord2(1f, 1f); GL.Vertex2(sceneManager.Width, 0f);
            GL.TexCoord2(1f, 0f); GL.Vertex2(sceneManager.Width, sceneManager.Height);
            GL.TexCoord2(0f, 0f); GL.Vertex2(0f, sceneManager.Height);
            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);
        }

        public void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, sceneManager.Width, 0, sceneManager.Height, -1.0f, +1.0f);

            ball.Render(projectionMatrix);

            player1Paddle.Render(projectionMatrix);

            if (SceneManager.ai)
            {
                aiPaddle.Render(projectionMatrix);
            }
            else
            {
                player2Paddle.Render(projectionMatrix);
            }

            if (textBMP != null)
            {
                if (SceneManager.clientNetworked)
                {
                    if (SceneManager.networkedPlayerNumber == 0)
                    {
                        // Display "Player 1" at (0, - (sceneManager.Height / 24))
                        RenderText("Player 1", SceneManager.textColourArray[4], 10, 10);
                    }
                    else
                    {
                        // Display "Player 1" at (0, - (sceneManager.Height / 24))
                        RenderText("Player 2", SceneManager.textColourArray[4], 10, 10);
                    }
                }

            }
        }
    }
}