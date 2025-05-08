using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject attackerPrefab;

    public float speed = 1f; // Spawner movement speed
    public float leftEdge = 403f; // Left boundary X position
    public float rightEdge = 544f; // Right boundary X position
    public float chanceToChangeDirections = 0.1f;
    public float secondsBetweenSpawns = 1f;

    private bool isSpawning = false;

    void Start()
    {
        // Optional: clamp initial position within allowed range
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, leftEdge, rightEdge);
        transform.position = pos;
    }

    void Update()
    {
        if (isSpawning)
        {
            Vector3 pos = transform.position;
            pos.x += speed * Time.deltaTime;

            // Reverse and clamp if beyond left or right edges
            if (pos.x < leftEdge)
            {
                speed = Mathf.Abs(speed); // move right
                pos.x = leftEdge;
                Debug.Log("Spawner hit left boundary -> reversing right.");
            }
            else if (pos.x > rightEdge)
            {
                speed = -Mathf.Abs(speed); // move left
                pos.x = rightEdge;
                Debug.Log("Spawner hit right boundary -> reversing left.");
            }

            transform.position = pos;
        }
    }

    void FixedUpdate()
    {
        if (isSpawning)
        {
            if (Random.value < chanceToChangeDirections)
            {
                speed *= -1;
                Debug.Log("Spawner randomly changed direction.");
            }
        }
    }

    void SpawnAttacker()
    {
        Debug.Log("SpawnAttacker() called!");

        if (attackerPrefab != null)
        {
            Vector3 spawnPos = transform.position;
            spawnPos.y += 10f;  // Offset up
            spawnPos.z += 10f;  // Offset forward

            Instantiate(attackerPrefab, spawnPos, Quaternion.identity);
            Debug.Log("Attacker instantiated at: " + spawnPos);
        }
        else
        {
            Debug.LogWarning("attackerPrefab is NOT assigned in Inspector!");
        }
    }

    public void StartSpawning()
    {
        Debug.Log("StartSpawning() was called!");

        if (!isSpawning)
        {
            isSpawning = true;
            Debug.Log("Spawning activated! Starting InvokeRepeating...");
            InvokeRepeating("SpawnAttacker", 0f, secondsBetweenSpawns);
        }
        else
        {
            Debug.Log("StartSpawning() called but isSpawning was already true.");
        }
    }
}
