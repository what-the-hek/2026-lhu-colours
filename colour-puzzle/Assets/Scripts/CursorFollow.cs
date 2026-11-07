using UnityEngine;
using UnityEngine.InputSystem;

public class CursorFollow : MonoBehaviour
{
    private Camera mainCamera;
    private ParticleSystem particleSys;
    private ParticleSystem.ColorOverLifetimeModule colorOverLifetime;

    [Header("Cursor Settings")]
    public Color cursorColor = Color.white;

    // 1. New control variables for the hovering states
    [Tooltip("Is the cursor currently hovering over an interactable object?")]
    public bool isHoveringInteractable = false;

    [Range(0f, 1f), Tooltip("Alpha transparency (0-1) when NOT hovering an interactable.")]
    [SerializeField] private float dimAlpha = 0.25f;

    [Range(0f, 1f), Tooltip("Alpha transparency (0-1) when hovering an interactable.")]
    [SerializeField] private float brightAlpha = 1f;

    void Start()
    {
        mainCamera = Camera.main;

        // Cache the particle system components
        particleSys = GetComponent<ParticleSystem>();
        colorOverLifetime = particleSys.colorOverLifetime;
    }

    void Update()
    {
        // Check if a mouse is connected to the system
        if (Mouse.current == null) return;

        // Read the mouse screen position using the Input System api
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        // Convert screen position to World Space (2D/UI setup)
        Vector3 mousePos3D = new Vector3(mouseScreenPos.x, mouseScreenPos.y, 10f);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePos3D);
        transform.position = worldPosition;

        // 2. Generate and apply a gradient based on your single Color variable
        ApplySolidColorWithFade(cursorColor);
    }

    private void ApplySolidColorWithFade(Color targetColor)
    {
        Gradient gradient = new Gradient();

        // Define the color keys (Keeps your variable color uniform across its lifetime)
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0] = new GradientColorKey(targetColor, 0.0f); // Starts as your color
        colorKeys[1] = new GradientColorKey(targetColor, 1.0f); // Ends as your color

        // 3. Determine the starting opacity based on the boolean state
        float startingAlpha = isHoveringInteractable ? brightAlpha : dimAlpha;

        // Define the alpha keys (Starts at chosen opacity and fades completely to 0 transparency)
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(startingAlpha, 0.0f); // Dynamically set alpha at start
        alphaKeys[1] = new GradientAlphaKey(0.0f, 1.0f);          // 0% visible (invisible) at end

        // Assemble the gradient and pass it to the particle system
        gradient.SetKeys(colorKeys, alphaKeys);
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
    }
}
