using System;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static event Action OnPointCollected;
    public static event Action OnBallDestroyed;

    [SerializeField] bool isGetSmaller;
    [SerializeField] float ballHealth = 5;
    [SerializeField] int ballDamage = 1;
    private float currentHp;
    private Vector3 startScale;
    private float startTrailWidth;

    [SerializeField] ParticleSystem hittingParticle;
    [SerializeField] ParticleSystem destroyingParticle;

    private void Start()
    {
        currentHp = ballHealth;
        startScale = transform.localScale;
        startTrailWidth = transform.GetChild(1).GetComponent<TrailRenderer>().widthMultiplier;
    }

    void BallDeath()
    {
        Instantiate(destroyingParticle).transform.position = transform.position;

        Destroy(gameObject);
        OnBallDestroyed?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("death"))
        {
            // Spawning and moving in one line. It will be destroyed because of ParticleController after
            BallDeath();
        }

        if (collision.gameObject.CompareTag("peg"))
        {
            Instantiate(hittingParticle).transform.position = transform.position;

            collision.gameObject.GetComponent<PegBehaviour>().TakeDamage(ballDamage);

            OnPointCollected?.Invoke();

            if (isGetSmaller)
            {
                currentHp--;

                float scaleFactor = 1 - (0.1f * (ballHealth - currentHp));
                transform.localScale = startScale * scaleFactor;
                TrailRenderer trail = transform.GetChild(1).GetComponent<TrailRenderer>();
                trail.widthMultiplier = startTrailWidth * scaleFactor;

                if (currentHp <= 0)
                {
                    BallDeath();
                }
            }
            
        }
    }
}