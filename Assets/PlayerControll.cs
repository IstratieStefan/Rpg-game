using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControll : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    List<RaycastHit2D> castCollisions =new List<RaycastHit2D> ();
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator> ();
    }

    void Update()
    {
        float horizontal=movementInput.x;
        float vertical=movementInput.y;

        animator.SetFloat ("Horizontal",movementInput.x);
        animator.SetFloat ("Vertical",movementInput.y);
        
        if(horizontal==1||horizontal==-1||vertical==1||vertical==-1)
        {
            animator.SetFloat("last_move_x", horizontal);
            animator.SetFloat("last_move_y", vertical);
        }

        if (Input.GetKeyDown(KeyCode.Space))
            Attack();
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
    }
   
    private void FixedUpdate()
    {
        // If movement input is not 0, try to move
        if(movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);

            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));
                
                if(!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
            }

            animator.SetBool("IsMoving", success);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
       //direction of the animation
    }
    private bool TryMove(Vector2 direction)
    {
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            moveSpeed * Time.fixedDeltaTime + collisionOffset);
        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            return false;
        }

    }
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
}
