using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    //prefabs
    public Transform projectile;

    //game objects
    private GameObject[] projectileArray = new GameObject[30];
    private int projectileCounter = 0; 

    //ship variables
    private string shipType;
    private string weaponType;
    public bool isPlayer { get; private set; }
    public bool isBot { get; private set; }
    private float maxSpeed;
    private float acceleration;
    private float rotation;
    private int maxHealth;
    private float firePointDistance;
    public int health { get; set; }
    public int IDnumber { get; set; }

    //ship movement
    private Vector2 movementVector = new Vector2(0, 0);
    private Vector2 moveDifference = new Vector2(0, 0);

    //mouse traking
    private float mouseAngle;
    private float angleBetween;
    private Vector2 distanceFromShip = new Vector2(0,0); 

    //weapon variables
    private Vector2 firePoint;
    private int ticker = 30;

    //log of input
    public string inputLog { get; set; }

    //in chat
    public bool inChat;

    // Use this for initialization
    void Start()
    {
        inputLog = string.Empty;
        inChat = false;
        this.transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0,360));
        createMinimapArrow();
        
        for (int i = 0; i < projectileArray.Length; i++)
        {
            projectileArray[i] = Instantiate(projectile.gameObject, new Vector2(), Quaternion.Euler(0, 0, 0)) as GameObject;
            projectileArray[i].SetActive(false);
        }
    }

    // Update is called at a fixed interval
    void FixedUpdate()
    {
        ticker++;

        wrapShip();

        if (isBot == false)
        {
            if (isPlayer == true)
            {
                inputLog = "1";

                findMouseAngle();
                turnShipTowardAngle(mouseAngle);
                playerInputGeneral();
                moveShip();
                setFirePoint();
                playerInputWeapon();
            }
            else
            {
                updateFromInputLog();
                moveShip();
            }
        }
        else
        {
            botMovement();
            moveShip();
            setFirePoint();
            botWeapons();
        }

        //minimap arrow
        ObjectFactory.instance.moveArrow(IDnumber, this.transform.position, this.transform.rotation.eulerAngles.z);
    }

    //collision detection
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Projectile>().IDnumber != IDnumber)
        {
            health -= Data.instance.weaponDictionary.getDamage(other.GetComponent<Projectile>().weaponType);
            other.gameObject.SetActive(false);
            if (isPlayer == true)
            {
                GameObject.Find("HealthBar").GetComponent<Healthbar>().setHealthbarSize(maxHealth, health);
            }
            if(health <= 0)
            {
                ObjectFactory.instance.addPlayerScore(other.GetComponent<Projectile>().IDnumber, 1);
                GameObject.Find("GUI").GetComponent<ScoreBoard>().updateScoreboardScores();
                destroyShip();
            }
            if (ObjectFactory.instance.getPlayerScore(other.GetComponent<Projectile>().IDnumber) >= Game.instance.scoreCap)
            {
                ObjectFactory.instance.endMatch();
            }
        }
    }

    //run when the object is turned off
    void OnDisable()
    {
        
    }

    //converts a -180 to +180 degree spreed to 0 - +360
    private float convertTo306(float angle)
    {
        if (angle < 0)
        {
            return 360 + angle;
        }
        return angle;
    }

    //sets all the statistics of the ship
    public void setShipStats(string ShipType, string WeaponType, bool IsPlayer, bool IsBot)
    {
        shipType = ShipType;
        weaponType = WeaponType;
        isPlayer = IsPlayer;
        isBot = IsBot;
        maxSpeed = Data.instance.shipDictionary.getMaxSpeed(ShipType);
        acceleration = Data.instance.shipDictionary.getAcceleration(ShipType);
        rotation = Data.instance.shipDictionary.getRotation(ShipType);
        health = Data.instance.shipDictionary.getHealth(ShipType);
        maxHealth = Data.instance.shipDictionary.getHealth(ShipType);
        firePointDistance = Data.instance.shipDictionary.getFirePointDistance(ShipType);
    }

    //creates a minimap arrow
    private void createMinimapArrow()
    {
        if (isPlayer == true)
        {
            ObjectFactory.instance.newArrow(IDnumber, "Player");
        }
        else if (isPlayer == false)
        {
            ObjectFactory.instance.newArrow(IDnumber, "Enemy");
        }
        else if (isPlayer == true && tag == "Friendly")
        {
            ObjectFactory.instance.newArrow(IDnumber, "Ally");
        }
    }

    //destroys the ship if health is 0 or lower
    private void destroyShip()
    {
        ObjectFactory.instance.createExplotion(IDnumber, transform.position);
        ObjectFactory.instance.deleteArrow(IDnumber);

        if (isPlayer == true)
        {
            GameObject.Find("RespawnCounter").GetComponent<RespawnCounter>().reset();
            GameObject.Find("RightTop").GetComponent<CanvasGroup>().alpha = 0;
        }
        if (isBot == true)
        {
            ObjectFactory.instance.createBotSpawn(shipType, weaponType, IDnumber);
        }

        gameObject.SetActive(false);
    }

    //wraps the ship around the level if out of bounds
    private void wrapShip()
    {
        if (transform.position.x <= 0)
        {
            transform.position = new Vector2(20.4f * Game.instance.levelSize, transform.position.y);
        }
        else if (transform.position.x >= 20.4f * Game.instance.levelSize)
        {
            transform.position = new Vector2(0, transform.position.y);
        }
        if (transform.position.y <= 0)
        {
            transform.position = new Vector2(transform.position.x, 20.4f * Game.instance.levelSize);
        }
        else if (transform.position.y >= 20.4f * Game.instance.levelSize)
        {
            transform.position = new Vector2(transform.position.x, 0);
        }
    }

    //gets input for movement
    private void playerInputGeneral()
    {
        if (inChat == false)
        {
            //moves the ship
            if (Input.GetButton("ThrusterForward") == true)
            {
                moveDifference.x = transform.position.x + (float)Math.Cos(Math.PI * transform.rotation.eulerAngles.z / 180.0) * acceleration;
                movementVector.x += transform.position.x - moveDifference.x;
                moveDifference.y = transform.position.y + (float)Math.Sin(Math.PI * transform.rotation.eulerAngles.z / 180.0) * acceleration;
                movementVector.y += transform.position.y - moveDifference.y;
                inputLog += "F";
            }
            if (Input.GetButton("ThrusterBack") == true)
            {
                moveDifference.x = transform.position.x - (float)Math.Cos(Math.PI * transform.rotation.eulerAngles.z / 180.0) * acceleration;
                movementVector.x += transform.position.x - moveDifference.x;
                moveDifference.y = transform.position.y - (float)Math.Sin(Math.PI * transform.rotation.eulerAngles.z / 180.0) * acceleration;
                movementVector.y += transform.position.y - moveDifference.y;
                inputLog += "B";

            }
            if (Input.GetButton("ThrusterRight") == true)
            {
                moveDifference.x = transform.position.x - (float)Math.Cos(Math.PI * (transform.rotation.eulerAngles.z + 90) / 180.0) * acceleration;
                movementVector.x += transform.position.x - moveDifference.x;
                moveDifference.y = transform.position.y - (float)Math.Sin(Math.PI * (transform.rotation.eulerAngles.z + 90) / 180.0) * acceleration;
                movementVector.y += transform.position.y - moveDifference.y;
                inputLog += "R";

            }
            if (Input.GetButton("ThrusterLeft") == true)
            {
                moveDifference.x = transform.position.x + (float)Math.Cos(Math.PI * (transform.rotation.eulerAngles.z + 90) / 180.0) * acceleration;
                movementVector.x += transform.position.x - moveDifference.x;
                moveDifference.y = transform.position.y + (float)Math.Sin(Math.PI * (transform.rotation.eulerAngles.z + 90) / 180.0) * acceleration;
                movementVector.y += transform.position.y - moveDifference.y;
                inputLog += "L";
            }

            //player suicide
            if (Input.GetButton("Suicide") == true)
            {
                health = 0;
                ObjectFactory.instance.addPlayerScore(IDnumber, -1);
                destroyShip();
            }
        }

        //moves the camera around
        Camera.main.transform.position = new Vector3(transform.position.x + (distanceFromShip.x / 4), transform.position.y + (distanceFromShip.y / 4), -10);
    }

    //fires the ships weapon when the mouse is clicked
    private void playerInputWeapon()
    {
        //fires your weapon
        if (Input.GetButton("Fire") == true)
        {
            inputLog += "W";
            createProjectile();
        }
    }

    //creates the projectile
    private void createProjectile()
    {
        if (ticker >= Data.instance.weaponDictionary.getFireRate(weaponType))
        {
            projectileArray[projectileCounter].SetActive(true);
            projectileArray[projectileCounter].transform.position = firePoint;
            projectileArray[projectileCounter].transform.rotation = transform.rotation;
            projectileArray[projectileCounter].GetComponent<Projectile>().speed = Data.instance.weaponDictionary.getProjectileSpeed(weaponType);
            projectileArray[projectileCounter].GetComponent<SpriteRenderer>().sprite = AssetManager.instance.weaponDictionary.getSprite(weaponType);
            projectileArray[projectileCounter].GetComponent<BoxCollider2D>().size = AssetManager.instance.weaponDictionary.getBoxCollider(weaponType);
            projectileArray[projectileCounter].GetComponent<Projectile>().weaponType = weaponType;
            projectileArray[projectileCounter].GetComponent<Projectile>().setMovmentVector(movementVector);
            projectileArray[projectileCounter].GetComponent<Projectile>().IDnumber = IDnumber;
            ticker = 0;

            projectileCounter++;
            if (projectileCounter == 30)
            {
                projectileCounter = 0;
            }
        }
    }

    //finds the angle between the mouse and the ship
    private void findMouseAngle()
    {
        distanceFromShip.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        distanceFromShip.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
        mouseAngle = (float)(Math.Atan2(distanceFromShip.y, distanceFromShip.x) * (180 / Math.PI));
        mouseAngle = convertTo306(mouseAngle);

        //adds the position of the mouse to the input log
        inputLog += ("X" + Camera.main.ScreenToWorldPoint(Input.mousePosition).x.ToString("F4") +
                     "Y" + Camera.main.ScreenToWorldPoint(Input.mousePosition).y.ToString("F4"));
    }

    //turns the ship toward the mouse
    private void turnShipTowardAngle(float angle)
    {
        if (angle > -1 && angle < 89 && transform.eulerAngles.z > 271 && transform.eulerAngles.z < 360)
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + rotation);
        }
        else if (transform.eulerAngles.z > -1 && transform.eulerAngles.z < 89 && angle > 271 && angle < 360)
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z - rotation);
        }
        else if (transform.eulerAngles.z < angle)
        {

            if (angle - transform.eulerAngles.z < rotation)
            {
                //transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + mouseAngle - transform.eulerAngles.z);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + rotation);
            }
        }
        else if (transform.eulerAngles.z > angle)
        {

            if (transform.eulerAngles.z - angle < rotation)
            {
                //transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z - mouseAngle + transform.eulerAngles.z);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z - rotation);
            }
        }
    }

    //finds the position of the fire point
    private void setFirePoint()
    {
        firePoint = new Vector2(transform.position.x + (float)Math.Cos(Math.PI * transform.rotation.eulerAngles.z / 180.0) * firePointDistance,
                        transform.position.y + (float)Math.Sin(Math.PI * transform.rotation.eulerAngles.z / 180.0) * firePointDistance);
    }

    //moves the ship
    private void moveShip()
    {
        if (movementVector.x > maxSpeed)
        {
            movementVector.x = maxSpeed;
        }
        else if (movementVector.x < -maxSpeed)
        {
            movementVector.x = -maxSpeed;
        }
        if (movementVector.y > maxSpeed)
        {
            movementVector.y = maxSpeed;
        }
        else if (movementVector.y < -maxSpeed)
        {
            movementVector.y = -maxSpeed;
        }

        transform.position = new Vector2(transform.position.x - movementVector.x, transform.position.y - movementVector.y);
    }

    //updates position from inputLog
    private void updateFromInputLog()
    {
        if (inputLog != string.Empty)
        {
            distanceFromShip.x = MessageParser.gameUpdateMousePositionX(inputLog) - transform.position.x;
            distanceFromShip.y = MessageParser.gameUpdateMousePositionY(inputLog) - transform.position.y;
            mouseAngle = (float)(Math.Atan2(distanceFromShip.y, distanceFromShip.x) * (180 / Math.PI));

            setFirePoint();

            for (int i = 1; i < 6; i++)
            {
                if (inputLog[i] == 'F')
                {
                    moveDifference.x = transform.position.x + (float)Math.Cos(Math.PI * transform.rotation.eulerAngles.z / 180.0) * acceleration;
                    movementVector.x += transform.position.x - moveDifference.x;
                    moveDifference.y = transform.position.y + (float)Math.Sin(Math.PI * transform.rotation.eulerAngles.z / 180.0) * acceleration;
                    movementVector.y += transform.position.y - moveDifference.y;
                }
                else if (inputLog[i] == 'B')
                {
                    moveDifference.x = transform.position.x - (float)Math.Cos(Math.PI * transform.rotation.eulerAngles.z / 180.0) * acceleration;
                    movementVector.x += transform.position.x - moveDifference.x;
                    moveDifference.y = transform.position.y - (float)Math.Sin(Math.PI * transform.rotation.eulerAngles.z / 180.0) * acceleration;
                    movementVector.y += transform.position.y - moveDifference.y;
                }
                else if (inputLog[i] == 'R')
                {
                    moveDifference.x = transform.position.x - (float)Math.Cos(Math.PI * (transform.rotation.eulerAngles.z + 90) / 180.0) * acceleration;
                    movementVector.x += transform.position.x - moveDifference.x;
                    moveDifference.y = transform.position.y - (float)Math.Sin(Math.PI * (transform.rotation.eulerAngles.z + 90) / 180.0) * acceleration;
                    movementVector.y += transform.position.y - moveDifference.y;
                }
                else if (inputLog[i] == 'L')
                {
                    moveDifference.x = transform.position.x + (float)Math.Cos(Math.PI * (transform.rotation.eulerAngles.z + 90) / 180.0) * acceleration;
                    movementVector.x += transform.position.x - moveDifference.x;
                    moveDifference.y = transform.position.y + (float)Math.Sin(Math.PI * (transform.rotation.eulerAngles.z + 90) / 180.0) * acceleration;
                    movementVector.y += transform.position.y - moveDifference.y;
                }
                else if (inputLog[i] == 'W')
                {
                    createProjectile();
                }
            }
        }
    }

    //moves the bot
    private void botMovement()
    {
        //finds enemy and turns towards it
        if (ObjectFactory.instance.getClosestShip(IDnumber) != -1)
        {
            turnShipTowardAngle(convertTo306((Mathf.Atan2(ObjectFactory.instance.getShipPostion(ObjectFactory.instance.getClosestShip(IDnumber)).y - this.transform.position.y,
                                                          ObjectFactory.instance.getShipPostion(ObjectFactory.instance.getClosestShip(IDnumber)).x - this.transform.position.x) * 180 / Mathf.PI)));

        }

        //moves the ship forward
        moveDifference.x = transform.position.x + (float)Math.Cos(Math.PI * transform.rotation.eulerAngles.z / 180.0) * acceleration;
        movementVector.x += transform.position.x - moveDifference.x;
        moveDifference.y = transform.position.y + (float)Math.Sin(Math.PI * transform.rotation.eulerAngles.z / 180.0) * acceleration;
        movementVector.y += transform.position.y - moveDifference.y;
    }

    //fires the bots weapons
    private void botWeapons()
    {
        if (ObjectFactory.instance.getClosestShip(IDnumber) != -1)
        {
            if (ObjectFactory.instance.getDistanceBetweenShips(IDnumber, ObjectFactory.instance.getClosestShip(IDnumber)) < 12)
            {
                createProjectile();
            }
        }
    }

    //destroys all projectiles
    public void destroyProjectiles()
    {
        for (int i = 0; i < projectileArray.Length; i++ )
        {
            if (projectileArray[i] != null)
            {
                Destroy(projectileArray[i]);
            }
        }
    }
}
