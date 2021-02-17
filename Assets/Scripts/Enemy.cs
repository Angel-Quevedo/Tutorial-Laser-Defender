using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject enemyLaserPrefab;
    [SerializeField] GameObject enemyExplosionParticles;
    [SerializeField] AudioClip audioClipShoot;
    [SerializeField] AudioClip audioClipDie;
    [SerializeField] float laserMoevementSpeed = 10;
    [SerializeField] float laserYPadding = 0.5f;
    [SerializeField] float health = 100;
    [SerializeField] float ShotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] int points;

    // Start is called before the first frame update
    void Start()
    {
        ShotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        ShotCounter -= Time.deltaTime;
        if (ShotCounter <= 0)
        {
            Fire();
            ShotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        var enemyLaser = Instantiate(enemyLaserPrefab, transform.position - new Vector3(0, laserYPadding), quaternion.identity);
        enemyLaser.GetComponent<Rigidbody2D>().velocity = Vector2.down * laserMoevementSpeed;
        AudioSource.PlayClipAtPoint(audioClipShoot, Camera.main.transform.position, 0.25f);
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
        GameSession.Instance.AddToScore(points);
        Destroy(gameObject);

        AudioSource.PlayClipAtPoint(audioClipDie, Camera.main.transform.position, 0.5f);

        var explosionParticles = Instantiate(enemyExplosionParticles, transform.position, Quaternion.identity);
        Destroy(explosionParticles, 1f);

    }
}
