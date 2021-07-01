using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    float h, v; //se guardara el eje 1.horizontal y 2.vertical de player
    Vector3 moveDirection; //vector que guardara el movimiento
    Vector2 facingDirection; // direccion a la que voltea player
    public float speed; //puedo usar [SerializeField] en vez de public
    [SerializeField] Transform mira;
    [SerializeField] Camera camera;
    [SerializeField] Transform bulletPrefab, ultraBulletPrefab;
    bool gunLoaded = true;
    [SerializeField] float fireRate = 1;
    [SerializeField] int health;
    public Text timeText;
    bool powerShotEnabled = true;
    bool ultraShotEnabled = false;
    [SerializeField] bool invulnerability;
    [SerializeField] float invulnerabilityTime = 3;
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float blinkRate = 1;
    [SerializeField] AudioClip deathClip, powerClip;
    CameraController camController;
    bool isPaused = false;

    public int Health
    {
        get => health;
        set 
        {
            health = value;
            UIManager.Instance.UpdateUIHealth(health);
        }
    }

    void Start()
    {
        UIManager.Instance.UpdateUIHealth(health);
        camController = FindObjectOfType<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {

        //MOVIMIENTO DEL JUGADOR
        h = Input.GetAxis("Horizontal"); //extrae la info de las letras A y D, movimiento horizontal
        v = Input.GetAxis("Vertical"); //extrae la info de las letras A y D, movimiento vertical

        moveDirection.x = h;
        moveDirection.y = v; //asignamos la posicion h y v al moveDirection para que pueda moverse

        transform.position += moveDirection * Time.deltaTime * speed; //vamos sumando el valor de la posicion para que no vuelva a su mismo lugar
        
        //MOVIMIENTO DE LA MIRA
        facingDirection = camera.ScreenToWorldPoint(Input.mousePosition) - transform.position; //seteo a la mira para que siga al puntero del mouse
        mira.position = transform.position + (Vector3)facingDirection.normalized;

        //CREACION DE LA BALA EN LA POSICION DEL PLAYER
        if(Input.GetMouseButton(0) && gunLoaded)
        {
            gunLoaded = false;
            float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg; //angulo convertido a grados
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward); //rotacion
            Transform bulletClone = Instantiate(bulletPrefab, transform.position, targetRotation);
            
            if(powerShotEnabled && !ultraShotEnabled)
            {
                bulletClone.GetComponent<BulletScript>().powerShot = true;
            }
            
            if(ultraShotEnabled)
            {
                StartCoroutine(UltraShotRoutine());
            }
            StartCoroutine(ReloadGun());
        }

        anim.SetFloat("Speed", moveDirection.magnitude);

        if(mira.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }else if(mira.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

    }


    IEnumerator UltraShotRoutine()
    {
        gunLoaded = false;
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg; //angulo convertido a grados
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward); //rotacion
        Transform ultraBulletClone = Instantiate(ultraBulletPrefab, transform.position, targetRotation);
        ultraBulletClone.GetComponent<UltraShot>().powerShot = true;
        yield return new WaitForSeconds(5f);
        ultraShotEnabled = false;
    }


    IEnumerator InvulnerabilityDisabled()
    {
        StartCoroutine(BlinkRoutine());
        yield return new WaitForSeconds(invulnerabilityTime);
        invulnerability = false;
    }

    IEnumerator BlinkRoutine()
    {
        int t = 10;
        while(t > 0)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(t * blinkRate);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(t * blinkRate);
            t--;
        }
    }

    IEnumerator ReloadGun()
    {
        yield return new WaitForSeconds(1/fireRate);
        gunLoaded = true;
    }

    public void TakeDamagePlayer()
    {
        if(invulnerability)
            return;

        Health--;
        invulnerability = true;
        fireRate = 1;
        powerShotEnabled = false;
        camController.Shake();
        StartCoroutine(InvulnerabilityDisabled());
        
        if(!isPaused)
        {
            if(Health <= 0)
            {

                GameManager.Instance.gameOver = true;
                UIManager.Instance.ShowGameOverScreen();
                UIManager.Instance.SpawnPointsDisabled(true);
                UIManager.Instance.PlayerDisabled(true);
                Camera.main.GetComponent<AudioSource>().Stop();
                AudioSource.PlayClipAtPoint(deathClip, transform.position);
                Time.timeScale = 0;
                isPaused = true;
                timeText.text = "0";
            }
                
        }
        
        if(isPaused)
        {
            Time.timeScale = 1;
            UIManager.Instance.SpawnPointsDisabled(false);
            UIManager.Instance.PlayerDisabled(false);
            isPaused = false;
        }   
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PowerUp"))
        {
            switch(other.GetComponent<PowerUpScript>().powerUpType)
            {
                case PowerUpScript.PowerUpType.FireRateIncrease:
                    fireRate++;
                    AudioSource.PlayClipAtPoint(powerClip, transform.position);
                    break;
                case PowerUpScript.PowerUpType.UltraShot:
                    ultraShotEnabled = true;
                    AudioSource.PlayClipAtPoint(powerClip, transform.position);
                    break;
                case PowerUpScript.PowerUpType.RecoverHealth:
                    health++;
                    UIManager.Instance.UpdateUIHealth(health);
                    AudioSource.PlayClipAtPoint(powerClip, transform.position);
                    break;
            }
            Destroy(other.gameObject, 0.1f);
        }
    }

}
