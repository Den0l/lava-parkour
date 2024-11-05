using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 2.0f;
    public float SprintSpeed = 4.0f;
    public float JumpForce = 5.0f;
    public float RotationSmoothing = 20f;
    public GameObject[] WeaponInventory;
    public GameObject[] WeaponMeshes;
    private int SelectedWeaponId = 0;
    public Weapon _Weapon;
    public GameObject HandMeshes;
    private float pitch, yaw;
    private Rigidbody rb;
    private GameManager _GameManager;
    private bool IsGround;
    public float DistationToGround = 0.1f;
    private AnimationManager _AnimationManager;
    private bool IsSprinting = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _GameManager = FindObjectOfType<GameManager>();
        _Weapon = WeaponInventory[SelectedWeaponId].GetComponent<Weapon>();
        WeaponMeshes[SelectedWeaponId].SetActive(true);
        _AnimationManager = WeaponMeshes[SelectedWeaponId].GetComponent<AnimationManager>();
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
        IsSprinting = true;
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

        if(Input.GetKey(KeyCode.Mouse0))
        {
            _Weapon.Fire();
            _AnimationManager.SetAnimationFire();
        }

        if (Input.GetKey(KeyCode.R))
        {
            _Weapon.Reload();
            _AnimationManager.SetAnimationReload();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0) 
        {
            SelectNextWeapon();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            SelectPrevWeapon();
        }

        if (Input.GetKey(KeyCode.LeftShift) && !_GameManager.IsStaminaRestroing)
        {
            _GameManager.SpendStamina();
            rb.MovePosition(CalulateSpeed());
        }
        else rb.MovePosition(CalulateMovement());

        SetRotation();

        SetAnimation(); 
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

    private void SelectPrevWeapon()
    {
        if(SelectedWeaponId != 0)
        {
            WeaponMeshes[SelectedWeaponId].SetActive(false);
            SelectedWeaponId -= 1;
            _Weapon = WeaponInventory[SelectedWeaponId].GetComponent<Weapon>();
            WeaponMeshes[SelectedWeaponId].SetActive(true);
            Debug.Log("Оружие: " + _Weapon.WeaponType);
        }
    }

    private void SelectNextWeapon()
    {
        if(WeaponInventory.Length > SelectedWeaponId + 1)
        {
            WeaponMeshes[SelectedWeaponId].SetActive(false);
            SelectedWeaponId += 1;
            _Weapon = WeaponInventory[SelectedWeaponId].GetComponent<Weapon>();
            WeaponMeshes[SelectedWeaponId].SetActive(true);
            Debug.Log("Оружие: " + _Weapon.WeaponType);
        }
    }

    public void AddWeaponToInventory(GameObject pickedWeapon, GameObject mash)
    {

        pickedWeapon.SetActive(true);

        List<GameObject> weaponList = new List<GameObject>(WeaponInventory);
        weaponList.Add(pickedWeapon);
        WeaponInventory = weaponList.ToArray();

        List<GameObject> weaponMeshesList = new List<GameObject>(WeaponMeshes);
        weaponMeshesList.Add(mash);
        WeaponMeshes = weaponMeshesList.ToArray();

        Debug.Log("Оружие добавлено в инвентарь: " + pickedWeapon.name);


    }

    private bool IsMoving()
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }

    private void SetAnimation()
    {
        if(IsMoving())
        {
            if(IsSprinting)
            {
                _AnimationManager.SetAnimationRun();
            }
            else
            {
                _AnimationManager.SetAnimationWalk();
            }
        }
        else
        {
            _AnimationManager.SetAnimationIdle();
        }
    }

}
