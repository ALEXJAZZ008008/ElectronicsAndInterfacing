using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game
{
    class DisplayHighscores : Scene, IScene
    {
        Bitmap textBMP;
        Graphics textGFX;
        int textTexture;

        public string SceneTypeCheck()
        {
            return "DisplayHighscores";
        }

        public DisplayHighscores(SceneManager sceneManager) : base(sceneManager)
        {
            // Set the title of the window
            sceneManager.Title = "Pong - Highscores";

            // Set the SceneTypeCheck, Update and Render delegates to the SceneTypeCheck, Update and Render methods of this class
            sceneManager.sceneTypeChecker = SceneTypeCheck;
            sceneManager.updater = Update;
            sceneManager.renderer = Render;

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
                int length = SceneManager.highscoreArray.Length / 2;

                if (length % 2 == 0)
                {
                    int i = 8;

                    for (int j = 0; j < length / 2; j++)
                    {
                        if (j == 0)
                        {
                            RenderText(SceneManager.highscoreArray[(length / 2) - (j + 1), 0] + " " + SceneManager.highscoreArray[(length / 2) - (j + 1), 1], SceneManager.textColourArray[4], 0, - (sceneManager.Height / 24));
                        }
                        else
                        {
                            int position = -(sceneManager.Height / i);

                            RenderText(SceneManager.highscoreArray[(length / 2) - (j + 1), 0] + " " + SceneManager.highscoreArray[(length / 2) - (j + 1), 1], SceneManager.textColourArray[4], 0, position);

                            i = i / 2;
                        }
                    }

                    i = 8;

                    for (int j = length / 2; j < length; j++)
                    {
                        if (j == length / 2)
                        {
                            RenderText(SceneManager.highscoreArray[j, 0] + " " + SceneManager.highscoreArray[j, 1], SceneManager.textColourArray[4], 0, (sceneManager.Height / 24));
                        }
                        else
                        {
                            int position = (sceneManager.Height / i);

                            RenderText(SceneManager.highscoreArray[j, 0] + " " + SceneManager.highscoreArray[j, 1], SceneManager.textColourArray[4], 0, position);

                            i = i / 2;
                        }
                    }
                }
                else
                {
                    RenderText(SceneManager.highscoreArray[length / 2, 0] + " " + SceneManager.highscoreArray[length / 2, 1], SceneManager.textColourArray[4], 0, 0);

                    int i = 16;

                    for (int j = 0; j < length / 2; j++)
                    {
                        int position = - sceneManager.Height / i;

                        RenderText(SceneManager.highscoreArray[(length / 2) - (j + 1), 0] + " " + SceneManager.highscoreArray[(length / 2) - (j + 1), 1], SceneManager.textColourArray[4], 0, position);

                        i = i / 2;
                    }

                    i = 16;

                    for (int j = length / 2; j < length - 1;)
                    {
                        j++;

                        int position = sceneManager.Height / i;

                        RenderText(SceneManager.highscoreArray[j, 0] + " " + SceneManager.highscoreArray[j, 1], SceneManager.textColourArray[4], 0, position);

                        i = i / 2;
                    }
                }
            }
        }
    }
}
