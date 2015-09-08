using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        //enters message into chat
        if (Input.GetButtonDown("Chat") == true)
        {
            sendToChat();
        }
	}

    //sends text to the chat box
    public void sendToChat()
    {
        GameObject.Find("ChatText").GetComponentInChildren<Text>().text =
            GameObject.Find("ChatText").GetComponentInChildren<Text>().text +
            GameObject.Find("ChatInput").GetComponent<InputField>().text + "\n";
        GameObject.Find("ChatInput").GetComponent<InputField>().text = string.Empty;
        //todo
        //network stuff
    }

    //sets the username
    public void setUsername(string username)
    {
        
    }
};
