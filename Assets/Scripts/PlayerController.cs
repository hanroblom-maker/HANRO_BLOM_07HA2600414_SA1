using System.Collections;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private bool canMove = true;
    private bool isShaking = false;
    private float shakeTimer = 0f;

    private Rigidbody2D rb;
    private bool isGrounded;

    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private Transform checkpoint;
    private Vector3 respawnPosition;

    [SerializeField] private DoorController doorController;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        respawnPosition = checkpoint.position;
    }

    void Update()
    {
        HandleInput();
    }

    private void LateUpdate()
    {
        CameraFollow();
    }

    public void HandleInput()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    // Sourced from: "Controlling Unity Camera Behaviour", Unity Learn
    // https://learn.unity.com/tutorial/controlling-unity-camera-behaviour-2019-3
    // Adapted by: Hanro / Johannes Blom
    // Modified : Made the camera follow a lil slower and smoother, and added camera shake


    [SerializeField] private float cameraFollowSpeed = 5f;
    private void CameraFollow()
    {
        Vector3 targetCameraPosition =
            new Vector3(
                transform.position.x,
                transform.position.y,
                -10f);
        if (shakeTimer > 0)
        {
            targetCameraPosition += new Vector3(
                Random.Range(-5f, 5f),
                Random.Range(-5f, 5f),
                0f);

            shakeTimer -= Time.deltaTime;
        }

        Camera.main.transform.position =
            Vector3.Lerp(
                Camera.main.transform.position,
                targetCameraPosition,
                cameraFollowSpeed * Time.deltaTime);
    }
    public void Move()
    {
        if (!canMove)
        {
            return;
        }

        float move = 0;

        if (Input.GetKey(KeyCode.A))
            move = -1;

        if (Input.GetKey(KeyCode.D))
            move = 1;

        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Key"))
        {
            inventoryManager.AddKey();

            Debug.Log("Key collected");

            Destroy(other.gameObject);
        }

        if (other.CompareTag("Trap"))
        {
            gameManager.TakeDamage();

            shakeTimer = 0.5f;

            StartCoroutine(DamagePause());

            Respawn();
        }

        if (other.CompareTag("Enemy"))
        {
            gameManager.TakeDamage();

            shakeTimer = 0.5f;

            StartCoroutine(DamagePause());

            Respawn();
        }

        if (other.CompareTag("Door"))
        {
            doorController.AttemptExit();
        }
    }

    public IEnumerator DamagePause()
    {
        canMove = false;

        yield return new WaitForSeconds(1f);
        
        canMove = true;
    }

    public void Respawn()
    {
        transform.position = respawnPosition;
    }
}
