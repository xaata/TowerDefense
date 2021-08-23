using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Towers
{
    public int type;
    public float range, cooldown;
    public float currCooldown = 0;
    public Towers(int type, float range, float cd)
    {
        this.type = type;
        this.range = range;
        cooldown = cd;
    }
}

public class TowerProjectiles
{
    public float speed;
    public int damage;
    public TowerProjectiles(float speed, int dmg)
    {
        this.speed = speed;
        damage = dmg;
    }
}

public class TowerUpgrade
{
    public int type;
    public float range, cooldown;
    public float currCooldown = 0;
    public float speed;
    public int damage;
}

public enum TowersType
{
    STONE_TOWER,//simple tower which upgrading into AOE with more dmg but lower AS or dps with more range
    FIRE_TOWER,//fire tower which dealing damage by precent of enemy HP, upgrading into more dmg or AOE
    ICE_TOWER,//just slowing enemy, upgrading into more AOE or more slow effect
    STORM_TOWER,//attacking with lightning has static effect, upgrading into more radius or stun duration
    STONE_TOWER_2,//simple tower which upgrading into AOE with more dmg but lower AS or dps with more range
    FIRE_TOWER_2,//fire tower which dealing damage by precent of enemy HP, upgrading into more dmg or AOE
    ICE_TOWER_2,//just slowing enemy, upgrading into more AOE or more slow effect
    STORM_TOWER_2,
    STONE_TOWER_3,//simple tower which upgrading into AOE with more dmg but lower AS or dps with more range
    FIRE_TOWER_3,//fire tower which dealing damage by precent of enemy HP, upgrading into more dmg or AOE
    ICE_TOWER_3,//just slowing enemy, upgrading into more AOE or more slow effect
    STORM_TOWER_3
}

public class TowerController : MonoBehaviour
{
    public List<Towers> AllTowers = new List<Towers>();
    public List<TowerProjectiles> AllProjectiles = new List<TowerProjectiles>();

    public void Awake()
    {
        AllTowers.Add(new Towers(0, 6, .5f));
        AllTowers.Add(new Towers(1, 10, 1.5f));
        AllTowers.Add(new Towers(2, 2, 1.5f));

        AllProjectiles.Add(new TowerProjectiles(7, 10));
        AllProjectiles.Add(new TowerProjectiles(7, 20));
        AllProjectiles.Add(new TowerProjectiles(7, 10));
    }
}