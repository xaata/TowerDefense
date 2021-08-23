using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;


//struct s_Enemy
//{
//    public int type, reward;
//    public float health, speed, startSpeed, damage;
//    public bool isAlive;
//    public s_Enemy(int type, float health, float speed, float damage, int reward)
//    {
//        this.type = type;
//        this.startSpeed = speed;
//        this.health = health;
//        this.speed = speed;
//        this.damage = damage;
//        this.reward = reward;
//        isAlive = true;
//    }

//    public s_Enemy(s_Enemy other)
//    {
//        type = other.type;
//        health = other.health;
//        startSpeed = speed = other.speed;
//        damage = other.damage;
//        reward = other.reward;
//        isAlive = true;
//    }
//};

public class Enemy : MonoBehaviour
{
    //-----navigation------------
    [HideInInspector]
    //public float navigation;
    public Transform[] wayPoints;
    public int target = 0;
    public Transform exit;
    //float navigationTime = 0;
    Animator animator;
    
    Transform enemy;

    //-------Enemy-------
    public Enemies selfEnemy;
    
    void Start()
    {   
        enemy = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }
 
    void Update()
    {
        Move();
    }

    //-------collision on MovePoint and Finish---------
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MovingPoint")// �������� �� �������� � �������� ��� ������
        {
            target += 1;//�������� ��� ������
        }
        else if (collision.tag == "Finish")// �������� �� �������� � �������� ������
        {
            Destroy(gameObject);//����������� �������
            DamageDeal();// ��������� ����� �� ������
            EnemySpawner.Instance.RemoveEnemyFromList(gameObject);//������� ������ ����� �� ������� ������
        }
    }
    //---------����� �������� �� ��� ������ �� ��� ������
    private void Move()
    {
        if (wayPoints != null && selfEnemy.isAlive == true)
        {
                if (target < wayPoints.Length)// �������� �� ������� ��� ������ �� ��������
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, wayPoints[target].position, selfEnemy.speed * Time.deltaTime);//�������� �� ��� ������
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exit.position, selfEnemy.speed * Time.deltaTime);// �������� �� ������
                }
        }
    }
    //-----------����� ��������� �����--------
    public void TakeDamage(float damage)
    {
        selfEnemy.health -= damage;
        //animator.Play("SkeletonHurtanim");//�������� ��������� �����
        LifeCheck(); //�������� �� ����� �����
    }
    //---------����� ��������� �����-------
    void DamageDeal()
    {
        GameManager.Instance.Lives -= selfEnemy.damage;
    }
    //--------����� �������� ��� �� ����------
    private void LifeCheck()
    {
        if (selfEnemy.health <= 0)
        {
            selfEnemy.isAlive = false;
            animator.SetTrigger("Die");
            GetComponent<SpriteRenderer>().sortingOrder--;
            SoundManager.Instance.PlaySFX("SkeletonDeath");
            //Destroy(gameObject);
            GameManager.Instance.Currency += selfEnemy.reward;
            EnemySpawner.Instance.RemoveEnemyFromList(gameObject);
        }
    }
    //--------����� ��������� ����������------------
    public void StartSlow(float duration, float slowValue)
    {
        StopCoroutine("GetSlow");
        selfEnemy.speed = selfEnemy.startSpeed;
        StartCoroutine(GetSlow(duration, slowValue));
    }
    //--------����� ���������� ����������------------
    IEnumerator GetSlow(float duration, float slowValue)
    {
        selfEnemy.speed -= slowValue;
        yield return new WaitForSeconds(duration);
        selfEnemy.speed = selfEnemy.startSpeed;
    }
    public void GetBurn(float duration)
    {
        InvokeRepeating("StartBurn", duration, 1);
    }
    public void StartBurn(float damage)
    {
        selfEnemy.health -= selfEnemy.startHealth * (damage / 100);
    }


    //--------����� ��� �����-----------
    public void AOEDamage(float radius, float damage)
    {
        List<Enemy> enemies = new List<Enemy>();
        foreach ( GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (Vector2.Distance(transform.position, go.transform.position) <= radius)
                enemies.Add(go.GetComponent<Enemy>());
        }
        foreach (Enemy es in enemies)
            es.TakeDamage(damage);
    }
    public void Destr()
    {
         Destroy(gameObject);
    }
}
