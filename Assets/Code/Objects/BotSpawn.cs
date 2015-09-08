using UnityEngine;
using System.Diagnostics;

public class BotSpawn
{
    private Stopwatch stopWatch = new Stopwatch();
    private int currentCount;
    public string shipType { get; private set; }
    public string weaponType { get; private set; }
    public int shipID { get; private set; }
    private bool isLapsed = false;

    public BotSpawn(string ship, string weapon, int IDnumber)
    {
        shipType = ship;
        weaponType = weapon;
        shipID = IDnumber;
        currentCount = Game.instance.respawnTime;
        stopWatch.Start();
    }


    //checks to see if the timer is up to respawn
    public void updateTime()
    {
        if (stopWatch.Elapsed.TotalSeconds > 1)
        {
            currentCount--;
            stopWatch.Stop();
            stopWatch.Reset();
            stopWatch.Start();
        }
        if (currentCount == 0)
        {
            isLapsed = true;
        }
    }

    //returns if the timer is lapsed
    public bool getLapsed()
    {
        return isLapsed;
    }
}
