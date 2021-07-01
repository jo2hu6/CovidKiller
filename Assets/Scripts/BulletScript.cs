using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] int health = 3;
    public bool powerShot;

    void Start()
    {
        Destroy(gameObject, 5);
    }

    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyScript>().TakeDamage();
            if(!powerShot)
                Destroy(gameObject);
            
            health--;
            if(health <= 0)
                Destroy(gameObject);
        }
    }
}
