/*****************************************************************************
// File Name : Comments.cs
// Author : Alexander R. Safranek
// Creation Date : January 27, 2026
// Last Updated : 3/25/2026 
// Brief Description : Handles player movement, camera look, fall/lives, 1-min timer, and Game Over.
*****************************************************************************/
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// 3D Player movement, lives, timer handling.
/// </summary>
public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    private Vector3 playerMovement;
    private Rigidbody rb;
    public bool isGrappling;

    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 200f;
    [SerializeField] private Transform cameraTarget;

    private float xRotation = 0f;
    private Vector2 lookInput;

    private InputAction move;
    private InputAction look;

    [Header("Fall Death / Lives")]
    [SerializeField] private float deathHeight = -10f;
    [SerializeField] private Transform respawnPoint;

    [SerializeField] private int maxLives = 3;
    private int currentLives;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private GameObject gameOverUI;

    private bool isDead = false;

    [Header("Timer")]
    [SerializeField] private float timerDuration = 60f;
    private float timer;
    [SerializeField] private TextMeshProUGUI timerText;

    /// <summary>
    /// Initializes player, input, lives, and timer.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        move = InputSystem.actions.FindAction("Move");
        look = InputSystem.actions.FindAction("Look");

        move.performed += MovePerformed;
        move.canceled += MoveCanceled;
        look.performed += LookPerformed;
        look.canceled += LookCanceled;

        Cursor.lockState = CursorLockMode.Locked;

        currentLives = maxLives;
        gameOverUI.SetActive(false);
        UpdateLivesUI();

        timer = timerDuration;
        UpdateTimerUI();
    }

    /// <summary>
    /// Updates player rotation, death, and timer.
    /// </summary>
    void Update()
    {
        MouseLook();
        CheckFallDeath();
        CountdownTimer();
    }

    /// <summary>
    /// Applies player movement in physics update.
    /// </summary>
    void FixedUpdate()
    {
        if (isGrappling) return;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 moveVector = right * playerMovement.x + forward * playerMovement.z;
        moveVector.y = rb.linearVelocity.y;

        rb.linearVelocity = moveVector;
    }

    #region Input Callbacks
    /// <summary>
    /// Handles look input performed event.
    /// </summary>
    /// <param name="context">Look input callback context</param>
    private void LookPerformed(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Handles look input canceled event.
    /// </summary>
    /// <param name="context">Look input callback context</param>
    private void LookCanceled(InputAction.CallbackContext context)
    {
        lookInput = Vector2.zero;
    }

    /// <summary>
    /// Handles move input performed event.
    /// </summary>
    /// <param name="context">Move input callback context</param>
    private void MovePerformed(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        playerMovement.x = input.x * playerSpeed;
        playerMovement.z = input.y * playerSpeed;
    }

    /// <summary>
    /// Handles move input canceled event.
    /// </summary>
    /// <param name="context">Move input callback context</param>
    private void MoveCanceled(InputAction.CallbackContext context)
    {
        playerMovement = Vector3.zero;
    }
    #endregion

    /// <summary>
    /// Applies mouse rotation to player and camera.
    /// </summary>
    private void MouseLook()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraTarget.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, mouseX, 0f));
    }

    #region Fall Death & Lives
    /// <summary>
    /// Checks if player fell below death height.
    /// </summary>
    private void CheckFallDeath()
    {
        if (!isDead && transform.position.y <= deathHeight)
        {
            LoseLife();
        }
    }

    /// <summary>
    /// Deducts a life and handles respawn or game over.
    /// </summary>
    private void LoseLife()
    {
        isDead = true;
        currentLives--;
        UpdateLivesUI();

        if (currentLives > 0)
        {
            Respawn();
        }
        else
        {
            GameOver();
        }
    }

    /// <summary>
    /// Respawns player and resets velocity and timer.
    /// </summary>
    private void Respawn()
    {
        transform.position = respawnPoint.position;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        timer = timerDuration;
        UpdateTimerUI();

        Invoke(nameof(ResetDeath), 0.5f);
    }

    /// <summary>
    /// Triggers game over UI and pauses game.
    /// </summary>
    private void GameOver()
    {
        Debug.Log("GAME OVER");
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Resets player death flag after respawn.
    /// </summary>
    private void ResetDeath()
    {
        isDead = false;
    }

    /// <summary>
    /// Updates player lives UI text.
    /// </summary>
    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Lives: " + currentLives;
    }
    #endregion

    #region Countdown Timer
    /// <summary>
    /// Counts down timer and loses life at zero.
    /// </summary>
    private void CountdownTimer()
    {
        if (isDead) return;

        timer -= Time.deltaTime;

        if (timerText != null)
            UpdateTimerUI();

        if (timer <= 0f)
        {
            LoseLife();
            timer = timerDuration;
        }
    }

    /// <summary>
    /// Updates the timer UI text display.
    /// </summary>
    private void UpdateTimerUI()
    {
        if (timerText == null) return;
        int seconds = Mathf.CeilToInt(timer);
        timerText.text = "Time: " + seconds + "s";
    }
    #endregion

    /// <summary>
    /// Unsubscribes input actions when object destroyed.
    /// </summary>
    private void OnDestroy()
    {
        move.performed -= MovePerformed;
        move.canceled -= MoveCanceled;
        look.performed -= LookPerformed;
        look.canceled -= LookCanceled;
    }
}