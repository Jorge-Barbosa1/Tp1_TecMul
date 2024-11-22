using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interface2 : MonoBehaviour
{
    public void SelectGameMode(Button button)
    {
        if( button.name == "SoloButton")
        {
            GameMode.IsSinglePlayer = true; 
            SceneManager.LoadScene("Interface3");
        }
        else if(button.name == "1v1Button") 
        {
            GameMode.IsSinglePlayer = false;
            SceneManager.LoadScene("Interface3");// TODO : PLAY 1v1 Split-Screen
        }else if(button.name == "ExitButton")
        {
            SceneManager.LoadScene("Interface");
        }
    }
}