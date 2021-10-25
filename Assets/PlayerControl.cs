using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    public Text life;
    private bool facingRight = true;
    private int scoreValue = 0;
    private int lifeValue = 3;
    private bool isOnGround;
    private bool gameWon;
    public float jumpForce;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public AudioSource musicSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        life.text = "Lives: " + lifeValue.ToString();
        lifeValue = 3;
        {
            anim = GetComponent<Animator>();
            musicSource.clip = musicClipOne;
            musicSource.Play();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    void FixedUpdate()
    {
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
    }


    // Update is called once per frame
    void Update()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetInteger("State", 1);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                anim.SetInteger("State", 0);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                anim.SetInteger("State", 2);
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                anim.SetInteger("State", 0);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                anim.SetInteger("State", 2);
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                anim.SetInteger("State", 0);
            }

            if (facingRight == false && hozMovement > 0)
            {
                Flip();
            }
            else if (facingRight == true && hozMovement < 0)
            {
                Flip();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            if (scoreValue == 4)
            {
                transform.position = new Vector3(63.0f, 10.0f, 0.0f);
                lifeValue = 3;
                life.text = "Lives: " + lifeValue.ToString();
            }
        }
        if (scoreValue >= 8 && gameWon == false)
        {
            winTextObject.SetActive(true);
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            gameWon = !gameWon;
            rd2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if (collision.collider.tag == "EnemyCoin" && scoreValue != 8)
        {
            Destroy(collision.collider.gameObject);
            lifeValue = lifeValue -= 1;
            life.text = "Lives: " + lifeValue.ToString();
        }
        if (lifeValue == 0)
        {
            Destroy(gameObject);
            loseTextObject.SetActive(true);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
        }
    }
}
