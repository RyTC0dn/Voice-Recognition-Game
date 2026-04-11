using UnityEngine;

public class Reset : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check for player tag and reset position to start position
        if (other.CompareTag("Player"))
        {
            other.transform.position =
                GameManager.Instance.startPos.transform.position;
        }
        else
        {
            Debug.LogWarning("Object with tag " + other.tag + " entered reset trigger.");
        }
    }
}