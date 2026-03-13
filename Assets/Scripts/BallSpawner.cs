using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallSpawner : MonoBehaviour
{
    public static event Action OnBallShot;

    public GameObject ball;
    public Transform pivot;
    public Transform shootPoint;
    public float shootStrength = 5f;

    private bool gamePaused = false;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!gamePaused)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            pivot.rotation = Quaternion.Euler(0, 0, -Quaternion.LookRotation(pos - transform.position, Vector3.back).eulerAngles.z);
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (gameManager.GetLeftShotsCount() > 0)
                {
                    pos.z = 0;
                    var newBall = Instantiate(ball, shootPoint.position, Quaternion.identity);
                    newBall.GetComponent<Rigidbody2D>().AddForce((pos - transform.position).normalized * shootStrength);
                    OnBallShot?.Invoke();
                }
                else
                {
                    // TODO: Add the "NoShotsLeft" function to make the sign red or just shake the count 0. to show that no shots left
                }
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
