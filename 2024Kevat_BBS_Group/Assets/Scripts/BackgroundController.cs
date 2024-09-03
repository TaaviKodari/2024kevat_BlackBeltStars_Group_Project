using UnityEngine;

// Handles moving the background to match the player position and window size
public class BackgroundController : MonoBehaviour
{
    [SerializeField] 
    private new Camera camera;
    [SerializeField] 
    private Transform vCam;
    private new SpriteRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var spriteSize = renderer.sprite.bounds.size;
        
        // Get the width and height of the screen in world space to know how much background we need
        var worldScreenHeight = camera.orthographicSize * 2f;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        var vCamPos = vCam.position;
        // Move the background to the correct location
        // We use a trick of dividing by the sprite size, then flooring and then multiplying by the size again to make the background snap to a grid
        transform.position = new Vector3(Mathf.Floor(vCamPos.x / spriteSize.x) * spriteSize.x, Mathf.Floor(vCamPos.y / spriteSize.y) * spriteSize.y, transform.position.z);
        // Set the size of the background to match the world-space size of the camera, with a bit of extra padding
        renderer.size = new Vector2(Mathf.Ceil(worldScreenWidth) + 2 * spriteSize.x, Mathf.Ceil(worldScreenHeight) + 2 * spriteSize.y);
    }
}
