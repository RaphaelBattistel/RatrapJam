using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionRaycaster : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 5f;
    [SerializeField] private LayerMask interactableLayer;

    private RotationButton _currentTarget;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            RotationButton hitButton = hit.collider.GetComponent<RotationButton>();

            if (hitButton != _currentTarget)
            {
                _currentTarget?.OnHoverExit();
                _currentTarget = hitButton;
                _currentTarget?.OnHoverEnter();
                
            }
        }
        else
        {
            _currentTarget?.OnHoverExit();
            _currentTarget = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * interactionDistance);
    }

}
