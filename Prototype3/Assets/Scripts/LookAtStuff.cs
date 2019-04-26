using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtStuff : MonoBehaviour
{
    // for use in RayCasting to interact with environment
    public Transform lookPoint;
    public Transform debugItem;
    public float castRange = 5;

    void Start()
    {
		
	}

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(lookPoint.position, lookPoint.forward, out hit, castRange))
        {
            debugItem.position = hit.point;
            Debug.Log(hit.transform.gameObject.name);
            if (Input.GetButtonDown("Fire1"))
            {
                hit.transform.root.gameObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
                hit.transform.root.gameObject.SendMessage("TakeDamage", 2, SendMessageOptions.DontRequireReceiver);
            }
        }
	}
}
