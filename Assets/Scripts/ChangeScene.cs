using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] AudioClip buttonClip;
    
    public void LoadBeginScene()
    {
        SceneManager.LoadScene("Game");
        AudioSource.PlayClipAtPoint(buttonClip, transform.position);
    }
}
