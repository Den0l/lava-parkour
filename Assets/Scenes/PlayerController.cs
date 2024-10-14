using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 2.0f;
    public float SprintSpeed = 4.0f;
    public float JumpForce = 5.0f;
    public float RotationSmoothing = 20f;
    public GameObject HandMeshes;
    private float pitch, yaw;
    private Rigidbody rb;
    private GameManager _GameManager;
    private bool IsGround;
    public float DistationToGround = 0.1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _GameManager = FindObjectOfType<GameManager>();
    }
    private void Jump()
    {
        rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }

    private void GroundCheck()
    {
        IsGround = Physics.Raycast(transform.position, Vector3.down, DistationToGround);
    }

    private Vector3 CalulateMovement()
    {
        float HorizontalDirection = Input.GetAxis("Horizontal");
        float VerticalDirection = Input.GetAxis("Vertical");
        Vector3 Move = transform.right * HorizontalDirection + transform.forward * VerticalDirection;

        return rb.transform.position + Move * Time.fixedDeltaTime * MovementSpeed;
    }

    private Vector3 CalulateSpeed()
    {
        float HorizontalDirection = Input.GetAxis("Horizontal");
        float VerticalDirection = Input.GetAxis("Vertical");
        Vector3 Move = transform.right * HorizontalDirection + transform.forward * VerticalDirection;

        return rb.transform.position + Move * Time.fixedDeltaTime * SprintSpeed;
    }

    private void FixedUpdate()
    {
        GroundCheck();
        if (Input.GetKey(KeyCode.Space) && IsGround)
        {
            Jump();
        }

        if(Input.GetKey(KeyCode.LeftShift) && !_GameManager.IsStaminaRestroing)
        {
            _GameManager.SpendStamina();
            rb.MovePosition(CalulateSpeed());
        }
        else rb.MovePosition(CalulateMovement());

        SetRotation();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * DistationToGround));
    }

    private void Update()
    {
        if (transform.position.y < -5)
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }

    public void SetRotation()
    {
        yaw += Input.GetAxis("Mouse X");
        pitch -= Input.GetAxis("Mouse Y");

        pitch = Mathf.Clamp(pitch, -60, 90);

        Quaternion SmoothRotation = Quaternion.Euler(pitch, yaw, 0);

        HandMeshes.transform.rotation = Quaternion.Slerp(HandMeshes.transform.rotation, SmoothRotation, 
            RotationSmoothing * Time.fixedDeltaTime);

        SmoothRotation = Quaternion.Euler(0, yaw, 0);

        transform.rotation = Quaternion.Slerp(transform.rotation, SmoothRotation, RotationSmoothing
            * Time.fixedDeltaTime);
    }


}
