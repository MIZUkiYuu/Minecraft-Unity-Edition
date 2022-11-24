using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    /// <summary>
    /// Math Pro Max Plus Edition
    /// </summary>
    public static class MathP
    {
        /// <summary>
        /// return a new value which value between min and max.
        /// </summary>
        /// <param name="value">target value</param>
        /// <param name="min">minimum</param>
        /// <param name="max">maximum</param>
        /// <returns></returns>
        public static Vector3Int Clamp(Vector3Int value, Vector3Int min, Vector3Int max)
        {
            return new Vector3Int()
            {
                x = Math.Clamp(value.x, min.x, max.x),
                y = Math.Clamp(value.y, min.y, max.y),
                z = Math.Clamp(value.z, min.z, max.z),
            };
        }
        
        /// <summary>
        /// return a new value which value between min and max.
        /// </summary>
        /// <param name="value">target value</param>
        /// <param name="min">minimum</param>
        /// <param name="max">maximum</param>
        /// <returns></returns>
        public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
        {
            return new Vector3()
            {
                x = Math.Clamp(value.x, min.x, max.x),
                y = Math.Clamp(value.y, min.y, max.y),
                z = Math.Clamp(value.z, min.z, max.z),
            };
        }

    }
    
}