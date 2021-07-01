using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManchScript : MonoBehaviour
{
    float originalSpeed;
    PlayerScript player;
    [SerializeField] float speedReduction = 0.5f;

    void Start()
    {
        player = FindObjectOfType<PlayerScript>();
        originalSpeed = player.speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            player.speed *= speedReduction;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            player.speed = originalSpeed;
        }
    }
}
