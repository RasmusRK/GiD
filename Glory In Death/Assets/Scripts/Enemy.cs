using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {


    public int playerDamage;
    private Animator animator;
    private Transform target;
    private Vector2 enemyPos;
    private Vector2 targetPos;
    private float distance;
    public float agroRange;
    public float safeRange;
    public float superSafeRange;
    public float moveSpeed;
    private Vector2 direction;
    private Rigidbody2D r2rb;
    public GameObject shotType;
    public Transform shotSpawn;

    private float moveTime = 0.0f;
    private float waitingTime = 0.3f;

    private float myTime = 0.0f;
    public float fireDelta = 0.2f;
    public float nextFire = 0.2f;
    public float shotSpeed;

    void Start () {
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        r2rb = GetComponent<Rigidbody2D> ();

	}
	

    private void shootAtTarget(Vector2 direction)
    {
        myTime = myTime + Time.deltaTime;
        if(myTime > fireDelta){
            nextFire = myTime + fireDelta;
            GameObject projectile_snek = (GameObject)Instantiate(shotType, shotSpawn.position, Quaternion.identity);
            //rotation
            projectile_snek.GetComponent<Rigidbody2D>().transform.eulerAngles = new Vector3(0,0,Mathf.Atan2((target.position.y - projectile_snek.transform.position.y), (target.position.x - projectile_snek.transform.position.x))*Mathf.Rad2Deg - 90);
            //rotation

            projectile_snek.GetComponent<Rigidbody2D>().velocity =  direction * shotSpeed;
            nextFire = nextFire - myTime;
            myTime = 0.0F;
        }
    }


    private void MoveEnemy()
    {
        enemyPos = this.transform.position;
        targetPos = target.position;
        distance = Vector2.Distance(enemyPos,targetPos);
        direction = (targetPos - enemyPos);
        direction.Normalize();
        moveTime = moveTime + Time.deltaTime;
        if(distance < agroRange && distance > safeRange)
        {
            r2rb.velocity = direction * moveSpeed;
            //move closer to target
        }
        else if(distance < safeRange && distance > superSafeRange)
        {
            shootAtTarget(direction);
            if(moveTime > waitingTime)
            {
                r2rb.velocity = new Vector2(Random.value*2-1,Random.value*2-1) * moveSpeed;
                moveTime = 0.0f;

            }
            // stay at range and move at random
        }
        if(distance < superSafeRange)
        {
            shootAtTarget(direction);
            r2rb.velocity = -direction * moveSpeed;
        }
    }
	// Update is called once per frame
	void Update () {
		MoveEnemy();
	}
}
