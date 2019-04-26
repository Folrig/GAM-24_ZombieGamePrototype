using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSafeZone : MonoBehaviour
{
    #region Events
    public delegate void EnterSafeZone();
    public static event EnterSafeZone SafeZoneEvent;
    #endregion

    #region Methods
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SafeZoneEvent();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SafeZoneEvent();
        }
    }
    #endregion
}
