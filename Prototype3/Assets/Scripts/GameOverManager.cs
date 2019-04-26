using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    #region Variables
    private GameObject _bgmMusic;
    #endregion

    #region Methods
    private void Start()
    {
        if (_bgmMusic == null)
        {
            _bgmMusic = GameObject.Find("Background Music");
            Destroy(_bgmMusic);
        }
    }
    void Update()
    {
		if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Fire1"))
        {
            Application.Quit();
        }
	}
    #endregion
}
