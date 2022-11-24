using UnityEngine;

namespace World
{
    [CreateAssetMenu(fileName = "WorldData", menuName = "Data/WorldData_SO", order = 0)]
    public class WorldData_SO : ScriptableObject
    {
        public int seed;

        [Range(0, 10)] public int viewDistance = 10;
    }
}