using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraSettings cameraSettings;
    [SerializeField] private Transform target;
    [SerializeField] private float interactionDistance = 5f;
    [SerializeField] private LayerMask interactableLayer;

    private Vector3 _lookPosition;
    private float rotationX = 0;
    private float rotationY = 0;

    private bool _isPressed = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        Look(_lookPosition);
        PlaceCameraFromTarget();
        UpdateInteractionRaycast();
    }

    [Button]
    void PlaceCameraFromTarget()
    {
        switch (cameraSettings.Options.CameraType)
        {
            default:
                transform.position = target.position +
                             (target.forward * cameraSettings.Options.offset.z) +
                             (target.right * cameraSettings.Options.offset.x) +
                             (target.up * (cameraSettings.Options.offset.y - 1));
                break;
            case CameraSettings.CameraOfType.MoveAroundChara:
                transform.position = target.position + (-transform.forward * cameraSettings.Options.distance);
                break;
        }
    }

    private IInteractable _currentTarget; // au lieu de Button

    private void UpdateInteractionRaycast()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayer))
        {
            IInteractable hitTarget = hit.collider.GetComponent<IInteractable>();

            if (hitTarget != _currentTarget)
            {
                SwitchTarget(hitTarget);
            }
        }
        else if (_currentTarget != null)
        {
            SwitchTarget(null);
        }
    }

    private void SwitchTarget(IInteractable newTarget)
    {
        _currentTarget?.OnHoverExit();
        _currentTarget = newTarget;
        _currentTarget?.OnHoverEnter();

        if (_currentTarget != null)
        {
            _currentTarget.SetPressed(_isPressed);
        }
    }

    public void InputClic(InputAction.CallbackContext ctx)
    {
        _isPressed = ctx.performed;
        _currentTarget?.SetPressed(_isPressed);
    }

    public void InputScroll(InputAction.CallbackContext ctx)
    {
        if (_currentTarget is IScrollable scrollable)
        {
            float scrollValue = ctx.ReadValue<float>();
            scrollable.OnScroll((int)scrollValue);
        }
    }

    public void InputLook(InputAction.CallbackContext ctx)
    {
        _lookPosition.y = -ctx.ReadValue<Vector2>().y;
        _lookPosition.x = ctx.ReadValue<Vector2>().x;
    }

    private void Look(Vector2 lookPos)
    {
        Vector3 look = lookPos * cameraSettings.Options.sensitivity;
        rotationY += look.y;
        rotationX += look.x;
        rotationY = Mathf.Clamp(rotationY, cameraSettings.Options.rotationClamp.x, cameraSettings.Options.rotationClamp.y);
        transform.eulerAngles = new Vector3(rotationY, rotationX, 0);
    }
}