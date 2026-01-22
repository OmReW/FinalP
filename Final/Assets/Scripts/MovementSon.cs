using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class MovementSon : MonoBehaviour
{
    [Header("Ayarlar")]
    public float laneSwitchSpeed = 10.0f;
    public float forwardSpeed;
    private int currentLane = 0; // 0, 1, 2, 3 (4 lane)
    private float targetX;

    // 4 lane pozisyonları: -6, -2, 2, 6
    private float[] lanePositions = new float[] { -6f, -2f, 2f, 6f };
    public int Health = 3;
    
    [Header("Ses Efektleri")]
    public AudioClip obstacleHitSound;
    public AudioClip boosterSound;

    void Start()
    {
        // Başlangıçta lane 0'da (-6 pozisyonunda)
        currentLane = 0;
        targetX = lanePositions[currentLane];

        // Rigidbody kontrolü
        if (GetComponent<Rigidbody>() == null)
        {
            Debug.LogError("Player'da Rigidbody yok! Ekleyin!");
        }

        // Collider kontrolü
        if (GetComponent<Collider>() == null)
        {
            Debug.LogError("Player'da Collider yok! Ekleyin!");
        }
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.leftArrowKey.wasPressedThisFrame || keyboard.aKey.wasPressedThisFrame)
        {
            ChangeLane(-1);
        }
        else if (keyboard.rightArrowKey.wasPressedThisFrame || keyboard.dKey.wasPressedThisFrame)
        {
            ChangeLane(1);
        }

        // X pozisyonunu hedef lane'e kaydır
        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSwitchSpeed * Time.deltaTime);

        // İleri hareket
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // Hız 0 olursa oyun biter
        if (Health == 0)
        {
            SceneManager.LoadScene(2);
        }
    }

    void ChangeLane(int direction)
    {
        currentLane += direction;

        // Lane sınırlaması: 0 ile 3 arası (4 lane)
        currentLane = Mathf.Clamp(currentLane, 0, 3);

        // Hedef X pozisyonunu güncelle
        targetX = lanePositions[currentLane];

        Debug.Log("Şu anda lane: " + currentLane + " | Hedef X: " + targetX);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bir şeye çarptı: " + other.gameObject.name + " | Tag: " + other.tag);

        if (other.CompareTag("obstacle"))
        {
            Health--;

            // Ses çal
            if (obstacleHitSound != null)
            {
                AudioSource.PlayClipAtPoint(obstacleHitSound, other.transform.position, 1f);
                Debug.Log("Obstacle sesi çalıyor!");
            }
            else
            {
                Debug.LogWarning("Obstacle Hit Sound atanmamış!");
            }

            Destroy(other.gameObject);
            Debug.Log("Engele çarpıldı! Kalan hız: " + forwardSpeed);
        }
        else if (other.CompareTag("booster"))
        {
            forwardSpeed++;

            // Ses çal
            if (boosterSound != null)
            {
                AudioSource.PlayClipAtPoint(boosterSound, other.transform.position, 1f);
                Debug.Log("Booster sesi çalıyor!");
            }
            else
            {
                Debug.LogWarning("Booster Sound atanmamış!");
            }

            Destroy(other.gameObject);
            Debug.Log("Booster alındı! Yeni hız: " + forwardSpeed);
        }
    }
}