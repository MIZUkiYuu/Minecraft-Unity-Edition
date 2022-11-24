namespace Render.Texture
{
    public enum BlockFace
    {
        Back,
        Down,
        Front,
        Left,
        Right,
        Top,
    }

    public struct TextureSuffix
    {
        public static readonly string[] Face =
        {
            "_back",
            "_down",
            "_front",
            "_left",
            "_right",
            "_top",
        };
    }
}