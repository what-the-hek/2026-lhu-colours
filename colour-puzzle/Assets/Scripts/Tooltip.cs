
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CanvasTooltip : MonoBehaviour
{
    [Header("Tooltip Settings")]
    [TextArea]
    public string tooltipText;

    public GameObject tooltipPrefab;
    public Canvas canvas;

    private GameObject tooltipInstance;
    private RectTransform tooltipRect;
    private Text tooltipLabel;

    private Camera mainCamera;
    private bool isHovering;

    private void Start()
    {
        mainCamera = Camera.main;

        if (tooltipPrefab == null || canvas == null)
        {
            Debug.LogWarning("Tooltip prefab or canvas is missing.", this);
            enabled = false;
            return;
        }

        tooltipInstance = Instantiate(tooltipPrefab, canvas.transform);

        tooltipRect = tooltipInstance.GetComponent<RectTransform>();
        tooltipLabel = tooltipInstance.GetComponentInChildren<Text>();

        if (tooltipLabel != null)
            tooltipLabel.text = tooltipText;

        tooltipInstance.SetActive(false);
    }

    private void Update()
    {
        if (Mouse.current == null || mainCamera == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(mousePosition.x, mousePosition.y,
            Mathf.Abs(mainCamera.transform.position.z - transform.position.z))
        );

        Vector2 worldPosition2D = new Vector2(
            worldPosition.x,
            worldPosition.y
        );

        Collider2D hit = Physics2D.OverlapPoint(worldPosition2D);

        bool hoveringNow = hit != null && hit.gameObject == gameObject;

        if (hoveringNow != isHovering)
        {
            isHovering = hoveringNow;

            if (tooltipInstance != null)
                tooltipInstance.SetActive(isHovering);
        }

        if (isHovering && tooltipInstance != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                mousePosition,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay
                    ? null
                    : canvas.worldCamera,
                out Vector2 localPoint
            );

            tooltipRect.localPosition = localPoint;
        }
    }

    private void OnDestroy()
    {
        if (tooltipInstance != null)
            Destroy(tooltipInstance);
    }
}