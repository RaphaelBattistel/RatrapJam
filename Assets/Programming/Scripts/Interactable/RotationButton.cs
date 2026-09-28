using UnityEngine;

public class RotationButton : MonoBehaviour, IInteractable
{

    [SerializeField, Range(0f, 1f)] private float darkenFactor = 0.2f;

    private Material _material;
    private Color _baseColor;
    private bool isPressed = false;

    public bool IsPressed => isPressed;

    void Start()
    {
        if (TryGetComponent<Renderer>(out Renderer renderer))
        {
            _material = renderer.material;
            _baseColor = _material.color;
        }
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
        isPressed = pressed;
    }
}