using UnityEngine;

namespace Render.Mesh
{
    public class Vertices
    {
        public Vector3[] array = new Vector3[4];

        public static Vector3[] operator +(Vertices v1, Vector3 vector)
        {
            Vector3[] v = new Vector3[4];
            for (int i = 0; i < v1.array.Length; i++)
            {
                v[i] = v1.array[i] + vector;
            }

            return v;
        }

        public static Vertices operator +(Vertices v1, Vertices v2)
        {
            Vertices v = new();
            for (int i = 0; i < v1.array.Length; i++)
            {
                v.array[i] = v1.array[i] + v2.array[i];
            }

            return v;
        }
    }
}