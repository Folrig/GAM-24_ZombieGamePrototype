using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    #region Events
    public delegate void StatueCollected();
    public delegate void FlashlightToggle();
    public delegate void TakeDamage();
    public delegate void PlayerDied();
    public static event StatueCollected StatueCollectedEvent;
    public static event FlashlightToggle FlashlightToggleEvent;
    public static event TakeDamage TakeDamageEvent;
    public static event PlayerDied PlayerDiedEvent;
    #endregion

    #region Variables
    [SerializeField] private int _health = 3;
    [SerializeField] private Light _flashLight;
    [SerializeField] private Transform _lookPoint;
    [SerializeField] private float _castRange = 5.0f;
    [SerializeField] private AudioClip[] _damageSounds;
    [SerializeField] private AudioClip[] _deathSounds;
    [SerializeField] private AudioClip[] _flashlightSounds;
    public Transform _targetItem;

    [SerializeField] private AudioSource _damageSoundEffects;
    [SerializeField] private AudioSource _flashlightSource;
    private bool _isDead = false;

    private KeyCode _camera = KeyCode.Q;
    [SerializeField] private Camera _fpsCamera;
    [SerializeField] private Camera _mapCamera;
    private bool _mapCameraToggle = false;
    #endregion

    #region Properties
    public int Health { get { return this._health; } }
    #endregion

    #region Methods
    private void Start()
    {
        _fpsCamera.enabled = true;
        _mapCamera.enabled = false;
    }

    void Update()
    {
        if (this._health <= 0 && !_isDead)
        {
            PlayerDeath();
        }

        if (Input.GetButtonDown("Fire2") && _flashLight.intensity == 0.0f)
        {
            int random = Random.Range(0, _flashlightSounds.Length);
            _flashlightSource.clip = _flashlightSounds[random];
            _flashlightSource.Play();
            _flashLight.intensity = 0.90f;
            FlashlightToggleEvent();
        }
        else if (Input.GetButtonDown("Fire2") && _flashLight.intensity == 0.90f)
        {
            int random = Random.Range(0, _flashlightSounds.Length);
            _flashlightSource.clip = _flashlightSounds[random];
            _flashlightSource.Play();
            _flashLight.intensity = 0.0f;
            FlashlightToggleEvent();
        }

        if (Input.GetKeyDown(_camera) && !_mapCameraToggle)
        {
            _fpsCamera.enabled = false;
            _mapCamera.enabled = true;
            _mapCameraToggle = true;

        }
        else if (Input.GetKeyDown(_camera) && _mapCameraToggle)
        {
            _mapCamera.enabled = false;
            _fpsCamera.enabled = true;
            _mapCameraToggle = false;
        }

        RaycastHit hit;
        if (Physics.Raycast(_lookPoint.position, _lookPoint.forward, out hit, _castRange))
        {
            _targetItem.position = hit.point;
            if (Input.GetButtonDown("Fire1") && hit.transform.gameObject.tag == "Statue")
            {
                hit.transform.gameObject.SendMessage("PickedUp", SendMessageOptions.DontRequireReceiver);
                StatueCollectedEvent();
            }
        }
    }

    private void PlayerDeath()
    {
        _isDead = true;
        PlayerDiedEvent();
        int randomSound = Random.Range(0, _deathSounds.Length);
        _damageSoundEffects.clip = _deathSounds[randomSound];
        _damageSoundEffects.Play();
    }

    private void Damage()
    {
        TakeDamageEvent();
        this._health -= 1;
        if (this._health > 0)
        {
            int randomHitSound = Random.Range(0, _damageSounds.Length);
            _damageSoundEffects.clip = _damageSounds[randomHitSound];
            _damageSoundEffects.Play();
        }
    }
    #endregion
}