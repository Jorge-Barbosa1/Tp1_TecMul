using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera player1Camera;
    public Camera player2Camera;

    public GameObject player1Prefab;
    public GameObject player2Prefab;

    public Transform player1Spawn;
    public Transform player2Spawn;

    void Start()
    {
        if (GameMode.IsSinglePlayer)
        {
            // Configurar modo solo
            player1Camera.rect = new Rect(0, 0, 1, 1); // Tela inteira para o jogador 1
            player2Camera.gameObject.SetActive(false); // Desativa a câmera do jogador 2

            Instantiate(player1Prefab, player1Spawn.position, player1Spawn.rotation);
        }
        else
        {
            // Configurar modo 1v1
            player1Camera.rect = new Rect(0, 0, 0.5f, 1); // Metade esquerda da tela
            player2Camera.rect = new Rect(0.5f, 0, 0.5f, 1); // Metade direita da tela

            Instantiate(player1Prefab, player1Spawn.position, player1Spawn.rotation);
            Instantiate(player2Prefab, player2Spawn.position, player2Spawn.rotation);
        }
    }
}
