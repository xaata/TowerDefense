using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    Towers selfTower;
    public TowersType selfType;
    TowerController twrCtl;
    public GameObject Projectile;
    public Enemies selfEnemy;
    public int Price { get; set; }
    private void Start()
    {
        twrCtl = FindObjectOfType<TowerController>();
        selfTower = twrCtl.AllTowers[(int)selfType];
        InvokeRepeating("SearchTarget", 0, 0.1f);
    }
    private void Update()
    {
        if (selfTower.currCooldown > 0)
            selfTower.currCooldown -= Time.deltaTime;
    }

    bool CanShoot()
    {
        if (selfTower.currCooldown <= 0)
            return true;
        return false;
    }

    void SearchTarget()
    {
        if (CanShoot())
        {
            Transform nearestEnemy = null;
            float nearestEnemyDistance = Mathf.Infinity;

            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                float currDistance = Vector2.Distance(transform.position, enemy.transform.position);

                if (currDistance < nearestEnemyDistance &&
                    currDistance <= selfTower.range && enemy.GetComponent<Enemy>().selfEnemy.isAlive)
                {
                    nearestEnemy = enemy.transform;
                    nearestEnemyDistance = currDistance;
                }
            }
            if (nearestEnemy != null)
            {
                Shoot(nearestEnemy);
            }
        }
    }
    void Shoot(Transform enemy)
    {
        selfTower.currCooldown = selfTower.cooldown;
        GameObject projectile = Instantiate(Projectile);
        projectile.GetComponent<TowerProjectile>().selfTower = selfTower;
        projectile.transform.position = transform.position;
        projectile.GetComponent<TowerProjectile>().SetTarget(enemy);
    }

}
