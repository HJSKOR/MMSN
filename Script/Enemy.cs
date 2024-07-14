using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public Rigidbody2D target;
    public RuntimeAnimatorController[] animCon;
    public GameObject expPrefab;

    bool isAlive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
    }
    void FixedUpdate()
    {
        if (!GameManager.Instance.islive)
            return;
        if (!isAlive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dirvec = target.position - rigid.position;
        Vector2 nextVec = dirvec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!GameManager.Instance.islive)
            return;
        if (isAlive)
            spriter.flipX = target.position.x > rigid.position.x;
    }

    void OnEnable()
    {
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        isAlive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 1;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isAlive)
            return;
        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if(health > 0)
        {
            anim.SetTrigger("Hit");
        }
        else
        {
            isAlive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 0;
            anim.SetBool("Dead", true);
            GameManager.Instance.kill++;
            //GameManager.Instance.GetExp();
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void Dead()
    {
        DropExp();
        gameObject.SetActive(false);
    }
    void DropExp()
    {
        Instantiate(expPrefab, transform.position, Quaternion.identity);
    }
}
