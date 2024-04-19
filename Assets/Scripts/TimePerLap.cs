
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class TimePerLap : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI timerText;
    float lapTime;

    bool startTimer = false;
    private bool trigger2 = false;
    private bool trigger3 = false;

    private void Update()
    {
        if (startTimer == true)
        {
            lapTime += Time.deltaTime;
            int min = Mathf.FloorToInt(lapTime / 60);
            int sec = Mathf.FloorToInt(lapTime % 60);
            int milisec = Mathf.FloorToInt((lapTime * 1000) % 1000);
            timerText.text = string.Format("{0:00} : {01:00}: {2:000}", min, sec, milisec);

            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       
        //startTimer = true;

        if (other.gameObject.name == "Trigger")
        {
            if(trigger2 == true && trigger3 == true)
            {
                startTimer = false;
                StartCoroutine(ShowLapTimeAfterDelay(0.5f));// Serve para aparecer o painel ao fim de 2 segundo depois do ultimo trigger
            }
            else
            {
                startTimer = true;
                trigger2 = false;
                trigger3 = false;
            }
        }
        
        if(other.gameObject.name == "Trigger2")
        {
            Debug.Log("1º sector: " + timerText.text);
            trigger2 = true;
        }

        if (other.gameObject.name == "Trigger3")
        {
            Debug.Log("2º sector: " + timerText.text);
            trigger3 = true;
        }
    }

    public string GetLapTime()
    {
        return timerText.text;
    }

    IEnumerator ShowLapTimeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); //Esperar um certo tempo para aparecer o Panel
        FindObjectOfType<EndLapController>().ShowPanel(); //Chama o metodo para aparecer o painel
    }

}
