using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour, IEntity
{
    public int coinCount = 0;
    public float health = 100;
    public bool dead = false;

    [SerializeField] private float moveSpeed;
    [SerializeField] private int attackDamage;
    [SerializeField] ParticleSystem damageParticles;
    [SerializeField] private GameObject deathParticles;

    [Header("HUD Elements")]
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TMP_Text coinCounter;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TMP_Text finalCoinCount;

    private Animator anim;
    private SpriteRenderer SR;
    private Transform hitCheck;

    private void Start()
    {
        anim = GetComponent<Animator>();
        SR = transform.GetChild(0).GetComponent<SpriteRenderer>();
        hitCheck = transform.GetChild(1);
    }

    void Update()
    {
        if (!dead)
        {
            float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

            if (moveX < 0)
            {
                //facing left
                SR.flipX = true;
                hitCheck.localPosition = new Vector3(-0.4f, 0, 0);
            }
            else if (moveX > 0)
            {
                //facing right
                SR.flipX = false;
                hitCheck.localPosition = new Vector3(0.4f, 0, 0);
            }

            if (moveX == 0 && moveY == 0)
            {
                //not moving
                anim.SetBool("walking", false);
            }
            else
            {
                //moving
                transform.position += new Vector3(moveX, moveY, 0);
                anim.SetBool("walking", true);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("attack");
                Collider2D[] enemies = Physics2D.OverlapCircleAll(hitCheck.position, 0.75f);
                foreach (Collider2D enemy in enemies)
                {
                    if (enemy.gameObject.tag == "Enemy")
                    {


                        IEntity currentEnemy = enemy.transform.GetComponent<IEntity>();
                        if (currentEnemy != null)
                        {
                            currentEnemy.Damaged(attackDamage); //call damaged function on enemy with given attack damage

                            //also add slight knockback in opposite direction
                            var knockbackDir = transform.position - enemy.transform.position;
                            enemy.transform.position -= knockbackDir.normalized * 0.4f;
                        }
                    }
                }
            }
        }

        healthBarFill.fillAmount = health / 100;
        coinCounter.text = "Coins: " + coinCount.ToString();
    }

    public void Damaged(int damageAmount)
    {
        anim.SetTrigger("damaged");
        health -= damageAmount;
        if(health <= 0)
        {
            //GAME OVER

            health = 0;

            Instantiate(deathParticles, transform.position - new Vector3(0, 0.5f, 0), Quaternion.identity); //<--spawn death particles
            gameOverScreen.SetActive(true); //<--turn on game over screen
            finalCoinCount.text = "You collected " + coinCount + " coins!"; //<--set final coin count text

            //lastly, destroy player sprite and disable enemy and player movement;
            Destroy(transform.GetChild(0).gameObject);
            dead = true;
        }
        else
        {
            //if player didn't die, play damage particles
            damageParticles.Play();
        }
    }
}
