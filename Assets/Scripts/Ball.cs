using System;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static event Action OnPointCollected;
    public static event Action OnBallDestroyed;


    [SerializeField] int ballDamage = 1;

    [SerializeField] ParticleSystem hittingParticle;
    [SerializeField] ParticleSystem destroyingParticle;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("death"))
        {
            // Spawning and moving in one line. It will be destroyed because of ParticleController after
            Instantiate(destroyingParticle).transform.position = transform.position;

            Destroy(gameObject);
            OnBallDestroyed?.Invoke();
        }

        if (collision.gameObject.CompareTag("peg"))
        {
            Instantiate(hittingParticle).transform.position = transform.position; 

            collision.gameObject.GetComponent<PegBehaviour>().TakeDamage(ballDamage);

            OnPointCollected?.Invoke();
        }
    }
}