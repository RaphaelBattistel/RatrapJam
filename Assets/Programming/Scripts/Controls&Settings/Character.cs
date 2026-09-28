using NaughtyAttributes;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class Character : MonoBehaviour
{
    [SerializeField] private ControlsData controls;

    [Header("MOVE")]
    [SerializeField] private int runSpeed;
    [SerializeField] private int rotationSpeed;
    private Vector2 _direction;
    private float roationWithKey = 0;

    [Header("INTERACT")]
    [SerializeField] private BoxCollider interactBox;

    [Header("CROUCH")]
    [SerializeField] private CapsuleCollider mainCollider;
    float mainColHeight;
    bool _isCrouched = false;


    [Header("JUMP")]
    [SerializeField] private int jumpForce;
    [SerializeField] private int nbJumpMax;
    [SerializeField] private Vector3 groundCheck;
    [SerializeField] private float groundCastDistance;
    [SerializeField] private LayerMask groundLayer;
    private int _currentJump;
    private bool _isJumping;



    private Rigidbody _rb;

    void Start()
    {
        mainColHeight = mainCollider.height;
        TryGetComponent(out _rb);
    }

    void Update()
    {
        
        Move(_direction);
        if (_isJumping)
        {
            Jump();
        }
        if (IsGrounded())
        {
            mainCollider.height = mainColHeight;
            mainCollider.center = Vector3.zero;
        }
    }

    private void LateUpdate()
    {
        if(controls.MoveType == ControlsData.MovementType.RotationWithMouse)
        {
            transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
        }
    }




    public void InputMove(InputAction.CallbackContext ctx)
    {
        _direction = ctx.ReadValue<Vector2>();
    }
    private void Move(Vector2 direction)
    {
        switch (controls.MoveType)
        {
            default:
                Vector3 delta = new Vector3(direction.x, 0f, direction.y) * Time.fixedDeltaTime * runSpeed;
                transform.Translate(delta);
                break;

            case ControlsData.MovementType.RotationWithKeys:
                roationWithKey += direction.x * rotationSpeed;

                transform.position += (transform.forward * direction.y) * Time.fixedDeltaTime * runSpeed;
                transform.eulerAngles = new Vector3(0, roationWithKey, 0);
                break;
        }
    }




    public void InputJump(InputAction.CallbackContext ctx)
    {
        //if (ctx.started || ctx.performed)
        if (ctx.ReadValue<float>() == 1)
        {
            _isJumping = true;
            _currentJump++;
            return;
        }
        else
        {
            _isJumping = false;
        }
    }
    private void Jump()
    {
        if (_isJumping && _currentJump < nbJumpMax)
        {
            mainCollider.height /= 2;
            mainCollider.center = Vector3.up / 2;
            _rb.linearVelocity = Vector2.up * jumpForce;
        }
        if (IsGrounded())
        {
            _currentJump = 0;
        }
    }
    
    
    
    
    public void InputInteract(InputAction.CallbackContext ctx)
    {

        if (ctx.ReadValue<float>() == 1)
        {

            interactBox.enabled = true;
        }
        else
        {

            interactBox.enabled = false;
        }
    }



    public void InputCrouch(InputAction.CallbackContext ctx)
    {
        
        if (ctx.ReadValue<float>() == 1)
        {
            if (!_isCrouched)
            {
                mainCollider.height /= 2;
                mainCollider.center = new Vector3(0, -0.5f,0);
                _isCrouched = true;
            }
            else
            {
                mainCollider.height = mainColHeight;
                mainCollider.center = Vector3.zero;
                _isCrouched = false;
            }
        }

    }






    private bool IsGrounded()
    {
        return Physics.BoxCast(transform.position,

                        groundCheck * 0.5f,

                        -transform.up,

                        out RaycastHit hit,

                        transform.rotation,

                        groundCastDistance,

                        groundLayer);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 centerGround = transform.position - transform.up * groundCastDistance;

        Gizmos.matrix = Matrix4x4.TRS(centerGround, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, groundCheck);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (interactBox.enabled)
        {
            if(other.gameObject.TryGetComponent<IObjectInteractable>(out IObjectInteractable interactable))
            {
                interactable.DoOnInteract();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
       
    }
}
