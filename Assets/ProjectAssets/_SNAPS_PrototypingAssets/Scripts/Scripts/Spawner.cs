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
            // Move spawner left and right along the z-axis
            Vector3 pos = transform.position;
            pos.z += speed * Time.deltaTime;
            transform.position = pos;

            // Reverse direction when hitting movement boundaries
            if (pos.z < -leftAndRightEdge)
            {
                speed = Mathf.Abs(speed); // Move right
            }
            else if (pos.z > leftAndRightEdge)
            {
                speed = -Mathf.Abs(speed); // Move left
            }
        }
    }

    void FixedUpdate()
    {
        if (isSpawning)
        {
            // Random chance to change direction
            if (Random.value < chanceToChangeDirections)
            {
                speed *= -1;
            }
        }
    }

    void SpawnAttacker()
    {
        // Instantiate attacker at the spawner's position
        Instantiate(attackerPrefab, transform.position, Quaternion.identity);
    }

    // Call this method from PlayerController when 15 pick ups are collected
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            InvokeRepeating("SpawnAttacker", 0f, secondsBetweenSpawns);
        }
    }
}
