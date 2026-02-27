using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;
    public float jump;
    public float groundedY;
    public float ledgeRay1;
    public float ledgeRay2;
    public float rayStart;
    public float ledgeRayLength;
    public Vector3 ledgeGrabTarget;
    public float ledgeGrabSpeed;

    bool controls = true;

    Animator animator;
    new Rigidbody2D rigidbody;

    private void Awake() {

        animator = GetComponent<Animator>();

        rigidbody = GetComponent<Rigidbody2D>();

    }

    void Update()
    {

        if (controls) {

            // Move
            transform.Translate(Vector2.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime);

            CheckJump();

            CheckLedgeGrab();

        }

        CheckAnimations();

    }

    void CheckAnimations() {

        if (IsJumpFinished()) {

            if (Input.GetAxis("Horizontal") < 0) {

                animator.Play("PlayerGoLeft");

            }
            else if (Input.GetAxis("Horizontal") > 0) {

                animator.Play("PlayerGoRight");

            }
            else {

                animator.Play("Idle");

            }

        }

    }

    void CheckJump() {

        if (Input.GetButtonDown("Jump") && IsGrounded()) {

            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jump, ForceMode2D.Impulse);

            animator.Play("PlayerJump");

            if (Input.GetAxis("Horizontal") < 0) { animator.Play("PlayerJumpLeft"); }

        }

    }

    void CheckLedgeGrab() {

        float rayStartOriented = rayStart;

        Vector2 orientation = Vector2.right;

        Vector3 targetOriented = ledgeGrabTarget;

        if (Input.GetAxis("Horizontal") < 0) {

            rayStartOriented = -rayStart;

            orientation = -orientation;

            targetOriented.x = -targetOriented.x;

        }


        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(rayStartOriented, ledgeRay1), orientation, ledgeRayLength);

        if (hit1.collider != null) { return; }
        
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(rayStartOriented, ledgeRay2), orientation, ledgeRayLength);

        if (hit2.collider == null) { return; }

        if (!Input.GetButton("Jump")) { return; }


        StartCoroutine(LedgeGrabRoutine(targetOriented));

    }

    public IEnumerator LedgeGrabRoutine(Vector3 targetPosition) {

        // Play your animation

        rigidbody.linearVelocity = Vector2.zero;

        rigidbody.gravityScale = 0;

        controls = false;


        Vector3 targetPositionWorld = transform.position + targetPosition;

        while (transform.position.y < targetPositionWorld.y) {

            transform.Translate(Vector2.up * Time.deltaTime * ledgeGrabSpeed);

            yield return null;

        }
        
        while (targetPosition.x < 0 && transform.position.x > targetPositionWorld.x || targetPosition.x > 0 && transform.position.x < targetPositionWorld.x) {

            if (targetPosition.x <= 0) { transform.Translate(Vector2.left * Time.deltaTime * ledgeGrabSpeed); }

            if (targetPosition.x > 0) { transform.Translate(Vector2.right * Time.deltaTime * ledgeGrabSpeed); }

            yield return null;

        }
        
        rigidbody.linearVelocity = Vector2.zero;

        rigidbody.gravityScale = 1;

        controls = true;

    }

    public bool IsGrounded() {

        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, groundedY), Vector2.down, .1f);

        if (hit.collider != null) {

            return true;

        }

        return false;

    }

    public bool IsJumpFinished() {

        if (!IsGrounded()) { return false; }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump")) { return true; }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < animator.GetCurrentAnimatorStateInfo(0).length) { return false; }

        return true;
    }

    private void OnDrawGizmos() {

        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position + new Vector3(0, groundedY), Vector2.down * .1f);

        Gizmos.color = Color.green;

        Gizmos.DrawRay(transform.position + new Vector3(rayStart, ledgeRay1), Vector2.right * ledgeRayLength);

        Gizmos.DrawRay(transform.position + new Vector3(rayStart, ledgeRay2), Vector2.right * ledgeRayLength);

    }

}