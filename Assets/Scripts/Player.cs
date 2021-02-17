using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float shipMovementSpeed;
    [SerializeField] float laserMovementSpeed;
    [SerializeField] int health = 200;
    [SerializeField] AudioClip audioClipShoot;
    [SerializeField] AudioClip audioClipDie;

    [SerializeField] float projectileFiringPeriod = 0.1f;
    [SerializeField] float laserYPadding;
    [SerializeField] float extraXPadding = 1f;
    [SerializeField] float extraYPadding = 1f;
    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;


    Coroutine firingCoroutine;

    float minXPos;
    float maxXPos;
    float minYPos;
    float maxYPos;
    float spritreWidth;
    float spriteHeight;


    // Start is called before the first frame update
    void Start()
    {
        GetSpriteSize();
        SetUpMovementBoundaries();
    }

    private void GetSpriteSize()
    {
        spritreWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        spriteHeight = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    private void SetUpMovementBoundaries()
    {
        var xPadding = (spritreWidth / 2f) + extraXPadding;
        var yPadding = (spriteHeight / 2f) + extraYPadding;

        Camera mainCamera = Camera.main;

        minXPos = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + xPadding;
        maxXPos = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - xPadding;

        minYPos = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + yPadding;
        maxYPos = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - yPadding;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1") && firingCoroutine == null)
        {
            if (firingCoroutine == null)
                firingCoroutine = StartCoroutine(FireContinuosly());
        }

        if (Input.GetButtonUp("Fire1") && firingCoroutine != null)
        {
            //StopAllCoroutines();
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    IEnumerator FireContinuosly()
    {
        while (true)
        {
            var laser = Instantiate(laserPrefab, transform.position + new Vector3(0, laserYPadding), Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = Vector2.up * laserMovementSpeed;
            AudioSource.PlayClipAtPoint(audioClipShoot, Camera.main.transform.position, 0.25f);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxisRaw("Horizontal") * Time.deltaTime * shipMovementSpeed;
        var deltaY = Input.GetAxisRaw("Vertical") * Time.deltaTime * shipMovementSpeed;

        if (deltaX != 0 && deltaY != 0)
        {
            deltaX /= Mathf.Sqrt(2);
            deltaY /= Mathf.Sqrt(2);
        }

        var newXPos = transform.position.x + deltaX;
        var newYPos = transform.position.y + deltaY;

        transform.position = new Vector2(Mathf.Clamp(newXPos, minXPos, maxXPos), Mathf.Clamp(newYPos, minYPos, maxYPos));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer)
            return;

        ProcecsHit(damageDealer);
    }

    private void ProcecsHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(audioClipDie, Camera.main.transform.position);
        FindObjectOfType<LvlManager>().LoadGameOver();
    }

    public int GetHealth()
    {
        return health;
    }
}
