using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    Transform target;
    TowerController twrCtl;
    public TowerProjectiles selfProjectile;
    public Towers selfTower;
    Animator animator;

    private void Start()
    {
        twrCtl = FindObjectOfType<TowerController>();
        selfProjectile = twrCtl.AllProjectiles[selfTower.type];
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }
    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }
    private void Move()
    {
        if (target != null)
        {   
                Vector2 direction = target.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * selfProjectile.speed);
        }
        else
            Destroy(gameObject);//��������� ������ �����
    }

    //----����� ��������� ����� � ��������------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy) && enemy.selfEnemy.isAlive)
        {
            switch (selfTower.type)
            {
                case (int)TowersType.STONE_TOWER:
                    enemy.TakeDamage(selfProjectile.damage);//�������� ��������� ����� ������
                    SoundManager.Instance.PlaySFX("StoneProjectile");//����
                    break;
                case (int)TowersType.FIRE_TOWER:
                    enemy.AOEDamage(3, selfProjectile.damage);//�������� ��������� ��� ����� ������
                    animator.SetTrigger("Destroy");
                    SoundManager.Instance.PlaySFX("FireProjectile");//����
                    break;
                case (int)TowersType.ICE_TOWER:
                    enemy.StartSlow(5, 2.5f);//�������� ��������� ���������� ������
                    enemy.AOEDamage(5, selfProjectile.damage);//�������� ��������� ��� ����� ������
                    SoundManager.Instance.PlaySFX("IceProjectile");//����              
                    break;
            }
            Destroy(gameObject);// ��������� ������ �������
        }
        //else if (!enemy.selfEnemy.isAlive)
        //{
        //    Destroy(gameObject);
        //}
    }
}
   

