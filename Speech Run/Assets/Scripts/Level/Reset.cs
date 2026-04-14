using UnityEngine;

public class Reset : MonoBehaviour
{
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