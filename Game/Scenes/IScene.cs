using OpenTK;

namespace Game
{
    interface IScene
    {
        string SceneTypeCheck();
        void Update(FrameEventArgs e);
        void Render(FrameEventArgs e);
    }
}
