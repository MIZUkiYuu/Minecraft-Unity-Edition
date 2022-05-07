using UnityEngine;
public class DebugPro : MonoBehaviour {

    //[UnityEditor.MenuItem("Debug/Log")]
    public static void Log() {
        Debug.Log(Block.GetBlock(0 ,50, 0));
    }
}
