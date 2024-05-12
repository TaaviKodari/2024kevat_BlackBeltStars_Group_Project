using UnityEngine;

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
        
        var worldScreenHeight = camera.orthographicSize * 2f;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        var vCamPos = vCam.position;
        transform.position = new Vector3(Mathf.Floor(vCamPos.x / spriteSize.x) * spriteSize.x, Mathf.Floor(vCamPos.y / spriteSize.y) * spriteSize.y, transform.position.z);
        renderer.size = new Vector2(Mathf.Ceil(worldScreenWidth) + 2 * spriteSize.x, Mathf.Ceil(worldScreenHeight) + 2 * spriteSize.y);
    }
}
