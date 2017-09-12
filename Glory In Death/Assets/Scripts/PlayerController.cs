using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public KeyCode moveUp;
    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode moveDown;
    public KeyCode fire;
    public float speed;
    private Rigidbody2D r2rb;
    private float moveVertical;
    private float moveHorizontal;
    private int health;

    private float nextFire = 0.2F;
    public float fireDelta;
    public float shotSpeed;
    private float myTime = 0.0F;
    public GameObject shotType;
    public Transform shotSpawn;


    // Use this for initialization

    private Animator animator;

    void Start () {
        r2rb = GetComponent<Rigidbody2D> ();

        animator = GetComponent<Animator>();
        health = GameManager.instance.playerHitPoints; 
    }

    private void OnDisable()
    {
        GameManager.instance.playerHitPoints = health;
    }
	
	// Update is called once per frame


    void Update()
    {

        // Fire
        myTime = myTime + Time.deltaTime;
        // Manage firing of projectiles
        if (Input.GetButton("Fire1") && myTime > nextFire) {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePos - shotSpawn.position;
            direction.Normalize();
            nextFire = myTime + fireDelta;
            GameObject projectile = (GameObject)Instantiate(shotType, shotSpawn.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().transform.eulerAngles = new Vector3(0,0,Mathf.Atan2((mousePos.y - projectile.transform.position.y), (mousePos.x - projectile.transform.position.x))*Mathf.Rad2Deg - 90);

            projectile.GetComponent<Rigidbody2D>().velocity = r2rb.velocity + direction * shotSpeed;
            nextFire = nextFire - myTime;
            myTime = 0.0F;
        }

    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if(other.tag == "projectile_small")
        {
            health -= 10;
            other.gameObject.SetActive(false);
        }
    }

	void FixedUpdate () {

        // Hvis vi skifter til horizontal/vertical så rundt op til 1.
        if(Input.GetKey(moveUp))
        {
            moveVertical = 1;
            animator.SetTrigger("up");
        }
        if(Input.GetKey(moveDown))
        {
            moveVertical = -1;
            animator.SetTrigger("down");
        }
        if(Input.GetKey(moveLeft))
        {
            moveHorizontal = -1;
            animator.SetTrigger("left");
        }
        if(Input.GetKey(moveRight))
        {
            moveHorizontal = 1;
            animator.SetTrigger("right");
        }

        Vector2 movement = new Vector2 (moveHorizontal, moveVertical);
        r2rb.velocity = movement * speed;
        moveVertical = 0;
        moveHorizontal = 0;



        CheckIfGameOver();

	}

    private void CheckIfGameOver()
    {
        if(health < 0){
            GameManager.instance.GameOver();
        }
    }

    void LateUpdate()
    {
        //add more idles
        if (Input.GetKeyUp (moveLeft) || Input.GetKeyUp (moveRight) || Input.GetKeyUp (moveUp) || Input.GetKeyUp (moveDown) )
        {
            animator.ResetTrigger("up");
            animator.ResetTrigger("down");
            animator.ResetTrigger("left");
            animator.ResetTrigger("right");
            animator.SetTrigger("idle");
        }
    }
}
