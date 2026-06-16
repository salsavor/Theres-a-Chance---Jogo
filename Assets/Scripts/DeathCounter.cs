using UnityEngine;
using UnityEngine.UI;

public class DeathCounter : MonoBehaviour
{
    private Text textoUI;

    void Start()
    {
        textoUI = GetComponent<Text>();
    }

    void Update()
    {
        if (GameManager.instance != null)
            textoUI.text = GameManager.instance.deathCount.ToString();
    }
}