using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Camera player1Camera;
    public Camera player2Camera;

    void Start()
    {

        if (GameMode.IsSinglePlayer)
        {
            setupSinglePlayerMode();
            player2Camera.gameObject.SetActive(false);
        }
        else
        {
            setup1v1Mode();
        }
    }

    void setupSinglePlayerMode()
    {
        GameObject player2 = GameObject.Find("Player2");
        if(player2 != null)
        {
            player2.SetActive(false); //Deactivate player 2
        }

        player1Camera.rect = new Rect(0, 0, 1, 1); //Full screen
        player2Camera.gameObject.SetActive(false); 

    }
    void setup1v1Mode()
    {
        GameObject player2 = GameObject.Find("Player2");
        if(player2 != null)
        {
            player2.SetActive(true); //Activate player 2
        }

        //Split screen
        player1Camera.rect = new Rect(0, 0, 0.5f, 1);
        player2Camera.rect = new Rect(0.5f, 0, 0.5f, 1);

        player2Camera.gameObject.SetActive(true);
    }
}