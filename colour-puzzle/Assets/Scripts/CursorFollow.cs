using UnityEngine;
using UnityEngine.InputSystem;

public class CursorFollow : MonoBehaviour
{
    private Camera mainCamera;
    private ParticleSystem particleSys;
    private ParticleSystem.ColorOverLifetimeModule colorOverLifetime;

    // 1. New module references to control emission, size, and speed
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.MainModule mainModule;

    [Header("Cursor Settings")]
    public Color cursorColor = Color.white;

    [Tooltip("Is the cursor currently hovering over an interactable object?")]
    public bool isHoveringInteractable = false;

    [Header("2D Physics Settings")]
    [Tooltip("Only check these specific layers for interactables (Highly recommended for performance).")]
    [SerializeField] private LayerMask interactableLayers;

    [Header("Hover Modifier Settings")]
    [Range(0f, 1f), Tooltip("How much to dim the original alpha when NOT hovering (e.g., 0.1 = 10% of targetColor.a)")]
    [SerializeField] private float dimFactor = 0.1f;

    [Range(0f, 1f), Tooltip("Multiplier for the number of particles spawned when NOT hovering.")]
    [SerializeField] private float nonHoverEmissionMultiplier = 0.3f;

    [Range(0f, 1f), Tooltip("Multiplier for particle size when NOT hovering.")]
    [SerializeField] private float nonHoverSizeMultiplier = 0.5f;

    [Range(0f, 1f), Tooltip("Multiplier for internal start speed when NOT hovering.")]
    [SerializeField] private float nonHoverSpeedMultiplier = 0.4f;

    // Caches to hold the original Inspector values designed in the Particle System UI
    private float originalRateOverDistance;
    private float originalStartSize;
    private float originalStartSpeed;

    void Start()
    {
        mainCamera = Camera.main;

        // Cache the particle system components and modules
        particleSys = GetComponent<ParticleSystem>();
        colorOverLifetime = particleSys.colorOverLifetime;
        emissionModule = particleSys.emission;
        mainModule = particleSys.main;

        // Ensure the particle module is active
        colorOverLifetime.enabled = true;

        // 2. Remember the original values configured in the Unity Inspector UI
        originalRateOverDistance = emissionModule.rateOverDistance.constant;
        originalStartSize = mainModule.startSize.constant;
        originalStartSpeed = mainModule.startSpeed.constant;
    }

    void Update()
    {
        // Check if a mouse/pointer is connected to the system
        if (Mouse.current == null) return;

        // Read the mouse screen position using the Input System API
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        // Position tracking: Convert screen position to 2D World Space
        Vector3 mousePos3D = new Vector3(mouseScreenPos.x, mouseScreenPos.y, 10f);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePos3D);
        transform.position = worldPosition;

        // 2D Raycast tracking: Check if the cursor is resting over a 2D Collider
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPos);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, interactableLayers);

        if (hit.collider != null)
        {
            isHoveringInteractable = hit.collider.GetComponentInParent<IInteractable>() != null;
        }
        else
        {
            isHoveringInteractable = false;
        }

        // 3. Dynamically adjust particle settings based on hover state
        UpdateParticleProperties();

        // Generate and apply the updated dynamic alpha gradient
        ApplySolidColorWithFade(cursorColor);
    }

    private void UpdateParticleProperties()
    {
        if (isHoveringInteractable)
        {
            // Reset to full/default values when hovering an interactable
            emissionModule.rateOverDistance = originalRateOverDistance;
            mainModule.startSize = originalStartSize;
            mainModule.startSpeed = originalStartSpeed;
        }
        else
        {
            // Diminish the settings using the provided multiplier constraints when just moving over the background
            emissionModule.rateOverDistance = originalRateOverDistance * nonHoverEmissionMultiplier;
            mainModule.startSize = originalStartSize * nonHoverSizeMultiplier;
            mainModule.startSpeed = originalStartSpeed * nonHoverSpeedMultiplier;
        }
    }

    private void ApplySolidColorWithFade(Color targetColor)
    {
        Gradient gradient = new Gradient();

        // Define the color keys (Keeps your variable color uniform across its lifetime)
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0] = new GradientColorKey(targetColor, 0.0f);
        colorKeys[1] = new GradientColorKey(targetColor, 1.0f);

        // Determine starting opacity using targetColor.a and the hovering state
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
