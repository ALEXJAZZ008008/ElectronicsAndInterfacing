using System;

namespace Game
{
    class PlayerPaddle : Paddle
    {
        public PlayerPaddle(int x, int y) : base(x, y)
        {

        }

        public override void Update(float dt)
        {

        }

        public void MoveX(int dx)
        {
            position.X += dx;

            if (position.X < (SceneManager.WindowWidth / 2) - (SceneManager.originalWidth / 2))
            {
                position.X = (SceneManager.WindowWidth / 2) - (SceneManager.originalWidth / 2);
            }
            else
            {
                if (position.X > (SceneManager.WindowWidth / 2) + (SceneManager.originalWidth / 2))
                {
                    position.X = (SceneManager.WindowWidth / 2) + (SceneManager.originalWidth / 2);
                }
            }
        }

        public void MoveY(int dy)
        {
            position.Y += dy;

            if (position.Y < (SceneManager.WindowHeight / 2) - (SceneManager.originalHeight / 2))
            {
                position.Y = (SceneManager.WindowHeight / 2) - (SceneManager.originalHeight / 2);
            }
            else
            {
                if (position.Y > (SceneManager.WindowHeight / 2) + (SceneManager.originalHeight / 2))
                {
                    position.Y = (SceneManager.WindowHeight / 2) + (SceneManager.originalHeight / 2);
                }
            }
        }
    }
}
