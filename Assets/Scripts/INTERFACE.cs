using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class INTERFACE : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Interface2");
    }
    public void OnApplicationQuit()
    {
        Application.Quit();
        Debug.Log("Player has Disconnected");
    }

}