using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{

    [SerializeField] int addedTime = 10;
    [SerializeField] AudioClip impactWatch;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.time += addedTime;
            AudioSource.PlayClipAtPoint(impactWatch, transform.position);
            Destroy(gameObject, 0.1f);
        }
    }
}
