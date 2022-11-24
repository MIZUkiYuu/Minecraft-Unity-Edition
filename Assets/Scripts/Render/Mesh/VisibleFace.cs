namespace Render.Mesh
{
    public struct VisibleFace
    {
        public bool all;
        public bool right;
        public bool left;
        public bool top;
        public bool down;
        public bool front;
        public bool back;

        public static VisibleFace NoFace => new()
        {
            all = false,
            right = false,
            left = false,
            top = false,
            down = false,
            front = false,
            back = false
        };
        public static VisibleFace AllFace => new()
        {
            all = true,
            right = true,
            left = true,
            top = true,
            down = true,
            front = true,
            back = true
        };
        
        public static VisibleFace NoDownFace =>new()
        {
            all = false,
            right = true,
            left = true,
            top = true,
            down = false,
            front = true,
            back = true
        };
    }
}