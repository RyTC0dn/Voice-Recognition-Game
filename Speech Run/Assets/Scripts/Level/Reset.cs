using UnityEngine;

public class Reset : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //Check for player tag and reset position to start position
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position =
                GameManager.Instance.startPos.transform.position;
        }

    }
}