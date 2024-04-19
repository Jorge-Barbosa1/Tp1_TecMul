using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndLapController : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI lapTimeText;

    public void ShowPanel()
    {
        panel.SetActive(true);
        panel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate
        {
            SceneManager.LoadScene("Interface2");
        });
    } 
}
