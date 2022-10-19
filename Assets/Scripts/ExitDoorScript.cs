using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorScript : MonoBehaviour
{
    public int keysNeeded = 0;
    private int currentKeys = 0;
    public Transform doorRotationPoint;
    public float rotateTime = 1f;
    public float doorRotation = 90f;
    private Quaternion endRotation;
    bool unlocked = true;
    bool isOpen = false;
    bool movingToNextScene = false;
    bool nextToPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        endRotation.eulerAngles = new Vector3(0, doorRotation + doorRotationPoint.rotation.eulerAngles.y, 0);
        if(keysNeeded > 0){
            unlocked = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            if(!movingToNextScene && unlocked){
                movingToNextScene = true;
                LevelLoaderScript.instance.LoadNextScene();
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    public void ObtainKey(){
        currentKeys++;
        if(keysNeeded <= currentKeys && !unlocked){
            unlocked = true;
            AudioManagerScript.instance.PlaySound2D("Get Final Key");
            StartCoroutine(RotateDoor());
        }
        else{
            AudioManagerScript.instance.PlaySound2D("Get Key");
        }
    }

    IEnumerator RotateDoor(){
        Quaternion originalRotation;
        Quaternion newRotation;
        if(isOpen){
            originalRotation = endRotation;
            newRotation = transform.rotation;
        }
        else{
            originalRotation = transform.rotation;
            newRotation = endRotation;
        }
        
        
        float rotateSpeed = 1 / rotateTime;
        float percent = 0;
        //AudioManagerScript.instance.PlaySound("Enemy Attack", transform.position);

        while(percent <= 1){

            percent += Time.deltaTime * rotateSpeed;
            float interpolation = percent * percent * (3f - 2f * percent);
            doorRotationPoint.localRotation = Quaternion.Lerp(originalRotation, newRotation, interpolation);

            yield return null;
        }
        isOpen = !isOpen;
    }
}
