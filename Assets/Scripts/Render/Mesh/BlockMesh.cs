using Render.Texture;

namespace Render.Mesh
{
    using UnityEngine;
    using Block;

    public class BlockMesh
    {
        public static readonly BlockMesh None = new();
        public static readonly CubeBlockMesh CubeBlock = new();

        public virtual int AllVerticesCount { get; } = 0;
        public virtual int AllTrianglesCount { get; } = 0;
        public virtual int AllUVsCount { get; } = 0;

        /// <summary>
        /// return the vertices of mesh in order
        /// <para>1 --- 0</para>
        /// |&#160;&#160;&#160;&#160;&#160;|
        /// <para>3 --- 2</para>
        /// </summary>
        /// <returns></returns>
        public virtual Vector3[] GetVertices(BlockFace _blockFace, int _x, int _y, int _z)
        {
            return null;
        }

        public virtual Vector3[] GetAllVertices(int _x, int _y, int _z)
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
        public virtual Vector2[] GetUVs(BlockProperty _block, BlockFace _face)
        {
            return null;
        }

        public virtual Vector2[] GetAllUVs(BlockProperty _block)
        {
            return null;
        }
    }
}