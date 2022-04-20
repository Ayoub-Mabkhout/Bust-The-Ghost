using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Click : MonoBehaviour
{   
    Button btnComponent;
    // Start is called before the first frame update
    void Start()
    {
        btnComponent = GetComponent<Button>();
        btnComponent.onClick.AddListener(ChangeColor);
    }

    private void ChangeColor(){
        Image img = GetComponent<Image>();
        img.color = new Color(1,0,0,1);
        GridManager.bust = true;
    }
}
