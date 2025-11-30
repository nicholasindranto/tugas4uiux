using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    // targetnya ke player
    [SerializeField] private Transform player;

    // offset alias batas cameranya / jarak dengan player
    private float distanceToPlayer;

    // biar smooth gerakan kameranya
    // [SerializeField] private float smoothTime;

    // kecepatan kameranya sekarang
    // private Vector3 currentVelocity = Vector3.zero;

    // ambil input dari inputactionnya
    private Vector2 mouseDirection;

    // reference ke mouse sensitivitynya
    [SerializeField] private MouseSensitivity mouseSensitivity;

    // reference ke rotasi kameranya
    private CameraRotation cameraRotation;

    // reference ke batas maximum dan minimum angle kameranya
    [SerializeField] private CameraAngle cameraAngle;

    private void Awake()
    {
        // isi jarak ke playernya dengan posisi kamera dikurangi
        // posisi si playernya
        // offset = transform.position - player.position;
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
    }

    private void Update()
    {
        // bikin rotasi yaw (rotasi y kamera) ditambahin mousedirection.x nya
        // biar dia rotatenya ke kanan kiri
        // lalu dikali dengan sensitivitynya, biar bisa diubah sesuka hati
        // kati deltatime biar konstan perubahannya
        cameraRotation.yaw += mouseDirection.x * mouseSensitivity.horizontal * GetValueInvert(mouseSensitivity.invertHorizontal) * Time.deltaTime;

        // ini yang atas sama bawahnya
        cameraRotation.pitch += mouseDirection.y * mouseSensitivity.vertical * GetValueInvert(mouseSensitivity.invertVertical) * Time.deltaTime;

        // dibatasin angle atas bawahnya
        cameraRotation.pitch = Mathf.Clamp(cameraRotation.pitch, cameraAngle.min, cameraAngle.max);
    }

    public void Look(InputAction.CallbackContext context)
    {
        // ambil dari inputactionnya set ke mousedirectionnya
        mouseDirection = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // gerakin kameranya disini biar smooth
        // ambil dulu target position si kameranya dengan nambahin posisi
        // player dengan offset
        // Vector3 targetPos = player.position + offset;

        // gerakin pakai smoothdamp biar halus
        // penjelasaannya sama persis dengan yang rotation pakai
        // smoothdampangle
        // transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref currentVelocity, smoothTime);

        // rotasi kameranya pakai eulerangle
        transform.eulerAngles = new Vector3(cameraRotation.pitch, cameraRotation.yaw, 0);

        // untuk posisinya tinggal kita kurangin dengan distancetoplayernya
        // kenapa kok transform.forward?
        // ngambil arah depannya lalu dikurangin biar posisi kamera dibelakang
        // playernya
        transform.position = player.position - transform.forward * distanceToPlayer;
    }

    private static int GetValueInvert(bool invert) => invert ? 1 : -1;
}

// bikin struct yang bakalan nyimpen mouse sensitivitynya
[Serializable]
public struct MouseSensitivity
{
    // isinya adalah horiz sama vertinya
    public float horizontal;
    public float vertical;

    // di invert apa nggak
    public bool invertHorizontal;
    public bool invertVertical;
}

// struct lagi buat rotasi kameranya
public struct CameraRotation
{
    // pitch itu rotasi di sumbu x
    // yaw itu rotasi di sumbu y
    public float pitch;
    public float yaw;
}

[Serializable]
// struct buat batas maksimum dan minimum angle cameranya
public struct CameraAngle
{
    public float min;
    public float max;
}