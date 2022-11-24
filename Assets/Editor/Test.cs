using System;
using System.Buffers;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class Test
    {
        [MenuItem("Test/Log01", false, 1)]
        private static void Log01()
        {
            MemoryPool<int> aPool = MemoryPool<int>.Shared;
            IMemoryOwner<int> a = aPool.Rent(10);

            Debug.Log(a.Memory.Length);
            foreach (int VARIABLE in a.Memory.Span)
            {
                Debug.Log(VARIABLE);
            }
        }

        [MenuItem("Test/Log02", false, 1)]
        private static void Log02()
        {
            int[] a = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            int b = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    b = a[i + j];
                }
            }
        }
    }
}