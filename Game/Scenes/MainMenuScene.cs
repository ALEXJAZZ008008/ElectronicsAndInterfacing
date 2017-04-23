using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Game
{
    class MainMenuScene : Scene, IScene
    {
        Bitmap textBMP;
        public static Graphics textGFX;
        int textTexture;

        public static string[] menuItemArray = new string[4] { "Display Highscores", "Start Single Player Game", "Start Local Multiplayer Game", "Start Networked Multiplayer Game" };

        public string SceneTypeCheck()
        {
            return "MainMenuScene";
        }

        public MainMenuScene(SceneManager sceneManager)
            : base(sceneManager)
        {
            // Set the title of the window
            sceneManager.Title = "Pong - Main Menu";

            // Set the SceneTypeCheck, Update and Render delegates to the SceneTypeCheck, Update and Render methods of this class
            sceneManager.sceneTypeChecker = SceneTypeCheck;
            sceneManager.updater = Update;
            sceneManager.renderer = Render;

            // Set Keyboard events to go to a method in this class
            if (SceneManager.eventUpdate)
            {
                SceneManager.eventUpdate = false;

                sceneManager.Keyboard.KeyDown += Keyboard_KeyDown;
            }

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

        private void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (DateTime.Now >= SceneManager.dateTimeArray[1])
            {
                switch (e.Key)
                {
                    case Key.Number0:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "0";

                        break;

                    case Key.Number1:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "1";

                        break;

                    case Key.Number2:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "2";

                        break;

                    case Key.Number3:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "3";

                        break;

                    case Key.Number4:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "4";

                        break;

                    case Key.Number5:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "5";

                        break;

                    case Key.Number6:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "6";

                        break;

                    case Key.Number7:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "7";

                        break;

                    case Key.Number8:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "8";

                        break;

                    case Key.Number9:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "9";

                        break;

                    case Key.Keypad0:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "0";

                        break;

                    case Key.Keypad1:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "1";

                        break;

                    case Key.Keypad2:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "2";

                        break;

                    case Key.Keypad3:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "3";

                        break;

                    case Key.Keypad4:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "4";

                        break;

                    case Key.Keypad5:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "5";

                        break;

                    case Key.Keypad6:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "6";

                        break;

                    case Key.Keypad7:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "7";

                        break;

                    case Key.Keypad8:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "8";

                        break;

                    case Key.Keypad9:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + "9";

                        break;

                    case Key.Period:

                        SceneManager.tempIPAddress = SceneManager.tempIPAddress + ".";

                        break;

                    case Key.BackSpace:

                        if (SceneManager.tempIPAddress != string.Empty)
                        {
                            if (SceneManager.tempIPAddress == "localhost")
                            {
                                SceneManager.tempIPAddress = string.Empty;
                            }
                            else
                            {
                                SceneManager.tempIPAddress = SceneManager.tempIPAddress.Remove(SceneManager.tempIPAddress.Length - 1);
                            }
                        }

                        break;

                    case Key.Enter:

                        SceneManager.updateComplete = true;

                        break;
                }

                SceneManager.dateTimeArray[1] = DateTime.Now + TimeSpan.FromMilliseconds(20);

                SceneManager.update = true;
            }
        }

        public void Update(FrameEventArgs e)
        {
            if (SceneManager.listening)
            {
                sceneManager.Title = "Pong - Server";
            }
            else
            {
                sceneManager.Title = "Pong - Main Menu";
            }
        }

        private void RenderText(string text, Brush textColour, float x, float y)
        {
            y = y + ((sceneManager.Height / 2) - (SceneManager.textFontSize / 2));

            Font menuItemFont = new Font(SceneManager.textFont, SceneManager.textFontSize);

            x = x + ((sceneManager.Width / 2) - (textGFX.MeasureString(text, menuItemFont).Width / 2));

            textGFX.DrawString(text, menuItemFont, textColour, x, y);

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

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, sceneManager.Width, 0, sceneManager.Height, -1, 1);

            if (textBMP != null)
            {
                RenderText(menuItemArray[0], SceneManager.textColourArray[0], 0, -(sceneManager.Height / 8));

                RenderText(menuItemArray[1], SceneManager.textColourArray[1], 0, -(sceneManager.Height / 24));

                RenderText(menuItemArray[2], SceneManager.textColourArray[2], 0, (sceneManager.Height / 24));

                RenderText(menuItemArray[3], SceneManager.textColourArray[3], 0, (sceneManager.Height / 8));

                RenderText(SceneManager.tempIPAddress, SceneManager.textColourArray[4], 0, (sceneManager.Height / 4));
            }
        }
    }
}