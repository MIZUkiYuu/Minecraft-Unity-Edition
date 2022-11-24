namespace Render.Mesh
{
    using UnityEngine;
    using Block;

    public abstract class BlockMesh
    {
        public virtual bool VisualAllFace => false;
        public virtual VisualFace VisualSomeFace => new();

        private readonly VisualFace canVisualAllFace = new()
        {
            all = true,
            right = true,
            left = true,
            top = true,
            down = true,
            front = true,
            back = true,
        };

        /// <summary>
        /// face must to be rendered.
        /// </summary>
        public VisualFace VisualFace => VisualAllFace ? canVisualAllFace : VisualSomeFace;


        /// <summary>
        /// return the vertices of mesh in order
        /// <para>1 --- 0</para>
        /// |&#160;&#160;&#160;&#160;&#160;|
        /// <para>3 --- 2</para>
        /// </summary>
        /// <returns></returns>
        public virtual Vector3[] GetVertices(BlockFace _blockFace, Vector3 _pos)
        {
            return null;
        }

        public virtual Vector3[] GetAllVertices(Vector3 _pos)
        {
            return null;
        }

        /// <summary>
        /// return the triangle list in order
        /// <para>1 ---- 0</para>
        /// |&#160;&#160;&#160;&#160;/&#160;|
        /// <para>|&#160;&#160;&#160;/&#160;&#160;|</para>
        /// |&#160;&#160;/&#160;&#160;&#160;|
        /// <para>|&#160;/&#160;&#160;&#160;&#160;|</para>
        /// 3 ---- 2
        /// </summary>
        /// <param name="_faces">the number of renderable surfaces</param>
        /// <param name="_vertices">the number of vertices of mesh</param>
        /// <param name="_face">block face</param>
        /// <param name="_uvs">output the uv array</param>
        /// <returns></returns>
        public virtual int[] GetTriangles(int _vertices)
        {
            return null;
        }

        public virtual int[] GetAllTriangles(int _vertices)
        {
            return null;
        }

        /// <summary>
        /// Each edge needs trim off a certain distance to avoid face rendering problems due to floating point errors.
        /// </summary>
        protected const float TrimmingDistance = 0.001f;

        /// <summary>
        /// return the UVs of texture in order
        /// <para>1 --- 0</para>
        /// |&#160;&#160;&#160;&#160;&#160;|
        /// <para>3 --- 2</para>
        /// </summary>
        /// <returns></returns>
        public virtual Vector2[] GetUVs(BlockFace _face)
        {
            return null;
        }

        public virtual Vector2[] GetAllUVs()
        {
            return null;
        }
    }
}