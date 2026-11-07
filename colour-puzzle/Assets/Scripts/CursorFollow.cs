using UnityEngine;
using UnityEngine.InputSystem;

public class CursorFollow : MonoBehaviour
{
    private Camera mainCamera;
    private ParticleSystem particleSys;
    private ParticleSystem.ColorOverLifetimeModule colorOverLifetime;

    [Header("Cursor Settings")]
    public Color cursorColor = Color.white;

    [Tooltip("Is the cursor currently hovering over an interactable object?")]
    public bool isHoveringInteractable = false;

    [Header("2D Physics Settings")]
    [Tooltip("Only check these specific layers for interactables (Highly recommended for performance).")]
    [SerializeField] private LayerMask interactableLayers;

    [Header("Alpha Multipliers")]
    [Range(0f, 1f), Tooltip("How much to dim the original alpha when NOT hovering (e.g., 0.25 = 25% of targetColor.a)")]
    [SerializeField] private float dimFactor = 0.1f;

    void Start()
    {
        mainCamera = Camera.main;

        // Cache the particle system components
        particleSys = GetComponent<ParticleSystem>();
        colorOverLifetime = particleSys.colorOverLifetime;

        // Ensure the particle module is active
        colorOverLifetime.enabled = true;
    }

    void Update()
    {
        // Check if a mouse/pointer is connected to the system
        if (Mouse.current == null) return;

        // Read the mouse screen position using the Input System API
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        // 1. Position tracking: Convert screen position to 2D World Space
        Vector3 mousePos3D = new Vector3(mouseScreenPos.x, mouseScreenPos.y, 10f);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePos3D);
        transform.position = worldPosition;

        // 2. 2D Raycast tracking: Check if the cursor is resting over a 2D Collider
        // GetRayIntersection shoots a ray straight down from the camera lens through the 2D coordinate plane
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPos);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, interactableLayers);

        if (hit.collider != null)
        {
            // Check if the 2D object (or its parent components) implements our custom interface
            isHoveringInteractable = hit.collider.GetComponentInParent<IInteractable>() != null;
        }
        else
        {
            isHoveringInteractable = false;
        }

        // 3. Generate and apply the updated dynamic alpha gradient
        ApplySolidColorWithFade(cursorColor);
    }

    private void ApplySolidColorWithFade(Color targetColor)
    {
        Gradient gradient = new Gradient();

        // Define the color keys (Keeps your variable color uniform across its lifetime)
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0] = new GradientColorKey(targetColor, 0.0f);
        colorKeys[1] = new GradientColorKey(targetColor, 1.0f);

        // Determine starting opacity using targetColor.a and the hovering state
        // If targetColor.a is 0, both states will resolve to 0 (fully transparent)
        float startingAlpha = isHoveringInteractable ? targetColor.a : (targetColor.a * dimFactor);

        // Define the alpha keys (Starts at chosen opacity and fades completely to 0 transparency)
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(startingAlpha, 0.0f);
        alphaKeys[1] = new GradientAlphaKey(0.0f, 1.0f);

        // Assemble the gradient and pass it to the particle system
        gradient.SetKeys(colorKeys, alphaKeys);
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
    }
}
