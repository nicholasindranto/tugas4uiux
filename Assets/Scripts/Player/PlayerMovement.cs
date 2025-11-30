using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// membutuhkan charactercontroller component
[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    #region Variables: CharacterController
    // reference ke charactercontroller
    private CharacterController characterController;
    #endregion

    #region Variables: Movement
    // kecepatan gerak
    // public float moveSpeed = 5f;

    // arah gerakannya
    [SerializeField] private Vector3 moveDirection;

    // reference ke input di inputactionnya
    private Vector2 input;

    // reference ke movementnya
    [SerializeField] private Movement movement;
    #endregion

    #region Variables: Rotation
    // kecepatan rotasi playernya
    [SerializeField] private float rotationSpeed = 500f;

    // kecepatan sekarang untuk rotatenya
    // private float currentVelocity;

    // reference ke kameranya
    private Camera mainCamera;
    #endregion

    #region Variables: Gravity
    // gravity bumi
    private float gravity = -9.81f;

    // modifier gravity bumi
    [SerializeField] private float gravityModifier = 3.0f;

    // kecepatan dari player turun kebawah atau naik keatas
    private float velocity;
    #endregion

    #region Variables: Jump and Double Jump
    // kekuatan dia lompat
    [SerializeField] private float jumpForce;

    // jumlah dia udah lompat
    private int numberOfJumps;

    // maximum dia lompat
    [SerializeField] private int maxNumberOfJumps = 2;
    #endregion

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        ApplyRotation();

        ApplyGravity();

        ApplyMovement();
    }

    private void ApplyRotation()
    {
        // pertama cek dulu, ada input nggak??
        // dengan cara pakai sqrmagnitude
        // sqrMagnitude = x*x + y*y
        // kalau gerak maka udah pasi antara 1 atau 2 karna 1*1 + 0*0
        if (input.sqrMagnitude == 0) return;

        // untuk rotasi, pakai atan2 untuk ngukur arah rotasinya
        // lalu pakai euler buat nentuin arahnya
        // atan2 buat ngukur sudut relatif terhadap z forward
        // kalau ke depan udah pasti 0 radian
        //          kanan            phi/2 radian
        //          kiri             -phi/2 radian
        //          belakang         phi atau -phi
        // dikali rad2deg biar berubah ke degree karna quaternion minta degree
        // float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

        // setelah itu kita bikin smooth pakai smoothdampangle
        // transform.eulerAngles.y = ambil rotasi sekarang berapa
        // targetAngle = mau rotasi ke berapa derajat?
        // ref currentVelocity = seberapa cepat perubahan rotasi per framenya
        // smoothRotateTime = mau seberapa cepet? semakin tinggi = semakin lambat
        // float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothRotateTime);

        // ubah rotationnya di sumbu y
        // transform.rotation = Quaternion.Euler(0, angle, 0);

        /*
        logika :
        1. hitung dulu arah gerakannya sesuai arah kamera sekarang
        2. bikin quaternion yang ngarah ke arah tersebut
        3. putar player ke arah yang udah dibikin quaternion
        */

        // ambil arah rotasinya lalu masukin ke movedirectionnya
        // disini kita kalikan input yang kita dapatkan dari inputaction
        // dengan rotasi kamera saat ini tapi yang sumbu y
        // kenapa kok dikali??
        // biar kita bisa ngebikin player menghadap ke arah depannya kamera
        // dengan rumus x = x * cosO + z * sinO
        //              z = -x * sinO + z * consO
        // otomatis arahnya tu nggak yang fix kaya 1, 0, 0 (kanan)
        // atau 0, 0, 1 (depan) tapi malah jadi koma koma
        // kaya 0.9, 0. -0.8 jadi sesuai dengan arah kamera saat ini
        moveDirection = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0) * new Vector3(input.x, 0, input.y);

        // set target rotationnya
        // setelah dapet movedirectionnya berapa, kita bikin target derajatnya
        // kenapa pakai lookrotation? karna fungsi ini tu otomatis bikin
        // player menghadap ke arah movedirectionnya terhadap sumbu z alias
        // vector3 up (0,0,1) nya
        // kalau pakai atan2 sama smoothdampangle gimana? apakah sama aja?
        // sama saja, pakai lookrotation jauh lebih simple
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

        // set rotasi playernya
        // transform.rotation = dari berapa derajat
        // targetRotation = ke berapa derajat
        // rotationSpeed * Time.deltaTime = maksimal berapa derajat perubahannya
        //                                  per framenya
        // kenapa pakai rotatetowards? biar smooth rotasinya
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        // set dulu kecepatannya, sprint apa nggak nya
        float targetSpeed = movement.isSprinting ? movement.speed * movement.multiplier : movement.speed;

        // kita set kecepatan dia sekarang
        // kenapa movetowards? biar halus kecepatan gerakannya
        // ada percepatannya
        // movement.currentSpeed = dari kecepatan berapa?
        // targetSpeed = target kecepatannya berapa?
        // movement.acceleration * Time.deltaTime = batas perubahan kecepatan
        //                                          per framenya
        movement.currentSpeed = Mathf.MoveTowards(movement.currentSpeed, targetSpeed, movement.acceleration * Time.deltaTime);

        // setelah di set arah gerakannya
        // dikali dengan currentspeed agar kita bisa atur kecepatan gerakannya 
        // semau kita
        // dikali dengan deltatime agar gerakannya sesuai dengan detik di 
        // dunia nyata
        characterController.Move(moveDirection * movement.currentSpeed * Time.deltaTime);
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
        // kenapa kok -1? kok nggak 0?
        // biar nggak ada kaya jitter soalnya charactercontroller.isgrounded
        // tu sensitif terhadap angka koma, kaya ada sedikit aja kemiringan 
        // dia sensitif banget + biar stabil juga deteksi isgroundednya
        if (IsGrounded() && velocity < 0.0f) velocity = -1.0f;
        // tambahin kecepatan gravitasinya
        // kenapa kok ditambahin? biar ada sensasi percepatannya
        // pas dia gerak jatuh bebas
        // gravitymodifier kaya pengubah seberapa kuat gravitynya di game
        else velocity += gravity * gravityModifier * Time.deltaTime;

        // masukin velocity dari gravitynya ke movedirectionnya
        // dimasukin ke y biar bisa naik turun pas charactercontroller.move
        moveDirection.y = velocity;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        // context.started = apabila tombol lagi diteken
        // ada juga context.performed = ketika tombol lagi ditahan
        // context.canceled = ketika tombol udah dilepas
        if (!context.started) return; // kalau spasi nggak diteken, skip

        // kalau nggak di ground -> dia lagi lompat -> jangan lompat lagi
        // kalau udah double jump, maka skip juga
        if (!IsGrounded() && numberOfJumps >= maxNumberOfJumps) return;

        // kalau masih belom lompat, maka startcoroutinenya untuk 
        // ngedeteksi lompatnya
        // kenapa kok pas belum lompat (numberofjumps == 0)?
        // biar ngecek udah nyampe di tanahnya tu cuma tepat sekali
        // kalau nggak maka tiap kali lompat maka nunggu terus dia, start
        // coroutine terus dia
        if (numberOfJumps == 0) StartCoroutine(WaitUntilLanding());

        // setiap kali lompat, maka tambah numberofjumps nya
        numberOfJumps++;

        // tinggal disamain aja velocitynya biar dia naik
        // kenapa disamain nggak dijumlahin?
        // biar kaya ada instant boost pas lompat mau itu sekali atau 2 kali
        velocity = jumpForce;
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        // kalau diteken atau di tahan maka dia true, lagi sprint
        movement.isSprinting = context.performed || context.started;
    }

    // coroutine buat nunggu sampai character udah di tanah lagi
    private IEnumerator WaitUntilLanding()
    {
        // tunggu sampe karakternya tu udah di udara udah lompat
        // bener bener udah lompat soalnya kadang isgrounded tu masih true di
        // awal frame
        yield return new WaitUntil(() => !IsGrounded());

        // tunggu sampai dia udah grounded
        yield return new WaitUntil(IsGrounded);

        // baru set numberofjumpsnya ke 0 lagi
        numberOfJumps = 0;
    }

    // getter buat ngambil lagi di ground apa nggak
    private bool IsGrounded() => characterController.isGrounded;
}

[Serializable]
// lagi sprint apa nggak
public struct Movement
{
    // kecepatan, pengalinya, sama percepatannya
    public float speed;
    public float multiplier;
    public float acceleration;

    [HideInInspector] public bool isSprinting;
    [HideInInspector] public float currentSpeed;
}