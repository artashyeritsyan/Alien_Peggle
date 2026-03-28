using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;


public class PegBehaviour : MonoBehaviour
{
    [Header("Scale changing parameters")]
    [SerializeField] float duration = 0.5f;
    [SerializeField] Vector2 transformedScale;

    private Vector2 startScale;
    private bool isStartScaling;
    private float elapsedTime = 0f;
    private bool backToNormalScale;

    [Header("Health Parameters")]
    [SerializeField] int health = 2;

    [Header("Sound Parameters")]
    [SerializeField] List<AudioClip> pegSounClips;
    
    [Header("Particle Parameters")]
    [SerializeField] List<ParticleSystem> particles;
    [SerializeField] Color firstColor;
    [SerializeField] Color secondColor;

    SpriteRenderer sr;
    
    private void Start()
    {
        startScale = transform.localScale;
        //pegSound = FindAnyObjectByType<AudioSource>()
        //transform.GetChild(0).GetComponent<SpriteRenderer>().color = colors[health - 1];

        if (health == 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else if (health == 2)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            gameObject.GetComponent<CircleCollider2D>().radius = 0.6f;
        }
    }

    //void SetColor()
    //{
    //    transform.GetChild(0).GetComponent<SpriteRenderer>().color = colors[health - 1];
    //    //sr.color = colors[health - 1];
    //}

    void BreakCover()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        gameObject.GetComponent<CircleCollider2D>().radius = 0.5f;
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }

  
    void Update() {
        if (isStartScaling)
        {
            float t;

            // Upscaling part
            if (!backToNormalScale)
            {
                elapsedTime += Time.deltaTime;
                t = Mathf.Clamp01(elapsedTime / duration);

                transform.localScale = Vector3.Lerp(startScale, transformedScale, t);

                if (t >= duration) backToNormalScale = true;

                //Destroy(gameObject);
            }
            else // Downscaling part
            {
                elapsedTime -= Time.deltaTime;
                t = Mathf.Clamp01(elapsedTime / duration);
                transform.localScale = Vector3.Lerp(startScale, transformedScale, t);

                if (t <= 0)
                {
                    backToNormalScale = false;
                    isStartScaling = false;
                }
            }

        }

    }

    public void ChangeSize()
    {
        isStartScaling = true;
    }

    public void TakeDamage(int damage)
    {
        PlayRandomSound();
        health -= damage;
        BreakCover();
        ChangeSize();
        if (health <= 0)
        {
            gameObject.GetComponent<Collider2D>().enabled = false; // Turn off the collider to not collide second time
            StartCoroutine(DestroyAfterWait(duration));

        }
        else
        {

            //SetColor();
        }
    }

    IEnumerator DestroyAfterWait(float delay)
    {
        yield return new WaitForSeconds(delay/2);

        if (particles.Count > 0)
        {
            // SPawning Particles
            int randomNumber = UnityEngine.Random.Range(0, particles.Count);
            ParticleSystem particleObj = Instantiate(particles[randomNumber], new Vector2(0, 0), Quaternion.identity);
            particleObj.transform.position = transform.position;

            //Setting the given color to particle
            particleObj.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            var mainModule = particleObj.main;
            mainModule.startColor = firstColor;

            particleObj.Play();

            var systems = particleObj.GetComponentsInChildren<ParticleSystem>();

            foreach (var ps in systems)
            {
                if (ps == particleObj) continue; // skip root

                var main2 = ps.main;
                main2.startColor = secondColor;
            }

            Destroy(particleObj.gameObject, 0.5f);

        }

        Destroy(gameObject);

    }

    void PlayRandomSound()   
    {
        if (pegSounClips.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, pegSounClips.Count);
            //pegSound.clip = pegSounClips[randomIndex];
            //pegSound.Play();

            AudioManager.Play(pegSounClips[randomIndex]);
        }
    }
}