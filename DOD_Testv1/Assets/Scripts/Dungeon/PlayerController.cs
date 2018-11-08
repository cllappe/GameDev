using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public Image HealthBarfull;

    private Rigidbody2D rb2d;
    public float maxSpeed = 7f;
    bool facingRight = true;
    bool attack = false;
    int clickNum = 1;
    Animator anim;

    //Health 
    public static int maxHealth = 100; // Minimum health is 0.0f (dead)
    public int currentHealth = 100; // Players current health
    public float healthBarLenght;

    //Gold
    public int Gold;
    public Text GoldCount;

    void Awake()
    {
        //Health
        PlayerPrefs.SetInt("healthP", maxHealth);
    }

	void Start () {
        
        rb2d = GetComponent<Rigidbody2D> ();
        anim = GetComponent<Animator> ();



        // Load Player data (i.e. gold, hp, etc)      
        if (PlayerPrefs.HasKey("Save0"))
        {
            Gold = DialogueLua.GetActorField("Player", "Gold").asInt;                 
            currentHealth = DialogueLua.GetActorField("Player", "health").asInt;
        }
        else
        {
            Gold = 0;
            currentHealth = 100;
        }

        //Save Gold after Battle 
        if(PlayerPrefs.HasKey("Save1"))
            Gold = DialogueLua.GetActorField("Player", "gold").asInt;

        //
        //if (PlayerPrefs.GetInt("lastlevel") == 7 || PlayerPrefs.GetInt("lastlevel") == 8 || PlayerPrefs.GetInt("lastlevel") == 9)
        if (PlayerPrefs.GetString("lastlevel") == "battle")
        {
            currentHealth = PlayerPrefs.GetInt("battlehp");
            PlayerPrefs.DeleteKey("battlehp");
            PlayerPrefs.DeleteKey("lastlevel");
        } 
        
        // Set Gold UI text element
        GoldCount.text = "Gold: " + DialogueLua.GetActorField("Player", "Gold").asString;

        //Health
        //currentHealth = PlayerPrefs.GetInt("healthP");
        healthBarLenght = Screen.width / 2;
    }

    void Update()
    {
        //Health
        adjustCurrentHealth(0);
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentHealth = currentHealth - 10;
        }

        HealthBarfull.fillAmount = currentHealth / 100f;
    }

    void OnGUI()
    {
        //Health 

        //GUI.Box(new Rect(10, 10, healthBarLenght, 20), currentHealth + "/" + maxHealth);

        if (currentHealth == 0)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Game Over");
            Time.timeScale = 0.0f;
        }
    }

    public void adjustCurrentHealth(int adj)
    {
        //Health
        currentHealth += adj;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (maxHealth < 1)
        {
            maxHealth = 1;
        }

        healthBarLenght = (Screen.width / 2) * (currentHealth / (float)maxHealth);
    }
    void SaveHealth()
    {
        //Health 

        //if (DialogueLua.DoesTableElementExist("Actor", "Player"))
            //currentHealth = DialogueLua.GetActorField("Player", "healthP").asInt;

        PlayerPrefs.SetInt("healthP", currentHealth);
    }

	// Update is called once per frame
    //CHANGE TO UPDATE - ASAP
	void FixedUpdate () {

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetFloat("Attack", clickNum);
            attack = true;
        }

        if (Input.GetButtonDown("Fire2") && anim.GetFloat("Speed") > 0.1)
        {
            anim.SetBool("Sliding", true);
        
        }

        if (attack != true)
        {
            if (x == 0 && y != 0)
                anim.SetFloat("Speed", Mathf.Abs(y));
            else if (x != 0 && y != 0)
            {
                if (Mathf.Abs(x) > Mathf.Abs(y))
                    anim.SetFloat("Speed", Mathf.Abs(x));
                else
                    anim.SetFloat("Speed", Mathf.Abs(y));
            }
            else if (x != 0 && y == 0)
                anim.SetFloat("Speed", Mathf.Abs(x));

            rb2d.velocity = new Vector2(Mathf.Lerp(0, x * maxSpeed, 0.3f), Mathf.Lerp(0, y * maxSpeed, 0.3f));

            if (x > 0 && !facingRight)
                Flip();
            else if (x < 0 && facingRight)
                Flip();
        }

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void AlertClick()
    {
        clickNum = 2;
    }
    void AlertClick2()
    {
        clickNum = 3;

        
    }

    void AlertObservers(string message)
    {
        if (message.Equals("AttackAnimationEnded"))
        {
            anim.SetFloat("Attack", 0);
            anim.SetFloat("Speed", 0);
            attack = false;
            clickNum = 1;
        }
    }

    void AlertSlide()
    {
        anim.SetBool("Sliding", false);
    }

    void OnTriggerEnter2D(Collider2D Pickup)
    {

        if (Pickup.gameObject.CompareTag("Coin"))
            DialogueLua.SetActorField("Player", "Gold", DialogueLua.GetActorField("Player", "Gold").asInt + 1);
        else if (Pickup.gameObject.CompareTag("Money Bag"))
            DialogueLua.SetActorField("Player", "Gold", DialogueLua.GetActorField("Player", "Gold").asInt + 15);    
        else if (Pickup.gameObject.CompareTag("Gold Bar"))
            DialogueLua.SetActorField("Player", "Gold", DialogueLua.GetActorField("Player", "Gold").asInt + 25);
        setGoldText();


        if (Pickup.gameObject.CompareTag("Carrot"))
            currentHealth += 10;
        else if (Pickup.gameObject.CompareTag("Grapes"))
            currentHealth += 15;
        else if (Pickup.gameObject.CompareTag("Steak"))
            currentHealth += 25;
    }

    void setGoldText()
    {
        GoldCount.text = "Gold: " + DialogueLua.GetActorField("Player","Gold").asString;
    }

    void changeHealth()
    {
        currentHealth += 25;
        healthBarLenght = (Screen.width / 2) * (currentHealth / (float)maxHealth);
    }

    public void OnRecordPersistentData()
    {
        //Saving Health :]
        //DialogueLua.SetActorField("Player", "Gold", Gold);
        DialogueLua.SetActorField("Player", "health", currentHealth);
    }

}
