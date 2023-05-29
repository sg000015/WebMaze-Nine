using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Animator animator;
    private CharacterController controller;

    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] float gravityValue = -9.8f;
    [SerializeField] float jumpHeight = 0.3f;
    [SerializeField] float jumpDelay = 2.0f;


    Vector3 jumpVelocity;

    float jumpTimer = 0;
    float animTimer = 0;
    bool isAnimated;

    [SerializeField] Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        GameManager.Inst.OnStatusUpdate += SetMoveValue;
        GameManager.Inst.OnCanMove += (can) =>
        {
            if (!can)
            {
                animator.SetBool("jumpFlag", false);
                animator.SetBool("walkFlag", false);
                animator.SetBool("idleFlag", true);
            }
        };
    }

    void Update()
    {
        if (!GameManager.Inst.CanMove) { return; }

        ThirdPersonMove();
    }

    public void SetMoveValue()
    {
        moveSpeed = 1.5f + GameManager.Inst.dataManager.UPGRADE_SPEED * 0.5f;
        jumpHeight = 0.25f + GameManager.Inst.dataManager.UPGRADE_JUMP * 0.05f;
    }



    void ThirdPersonMove()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
        }

        if (controller.isGrounded)
        {
            jumpTimer = 0;
            if (jumpVelocity.y < 0)
            {
                jumpVelocity.y = 0f;
            }
        }

        if (Input.GetButtonDown("Jump") && jumpTimer <= 0)
        {
            SoundManager.Inst.PlaySfx(1);
            jumpTimer = 1.5f;
            jumpVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.SetBool("jumpFlag", true);
            animator.SetBool("walkFlag", false);
            animator.SetBool("idleFlag", false);
        }
        else if (move != Vector3.zero)
        {
            animator.SetBool("jumpFlag", false);
            animator.SetBool("walkFlag", true);
            animator.SetBool("idleFlag", false);
        }
        else
        {
            animator.SetBool("jumpFlag", false);
            animator.SetBool("walkFlag", false);
            animator.SetBool("idleFlag", true);
            // animTimer += Time.deltaTime;
            // if (animTimer >= 5)
            // {
            //     animator.SetBool("jumpFlag", false);
            //     animator.SetBool("walkFlag", false);
            //     animator.SetBool("idleFlag", false);
            //     animator.SetTrigger("idleBFlag");
            //     animTimer = 0;
            // }
            // else
            // {
            //     animator.SetBool("jumpFlag", false);
            //     animator.SetBool("walkFlag", false);
            //     animator.SetBool("idleFlag", true);
            // }
        }

        float value = Mathf.Max(Mathf.Abs(move.x), Mathf.Abs(move.z));
        move = mainCam.transform.forward * move.z + mainCam.transform.right * move.x;
        move.y = 0f;

        controller.Move(transform.forward * value * Time.deltaTime * moveSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..

        jumpVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(jumpVelocity * Time.deltaTime);


    }
}
