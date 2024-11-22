using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Interface3 : MonoBehaviour
{
    public void SelectTrack(Button button)
    {
        if( button.name == "Lvl1Button")
        {
            SceneManager.LoadScene("Level1");
        }
        else if(button.name == "Lvl2Button")
        {
            SceneManager.LoadScene("Level2");
        }
        else if(button.name == "Lvl3Button") 
        {
            SceneManager.LoadScene("Level3");
        }else if(button.name == "ExitButton")
        {
            SceneManager.LoadScene("Interface");
        }
    }

}
