using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class OpenDoor : MonoBehaviour
{
   
    float smooth = 4.0f;
    float DoorOpenAngle = 90.0f;
    private bool open;
    private bool enter;
    private Vector3 defaultRot;
    private Vector3 openRot;
    public GameObject door;
    private AudioSource audioSource;
    public AudioClip closeDoorAudio;
    public AudioClip openDoorAudio;

    void Start()
    {
      
        defaultRot = transform.eulerAngles;
        openRot = new Vector3(defaultRot.x, defaultRot.y + DoorOpenAngle, defaultRot.z);
        audioSource = door.GetComponent<AudioSource>();
    }
    
   

    void Update()
    {
        if (open)
        {
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRot, Time.deltaTime * smooth);
        }
        
        else
        {
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, defaultRot, Time.deltaTime * smooth);
        }

       

        if (Input.GetKeyDown("f") && enter)
        {
          
            open = !open;

            if (open)
            {
                smooth = 4.0f;
                audioSource.clip = openDoorAudio;
                audioSource.Play();
            }
            else
            {
                smooth = 10.0f;
                audioSource.clip = closeDoorAudio;
                audioSource.Play();
            }
        }
    }
    
  
    private void OnGUI()
    {
       
        if (enter)
        {
           
            GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 100, 150, 30), "Press 'F' to open the door");
        }
    }
    
   
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            
            Debug.Log("Player entered the trigger area.");
            enter = true;
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            
            enter = false;
        }
    }
}