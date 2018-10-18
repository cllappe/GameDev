using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb2d;
    public float maxSpeed = 7f;
    bool facingRight = true;
    bool attack = false;
    int clickNum = 1;
    Animator anim;
    private int Gold;
    public Text GoldCount;

	void Start () {
        rb2d = GetComponent<Rigidbody2D> ();
        anim = GetComponent<Animator> ();
        Gold = 0;
        GoldCount.text = "Gold: " + Gold.ToString();
    }
	
	// Update is called once per frame
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
            Gold += 1;
        else if (Pickup.gameObject.CompareTag("Money Bag"))
            Gold += 15;
        else if (Pickup.gameObject.CompareTag("Gold Bar"))
            Gold += 25;

        setGoldText();
    }

    void setGoldText()
    {
        GoldCount.text = "Gold: " + Gold.ToString();
    }
}
