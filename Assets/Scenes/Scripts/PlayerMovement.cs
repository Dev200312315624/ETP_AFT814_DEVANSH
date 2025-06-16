using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);
        if (h != 0f) sr.flipX = (h < 0);

        anim.SetFloat("run", Mathf.Abs(h)); 

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetTrigger("jump");
            isGrounded = false;
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
