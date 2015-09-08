using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    //game settings
    public int levelSize { get; private set; }
    public int scoreCap { get; private set; }
    public string gameType { get; private set; }
    public int respawnTime { get; private set; }
    public bool isGameOver { get; set; }
    public bool isInMenu { get; set; }
    public bool isInGame { get; set; }
    public int idNumber { get; private set; }

    private static Game instanceOf;
    public static Game instance
    {
        get
        {
            if (instanceOf == null)
            {
                instanceOf = GameObject.FindObjectOfType<Game>();
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
        isGameOver = true;
        isInMenu = true;
        isInGame = false;
        System.Random random = new System.Random();
        idNumber = random.Next(10000, 99999);

	}
	
	// Update is called at a fixed interval
    //main game loop
	void FixedUpdate ()
    {
        if (isInGame == true && isGameOver == false)
        {
            ObjectFactory.instance.updateBotSpawns();
        }
        if (isInGame == true && isGameOver == true)
        {
            if (Input.anyKeyDown == true ||
                Input.GetMouseButtonDown(0) ||
                Input.GetMouseButtonDown(1))
            {
                ObjectFactory.instance.unload();
            }
        }
    }

    //sets starting game variables
    public void startGame(string background, int sizeOfLevel, int numberOfBots, int maxScore, int respawnTimer, string typeOfGame)
    {
        levelSize = sizeOfLevel;
        scoreCap = maxScore;
        gameType = typeOfGame;
        respawnTime = respawnTimer;

        ObjectFactory.instance.createBots(numberOfBots);
        ObjectFactory.instance.makeMap(levelSize, background);
        isGameOver = false;
        isInMenu = false;
    }
}
