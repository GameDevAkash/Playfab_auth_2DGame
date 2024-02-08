using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float jumpHeight;
    private Rigidbody2D rb;
    [SerializeField] private bool grounded;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] Animator playerAnim;
    [SerializeField] int jumpCount;
    [SerializeField] public int TotalBoostCount;
    [SerializeField] private float BoostTimer;
    [SerializeField] private bool BoostPressing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = 0;
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && grounded)
        {
            rb.velocity = new Vector2(0, jumpHeight);
            jumpCount += 1;
        }
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && jumpCount<1 && !grounded)
        {
            rb.velocity = new Vector2(0, 7);
            jumpCount += 1;
        }
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !grounded)
        {
            rb.velocity = new Vector2(0, -jumpHeight);
        }
        transform.Translate(moveSpeed * Time.deltaTime, 0f, 0f);

        JetSpeedBoostFunctionality();
        GroundCheck();
        if(TotalBoostCount >= 0)
            UIHandler.Singleton.JetPowerUp.text = TotalBoostCount.ToString();
    }

    private void JetSpeedBoostFunctionality()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            TotalBoostCount -= 1; JetPowerUp();
        }
        if (Input.GetKey(KeyCode.B))
        {
            BoostTimer += Time.deltaTime;
            if (BoostTimer > 5f) { moveSpeed = 4; }
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            moveSpeed = 4;
            BoostTimer = 0;
        }
    }

    public void OnBoostClick()
    {
        if (TotalBoostCount <= 0) { return; }
        BoostPressing = true;
        TotalBoostCount -= 1;
        moveSpeed = 20;
        rb.gravityScale = 0;
        Invoke(nameof(StopBoost), 2f);
    }

    public void StopBoost()
    {
        BoostPressing = false;
        moveSpeed = 4;
        rb.gravityScale = 1;
    }

    public void Jump()
    {
        if (jumpCount > 0) { return; }
        if (BoostPressing) { return; }
        rb.velocity = new Vector2(0, jumpHeight);
        jumpCount += 1;
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        if (Physics2D.Raycast(transform.position, Vector2.down, 2f, groundLayer))
        {
            jumpCount = 0;
            grounded = true;
            playerAnim.enabled = true;
            //moveSpeed = 4;
        }
        else
        {
            grounded = false;
            playerAnim.enabled = false;
            //moveSpeed = 3;
        }
    }

    private bool IsBoostAvailable()
    {
        if(TotalBoostCount < 0 && BoostTimer < 5f) { return false; }
        else { return true; }
    }

    private void JetPowerUp()
    {
        if (!IsBoostAvailable()) { moveSpeed = 4; return; }
        moveSpeed = 8;
    }
}
