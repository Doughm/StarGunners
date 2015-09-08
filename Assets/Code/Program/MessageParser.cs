using UnityEngine;
using System;
using System.Globalization;

public static class MessageParser
{
    //returns the position of the mouse X and Y for game update
    static public Vector2 gameUpdateMousePosition(string message)
    {
        int xPlace = 0;
        //x
        for (int i = 0; i < message.Length; i++)
        {
            if (message[i] == 'X')
            {
                xPlace = i;
                break;
            }
        }
        //y
        for (int i = 0; i < message.Length; i++)
        {
            if (message[i] == 'Y')
            {
                return new Vector2(float.Parse(message.Substring(xPlace + 1, i - xPlace - 1), CultureInfo.CurrentCulture),
                                   float.Parse(message.Substring(i + 1), CultureInfo.CurrentCulture));
            }
        }
        return new Vector2();
    }

    //returns the position of the mouse X for game update
    static public float gameUpdateMousePositionX(string message)
    {
        int xPlace = 0;
        //x
        for (int i = 0; i < message.Length; i++)
        {
            if (message[i] == 'X')
            {
                xPlace = i;
                break;
            }
        }
        //y
        for (int i = 0; i < message.Length; i++)
        {
            if (message[i] == 'Y')
            {
                return float.Parse(message.Substring(xPlace + 1, i - xPlace - 1), NumberStyles.Float, CultureInfo.CurrentCulture);
            }
        }
        return 0;
    }

    //returns the position of the mouse Y for game update
    static public float gameUpdateMousePositionY(string message)
    {
        for (int i = 0; i < message.Length; i++)
        {
            if (message[i] == 'Y')
            {
                return float.Parse(message.Substring(i + 1), NumberStyles.Float, CultureInfo.CurrentCulture);
            }
        }
        return 0;
    }

    //returns how many buttons are being pressed for game update
    static public int gameUpdateNumberOfKeysPressed(string message)
    {
        for (int i = 1; i < message.Length; i++)
        {
            if (message[i] == 'X')
            {
                return i - 1;
            }
        }
        return 0;
    }

    //returns the number of delimiters in a string
    static public int getNumberOfDelimiters(string message, char delimiter)
    {
        int counter = 0;
        for (int i = 0; i < message.Length; i++)
        {
            if (message[i] == delimiter)
            {
                counter++;
            }
        }
        return counter;
    }

    //returns the string at delimiter X
    static public string getDelimiterString(string message, int place, char delimiter)
    {
        string[] split = message.Split(delimiter);
        return split[place];
    }

    //returns the float at delimiter X
    static public float getDelimiterFloat(string message, int place, char delimiter)
    {
        string[] split = message.Split(delimiter);
        return (float)Convert.ToDouble(split[place]);
    }
}
