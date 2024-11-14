using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    public Animator anim;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool faceRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        if(Mathf.Abs(rb.velocity.x) > 0.01f)
        {
            anim.Play("Move");
            if (rb.velocity.x > 0 && !faceRight)
                Flip();
            if (rb.velocity.x < 0 && faceRight)
                Flip();
        }
        else
        {
            anim.Play("Idle");
        }
    }
    public void Flip()
    {
        anim.gameObject.transform.localScale = new Vector3(-anim.gameObject.transform.localScale.x, anim.gameObject.transform.localScale.y, anim.gameObject.transform.localScale.z);
        faceRight = !faceRight;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}