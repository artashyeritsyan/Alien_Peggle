using System;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static event Action OnPointCollected;
    public static event Action OnGameOver;

    [SerializeField] AudioSource pegSound;
    [SerializeField] List<AudioClip> pegSounClips;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("death"))
        {
            Destroy(gameObject);
            OnGameOver?.Invoke();
        }

        if (collision.gameObject.CompareTag("peg"))
        {
            collision.gameObject.GetComponent<PegBehaviour>().ChangeSize();

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