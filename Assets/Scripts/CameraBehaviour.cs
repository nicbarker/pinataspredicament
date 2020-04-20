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
        // Allow a horizontal stripe of 30% screen width between 0.3 and 0.6 where the player can move without
        // pushing the camera - helps avoid the motion sickness feeling with jumping and quick movements
        var cameraPositionY = transform.position.y;
        var topScreenBoundary = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * 0.6f, 0)).y;
        var bottomScreenBoundary = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * 0.3f, 0)).y;
        if (player.transform.position.y > topScreenBoundary) {
            cameraPositionY += player.transform.position.y - topScreenBoundary + 0.01f;
        } else if (player.transform.position.y < bottomScreenBoundary) {
            cameraPositionY += player.transform.position.y - bottomScreenBoundary - 0.01f;
        }
        
        // Allow a vertical stripe of 20% screen width between 0.4 and 0.6 where the player can move without
        // pushing the camera - helps avoid the motion sickness feeling with jumping and quick movements
        var cameraPositionX = transform.position.x;
        var leftScreenBoundary = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.45f, 0, 0)).x;
        var rightScreenBoundary = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.55f, 0, 0)).x;

        if (player.transform.position.x > rightScreenBoundary) {
            cameraPositionX += player.transform.position.x - rightScreenBoundary - 0.01f;
        } else if (player.transform.position.x < leftScreenBoundary) {
            cameraPositionX += player.transform.position.x - leftScreenBoundary + 0.01f;
        }
        
        transform.position = new Vector3(
            Mathf.Max(initialXPosition, cameraPositionX),
            cameraPositionY,
            transform.position.z
        );

    }
}
