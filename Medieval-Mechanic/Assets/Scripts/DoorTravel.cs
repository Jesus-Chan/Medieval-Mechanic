using System.Runtime.CompilerServices;
using UnityEngine;

public class DoorTravel : MonoBehaviour
{

    [SerializeField] private Transform DoorDestination;
    private bool onDoor = false;
    private GameObject Player;


    private void Update()
    {
        if(onDoor == true && Input.GetKeyDown(KeyCode.UpArrow) || onDoor == true && Input.GetKeyDown(KeyCode.W))
        {
            onDoor = false;
            Player.transform.position = DoorDestination.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player") == true)
        {
            GetComponent<Animator>().SetTrigger("StandingAtDoor");
            onDoor = true;
            Player = other.gameObject;  //spara spelarens referens
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<Animator>().SetTrigger("NotAtDoor");
            onDoor = false;
            Player = null; // clears reference
        }

    }
   

}
