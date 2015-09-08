using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AssetManager : MonoBehaviour
{
    //texture dictionaries
    public readOnlySpriteDictionary shipDictionary;
    public readOnlyWeaponDictionary weaponDictionary;
    public readOnlySpriteDictionary backgroundDictionary;
    public readOnlyAnimationDictionary animationDictionary;

    //sets this object as a singleton
    private static AssetManager instanceOf;
    public static AssetManager instance
    {
        get
        {
            if (instanceOf == null)
            {
                instanceOf = GameObject.FindObjectOfType<AssetManager>();
            }
            return instanceOf;
        }
    }

    void Awake()
    {
        instanceOf = this;
    }

	// Use this for initialization
	void Start ()
    {
        Dictionary<string, Sprite> tempShipSpriteDic = new Dictionary<string, Sprite>();
        Dictionary<string, Weapon> tempWeaponDic = new Dictionary<string, Weapon>();
        Dictionary<string, Sprite> tempBackgroundSpriteDic = new Dictionary<string, Sprite>();
        Dictionary<string, AnimationClass> tempAnimationDic = new Dictionary<string, AnimationClass>();

        //ships
        Texture2D tempRocket = (Texture2D)Resources.Load("rocket", typeof(Texture2D));
        tempShipSpriteDic.Add("SkyRocket", Sprite.Create(tempRocket, new Rect(0, 0, tempRocket.width, tempRocket.height),new Vector2(0.5f, 0.5f)));
        Texture2D tempDagger = (Texture2D)Resources.Load("dagger", typeof(Texture2D));
        tempShipSpriteDic.Add("TheDagger", Sprite.Create(tempDagger, new Rect(0, 0, tempDagger.width, tempDagger.height),new Vector2(0.5f, 0.5f)));
        Texture2D tempVulture = (Texture2D)Resources.Load("vulture", typeof(Texture2D));
        tempShipSpriteDic.Add("TheVulture", Sprite.Create(tempVulture,new Rect(0, 0, tempVulture.width, tempVulture.height),new Vector2(0.5f, 0.5f)));
        shipDictionary = new readOnlySpriteDictionary(tempShipSpriteDic);

        //weapons
        Texture2D tempBolt = (Texture2D)Resources.Load("bolt", typeof(Texture2D));
        tempWeaponDic.Add("Bolt", new Weapon(Sprite.Create(tempBolt, new Rect(0, 0, tempBolt.width, tempBolt.height), new Vector2(0.5f, 0.5f)),
            new Vector2(0.4f, 0.06f)));
        Texture2D tempBullet = (Texture2D)Resources.Load("bullet", typeof(Texture2D));
        tempWeaponDic.Add("Bullet", new Weapon(Sprite.Create(tempBullet, new Rect(0, 0, tempBullet.width, tempBullet.height), new Vector2(0.5f, 0.5f)),
            new Vector2(0.1f, 0.1f)));
        Texture2D tempLaser = (Texture2D)Resources.Load("laser", typeof(Texture2D));
        tempWeaponDic.Add("Laser", new Weapon(Sprite.Create(tempLaser, new Rect(0, 0, tempLaser.width, tempLaser.height), new Vector2(0.5f, 0.5f)),
            new Vector2(0.38f, 0.16f)));
        Texture2D tempMissile = (Texture2D)Resources.Load("missile", typeof(Texture2D));
        tempWeaponDic.Add("Missile", new Weapon(Sprite.Create(tempMissile, new Rect(0, 0, tempMissile.width, tempMissile.height), new Vector2(0.5f, 0.5f)),
                new Vector2(0.34f, 0.1f)));
        Texture2D tempPlasma = (Texture2D)Resources.Load("plasma", typeof(Texture2D));
        tempWeaponDic.Add("Plasma", new Weapon(Sprite.Create(tempPlasma, new Rect(0, 0, tempPlasma.width, tempPlasma.height), new Vector2(0.5f, 0.5f)),
            new Vector2(0.42f, 0.12f)));
        weaponDictionary = new readOnlyWeaponDictionary(tempWeaponDic);

        //backgrounds
        Texture2D tempBackgroundBlack = (Texture2D)Resources.Load("starfieldblack", typeof(Texture2D));
        tempBackgroundSpriteDic.Add("Black", Sprite.Create(tempBackgroundBlack, new Rect(0, 0, tempBackgroundBlack.width, tempBackgroundBlack.height), new Vector2(0, 0)));
        Texture2D tempBackgroundBlue = (Texture2D)Resources.Load("starfieldblue", typeof(Texture2D));
        tempBackgroundSpriteDic.Add("Blue", Sprite.Create(tempBackgroundBlue, new Rect(0, 0, tempBackgroundBlue.width, tempBackgroundBlue.height), new Vector2(0, 0)));
        Texture2D tempBackgroundRed = (Texture2D)Resources.Load("starfieldred", typeof(Texture2D));
        tempBackgroundSpriteDic.Add("Red", Sprite.Create(tempBackgroundRed, new Rect(0, 0, tempBackgroundRed.width, tempBackgroundRed.height), new Vector2(0, 0)));
        Texture2D tempBackgroundGreen = (Texture2D)Resources.Load("starfieldgreen", typeof(Texture2D));
        tempBackgroundSpriteDic.Add("Green", Sprite.Create(tempBackgroundGreen, new Rect(0, 0, tempBackgroundGreen.width, tempBackgroundGreen.height), new Vector2(0, 0)));
        backgroundDictionary = new readOnlySpriteDictionary(tempBackgroundSpriteDic);

        //animations
        Sprite[] tempArray = new Sprite[7];
        for (int i = 1; i < 8; i++)
        {
            Texture2D tempExplosion = (Texture2D)Resources.Load("explosion" + i.ToString(), typeof(Texture2D));
            tempArray[i - 1] = Sprite.Create(tempExplosion, new Rect(0, 0, tempExplosion.width, tempExplosion.height), new Vector2(0.5f, 0.5f));
        }
        tempAnimationDic.Add("Explosion", new AnimationClass(tempArray, 2, false, true));
        animationDictionary = new readOnlyAnimationDictionary(tempAnimationDic);
	}
}

