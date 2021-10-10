using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject model;
    public float      runSpeed        = 10f;
    public float      minSpeed        = 10f;
    public float      maxSpeed        = 30f;
    public float      laneChangeSpeed = 10f;
    public float      jumpSpeed       = 5f;
    public float      jumpLength      = 7.5f;
    public float      slideLength     = 10f;
    public float      jumpHeight      = 1f;
    public float      invisibleTime   = 5f;

    private Animator    animator;
    private Rigidbody   rb;
    private UIManager   uiManager;
    private BoxCollider boxCollider;
    private Vector3     targetPosition;
    private Vector3     boxColliderSize;
    private bool        isJumping   = false;
    private bool        isSliding   = false;
    private bool        isInvisible = false;
    private float       jumpStart;
    private float       slideStart;
    private int         currentLives = int.MaxValue;
    private int         coins;
    private float       score;

    void Start()
    {
        rb              = GetComponent<Rigidbody>();
        animator        = GetComponent<Animator>();
        uiManager       = FindObjectOfType<UIManager>();
        boxCollider     = GetComponent<BoxCollider>();
        boxColliderSize = boxCollider.size;
        animator.Play("runStart");

        if (Random.Range(0, 100) >= 50)
            ChangeLane(-1);
        else
            ChangeLane(1);
    }

    void Update()
    {
        HandleScore();
        MoveCharacter();
    }

    void FixedUpdate()
    {
        rb.velocity = Vector3.forward * runSpeed;
    }

    void HandleScore()
    {
        score += Time.deltaTime * runSpeed;

        if (uiManager != null)
            uiManager.UpdateScore((int)score);
    }

    void MoveCharacter()
    {
        HandleKeyboard();
        HandleJump();
        HandleSlide();

        Vector3 newPosition = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
        transform.localPosition =
            Vector3.MoveTowards(transform.position, newPosition, laneChangeSpeed * Time.deltaTime);
    }

    void HandleKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) ChangeLane(-1);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) ChangeLane(1);
        //else if (Input.GetKeyDown(KeyCode.UpArrow)) Jump();
        //else if (Input.GetKeyDown(KeyCode.DownArrow)) Slide();
    }

    void ChangeLane(int direction)
    {
        if (direction == -1)
        {
            targetPosition.x = -1;
        }
        else if (direction == 1)
        {
            targetPosition.x = 1;
        }

        //float targetLane = targetPosition.x + direction;
        //
        //if (targetLane >= 0f && targetLane <= 1f)
        //{
        //    targetPosition.x = targetLane;
        //}
    }

    void Jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            jumpStart = transform.position.z;
            animator.SetBool("Jumping", true);
            animator.SetFloat("JumpSpeed", runSpeed / jumpLength);
        }
    }

    void HandleJump()
    {
        if (isJumping && !isSliding)
        {
            float ratio = (transform.position.z - jumpStart) / jumpLength;

            if (ratio >= 1)
            {
                isJumping = false;
                animator.SetBool("Jumping", false);
            }
            else
            {
                targetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeight;
            }
        }
        else
        {
            targetPosition.y = Mathf.MoveTowards(targetPosition.y, 0, jumpSpeed * Time.deltaTime);
        }
    }

    void Slide()
    {
        if (!isJumping && !isSliding)
        {
            isSliding        =  true;
            slideStart       =  transform.position.z;
            boxCollider.size /= 2;
            animator.SetBool("Sliding", true);
            animator.SetFloat("JumpSpeed", runSpeed / jumpLength);
        }
    }

    void HandleSlide()
    {
        if (isSliding)
        {
            float ratio = (transform.position.z - slideStart) / slideLength;

            if (ratio >= 1)
            {
                isSliding        = false;
                boxCollider.size = boxColliderSize;
                animator.SetBool("Sliding", false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin")) GetCoins(other);
        if (!isInvisible && other.CompareTag("Obstacle")) HitObstacles();
    }

    void GetCoins(Collider other)
    {
        coins++;

        if (uiManager != null)
            uiManager.UpdateCoins(coins);
        other.gameObject.SetActive(false);
    }

    void HitObstacles()
    {
        currentLives--;

        if (uiManager != null)
            uiManager.UpdateLives(currentLives);
        animator.SetTrigger("Hit");

        if (currentLives <= 0)
        {
            runSpeed = 0;
            animator.SetBool("Dead", true);
            // Call gameover
        }
        else
        {
            StartCoroutine(Blinking());
        }
    }

    IEnumerator Blinking()
    {
        float timer        = 0;
        float currentBlink = 1f;
        float lastBlink    = 0;
        float blinkPeriod  = 0.1f;
        bool  enabled      = false;
        isInvisible = true;
        yield return new WaitForSeconds(0.5f);
        runSpeed = minSpeed;

        while (timer < invisibleTime && isInvisible)
        {
            model.SetActive(enabled);
            yield return null;
            timer     += Time.deltaTime;
            lastBlink += Time.deltaTime;

            if (blinkPeriod < lastBlink)
            {
                lastBlink    = 0;
                currentBlink = 1f - currentBlink;
                enabled      = !enabled;
            }
        }

        model.SetActive(true);
        isInvisible = false;
    }

    public void IncreaseSpeed()
    {
        runSpeed *= 1.15f;
        runSpeed =  (runSpeed >= maxSpeed) ? maxSpeed : runSpeed;
    }
}