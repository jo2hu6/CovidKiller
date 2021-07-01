using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraShot : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int health = 3;
    public bool powerShot;
    [SerializeField] AudioClip deathEnemy;

    void Start()
    {
        Destroy(gameObject, 5);
    }

    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            Invoke("DestroyObject", 5f);
            Destroy(other.gameObject);
        }    
    }

    private void DestroyObject()
    {
        AudioSource.PlayClipAtPoint(deathEnemy, transform.position);
        Destroy(gameObject);
    }
}
