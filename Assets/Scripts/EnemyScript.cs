using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    Transform player;
    [SerializeField] int health;
    [SerializeField] float speed;
    [SerializeField] int scorePoints = 100;
    [SerializeField] AudioClip impactClip, damagePlayer;

    private void Start()
    {
        player = FindObjectOfType<PlayerScript>().transform;
        GameObject[] spawnPoint = GameObject.FindGameObjectsWithTag("SpawnPoint");
        int randomSpawnPoint = Random.Range(0, spawnPoint.Length);
        transform.position = spawnPoint[randomSpawnPoint].transform.position;
    }

    private void Update()
    {
        Vector2 direction = player.position - transform.position;
        transform.position += (Vector3)direction.normalized * Time.deltaTime * speed;
    }

    public void TakeDamage()
    {
        health--;
        AudioSource.PlayClipAtPoint(impactClip, transform.position);
        if(health <= 0)
        {
            GameManager.Instance.Score += scorePoints;
            Destroy(gameObject, 0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(damagePlayer, transform.position);
            collision.GetComponent<PlayerScript>().TakeDamagePlayer();
        }
    }
}
