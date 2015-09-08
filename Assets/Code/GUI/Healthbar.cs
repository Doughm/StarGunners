using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    //sets the size of the heath bar
    public void setHealthbarSize(float total, float current)
    {
        this.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(this.GetComponent<Image>().rectTransform.sizeDelta.x, (current / total) * Screen.height);
    }

    //resets the healthbar
    public void resetHealthbar()
    {
        this.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(this.GetComponent<Image>().rectTransform.sizeDelta.x, Screen.height);
    }
}
