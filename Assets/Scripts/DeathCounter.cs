using UnityEngine;
using UnityEngine.UI;

public class DeathCounter : MonoBehaviour
{
    public static int deathCount = 0;

    private Text textoUI;

    void Start()
    {
        textoUI = GetComponent<Text>(); 
    }

    void Update()
    {
        textoUI.text = deathCount.ToString();
    }
}