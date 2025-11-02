using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// membutuhkan charactercontroller component
[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    // reference ke charactercontroller
    private CharacterController characterController;

    // kecepatan gerak
    public float moveSpeed = 5f;

    // gravitasi
    public float gravity = -9.8f;

    // kekuatan jump
    public float jumpForce = 2f;

    // input dari input actionnya
    [SerializeField] private Vector2 input;

    [SerializeField] private Vector3 movement;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        characterController.Move(movement);
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();

        movement = new Vector3(input.x, 0, input.y) * moveSpeed * Time.deltaTime;
    }

    private void GravityPullControl()
    {
        // gravitasinya
        // kalau nggak nyentuh tanah, maka tarik, kalau udah nyentuh
        // maka nggak usah tarik
        Vector3 gravityPull;
        if (characterController.isGrounded) gravityPull = Vector3.zero;
        else gravityPull = Vector3.up * gravity * Time.deltaTime;

        characterController.Move(gravityPull);
    }

    private void JumpControl()
    {
        // kalau teken spasi, maka dia ngejump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 jump = Vector3.up * jumpForce * Time.deltaTime;

            characterController.Move(jump);
        }
    }
}
