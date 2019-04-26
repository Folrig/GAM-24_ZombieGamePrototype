using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    #region Variables
    [SerializeField] private SphereCollider _flashlightTriggerZone;
    [SerializeField] private SphereCollider _darknessTriggerZone;
    [SerializeField] private float _idleSpeed = 1.5f;
    [SerializeField] private float _runSpeed = 3.0f;
    private float _randomX = 500.0f;
    private float _randomZ = 500.0f;
    [SerializeField] private Vector3 _randomPos;
    [SerializeField] private Vector3 _currentPos;
    [SerializeField] private float _moveTime = 10.0f;
    [SerializeField] private float _moveTimer = 0.0f;
    [SerializeField] private GameObject _player;
    private List<Vector3> _respawnPoints = new List<Vector3>();
    [SerializeField] private bool _isTriggered = false;      // I'm sure this could be an enum but I don't know how to do it
    [SerializeField] private bool _isFlashlightOn = true;
    [SerializeField] private bool _isPlayerSafe = false;
    private bool _hasAttackSoundPlayed = false;
    [SerializeField] private Animator _zombieWalkAnimation;

    [SerializeField] private Terrain terrain;
    private int _terrainRandomX;
    private int _terrainRandomY;
    private float _terrainHeight;
    
    #region Sound Effects
    [SerializeField] private AudioClip[] _zombieIdleSounds;
    [SerializeField] private AudioClip[] _zombieAttackSounds;
    [SerializeField] private AudioSource _zombieIdleSource;
    [SerializeField] private AudioSource _zombieAttackSource;
    private float _soundTimer = 0.0f;
    private float _timeBeforeSound = 5.0f;
    #endregion

    #endregion

    #region Methods
    public void Start()
    {
        Player.FlashlightToggleEvent += TriggerZoneToggle;
        HouseSafeZone.SafeZoneEvent += PlayerSafeToggle;
        _currentPos = this.transform.position;

        _terrainRandomX = Random.Range(0, 600);
        _terrainRandomY = Random.Range(0, 600);
        _terrainHeight = terrain.terrainData.GetHeight(_terrainRandomX, _terrainRandomY);
        _randomPos = new Vector3(_terrainRandomX, _terrainHeight, _terrainRandomY);

        _player = GameObject.FindGameObjectWithTag("Player");
        _timeBeforeSound = Random.Range(5.0f, 22.0f);

        //Set spawn points
        _respawnPoints.Add(new Vector3(69, 55, 60));
        _respawnPoints.Add(new Vector3(60, 51, 494));
        _respawnPoints.Add(new Vector3(60, 53, 309));
        _respawnPoints.Add(new Vector3(293, 54, 48));
        _respawnPoints.Add(new Vector3(507, 69, 85));
        _respawnPoints.Add(new Vector3(548, 61, 254));
        _respawnPoints.Add(new Vector3(554, 52, 520));
        _respawnPoints.Add(new Vector3(309, 52, 425));
    }

    public void Destroy()
    {
        Player.FlashlightToggleEvent -= TriggerZoneToggle;
        HouseSafeZone.SafeZoneEvent -= PlayerSafeToggle;
        Destroy(gameObject);
    }

    void Update()
    {
        // Play random groaning sounds
        _soundTimer += Time.deltaTime;
        if (_soundTimer >= _timeBeforeSound)
        {
            _soundTimer = 0;
            _timeBeforeSound = Random.Range(11.0f, 25.0f);
            int random = Random.Range(0, _zombieIdleSounds.Length);
            _zombieIdleSource.clip = _zombieIdleSounds[random];
            _zombieIdleSource.Play();
        }
    }

    public void FixedUpdate()
    {
        _moveTimer += Time.fixedDeltaTime;
        // Random wandering if player is not near
        if (!_isTriggered)
        {
            _hasAttackSoundPlayed = false;
            if (_moveTimer <= _moveTime)
            {
                //_moveTimer += Time.fixedDeltaTime;
                this.transform.LookAt(_randomPos);
                this.transform.Translate(Vector3.forward * Time.fixedDeltaTime * _idleSpeed);
                _zombieWalkAnimation.speed = 0.95f;
                //float distanceCovered = _moveTimer * _idleSpeed;
                //float journeyPercentage = distanceCovered / Vector3.Distance(_currentPos, _randomPos);
                //this.transform.position = Vector3.Lerp(_currentPos, _randomPos, journeyPercentage);
            }
            // Find a new place to wander after a time
            if (_moveTimer >= _moveTime)
            {
                _moveTimer = 0;
                _currentPos = this.transform.position;
                _terrainRandomX = Random.Range(0, 500);
                _terrainRandomY = Random.Range(0, 500);
                _terrainHeight = terrain.terrainData.GetHeight(_terrainRandomX, _terrainRandomY);
                _randomPos = new Vector3(_terrainRandomX, _terrainHeight, _terrainRandomY);
            }
        }
            //// Speed increase and head for player when near
        if (!_isPlayerSafe)
        {
            if (_isTriggered)
            {
                if (!_hasAttackSoundPlayed)
                {
                    _hasAttackSoundPlayed = true;
                    int random = Random.Range(0, _zombieAttackSounds.Length);
                    _zombieAttackSource.clip = _zombieAttackSounds[random];
                    _zombieAttackSource.Play();
                }
                this.transform.LookAt(_player.transform.position);
                this.transform.Translate(Vector3.forward * Time.fixedDeltaTime * _runSpeed);
                _zombieWalkAnimation.speed = 1.75f;
            }
        }
    }
    // Adjust trigger size if flashlight is on. Is this better done by changing the size instead of using two triggers?
    private void TriggerZoneToggle()
    {
        if (!_isFlashlightOn)
        {
            _isFlashlightOn = true;
        }
        else
        {
            _isFlashlightOn = false;
        }
        if (_isFlashlightOn)
        {
            _flashlightTriggerZone.center = new Vector3(0, 5, 0);
            _darknessTriggerZone.center = new Vector3(0, 250, 0);
        }
        else
        {
            _flashlightTriggerZone.center = new Vector3(0, 250, 0);
            _darknessTriggerZone.center = new Vector3(0, 5, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _currentPos = this.transform.position;
            _isTriggered = true;
            _moveTimer = 0.0f;
            // For some reason, it plays the sound every time the light is toggled. Bool added and code moved to FixedUpdate
            //int random = Random.Range(0, _zombieAttackSounds.Length);
            //_zombieAttackSource.clip = _zombieAttackSounds[random];
            //_zombieAttackSource.Play();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !_isPlayerSafe && !_isTriggered)
        {
            _currentPos = this.transform.position;
            _isTriggered = true;
            int random = Random.Range(0, _zombieAttackSounds.Length);
            _zombieAttackSource.clip = _zombieAttackSounds[random];
            _zombieAttackSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isTriggered = false;
            _moveTimer = 0.0f;
            _currentPos = this.transform.position;
            _randomPos = new Vector3(Random.Range(0, _randomX), 0, Random.Range(0, _randomZ));
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("Damage", SendMessageOptions.DontRequireReceiver);
            int randomSound = Random.Range(0, _zombieAttackSounds.Length);
            _zombieAttackSource.clip = _zombieAttackSounds[randomSound];
            _zombieAttackSource.Play();
            int randomSpawn = Random.Range(0, _respawnPoints.Count);
            this.transform.position = _respawnPoints[randomSpawn];
        }
    }
    // For not hunting player if they enter a building
    private void PlayerSafeToggle()
    {
        if (!_isPlayerSafe)
        {
            _currentPos = this.transform.position;
            _isPlayerSafe = true;
            _isTriggered = false;
            _moveTimer = 0.0f;
        }
        else
        {
            _isPlayerSafe = false;
            _moveTimer = 0.0f;
        }
    }
    #endregion
}
