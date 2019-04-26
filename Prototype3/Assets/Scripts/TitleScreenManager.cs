using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScreenManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject[] _titleScreenImages;
    [SerializeField] private GameObject[] _titleScreenText;
    private bool _introTextScreen = false;
    #endregion

    #region Methods
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && _introTextScreen)
        {
            SceneManager.LoadScene("MainScene");
        }
        if (Input.GetButtonDown("Fire1") && !_introTextScreen)
        {
            _titleScreenImages[1].SetActive(true);
            _titleScreenImages[0].SetActive(false);
            _titleScreenText[0].SetActive(false);
            _titleScreenText[1].SetActive(false);
            _titleScreenText[2].SetActive(true);
            _titleScreenText[3].SetActive(true);
            _titleScreenText[4].SetActive(true);
            _introTextScreen = true;
        }
	}
    #endregion
}
