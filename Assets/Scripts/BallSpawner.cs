using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallSpawner : MonoBehaviour
{
    public GameObject ball;
    public Transform pivot;
    public Transform shootPoint;
    public float shootStrength = 5f;

    private bool gamePaused = false;

    // Update is called once per frame
    void Update()
    {
        if (!gamePaused)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            pivot.rotation = Quaternion.Euler(0, 0, -Quaternion.LookRotation(pos - transform.position, Vector3.back).eulerAngles.z);
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                pos.z = 0;
                var newBall = Instantiate(ball, shootPoint.position, Quaternion.identity);
                newBall.GetComponent<Rigidbody2D>().AddForce((pos - transform.position).normalized * shootStrength);
            }
        }
    }

    private void OnEnable()
    {
        GameManager.IsGamePaused += ChangePauseMode;
    }

    private void OnDisable()
    {
        GameManager.IsGamePaused -= ChangePauseMode;
    }


    void ChangePauseMode(bool isPaused)
    {
        gamePaused = isPaused;
    }
}
