using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    Button btnComponent;
    // Start is called before the first frame update
    void Start()
    {
        btnComponent = GetComponent<Button>();
        btnComponent.onClick.AddListener(doExitGame);
    }

    private void doExitGame(){
        Debug.Log("Quitting the game");
        Application.Quit();
    }
}
