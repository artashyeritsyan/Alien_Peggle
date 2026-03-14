using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PegBehaviour : MonoBehaviour
{

    public float duration = 0.6f;
    public Vector2 transformedScale;

    private Vector2 startScale;
    private bool isStartScaling;
    private float elapsedTime = 0f;

    [SerializeField] List<AudioClip> pegSounClips;

    private void Start()
    {
        startScale = transform.localScale;
        //pegSound = FindAnyObjectByType<AudioSource>();
    }

    public void ChangeSize()
    {
        isStartScaling = true;
        gameObject.GetComponent<Collider2D>().enabled = false; // Turn off the collider to not collide second time

        if (pegSounClips.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, pegSounClips.Count);
            //pegSound.clip = pegSounClips[randomIndex];
            //pegSound.Play();

            AudioManager.Play(pegSounClips[randomIndex]);
        }
    }

    void Update()
    {
        if (isStartScaling)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

                transform.localScale = Vector3.Lerp(startScale, transformedScale, t);
                //transform.localScale = Vector3.Lerp(targetScale, startScale, t);

            if (t >= duration)
            {
                Destroy(gameObject);
            }
        }
       
    }

}