using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Game
{
    class GameOverScene : Scene, IScene
    {
        Bitmap textBMP;
        Graphics textGFX;
        int textTexture;

        public string SceneTypeCheck()
        {
            return "GameOverScene";
        }

        public GameOverScene(SceneManager sceneManager) : base(sceneManager)
        {
            // Set the title of the window
            sceneManager.Title = "Pong - Game Over";

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
            if (SceneManager.playerNameArray[0] == "N/A")
            {
                SceneManager.playerNameBool = false;
            }

            if (DateTime.Now >= SceneManager.dateTimeArray[3])
            {
                switch (e.Key)
                {
                    case Key.A:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "A";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "a";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "A";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "a";
                            }
                        }

                        break;

                    case Key.B:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "B";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "b";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "B";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "b";
                            }
                        }

                        break;

                    case Key.C:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "C";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "c";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "C";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "c";
                            }
                        }

                        break;

                    case Key.D:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "D";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "d";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "D";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "d";
                            }
                        }

                        break;

                    case Key.E:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "E";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "e";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "E";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "e";
                            }
                        }

                        break;

                    case Key.F:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "F";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "f";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "F";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "f";
                            }
                        }

                        break;

                    case Key.G:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "G";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "g";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "G";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "g";
                            }
                        }

                        break;

                    case Key.H:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "H";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "h";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "H";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "h";
                            }
                        }

                        break;

                    case Key.I:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "I";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "i";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "I";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "i";
                            }
                        }

                        break;

                    case Key.J:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "J";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "j";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "J";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "j";
                            }
                        }

                        break;

                    case Key.K:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "K";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "k";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "K";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "k";
                            }
                        }

                        break;

                    case Key.L:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "L";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "l";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "L";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "l";
                            }
                        }

                        break;

                    case Key.M:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "M";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "m";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "M";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "m";
                            }
                        }

                        break;

                    case Key.N:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "N";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "n";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "N";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "n";
                            }
                        }

                        break;

                    case Key.O:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "O";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "o";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "O";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "o";
                            }
                        }

                        break;

                    case Key.P:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "P";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "p";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "P";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "p";
                            }
                        }

                        break;

                    case Key.Q:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "Q";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "q";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "Q";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "q";
                            }
                        }

                        break;

                    case Key.R:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "R";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "r";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "R";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "r";
                            }
                        }

                        break;

                    case Key.S:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "S";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "s";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "S";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "s";
                            }
                        }

                        break;

                    case Key.T:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "T";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "t";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "T";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "t";
                            }
                        }

                        break;

                    case Key.U:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "U";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "u";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "U";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "u";
                            }
                        }

                        break;

                    case Key.V:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "V";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "v";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "V";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "v";
                            }
                        }

                        break;

                    case Key.W:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "W";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "w";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "W";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "w";
                            }
                        }

                        break;

                    case Key.X:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "X";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "x";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "X";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "x";
                            }
                        }

                        break;

                    case Key.Y:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "Y";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "y";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "Y";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "y";
                            }
                        }

                        break;

                    case Key.Z:

                        if (SceneManager.playerNameBool)
                        {
                            if (SceneManager.playerNameArray[0] == string.Empty || SceneManager.playerNameArray[0].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "Z";
                            }
                            else
                            {
                                SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + "z";
                            }
                        }
                        else
                        {
                            if (SceneManager.playerNameArray[1] == string.Empty || SceneManager.playerNameArray[1].EndsWith(" "))
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "Z";
                            }
                            else
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + "z";
                            }
                        }

                        break;

                    case Key.Space:

                        if (SceneManager.playerNameBool)
                        {
                            SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0] + " ";
                        }
                        else
                        {
                            SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1] + " ";
                        }

                        break;

                    case Key.BackSpace:

                        if (SceneManager.playerNameBool && SceneManager.playerNameArray[0] != string.Empty)
                        {
                            SceneManager.playerNameArray[0] = SceneManager.playerNameArray[0].Remove(SceneManager.playerNameArray[0].Length - 1);
                        }
                        else
                        {
                            if (!SceneManager.playerNameBool && SceneManager.playerNameArray[1] != string.Empty)
                            {
                                SceneManager.playerNameArray[1] = SceneManager.playerNameArray[1].Remove(SceneManager.playerNameArray[1].Length - 1);
                            }
                        }

                        break;

                    case Key.Enter:

                        if (SceneManager.playerNameBool)
                        {
                            SceneManager.playerNameBool = false;
                            SceneManager.updateComplete = false;

                            if (SceneManager.playerNameArray[1] == "AI")
                            {
                                SceneManager.updateComplete = true;
                            }

                            if (SceneManager.playerNameArray[1] == "N/A")
                            {
                                SceneManager.updateComplete = true;
                            }
                        }
                        else
                        {
                            SceneManager.updateComplete = true;
                        }

                        break;
                }

                SceneManager.dateTimeArray[3] = DateTime.Now + TimeSpan.FromMilliseconds(100);

                SceneManager.update = true;
            }
        }

        public void Update(FrameEventArgs e)
        {
            
        }

        private void RenderText(string text, Brush textColour, float x, float y)
        {
            y = y + ((sceneManager.Height / 2) - (SceneManager.textFontSize / 2));

            Font textFont = new Font(SceneManager.textFont, SceneManager.textFontSize);

            x = x + ((sceneManager.Width / 2) - (textGFX.MeasureString(text, textFont).Width / 2));

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

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, sceneManager.Width, 0, sceneManager.Height, -1, 1);

            if (textBMP != null)
            {
                // Display "Game Over" at (0, - (sceneManager.Height / 24))
                RenderText("Game Over", SceneManager.textColourArray[4], 0, -(sceneManager.Height / 8));

                string score = SceneManager.playerNameArray[0] + ": " + SceneManager.playerScoreArray[0] + " " + SceneManager.playerNameArray[1] + ": " + SceneManager.playerScoreArray[1];

                // Display score at (0, (sceneManager.Height / 24))
                RenderText(score, SceneManager.textColourArray[4], 0, (sceneManager.Height / 24));
            }
        }
    }
}