using UnityEngine;
using UnityEngine.UI;
using System;

public class Menu : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called at each frame
    void Update()
    {
        gameMenu();
    }

    //sets game mode to Lan
    public void playNetworkGame(bool isLan)
    {
        GameObject.Find("MainMenu").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("MainMenu").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("MainMenu").GetComponent<CanvasGroup>().blocksRaycasts = false;
        GameObject.Find("NetworkMenu").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("NetworkMenu").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("NetworkMenu").GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    //quits the game
    public void quit()
    {
        Application.Quit();
    }

    //sets up a network game
    public void setupNetworkGame()
    {
        if (GameObject.Find("UserNameInput").GetComponent<InputField>().text != string.Empty &&
            GameObject.Find("PortInput").GetComponent<InputField>().text != string.Empty)
        {
            GameObject.Find("NetworkGameSetup").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("NetworkGameSetup").GetComponent<CanvasGroup>().interactable = true;
            GameObject.Find("NetworkGameSetup").GetComponent<CanvasGroup>().blocksRaycasts = true;
            GameObject.Find("NetworkMenu").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("NetworkMenu").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("NetworkMenu").GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        if (GameObject.Find("UserNameInput").GetComponent<InputField>().text == string.Empty)
        {
            GameObject.Find("UserNameInput").GetComponentInChildren<Text>().text = "Please Enter Name";
        }
        if (GameObject.Find("PortInput").GetComponent<InputField>().text == string.Empty)
        {
            GameObject.Find("PortInput").GetComponentInChildren<Text>().text = "Please Enter Port";
        }
    }

    //starts a skrimish match
    public void skrimish()
    {
        GameObject.Find("MainMenu").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("MainMenu").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("MainMenu").GetComponent<CanvasGroup>().blocksRaycasts = false;
        GameObject.Find("Skirmish").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("Skirmish").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("Skirmish").GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    //goes back to the main menu
    public void backToMainMenu()
    {
        GameObject.Find("MainMenu").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("MainMenu").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("MainMenu").GetComponent<CanvasGroup>().blocksRaycasts = true;
        GameObject.Find("Skirmish").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("Skirmish").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("Skirmish").GetComponent<CanvasGroup>().blocksRaycasts = false;
        GameObject.Find("GameMenu").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameMenu").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameMenu").GetComponent<CanvasGroup>().blocksRaycasts = false;
        GameObject.Find("LeftTop").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("RightTop").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("RespawnCounter").GetComponent<RespawnCounter>().stopCounter();
    }

    //goes to game mode
    private void gameStart()
    {
        GameObject.Find("Skirmish").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("Skirmish").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("Skirmish").GetComponent<CanvasGroup>().blocksRaycasts = false;

        GameObject.Find("ShipSelect").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("ShipSelect").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("ShipSelect").GetComponent<CanvasGroup>().blocksRaycasts = true;

        GameObject.Find("GUI").GetComponent<ScoreBoard>().updateScoreboardNames();
        GameObject.Find("GUI").GetComponent<ScoreBoard>().updateScoreboardScores();

        GameObject.Find("RespawnCounter").GetComponent<RespawnCounter>().startCounter();

        Game.instance.isInMenu = true;
        Game.instance.isInGame = true;
    }

    //starts a new skirmish match
    public void startSkirmishMatch()
    {
        string background = string.Empty;
        if (GameObject.Find("BackgroundToggle1").GetComponent<Toggle>().isOn)
        {
            background = "Black";
        }
        else if (GameObject.Find("BackgroundToggle2").GetComponent<Toggle>().isOn)
        {
            background = "Blue";
        }
        else if (GameObject.Find("BackgroundToggle3").GetComponent<Toggle>().isOn)
        {
            background = "Red";
        }
        else if (GameObject.Find("BackgroundToggle4").GetComponent<Toggle>().isOn)
        {
            background = "Green";
        }
        
        Game.instance.startGame(background,
                                Convert.ToUInt16(GameObject.Find("SkrimishMapSize").GetComponent<InputField>().text),
                                Convert.ToUInt16(GameObject.Find("SkrimishNumberOfBots").GetComponent<InputField>().text),
                                Convert.ToUInt16(GameObject.Find("SkrimishMaxScore").GetComponent<InputField>().text),
                                Convert.ToUInt16(GameObject.Find("SkrimishRespawnTime").GetComponent<InputField>().text),
                                "DM");
        ObjectFactory.instance.addPlayer(GameObject.Find("SkrimishPlayerName").GetComponent<InputField>().text, Game.instance.idNumber);
        gameStart();
    }

    //game menu
    private void gameMenu()
    {
        if (Game.instance.isInGame == true && Game.instance.isGameOver == false)
        {
            if (Input.GetButtonDown("Menu") == true)
            {
                if (Game.instance.isInMenu == false && GameObject.Find("ShipSelect").GetComponent<CanvasGroup>().alpha == 0)
                {
                    Game.instance.isInMenu = true;
                    GameObject.Find("GameMenu").GetComponent<CanvasGroup>().alpha = 1;
                    GameObject.Find("GameMenu").GetComponent<CanvasGroup>().interactable = true;
                    GameObject.Find("GameMenu").GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
                else if (Game.instance.isInMenu == true && GameObject.Find("ShipSelect").GetComponent<CanvasGroup>().alpha == 0)
                {
                    Game.instance.isInMenu = false;
                    GameObject.Find("GameMenu").GetComponent<CanvasGroup>().alpha = 0;
                    GameObject.Find("GameMenu").GetComponent<CanvasGroup>().interactable = false;
                    GameObject.Find("GameMenu").GetComponent<CanvasGroup>().blocksRaycasts = false;
                }
            }
        }
    }

    //goes back to game
    public void backToGame()
    {
        Game.instance.isInMenu = false;
        GameObject.Find("GameMenu").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameMenu").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameMenu").GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
