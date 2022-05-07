using UnityEngine;

public class CameraFollowing : MonoBehaviour {
    public GameObject target;

    private void Update() {
        Vector3 position = target.transform.position;
        transform.position = new Vector3(position.x, position.y + 100, position.z);
    }
}
