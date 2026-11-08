using UnityEngine;

public class DVDLogoBounce : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;
    public Vector2 direction = new Vector2(1f, 1f);

    private Camera mainCamera;
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    void Start()
    {
        mainCamera = Camera.main;

        // Get object dimensions
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        if (sprite != null)
        {
            objectWidth = sprite.bounds.extents.x;
            objectHeight = sprite.bounds.extents.y;
        }

        // Get screen boundaries in world coordinates
        screenBounds = mainCamera.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height,
            -mainCamera.transform.position.z));

        direction.Normalize();
    }

    void Update()
    {
        // Move object
        transform.Translate(direction * speed * Time.deltaTime);

        Vector3 position = transform.position;

        // Bounce off left and right edges
        if (position.x + objectWidth > screenBounds.x ||
            position.x - objectWidth < -screenBounds.x)
        {
            direction.x *= -1;
        }

        // Bounce off top and bottom edges
        if (position.y + objectHeight > screenBounds.y ||
            position.y - objectHeight < -screenBounds.y)
        {
            direction.y *= -1;
        }

        transform.position = position;
    }
}