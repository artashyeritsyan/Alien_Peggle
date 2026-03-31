using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] float delay = 0.5f;

    void Start()
    {
        Destroy(gameObject, delay);
    }

    public float GetDelay()
    {
        return delay;
    }

    public void SetDelay(float newDelay)
    {
        delay = newDelay;
    }
}
