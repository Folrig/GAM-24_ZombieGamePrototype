using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject[] _statuesCollectedUI;
    private int _statuesCollected = 0;
    [SerializeField] private Image[] _healthImagesUI;
    private int _playerHealthRemaining = 3;
    [SerializeField] private Image _gameOverFade;
    #endregion

    #region Methods
    private void Start()
    {
        Player.StatueCollectedEvent += StatueUIActivate;
        Player.TakeDamageEvent += HealthUIActivate;
        Player.PlayerDiedEvent += GameOverActivate;
        AltarTrigger.WinGameEvent += WinGameActivate;
    }

    private void Destroy()
    {
        Player.StatueCollectedEvent -= StatueUIActivate;
        Player.TakeDamageEvent -= HealthUIActivate;
        Player.PlayerDiedEvent -= GameOverActivate;
        AltarTrigger.WinGameEvent -= WinGameActivate;
    }

	private void Update()
    {
		
	}

    private void StatueUIActivate()
    {
        _statuesCollectedUI[_statuesCollected].SetActive(true);
        if (_statuesCollected < 2)
        {
            _statuesCollected++;
        }
    }

    private void HealthUIActivate()
    { 
        --_playerHealthRemaining;
        _healthImagesUI[_playerHealthRemaining].enabled = false;
    }

    private void GameOverActivate()
    {
        StartCoroutine(GameOver());
    }

    private void WinGameActivate()
    {
        StartCoroutine(WinGame());
    }

    private IEnumerator GameOver()
    {
        _gameOverFade.CrossFadeAlpha(255, 4.0f, false);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("GameOverScene");
    }

    private IEnumerator WinGame()
    {
        _gameOverFade.CrossFadeAlpha(255, 4.0f, false);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("WinGameScene");
    }
    #endregion
}
