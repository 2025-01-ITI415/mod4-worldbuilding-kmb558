using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float pushBackForce = 10f;
    public float pushUpForce = 10f;
    public Text countText;
    public Text winText;

    private Rigidbody rb;
    private int count;

    [Header("Add reference in Inspector")]
    public Spawner spawner;  // ✅ Reference to the Spawner script

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.text = "";

        // ✅ Set RectTransform position, scale up 5x
        RectTransform rt = winText.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);  // center anchor
        rt.anchorMax = new Vector2(0.5f, 0.5f);  // center anchor
        rt.pivot = new Vector2(0.5f, 0.5f);      // center pivot
        rt.anchoredPosition = new Vector2(0, 470);  // X=0, Y=470
        rt.localScale = new Vector3(5f, 5f, 5f);    // scale up 5x
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();

            // ✅ Trigger Spawner activation when 15 pickups collected
            if (count == 15)
            {
                if (spawner != null)
                {
                    spawner.StartSpawning();
                }
                else
                {
                    Debug.LogWarning("Spawner reference not set on PlayerController!");
                }
            }
        }

        // ✅ Detect collision with Spawner
        if (other.gameObject.CompareTag("Spawner"))
        {
            winText.text = "Don't Mess With My Crib!";
            Debug.Log("Player touched Spawner → Don't Mess With My Crib!");
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 15)
        {
            winText.text = "Don't Mess With My Crib!";
        }
    }
}
