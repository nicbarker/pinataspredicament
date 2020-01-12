using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject player;

    private float initialXPosition;
    private float initialYPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialXPosition = transform.position.x;
        initialYPosition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            Mathf.Max(initialXPosition, player.transform.position.x),
            Mathf.Max(initialYPosition, player.transform.position.y),
            transform.position.z
        );

    }
}
