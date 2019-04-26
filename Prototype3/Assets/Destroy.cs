using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    private void PickedUp()
    {
        Destroy(this.gameObject);
    }
}
