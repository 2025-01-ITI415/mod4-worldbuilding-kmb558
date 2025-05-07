using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attacker : MonoBehaviour
{
    public float speed = 5f;
    public float destroyDistance = 40f;
    public float pushBackForce = 10f;
    public float pushUpForce = 10f;  // ✅ Made stronger for more "fly" effect

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
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (Vector3.Distance(spawnPosition, transform.position) >= destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 pushDirection = (transform.position - collision.transform.position).normalized;
            pushDirection.y = 0.5f;  // ✅ Add some vertical to push for more "flying" feel

            rb.AddForce(pushDirection * pushBackForce + Vector3.up * pushUpForce, ForceMode.Impulse);
        }

        if (collision.gameObject.CompareTag("GameOverTrigger"))
        {
            GameOver();
        }

        void GameOver()
        {
            Debug.Log("Game Over!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
