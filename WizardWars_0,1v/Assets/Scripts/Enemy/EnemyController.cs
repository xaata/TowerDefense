using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies
{
    public int type, reward;
    public float health, startHealth, speed, startSpeed, damage;
    public bool isAlive;
    public Enemies(int type, float health, float speed, float damage, int reward)
    {
        this.health = health;
        this.speed = speed;
        this.damage = damage;
        this.reward = reward;
        isAlive = true;
    }

    public Enemies(Enemies other)
    {
        startHealth = health = other.health;
        startSpeed = speed = other.speed;
        damage = other.damage;
        reward = other.reward;
        isAlive = true;
    }
}
public enum EnemiesType
{
    SKELETON,
    ZOMBIE,
    VAMPIRE,
    BOSS
}

public class EnemyController : MonoBehaviour
{
    public List<Enemies> AllEnemies = new List<Enemies>();
    public void Awake()
    {
        AllEnemies.Add(new Enemies(0, 50, 4, 2, 2));
        AllEnemies.Add(new Enemies(1, 100, 2, 5, 3));
        AllEnemies.Add(new Enemies(2, 125, 5, 10, 5));
    }
}