//read only dictionary for weapons
public class readOnlyWeaponDictionary
{
    private Dictionary<string, Weapon> dictionary;

    public readOnlyWeaponDictionary(Dictionary<string, Weapon> newDictionary)
    {
        dictionary = newDictionary;
    }

    //returns a sprite
    public Sprite getSprite(string name)
    {
        return dictionary[name].sprite;
    }

    //returns a box collider used for weapons
    public Vector2 getBoxCollider(string name)
    {
        return dictionary[name].boxCollider;
    }

    //returns how many elements are in the dictionary
    public int getSize()
    {
        return dictionary.Count;
    }

    //returns a string array of all keys in the dictionary
    public string[] getArray()
    { 
        string[] tempArray = new string[dictionary.Count];
        for (int i = 0; i < dictionary.Count; i++)
        {
            tempArray[i] = dictionary.ElementAt(i).Key;
        }
        return tempArray;
    }

    //returns a string of a key in the dictionary
    public string getKey(int key)
    {
        return dictionary.ElementAt(key).Key;
    }
}

//read only dictionary for weapon data
public class readOnlySpriteDictionary
{
    private Dictionary<string, Sprite> dictionary;

    public readOnlySpriteDictionary(Dictionary<string, Sprite> newDictionary)
    {
        dictionary = newDictionary;
    }

    //returns a sprite
    public Sprite getSprite(string name)
    {
        return dictionary[name];
    }

    //returns how many elements are in the dictionary
    public int getSize()
    {
        return dictionary.Count;
    }

    //returns a string array of all keys in the dictionary
    public string[] getArray()
    {
        string[] tempArray = new string[dictionary.Count];
        for (int i = 0; i < dictionary.Count; i++)
        {
            tempArray[i] = dictionary.ElementAt(i).Key;
        }
        return tempArray;
    }

    //returns a string of a key in the dictionary
    public string getKey(int key)
    {
        return dictionary.ElementAt(key).Key;
    }
}

//read only dictionary for weapon data
public class readOnlyAnimationDictionary
{
    private Dictionary<string, AnimationClass> dictionary;

    public readOnlyAnimationDictionary(Dictionary<string, AnimationClass> newDictionary)
    {
        dictionary = newDictionary;
    }

    //returns a sprite
    public Sprite getSprite(string name, int index)
    {
        return dictionary[name].spriteArray[index];
    }

    //returns the size of the array
    public int getArraySize(string name)
    {
        return dictionary[name].spriteArray.Length;
    }

    //returns the speed of the animation
    public int getSpeed(string name)
    {
        return dictionary[name].speed;
    }

    //returns if the animation repeats or not
    public bool getRepeat(string name)
    {
        return dictionary[name].repeat;
    }

    //returns if the object is destroyed at the end of the animation
    public bool getDestroyOnEnd(string name)
    {
        return dictionary[name].destroyOnEnd;
    }

    //returns how many elements are in the dictionary
    public int getSize()
    {
        return dictionary.Count;
    }

    //returns a string array of all keys in the dictionary
    public string[] getArray()
    {
        string[] tempArray = new string[dictionary.Count];
        for (int i = 0; i < dictionary.Count; i++)
        {
            tempArray[i] = dictionary.ElementAt(i).Key;
        }
        return tempArray;
    }

    //returns a string of a key in the dictionary
    public string getKey(int key)
    {
        return dictionary.ElementAt(key).Key;
    }
}

//class used to store a weapons sprite and bounding box
public class Weapon
{
    public Sprite sprite;
    public Vector2 boxCollider;

    public Weapon(Sprite newSprite, Vector2 newBoxCollider)
    {
        sprite = newSprite;
        boxCollider = newBoxCollider;
    }
}

//class used to store animation data
public class AnimationClass
{
    public int speed;
    public Sprite[] spriteArray;
    public bool repeat;
    public bool destroyOnEnd;

    public AnimationClass(Sprite[] SpriteArray, int Speed, bool Repeat, bool DestroyOnEnd)
    {
        speed = Speed;
        spriteArray = SpriteArray;
        repeat = Repeat;
        destroyOnEnd = DestroyOnEnd;
    }
}
