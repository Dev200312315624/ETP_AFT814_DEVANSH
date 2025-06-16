using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f, jumpForce = 12f, dashDistance = 5f, dashTime = 0.2f;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    bool isGrounded, isDashing;
    float dashStartTime;
    Vector2 dashStartPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");

        if (isDashing)
        {
            float t = (Time.time - dashStartTime) / dashTime;
            rb.position = Vector2.Lerp(dashStartPos, dashStartPos + Vector2.right * (sr.flipX ? -dashDistance : dashDistance), t);
            if (t >= 1f)
                isDashing = false;
        }
        else
        {
            rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);
            if (h != 0f) sr.flipX = (h < 0);
            anim.SetFloat("run", Mathf.Abs(h));

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                anim.SetTrigger("jump");
                isGrounded = false;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                isDashing = true;
                dashStartTime = Time.time;
                dashStartPos = rb.position;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        foreach (var c in col.contacts)
            if (c.normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        isGrounded = false;
    }
}
