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
            SceneManager.LoadScene("Level1");
        }
        else if(button.name == "1v1Button")
        {
            SceneManager.LoadScene("Level2");
        }else if(button.name == "ExitButton")
        {
            SceneManager.LoadScene("Interface");
        }
    }
}

