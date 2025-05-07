using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject attackerPrefab; // Prefab for instantiating attackers

    public float speed = 1f; // Speed at which the Spawner moves
    public float leftAndRightEdge = 10f; // Movement boundary
    public float chanceToChangeDirections = 0.1f; // Chance to change direction
    public float secondsBetweenSpawns = 1f; // Spawn rate

    private bool isSpawning = false; // Flag to control spawning

    void Update()
    {
        if (isSpawning)
        {
            Vector3 pos = transform.position;
            pos.x += speed * Time.deltaTime;
            transform.position = pos;

            if (pos.x < -leftAndRightEdge)
            {
                speed = Mathf.Abs(speed); // Move right
                Debug.Log("Spawner hit left boundary → reversing right.");
            }
            else if (pos.x > leftAndRightEdge)
            {
                speed = -Mathf.Abs(speed); // Move left
                Debug.Log("Spawner hit right boundary → reversing left.");
            }
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
            spawnPos.y += 5f;  // ✅ raise Y by 5
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
