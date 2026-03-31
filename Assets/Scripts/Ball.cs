using System;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static event Action OnPointCollected;
    public static event Action OnBallDestroyed;


    [SerializeField] int ballDamage = 1;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("death"))
        {
            Destroy(gameObject);
            OnBallDestroyed?.Invoke();
        }

        if (collision.gameObject.CompareTag("peg"))
        {
            collision.gameObject.GetComponent<PegBehaviour>().TakeDamage(ballDamage);

            OnPointCollected?.Invoke();

            //if (pegSounClips.Count > 0)
            //{
            //    int randomIndex = UnityEngine.Random.Range(0, pegSounClips.Count);
            //    pegSound.clip = pegSounClips[randomIndex];
            //    pegSound.Play();
            //}
        }
    }
}