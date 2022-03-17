using UnityEngine;

namespace server.world
{
    public class GroundGenerator : MonoBehaviour
    {
        public GameObject targetBlock;
        public GameObject targetPlayer;
        public int generateRange;

        private Vector3 _originPoint;
        void Start()
        {
            Vector3 playerPos = targetPlayer.transform.position;
            _originPoint.y = Mathf.Floor(playerPos.y - 1.625f) - 0.5f;  //the height of player
            _originPoint.x = Mathf.Floor(playerPos.x) + 0.5f - Mathf.Floor(generateRange / 2.0f);   //the x of Bottom left corner of player-centered square
            _originPoint.z = Mathf.Floor(playerPos.z) + 0.5f - Mathf.Floor(generateRange / 2.0f);   //the z of Bottom left corner of player-centered square
            GroundGeneration(_originPoint);
        }

        private void GroundGeneration(Vector3 origin)
        {
            for (int i = 0; i < generateRange; i++)
            {
                for (int j = 0; j < generateRange; j++)
                {
                    Instantiate(targetBlock, new Vector3(origin.x + i, origin.y, origin.z + j), Quaternion.identity);
                }
            }
        }
    }
}
