using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attacker : MonoBehaviour
{
    public float speed = 5f;
    public float destroyDistance = 40f;
    public float pushBackForce = 10f;
    public float pushUpForce = 10f;  // Adds lift when pushed

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
            pushDirection.y = 0.5f;  // Adds slight upward angle

            rb.isKinematic = false;  // Ensure Rigidbody physics is active
            rb.useGravity = true;

            rb.AddForce(pushDirection * pushBackForce + Vector3.up * pushUpForce, ForceMode.Impulse);
        }

        if (other.CompareTag("GameOverTrigger"))
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
