using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AltarTrigger : MonoBehaviour
{
    #region Events
    public delegate void WinGame();
    public static event WinGame WinGameEvent;
    #endregion

    #region Variables
    private int _statuesCollected = 0;
    #endregion

    #region Methods
    private void Start()
    {
        Player.StatueCollectedEvent += IncreaseStatueTotal;
    }

    private void Destroy()
    {
        Player.StatueCollectedEvent -= IncreaseStatueTotal;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && _statuesCollected >= 3)
        {
            WinGameEvent();
        }
    }

    private void IncreaseStatueTotal()
    {
        this._statuesCollected++;
    }
    #endregion
}