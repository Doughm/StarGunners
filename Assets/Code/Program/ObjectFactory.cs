using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectFactory : MonoBehaviour
{
    public Transform arrowPrefab;
    public Transform shipPrefab;
    public Transform backgroundPrefab;
    public Transform explosionPrefab;

    private List<GameObject> backgroundSprites = new List<GameObject>();
    private List<BotSpawn> botSpawns = new List<BotSpawn>();
    private Dictionary<int, Player> players = new Dictionary<int, Player>();

    private float miniMapSize;
    private float distance;
    private int closestShipID;

    //sets this object as a singleton
    private static ObjectFactory instanceOf;
    public static ObjectFactory instance
    {
        get
        {
            if (instanceOf == null)
            {
                instanceOf = GameObject.FindObjectOfType<ObjectFactory>();
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
        miniMapSize = Screen.height / 4;
        GameObject.Find("Minimap").GetComponent<Image>().rectTransform.sizeDelta = new Vector2(Screen.height / 4, Screen.height / 4);
    }

    //creates a new player
    public void addPlayer(string name, int IDnumber)
    {
        players.Add(IDnumber, new Player(name));
        players[IDnumber].ship = Instantiate(shipPrefab.gameObject, new Vector2(), Quaternion.Euler(0, 0, 0)) as GameObject;
        players[IDnumber].ship.SetActive(false);
        players[IDnumber].arrow = Instantiate(arrowPrefab.gameObject, new Vector3(0, 0, 0), transform.rotation) as GameObject;
        players[IDnumber].arrow.SetActive(false);
        players[IDnumber].explosion = Instantiate(explosionPrefab.gameObject, new Vector2(), Quaternion.Euler(0, 0, 0)) as GameObject;
        players[IDnumber].explosion.SetActive(false);
    }

    //creates a new arrow
    public void newArrow(int identifier, string type)
    {
        players[identifier].arrow.SetActive(true);
        players[identifier].arrow.transform.SetParent(GameObject.Find("LeftTop").GetComponent<RectTransform>().transform, false);
        if (type == "Player")
        {
            players[identifier].arrow.GetComponent<Image>().color = Color.green;
        }
        else if (type == "Enemy")
        {
            players[identifier].arrow.GetComponent<Image>().color = Color.red;
        }
        else if (type == "Ally")
        {
            players[identifier].arrow.GetComponent<Image>().color = Color.yellow;
        }
    }

    //moves a arrow on the minimap
    public void moveArrow(int identifier, Vector2 shipPosition, float rotation)
    {
        if (players.ContainsKey(identifier) == true)
        {
            players[identifier].arrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(((shipPosition.x / (Game.instance.levelSize * 20.4f)) * miniMapSize), ((shipPosition.y / (Game.instance.levelSize * 20.4f)) * miniMapSize) - miniMapSize);
            players[identifier].arrow.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, rotation);
        }
    }

    //deletes an arrow
    public void deleteArrow(int identifier)
    {
        players[identifier].arrow.SetActive(false);
    }

    //deletes an player
    public void deletePlayer(int identifier)
    {
        players.Remove(identifier);
    }

    //generates a map from background tiles
    public void makeMap(int size, string background)
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                backgroundSprites.Add(Instantiate(backgroundPrefab.gameObject, new Vector2(x * 20.4f, y * 20.4f), Quaternion.Euler(0, 0, 0)) as GameObject);
                backgroundSprites[backgroundSprites.Count - 1].GetComponent<SpriteRenderer>().sprite = AssetManager.instance.backgroundDictionary.getSprite(background);
            }
        }
    }

    //creates a ship
    public void createShip(string shipType, string gunType, bool isPlayer, bool isEnemy, bool isBot, int idNumber)
    {
        players[idNumber].ship.SetActive(true);
        //players[idNumber].ship.transform.position = new Vector2((Random.Range(0, 20.4f * Game.instance.levelSize)), (Random.Range(0, 20.4f * Game.instance.levelSize)));
        players[idNumber].ship.transform.position = new Vector2((Random.Range(0, 20.4f * Game.instance.levelSize)), (Random.Range(0, 20.4f * Game.instance.levelSize)));
        players[idNumber].ship.GetComponent<Ship>().setShipStats(shipType, gunType, isPlayer, isBot);
        players[idNumber].ship.GetComponent<SpriteRenderer>().sprite = AssetManager.instance.shipDictionary.getSprite(shipType);
        players[idNumber].ship.AddComponent<PolygonCollider2D>();
        players[idNumber].ship.GetComponent<Ship>().IDnumber = idNumber;
        
        if (isEnemy == false)
        {
            players[idNumber].ship.tag = "Friendly";
        }
        else
        {
            players[idNumber].ship.tag = "Enemy";
        }

        newArrow(idNumber, players[idNumber].ship.tag);
    }

    //creates a ship at position
    public void createShip(string shipType, string gunType, bool isPlayer, bool isEnemy, bool isBot, int idNumber, Vector2 position)
    {
        players[idNumber].ship.SetActive(true);
        players[idNumber].ship.transform.position = position;
        players[idNumber].ship.GetComponent<Ship>().setShipStats(shipType, gunType, isPlayer, isBot);
        players[idNumber].ship.GetComponent<SpriteRenderer>().sprite = AssetManager.instance.shipDictionary.getSprite(shipType);
        players[idNumber].ship.AddComponent<PolygonCollider2D>();
        players[idNumber].ship.GetComponent<Ship>().IDnumber = idNumber;

        if (isEnemy == false)
        {
            players[idNumber].ship.tag = "Friendly";
        }
        else
        {
            players[idNumber].ship.tag = "Enemy";
        }

        newArrow(idNumber, players[idNumber].ship.tag);
    }

    //creates a number of bots
    public void createBots(int numberOfBots)
    {
        for (int i = 0; i < numberOfBots; i++)
        {
            addPlayer("CPU " + i.ToString(), i);
            createShip(Data.instance.getRandomShip(), Data.instance.getRandomWeapon(), false, true, true, i);
        }
    }

    //update bot spawns
    public void updateBotSpawns()
    {
        for (int i = 0; i < botSpawns.Count; i++)
        {
            botSpawns[i].updateTime();

            if (botSpawns[i].getLapsed() == true)
            {
                createShip(botSpawns[i].shipType, botSpawns[i].weaponType, false, true, true, botSpawns[i].shipID);
                botSpawns.RemoveAt(i);
                i--;
            }
        }
    }

    //returns the identifier of the closest ship
    public int getClosestShip(int myShipID)
    {
        distance = Game.instance.levelSize * 20.4f;
        closestShipID = -1;

        for (int i = 0; i < players.Count; i++)
        {
            if (players.ElementAt(i).Key != myShipID)
            {
                if (players.ElementAt(i).Value.ship.activeSelf == true)
                {
                    if (Vector2.Distance(players[myShipID].ship.transform.position, players.ElementAt(i).Value.ship.transform.position) < distance)
                    {
                        distance = Vector2.Distance(players[myShipID].ship.transform.position, players.ElementAt(i).Value.ship.transform.position);
                        closestShipID = players.ElementAt(i).Key;
                    }
                }
            }
        }
        return closestShipID;
    }

    //returns the postion of a ship
    public Vector2 getShipPostion(int shipID)
    {
        if (players.ContainsKey(shipID) == true)
        {
            return players[shipID].ship.transform.position;
        }
        return new Vector2();
    }

    //returns the distance between two ships 
    public float getDistanceBetweenShips(int myID, int otherID)
    {
        return Vector2.Distance(players[myID].ship.transform.position, players[myID].ship.transform.position);
    }

    //makes a new bot spawn
    public void createBotSpawn(string ship, string weapon, int IDnumber)
    {
        botSpawns.Add(new BotSpawn(ship, weapon, IDnumber));
    }

    //get the ID number of a player
    public int getIDnumberAtIndex(int index)
    {
        return players.ElementAt(index).Key;
    }

    //get name of a player
    public string getPlayerName(int identifier)
    {
        return players[identifier].playerName;
    }

    //get name of a player
    public string getPlayerNameAtIndex(int index)
    {
        return players.ElementAt(index).Value.playerName;
    }

    //get name of a player
    public int getPlayerScoreAtIndex(int index)
    {
        return players.ElementAt(index).Value.playerScore;
    }

    //gets a players score by identifier
    public int getPlayerScore(int identifier)
    {
        return players[identifier].playerScore;
    }

    //adds to a players score
    public void addPlayerScore(int identifier, int amount)
    {
        players[identifier].playerScore += amount;
    }

    //sets a ships input log
    public void setInputLog(int identifier, string input)
    {
        players[identifier].ship.GetComponent<Ship>().inputLog = input;
    }

    //returns the ip address of the player
    public string getIPAddress(int identifier)
    {
        return players[identifier].ipAddress;
    }

    //returns the ip address of the player
    public string getIPAddressAtIndex(int index)
    {
        return players.ElementAt(index).Value.ipAddress;
    }

    //returns if the player is a bot
    public bool getIsBot(int identifier)
    {
        return players[identifier].ship.GetComponent<Ship>().isBot;
    }

    //returns if the player is a bot
    public bool getIsBotAtIndex(int index)
    {
        return players.ElementAt(index).Value.ship.GetComponent<Ship>().isBot;
    }

    //returns if the player is the player
    public bool getIsPlayer(int identifier)
    {
        return players[identifier].ship.GetComponent<Ship>().isPlayer;
    }

    //returns if the player is the player
    public bool getIsPlayerAtIndex(int index)
    {
        return players.ElementAt(index).Value.ship.GetComponent<Ship>().isPlayer;
    }

    //returns the number of players
    public int getNumberOfPlayers()
    {
        return players.Count;
    }

    //sets a players IP address
    public void setPlayerIPAddress(int identifier, string address)
    {
        players[identifier].ipAddress = address;
    }

    //returns the names of all players
    public string getAllPlayerNames()
    {
        string tempStr = string.Empty;

        for (int i = 0; i < players.Count; i++)
        {
            tempStr += players.ElementAt(i).Value.playerName;
            if (i < players.Count - 1)
            {
                tempStr += "\n";
            }
        }
        return tempStr;
    }
    
    //makes an explotion
    public void createExplotion(int identifier, Vector2 position)
    {
        players[identifier].explosion.SetActive(true);
        players[identifier].explosion.transform.position = position;
        players[identifier].explosion.GetComponent<AnimationScript>().reset();
        players[identifier].explosion.GetComponent<AnimationScript>().animationName = "Explosion";
    }

    //end of a match
    public void endMatch()
    {
        for (int i = 0; i < players.Count; i++)
        { 
            Destroy(players.ElementAt(i).Value.explosion);
            players.ElementAt(i).Value.ship.GetComponent<Ship>().destroyProjectiles();
            Destroy(players.ElementAt(i).Value.ship);
            Destroy(players.ElementAt(i).Value.arrow);
        }
        players.Clear();
        Game.instance.isGameOver = true;

        GameObject.Find("Scoreboard").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GUI").GetComponent<ScoreBoard>().updateRectangleSize();
        GameObject.Find("LeftTop").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("RightTop").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("RespawnCounter").GetComponent<RespawnCounter>().stopCounter();
        GameObject.Find("ShipSelect").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ShipSelect").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("ShipSelect").GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    //unloads a level
    public void unload()
    {
        GameObject.Find("Scoreboard").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GUI").GetComponent<ScoreBoard>().updateRectangleSize();
        GameObject.Find("LeftTop").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("RightTop").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("RespawnCounter").GetComponent<RespawnCounter>().stopCounter();
        GameObject.Find("ShipSelect").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ShipSelect").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("ShipSelect").GetComponent<CanvasGroup>().blocksRaycasts = false;

        Game.instance.isGameOver = false;
        Game.instance.isInMenu = true;

        for (int i = 0; i < players.Count; i++)
        {
            Destroy(players.ElementAt(i).Value.explosion);
            players.ElementAt(i).Value.ship.GetComponent<Ship>().destroyProjectiles();
            Destroy(players.ElementAt(i).Value.ship);
            Destroy(players.ElementAt(i).Value.arrow);
        }
        players.Clear();
        for (int i = 0; i < backgroundSprites.Count; i++)
        {
            Destroy(backgroundSprites[i]);
        }
        backgroundSprites.Clear();
        botSpawns.Clear();

        GameObject.Find("Scoreboard").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GUI").GetComponent<Menu>().backToMainMenu();
    }
}




public class Player
{
    public string playerName;
    public int playerScore;
    public GameObject explosion = new GameObject();
    public GameObject ship = new GameObject();
    public GameObject arrow = new GameObject();
    public string ipAddress;
    public RandomGen randGen = new RandomGen();
    private int randCounter = 0;

    public Player(string name)
    {
        playerName = name;
        ipAddress = string.Empty;
    }

    public Player(string name, ulong seed, int randCount)
    {
        playerName = name;
        ipAddress = string.Empty;
        randGen = new RandomGen(seed);
        randCounter = randCount;
    }
    /*
    //returns a random number from the given seed
    public float getRandomNumber()
    {
        randCounter++;
        return (float)rand.NextDouble() * (20.4f * Game.instance.levelSize);
    }
    */
}
