using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    private ShipSelect shipSelect;

    // Use this for initialization
    void Start()
    {
        
    }

	// Update is called once per frame
	void Update ()
    {
        if (Game.instance.isInMenu == false && Game.instance.isGameOver == false)
        {
            if (Input.GetButton("Scoreboard") == true)
            {
                GameObject.Find("Scoreboard").GetComponent<CanvasGroup>().alpha = 1;
                updateRectangleSize();
            }
            else
            {
                GameObject.Find("Scoreboard").GetComponent<CanvasGroup>().alpha = 0;
            }
        }
	}

    //update the rectangles size
    public void updateRectangleSize()
    {
        GameObject.Find("ScoreboardBackground").GetComponent<Image>().rectTransform.sizeDelta = new Vector2(200, ObjectFactory.instance.getNumberOfPlayers() * 25);
    }

    //updates the scoreboard names
    public string updateScoreboardNames()
    {
        string tempStr = string.Empty;

        for (int i = 0; i < ObjectFactory.instance.getNumberOfPlayers(); i++)
        {
            tempStr += ObjectFactory.instance.getPlayerNameAtIndex(i);
            if (i < ObjectFactory.instance.getNumberOfPlayers() - 1)
            {
                tempStr += "\n";
            }
        }

        GameObject.Find("ScoreboardNames").GetComponent<Text>().text = tempStr;
        GameObject.Find("ScoreboardNames").GetComponent<Text>().rectTransform.sizeDelta = new Vector2(200, ObjectFactory.instance.getNumberOfPlayers() * 25);

        return tempStr;
    }

    //updates the scoreboard scores
    public string updateScoreboardScores()
    {
        string tempStr = string.Empty;

        for (int i = 0; i < ObjectFactory.instance.getNumberOfPlayers(); i++)
        {
            tempStr += ObjectFactory.instance.getPlayerScoreAtIndex(i).ToString();
            if (i < ObjectFactory.instance.getNumberOfPlayers() - 1)
            {
                tempStr += "\n";
            }
        }

        GameObject.Find("ScoreboardScores").GetComponent<Text>().text = tempStr;
        GameObject.Find("ScoreboardScores").GetComponent<Text>().rectTransform.sizeDelta = new Vector2(200, ObjectFactory.instance.getNumberOfPlayers() * 25);

        return tempStr;
    }
    
}
