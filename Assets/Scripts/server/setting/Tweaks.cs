using UnityEngine;

namespace server.setting
{
    [CreateAssetMenu(fileName = "Tweaks", menuName = "ScriptableObjects/Tweaks")]
    public class Tweaks : ScriptableObject
    {
        [Header("---- Movement ----")]
        public float moveSpeed;
        public float gravity;
        public float maxJumpHeight;

        [Header("---- ViewRotate ----")]
        public float sensitivity;
        public float horizontalSensitivity;
        public float verticalSensitivity;

        [Header("---- Operate ----")]
        public float maxOperateDistance;


    }
}
