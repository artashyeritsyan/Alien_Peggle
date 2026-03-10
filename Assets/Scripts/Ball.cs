using UnityEngine;

public class Ball : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("death"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("peg"))
        {
            //Destroy(collision.gameObject);
            collision.gameObject.GetComponent<PegBehaviour>().ChangeSize();
        }
    }
}
