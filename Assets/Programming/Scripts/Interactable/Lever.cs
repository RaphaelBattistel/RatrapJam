using UnityEngine;

public class Lever : MonoBehaviour, IInteractable, IScrollable
{
    [SerializeField] private float minAngle = -30f;
    [SerializeField] private float maxAngle = 30f;
    [SerializeField] private float degreesPerScroll = 5f;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField, Range(0f, 1f)] private float darkenFactor = 0.2f;

    private Material _material;
    private Color _baseColor;
    private bool isGrabbed = false;
    private float targetAngle = 0f;
    private float currentAngle = 0f;

    public float NormalizedValue => Mathf.InverseLerp(minAngle, maxAngle, currentAngle);

    void Start()
    {
        if (TryGetComponent<Renderer>(out Renderer renderer))
        {
            _material = renderer.material;
            _baseColor = _material.color;
        }

        targetAngle = minAngle;
        currentAngle = minAngle;
        transform.localEulerAngles = new Vector3(currentAngle, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    void FixedUpdate()
    {
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, smoothSpeed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(currentAngle, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    public void OnHoverEnter()
    {
        _material.color = _baseColor * darkenFactor;
    }

    public void OnHoverExit()
    {
        _material.color = _baseColor;
        SetPressed(false);
    }

    public void SetPressed(bool pressed)
    {
        isGrabbed = pressed;
    }

    public void OnScroll(int delta)
    {
        if (!isGrabbed) return;

        targetAngle = Mathf.Clamp(targetAngle + delta * degreesPerScroll, minAngle, maxAngle);
    }
}