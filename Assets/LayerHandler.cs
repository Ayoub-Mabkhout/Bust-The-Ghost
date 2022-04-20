using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Renderer>().sortingLayerID = 
            this.transform.parent.GetComponent<Renderer>().sortingLayerID;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
