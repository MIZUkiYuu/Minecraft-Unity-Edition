using UnityEngine;

namespace World
{
    [CreateAssetMenu(fileName = "WorldData", menuName = "Data/WorldData_SO", order = 0)]
    public class WorldData_SO : ScriptableObject
    {
        [Header("world gen")] 
        public int seed;
    }
}