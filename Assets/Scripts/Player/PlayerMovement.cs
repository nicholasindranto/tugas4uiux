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

    // arah gerakannya
    [SerializeField] private Vector3 moveDirection;

    // reference ke input di inputactionnya
    private Vector2 input;

    // seberapa halus rotatenya
    [SerializeField] private float smoothRotateTime = 0.1f;

    // kecepatan sekarang untuk rotatenya
    private float currentVelocity;

    // gravity bumi
    private float gravity = -9.81f;

    // modifier gravity bumi
    [SerializeField] private float gravityModifier = 3.0f;

    // kecepatan dari player turun kebawah
    private float velocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ApplyGravity();

        ApplyRotation();

        ApplyMovement();
    }

    private void ApplyRotation()
    {
        // pertama cek dulu, ada input nggak??
        // dengan cara pakai sqrmagnitude
        if (input.sqrMagnitude == 0) return;

        // untuk rotasi, pakai atan2 untuk ngukur arah rotasinya
        // lalu pakai euler buat nentuin arahnya
        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

        // setelah itu kita bikin smooth pakai smoothdampangle
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothRotateTime);

        // ubah rotationnya
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    private void ApplyMovement()
    {
        // setelah di set arah gerakannya
        // dikali dengan movespeed agar kita bisa atur kecepatan gerakannya 
        // semau kita
        // dikali dengan deltatime agar gerakannya sesuai dengan detik di 
        // dunia nyata
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        // ambil input dari inputaction
        input = context.ReadValue<Vector2>();

        // lalu set arah gerakannya
        moveDirection = new Vector3(input.x, 0, input.y);
    }

    private void ApplyGravity()
    {
        // kalau lagi di tanah sama velocitynya kurang dari 0
        // maka reset
        if (characterController.isGrounded && velocity < 0.0f) velocity = -1.0f;
        // tambahin kecepatan gravitasinya
        else velocity += gravity * gravityModifier * Time.deltaTime;

        // masukin velocity dari gravitynya ke movedirectionnya
        moveDirection.y = velocity;
    }

    // private void JumpControl()
    // {
    //     // kalau teken spasi, maka dia ngejump
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         Vector3 jump = Vector3.up * jumpForce * Time.deltaTime;

    //         characterController.Move(jump);
    //     }
    // }
}
