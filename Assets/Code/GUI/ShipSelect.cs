using UnityEngine.UI;
using UnityEngine;

public class ShipSelect : MonoBehaviour
{
    private bool shipSelected = false;
    private bool weaponSelected = false;
    public int shipType {get; private set;}
    public int weaponType { get; private set; }

    // Use this for initialization
    void Start()
    {
        shipType = -1;
        weaponType = -1;
    }

    //changes the selected button for ships
    public void buttonShip(string button)
    {
        switch (button)
        {
            case "SkyRocket":
                GameObject.Find("SkyRocket").GetComponent<Button>().interactable = false;
                GameObject.Find("TheDagger").GetComponent<Button>().interactable = true;
                GameObject.Find("TheVulture").GetComponent<Button>().interactable = true;
                shipType = 0;
                shipSelected = true;
                break;

            case "TheDagger":
                GameObject.Find("SkyRocket").GetComponent<Button>().interactable = true;
                GameObject.Find("TheDagger").GetComponent<Button>().interactable = false;
                GameObject.Find("TheVulture").GetComponent<Button>().interactable = true;
                shipType = 1;
                shipSelected = true;
                break;

            case "TheVulture":
                GameObject.Find("SkyRocket").GetComponent<Button>().interactable = true;
                GameObject.Find("TheDagger").GetComponent<Button>().interactable = true;
                GameObject.Find("TheVulture").GetComponent<Button>().interactable = false;
                shipType = 2;
                shipSelected = true;
                break;
        }

        checkDepoly();
    }

    //changes the selected button for weapons
    public void buttonWeapon(string button)
    {
        switch (button)
        {
            case "Bolt":
                GameObject.Find("Bolt").GetComponent<Button>().interactable = false;
                GameObject.Find("Bullet").GetComponent<Button>().interactable = true;
                GameObject.Find("Laser").GetComponent<Button>().interactable = true;
                GameObject.Find("Missile").GetComponent<Button>().interactable = true;
                GameObject.Find("Plasma").GetComponent<Button>().interactable = true;
                weaponType = 0;
                weaponSelected = true;
                break;

            case "Bullet":
                GameObject.Find("Bolt").GetComponent<Button>().interactable = true;
                GameObject.Find("Bullet").GetComponent<Button>().interactable = false;
                GameObject.Find("Laser").GetComponent<Button>().interactable = true;
                GameObject.Find("Missile").GetComponent<Button>().interactable = true;
                GameObject.Find("Plasma").GetComponent<Button>().interactable = true;
                weaponType = 1;
                weaponSelected = true;
                break;

            case "Laser":
                GameObject.Find("Bolt").GetComponent<Button>().interactable = true;
                GameObject.Find("Bullet").GetComponent<Button>().interactable = true;
                GameObject.Find("Laser").GetComponent<Button>().interactable = false;
                GameObject.Find("Missile").GetComponent<Button>().interactable = true;
                GameObject.Find("Plasma").GetComponent<Button>().interactable = true;
                weaponType = 2;
                weaponSelected = true;
                break;

            case "Missile":
                GameObject.Find("Bolt").GetComponent<Button>().interactable = true;
                GameObject.Find("Bullet").GetComponent<Button>().interactable = true;
                GameObject.Find("Laser").GetComponent<Button>().interactable = true;
                GameObject.Find("Missile").GetComponent<Button>().interactable = false;
                GameObject.Find("Plasma").GetComponent<Button>().interactable = true;
                weaponType = 3;
                weaponSelected = true;
                break;

            case "Plasma":
                GameObject.Find("Bolt").GetComponent<Button>().interactable = true;
                GameObject.Find("Bullet").GetComponent<Button>().interactable = true;
                GameObject.Find("Laser").GetComponent<Button>().interactable = true;
                GameObject.Find("Missile").GetComponent<Button>().interactable = true;
                GameObject.Find("Plasma").GetComponent<Button>().interactable = false;
                weaponType = 4;
                weaponSelected = true;
                break;
        }

        checkDepoly();
    }

    //turns on the deploy button
    private void checkDepoly()
    {
        if (weaponSelected == true && shipSelected == true)
        {
            GameObject.Find("Deploy").GetComponent<Button>().interactable = true;
        }
    }

    //spawns the ship when clicked 
    public void deployButton()
    {
        shipSelected = false;
        weaponSelected = false;

        Game.instance.isInMenu = false;

        GameObject.Find("ShipSelect").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ShipSelect").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("ShipSelect").GetComponent<CanvasGroup>().blocksRaycasts = false;
        GameObject.Find("RightTop").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("HealthBar").GetComponent<Healthbar>().resetHealthbar();
        GameObject.Find("LeftTop").GetComponent<CanvasGroup>().alpha = 1;

        ObjectFactory.instance.createShip(AssetManager.instance.shipDictionary.getKey(shipType), AssetManager.instance.weaponDictionary.getKey(weaponType), true, false, false, Game.instance.idNumber);
    }
}
