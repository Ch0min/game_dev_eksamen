using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float health = 100f;

    [SerializeField] private float moveSpeed = 1;

    public BulletProjectile bulletProjectile;
    public CameraShake cameraShake;
    public BulletClip bulletClip;

    private Animator animator;
    private CharacterController characterController;

    private int currentAmmo;
    private readonly int magazineSize = 30;
    private Camera mainCamera;
    private Vector2 moveVector;
    private int reserveAmmo = 90;
    private Rigidbody rigidBody;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        rigidBody = GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType<Camera>();
        animator = GetComponent<Animator>();
        currentAmmo = magazineSize;
    }

    private void Update()
    {
        Move();
        if (Input.GetButton("Fire1")) Fire();
    }

    private void FixedUpdate()
    {
        var cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        var groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            var pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    public void AddAmmo(int amount)
    {
        reserveAmmo += amount;
    }

    public void ModifyHealth(float amount)
    {
        health += amount;

        if (health > 100) health = 100;
        if (health <= 0) Die();
    }

    public void Die()
    {
        //TODO: add ragdoll instead of Destroy(gameObject)
        Destroy(gameObject);
    }

    public void Fire()
    {
        bulletProjectile.Shoot();
        bulletClip.ShootClip();
        cameraShake.Shake();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();

        if (Keyboard.current != null && Keyboard.current.wKey.wasPressedThisFrame)
            // Debug.Log("D was pressed!");
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);
        // if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame) {
        //     // Debug.Log("D was pressed!");
        //     animator.SetBool("isRolling", true);
        // }
        // else {
        //     animator.SetBool("isRolling", false);
        // }
        /*if(moveVector.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
            
        } */

        if (Keyboard.current != null && Keyboard.current.dKey.wasPressedThisFrame)
            // Debug.Log("D was pressed!");
            animator.SetBool("isRightStrafe", true);
        else
            animator.SetBool("isRightStrafe", false);
        if (Keyboard.current != null && Keyboard.current.aKey.wasPressedThisFrame)
            animator.SetBool("isLeftStrafe", true);
        else
            animator.SetBool("isLeftStrafe", false);
        if (Keyboard.current != null && Keyboard.current.sKey.wasPressedThisFrame)
            animator.SetBool("isBackwards", true);
        else
            animator.SetBool("isBackwards", false);
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            animator.SetBool("isRolling", true);
        else
            animator.SetBool("isRolling", false);
    }

    private void Move()
    {
        var move = transform.right * moveVector.x + transform.forward * moveVector.y;
        characterController.Move(move * moveSpeed * Time.deltaTime);
    }
}