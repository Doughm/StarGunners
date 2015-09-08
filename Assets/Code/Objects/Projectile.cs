using UnityEngine;
using System.Collections;
using System;

public class Projectile : MonoBehaviour
{
    public string weaponType { get; set; }
    public float speed { get; set; }
    public int IDnumber { get; set; }
    private Vector2 movementVector = new Vector2();

    // Update is called at a fixed interval
    void FixedUpdate()
    {
        wrapProjectile();
        moveProjectile();
    }
    
    //run when the object is turned on
    void OnEnable()
    {
        Invoke("turnOff", 1);
    }

    //run when the object is turned off
    void OnDisable()
    {
        CancelInvoke();
    }

    //turns the object off
    private void turnOff()
    {
        gameObject.SetActive(false);
    }

    //wraps the ship around the map
    private void wrapProjectile()
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

    //moves the projectile
    private void moveProjectile()
    {
        transform.position = new Vector2(transform.position.x - movementVector.x, transform.position.y - movementVector.y);
    }

    //sets the vector for movement
    public void setMovmentVector(Vector2 vector)
    {
        movementVector = vector;
        movementVector = new Vector2(movementVector.x - ((float)Math.Cos(Math.PI * transform.rotation.eulerAngles.z / 180.0) * speed),
                                     movementVector.y - ((float)Math.Sin(Math.PI * transform.rotation.eulerAngles.z / 180.0) * speed));
    }
}
