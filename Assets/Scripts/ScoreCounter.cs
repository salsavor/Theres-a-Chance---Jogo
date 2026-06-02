using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public Text scoreNumber; // referência ao componente Text do Canvas
    public static int coinAmount = 0; // valor do score, static para ser acedido externamente

    void Start()
    {
        coinAmount = 0; // reset do score ao iniciar a cena
    }

    void Update()
    {
        if (scoreNumber != null) // verifica se a referência ao Text existe
            scoreNumber.text = coinAmount.ToString(); // atualiza o texto com o valor atual do score
    }
}