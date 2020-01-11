using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject player;

    private float initialXPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialXPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Max(initialXPosition, player.transform.position.x), transform.position.y, transform.position.z);
    }
}
