using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class RespawnCounter : MonoBehaviour
{
    private Stopwatch stopWatch = new Stopwatch();
    private int currentCount;

    // Use this for initialization
    void Start()
    {
        currentCount = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopWatch.Elapsed.TotalSeconds > 1)
        {
            currentCount--;
            this.GetComponent<Text>().text = currentCount.ToString();
            stopWatch.Stop();
            stopWatch.Reset();
            stopWatch.Start();
        }
        if (currentCount == 0)
        {
            stopWatch.Stop();
            currentCount = -1;
            GameObject.Find("ShipSelect").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("ShipSelect").GetComponent<CanvasGroup>().interactable = true;
            GameObject.Find("ShipSelect").GetComponent<CanvasGroup>().blocksRaycasts = true;
            this.GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("Scoreboard").GetComponent<CanvasGroup>().alpha = 0;
            Game.instance.isInMenu = true;
            GameObject.Find("GameMenu").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("GameMenu").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("GameMenu").GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    //resets the stopwatch
    public void reset()
    {
        this.GetComponent<CanvasGroup>().alpha = 1;
        this.GetComponent<Text>().text = Game.instance.respawnTime.ToString();
        currentCount = Game.instance.respawnTime;
        stopWatch.Reset();
        stopWatch.Start();
    }

    //stops the clock
    public void stopCounter()
    {
        stopWatch.Stop();
        this.GetComponent<CanvasGroup>().alpha = 0;
    }

    //starts the clock
    public void startCounter()
    {
        stopWatch.Reset();
        stopWatch.Start();
    }
}
