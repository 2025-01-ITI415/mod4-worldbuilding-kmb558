using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attacker : MonoBehaviour
{
    public float speed = 5f;
    public float destroyDistance = 40f;
    public float pushBackForce = 100f;  // increased default for stronger knockback
    public float pushUpForce = 20f;     // increased default for more lift

    private Vector3 spawnPosition;
    private Rigidbody rb;

    void Start()
    {
        spawnPosition = transform.position;
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = false;
        rb.useGravity = true;
    }

    void Update()
    {
        transform.position += Vector3.forward * speed * Time.deltaTime;

        if (Vector3.Distance(spawnPosition, transform.position) >= destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Attacker triggered by Player!");

            Vector3 pushDirection = (transform.position - other.transform.position).normalized;
            pushDirection.y = 0.5f;  // slight upward angle

            Vector3 force = pushDirection * pushBackForce + Vector3.up * pushUpForce;

            rb.isKinematic = false;
            rb.useGravity = true;

            rb.AddForceAtPosition(force, other.transform.position, ForceMode.Impulse);
            Debug.DrawRay(other.transform.position, force, Color.red, 1f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("GameOverTrigger"))
        {
            Debug.Log("Attacker collided with GameOverTrigger -> triggering Game Over!");
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
