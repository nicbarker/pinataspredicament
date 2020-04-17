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
        var cameraPositionY = transform.position.y;
        var topScreenBoundary = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * 0.6f, 0)).y;
        var bottomScreenBoundary = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * 0.3f, 0)).y;
        if (player.transform.position.y > topScreenBoundary) {
            cameraPositionY += player.transform.position.y - topScreenBoundary + 0.01f;
        } else if (player.transform.position.y < bottomScreenBoundary) {
            cameraPositionY += player.transform.position.y - bottomScreenBoundary - 0.01f;
        }
        transform.position = new Vector3(
            Mathf.Max(initialXPosition, player.transform.position.x),
            cameraPositionY,
            transform.position.z
        );

    }
}
