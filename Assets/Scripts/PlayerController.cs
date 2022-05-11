using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray r = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, r.direction * 10, out hit))
        {
            //Debug.Log("Object hit: " + hit.transform.name);
            Door d = hit.transform.GetComponent<Door>();
            if (d != null)
            {
                Debug.Log("Door is not null");
                d.OpenDoor((hit.point - transform.position).normalized * Vector3.Dot(r.direction, hit.transform.forward) * 100);
            }
        }
    }
}
