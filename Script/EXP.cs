using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXP : MonoBehaviour
{
    public float pullSpeed;
    public Transform playerTransform;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GetExp();
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("EXPCollector"))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
                StartCoroutine(PullTowardsPlayer());
            }
        }
    }

    IEnumerator PullTowardsPlayer()
    {
        while (Vector2.Distance(transform.position, playerTransform.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, pullSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
