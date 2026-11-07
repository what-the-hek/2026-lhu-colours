using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

// A class to handle magical effects following the cursor and the different states it can be in (e.g. intense glow when hovering an interactable)
public class CursorFollow : MonoBehaviour
{
    private Camera mainCamera;
    private PlayerController playerController;
    private ParticleSystem particleSys;
    private ParticleSystem.ColorOverLifetimeModule colorOverLifetime;

    // Modules needed for controlling count, size, and speed
    private ParticleSystem.MainModule particleMain;
    private ParticleSystem.EmissionModule particleEmission;

    [Header("Cursor Settings")]
    public Color cursorColor = Color.white;

    [Tooltip("Is the cursor currently hovering over an interactable object?")]
    public bool isHoveringInteractable = false;

    [Header("2D Physics Settings")]
    [Tooltip("Only check these specific layers for interactables (Highly recommended for performance).")]
    [SerializeField] private LayerMask interactableLayers;

    [Header("Transition Settings")]
    [Tooltip("How fast the cursor transforms. Higher values mean a quicker fade-in/out.")]
    [SerializeField] private float transitionSpeed = 10f;

    [Tooltip("How fast the cursor alpha completely fades away to 0 after clicking an object.")]
    [SerializeField] private float clickFadeSpeed = 2f;

    [Header("Hover Visual Modifiers")]
    [Range(0f, 1f), Tooltip("How much to dim the original alpha when NOT hovering (e.g., 0.1 = 10% of targetColor.a)")]
    [SerializeField] private float dimFactor = 0.1f;

    [Range(0f, 1f), Tooltip("Multiplier for particle size when NOT hovering (e.g., 0.4 = 40% of original size)")]
    [SerializeField] private float sizeFactor = 0.8f;

    [Range(0f, 1f), Tooltip("Multiplier for particle speed when NOT hovering.")]
    [SerializeField] private float speedFactor = 0.9f;

    [Range(0f, 1f), Tooltip("Multiplier for particle emission rate when NOT hovering.")]
    [SerializeField] private float emissionFactor = 0.9f;

    // Track the initial configuration set up in the Unity Inspector
    private float baseStartSize;
    private float baseStartSpeed;
    private float baseEmissionRateOverTime;
    private float baseEmissionRateOverDistance;

    // A single value from 0 (fully idle) to 1 (fully hovering) to drive the lerp
    private float hoverProgress = 0f;

    // Smooth internal multiplier variable used to drive the click fade down to 0
    private float clickFadeProgress = 1f;

    void Start()
    {
        // Not good performance to find actors at runtime, but since it's a simple game and this only runs once at start its fine
        playerController = FindAnyObjectByType<PlayerController>();

        mainCamera = Camera.main;

        // Cache the particle system components and modules
        particleSys = GetComponent<ParticleSystem>();
        colorOverLifetime = particleSys.colorOverLifetime;
        particleMain = particleSys.main;
        particleEmission = particleSys.emission;

        // Ensure the particle module is active
        colorOverLifetime.enabled = true;

        // Store the default values you created in the Inspector so we can revert back to them
        baseStartSize = particleMain.startSize.constant;
        baseStartSpeed = particleMain.startSpeed.constant;
        baseEmissionRateOverTime = particleEmission.rateOverTime.constant;
        baseEmissionRateOverDistance = particleEmission.rateOverDistance.constant;

        clickFadeProgress = 1f;
    }

    private IEnumerator ExecuteAfterTime(float delay, Action task)
    {
        yield return new WaitForSeconds(delay);

        // ?. ensures it only runs if someone is subscribed/passed a method
        task?.Invoke();
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

        // Detect click to instantly surge the progress value and fire a burst
        if (Mouse.current.leftButton.wasPressedThisFrame && playerController.ChosenRune != "")
        {
            if(isHoveringInteractable)
            {
                particleSys.Emit(100); // Instant magic spell blast count
                // extra speed and size for a quick spell burst
                hoverProgress = 1f;    // Set progress to max opacity/size instantly so it "pulses"
                // Don't spawn any more after the burst
                //emissionFactor = 0f;
            }
            else
            {
                /// still give some visual feedback for a 'failed cast' - but less
                particleSys.Emit(30); // Instant magic spell blast count
                hoverProgress = 1f;    // Set progress to max opacity/size instantly so it "pulses"
                // Don't spawn any more after the burst
               // emissionFactor = 0f;
            }
            StartCoroutine(ExecuteAfterTime(0.75f, () =>
            {
                // Deselect the rune
                playerController.ChosenRune = "";
            }));
        }

        // Smoothly track whether we should be visible or faded based on the rune status
        float targetClickProgress = (playerController.ChosenRune != "") ? 1f : 0f;
        clickFadeProgress = Mathf.MoveTowards(clickFadeProgress, targetClickProgress, clickFadeSpeed * Time.deltaTime);

        // 3. Smooth out the transition variable over time
        CalculateTransitionProgress();

        // 4. Dynamically adjust particle properties based on smoothed state
        ApplyDynamicModifiers();

        // 5. Generate and apply the updated dynamic alpha gradient
        ApplySolidColorWithFade(cursorColor);
    }

    private void CalculateTransitionProgress()
    {
        // Target 1.0f if hovering, target 0.0f if not hovering
        float targetProgress = isHoveringInteractable ? 1f : 0f;

        // Smoothly step towards our target using Time.deltaTime to stay frame-rate independent
        hoverProgress = Mathf.MoveTowards(hoverProgress, targetProgress, transitionSpeed * Time.deltaTime);
    }

    private void ApplyDynamicModifiers()
    {
        // Linearly interpolate between the idle modifier and full size/speed base values
        float currentSizeMultiplier = Mathf.Lerp(sizeFactor, 1f, hoverProgress);
        float currentSpeedMultiplier = Mathf.Lerp(speedFactor, 1f, hoverProgress);
        float currentEmissionMultiplier = Mathf.Lerp(emissionFactor, 1f, hoverProgress);

        // Apply values to the particle system
        particleMain.startSize = baseStartSize * currentSizeMultiplier;
        particleMain.startSpeed = baseStartSpeed * currentSpeedMultiplier;

        // Concurrently drop emission generation completely to zero over time
        particleEmission.rateOverTime = baseEmissionRateOverTime * currentEmissionMultiplier * clickFadeProgress;
        particleEmission.rateOverDistance = baseEmissionRateOverDistance * currentEmissionMultiplier * clickFadeProgress;
    }

    private void ApplySolidColorWithFade(Color targetColor)
    {
        Gradient gradient = new Gradient();

        // Define the color keys (Keeps your variable color uniform across its lifetime)
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0] = new GradientColorKey(targetColor, 0.0f);
        colorKeys[1] = new GradientColorKey(targetColor, 1.0f);

        // Linearly interpolate alpha between the idle modifier and 100% of targetColor.a
        float currentAlphaMultiplier = Mathf.Lerp(dimFactor, 1f, hoverProgress);
        float startingAlpha = targetColor.a * currentAlphaMultiplier;

        // Multiply by the fading progress variable so newly spawned trailing items decay smoothly
        startingAlpha *= clickFadeProgress;

        // Define the alpha keys (Starts at chosen opacity and fades completely to 0 transparency)
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(startingAlpha, 0.0f);
        alphaKeys[1] = new GradientAlphaKey(0.0f, 1.0f);

        // Assemble the gradient and pass it to the particle system
        gradient.SetKeys(colorKeys, alphaKeys);
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
    }
}
