using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public ExitDoorScript doorScript;
    // Start is called before the first frame update
    void Start()
    {
        if(doorScript == null){
            doorScript = FindObjectOfType<ExitDoorScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            doorScript.ObtainKey();
            Destroy(gameObject);
        }
    }
}
