using UnityEngine;

public class AnimationScript: MonoBehaviour
{
    //animation variables
    public string animationName { get; set; }
    private int ticker;
    private int frameCounter = 0;
    
	// Use this for initialization
	void Start ()
    {
        ticker = 0;
	}

    // Update is called at a fixed interval
	void FixedUpdate ()
    {
        //Destroys the object at the end of the animation
        if (frameCounter == AssetManager.instance.animationDictionary.getArraySize(animationName) - 1 &&
            AssetManager.instance.animationDictionary.getDestroyOnEnd(animationName) == true)
        {
            gameObject.SetActive(false);
        }

        //plays the animation
        if (ticker >= AssetManager.instance.animationDictionary.getSpeed(animationName))
        {
            frameCounter++;
            if (frameCounter >= AssetManager.instance.animationDictionary.getArraySize(animationName))
            {
                if (AssetManager.instance.animationDictionary.getRepeat(animationName) == true)
                {
                    frameCounter = 0;
                }
                else
                {
                    frameCounter -= 1;
                }
            }
            ticker = 0;
            gameObject.GetComponent<SpriteRenderer>().sprite = AssetManager.instance.animationDictionary.getSprite(animationName, frameCounter);
        }
        else
        {
            ticker++;
        }
	}

    //resets the animation counter
    public void reset()
    {
        ticker = 0;
        frameCounter = 0;
    }
}
