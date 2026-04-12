using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class BallSpawner : MonoBehaviour
{
    public static event Action OnBallShot;

    public GameObject ball;
    public Transform pivot;
    public Transform shootPoint;
    public float shootStrength = 5f;

    private bool gamePaused = false;

    [SerializeField] AudioSource shootAudioSource;
    [SerializeField] List<AudioClip> shootSounds;
    [SerializeField] AudioClip destroyingSound;

    [SerializeField] ParticleSystem shootingEffect;

    // Update is called once per frame
    void Update()
    {
        if (!gamePaused)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            pivot.rotation = Quaternion.Euler(0, 0, -Quaternion.LookRotation(pos - transform.position, Vector3.back).eulerAngles.z);
            if (Mouse.current.leftButton.wasPressedThisFrame && GameManager.instance.CanShoot())
            {
                if (GameManager.instance.GetShotsLeft() > 0)
                {
                    PlayRandomSound();
                    pos.z = 0;
                    var newBall = Instantiate(ball, shootPoint.position, Quaternion.identity);

                    newBall.GetComponent<Rigidbody2D>().AddForce((pos - transform.position).normalized * shootStrength);
                    OnBallShot?.Invoke();

                    //Spawning and Setting the particle position
                    Instantiate(shootingEffect, shootPoint).transform.localPosition = new Vector2(0, -0.1f);
                }
                else
                {
                    // TODO: Add the "NoShotsLeft" function to make the sign red or just shake the count 0. to show that no shots left
                }
            }
        }
    }

    void PlayRandomSound()
    {
        if (shootSounds.Count > 0 && GameManager.instance.GetIsSoundOn())
        {
            int randomIndex = UnityEngine.Random.Range(0, shootSounds.Count);

            AudioManager.Play(shootSounds[randomIndex]);
        }
    }

    private void OnEnable()
    {
        GameManager.IsGamePaused += ChangePauseMode;
        Ball.OnBallDestroyed += BallDestroyed;
    }

    private void OnDisable()
    {
        GameManager.IsGamePaused -= ChangePauseMode;
        Ball.OnBallDestroyed -= BallDestroyed;
    }

    void ChangePauseMode(bool isPaused)
    {
        gamePaused = isPaused;
    }

    private void BallDestroyed()
    {
        Debug.Log("Ball Destroyed");
        if (GameManager.instance.GetIsSoundOn())
        {
            AudioManager.Play(destroyingSound);
        }
    }

}
