using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Data : MonoBehaviour
{
    public ReadOnlyDictionaryShip shipDictionary;
    public ReadOnlyDictionaryWeapon weaponDictionary;

    //sets this object as a singleton
    private static Data instanceOf;
    public static Data instance
    {
        get
        {
            if (instanceOf == null)
            {
                instanceOf = GameObject.FindObjectOfType<Data>();
            }
            return instanceOf;
        }
    }

    void Awake()
    {
        instanceOf = this;
    }

    // Use this for initialization
    void Start()
    {
        Dictionary<string, ShipData> tempShipDic = new Dictionary<string,ShipData>();
        tempShipDic.Add("SkyRocket", new ShipData(0.08f, 0.006f, 4, 75, 0.8f));
        tempShipDic.Add("TheDagger", new ShipData(0.1f, 0.005f, 3, 100, 0.6f));
        tempShipDic.Add("TheVulture", new ShipData(0.13f, 0.004f, 2, 125, 0.65f));
        shipDictionary = new ReadOnlyDictionaryShip(tempShipDic);

        Dictionary<string, WeaponData> tempWeaponDic = new Dictionary<string,WeaponData>();
        tempWeaponDic.Add("Bolt", new WeaponData(0.5f, 30, 20));
        tempWeaponDic.Add("Bullet", new WeaponData(0.3f, 2, 2));
        tempWeaponDic.Add("Laser", new WeaponData(0.7f, 4, 3));
        tempWeaponDic.Add("Missile", new WeaponData(0.2f, 15, 15));
        tempWeaponDic.Add("Plasma", new WeaponData(0.4f, 10, 9));
        weaponDictionary = new ReadOnlyDictionaryWeapon(tempWeaponDic);
    }

    //returns a random ship type
    public string getRandomShip()
    {
        return shipDictionary.getNameAt(Random.Range(0, shipDictionary.getCount()));
    }

    //returns a random weapon type
    public string getRandomWeapon()
    {
        return weaponDictionary.getNameAt(Random.Range(0, weaponDictionary.getCount()));
    }
}


//readonly dictionary for ships
public class ReadOnlyDictionaryShip
{
    Dictionary<string, ShipData> dictionary;

    public ReadOnlyDictionaryShip(Dictionary<string, ShipData> shipDictionary)
    {
        dictionary = shipDictionary;
    }

    public float getMaxSpeed(string name)
    {
        return dictionary[name].maxSpeed;
    }

    public float getAcceleration(string name)
    {
        return dictionary[name].acceleration;
    }

    public float getRotation(string name)
    {
        return dictionary[name].rotation;
    }

    public int getHealth(string name)
    {
        return dictionary[name].health;
    }

    public float getFirePointDistance(string name)
    {
        return dictionary[name].firePointDistance;
    }

    public int getCount()
    {
        return dictionary.Count;
    }

    public string getNameAt(int index)
    {
        return dictionary.ElementAt(index).Key;
    }
}


//readonly dictionary for weapons
public class ReadOnlyDictionaryWeapon
{
    Dictionary<string, WeaponData> dictionary;

    public ReadOnlyDictionaryWeapon(Dictionary<string, WeaponData> weaponDictionary)
    {
        dictionary = weaponDictionary;
    }

    public float getProjectileSpeed(string name)
    {
        return dictionary[name].projectileSpeed;
    }

    public float getFireRate(string name)
    {
        return dictionary[name].fireRate;
    }

    public int getDamage(string name)
    {
        return dictionary[name].damage;
    }

    public int getCount()
    {
        return dictionary.Count;
    }

    public string getNameAt(int index)
    {
        return dictionary.ElementAt(index).Key;
    }
}


//holds all data for a ship
public class ShipData
{
    public float maxSpeed;
    public float acceleration;
    public float rotation;
    public int health;
    public float firePointDistance;

    public ShipData(float MaxSpeed, float Acceleration, float Rotation,
                    int Health, float FirePointDistance)
    {
        maxSpeed = MaxSpeed;
        acceleration = Acceleration;
        rotation = Rotation;
        health = Health;
        firePointDistance = FirePointDistance;
    }
}


//holds all data for a weapon
public class WeaponData
{
    public float projectileSpeed;
    public float fireRate;
    public int damage;

    public WeaponData(float ProjectileSpeed, float FireRate, int Damage)
    {
        projectileSpeed = ProjectileSpeed;
        fireRate = FireRate;
        damage = Damage;
    }
}
