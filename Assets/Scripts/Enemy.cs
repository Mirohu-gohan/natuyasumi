using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Floating")]
    [SerializeField] private Vector2 floatSpeedRange = new Vector2(0.8f, 1.2f);
    [SerializeField] private Vector2 floatMagnitudeRange = new Vector2(0.3f, 0.7f);

    [Header("Damage Reaction")]
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.1f;
    [SerializeField] private float colorChangeDuration = 0.3f;

    private float _floatSpeed;
    private float _floatMagnitude;
    private Vector3 _initialPosition;
    private float _initialY;
    private bool _isShaking = false;
    private float _shakeTimer = 0f;
    private Color _initialColor;
    private Renderer _renderer;

    void Start()
    {
        _floatSpeed = Random.Range(floatSpeedRange.x, floatSpeedRange.y);
        _floatMagnitude = Random.Range(floatMagnitudeRange.x, floatMagnitudeRange.y);

        _initialPosition = transform.position;
        _initialY = transform.position.y;
        _renderer = GetComponent<Renderer>();
        _initialColor = _renderer.material.color;
    }

    void Update()
    {
        HandleFloating();
        HandleShaking();
    }

    private void HandleFloating()
    {
        float yOffset = Mathf.Sin(Time.time * _floatSpeed) * _floatMagnitude;
        transform.position = new Vector3(_initialPosition.x, _initialY + yOffset, _initialPosition.z);
    }

    private void HandleShaking()
    {
        if (_isShaking)
        {
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer > 0)
            {
                transform.position = _initialPosition + Random.insideUnitSphere * shakeMagnitude;
            }
            else
            {
                _isShaking = false;
                transform.position = _initialPosition;
            }
        }
    }

    public void SetLockedOn(bool isLockedOn)
    {
        if (isLockedOn)
        {
            _renderer.material.color = Color.red;
        }
        else
        {
            _renderer.material.color = _initialColor;
        }
    }

    public void TakeDamage()
    {
        // ダメージリアクションを開始
        _renderer.material.color = Color.yellow;
        Invoke("ResetColor", colorChangeDuration);

        _isShaking = true;
        _shakeTimer = shakeDuration;

        // **この行を削除しました**
        // Destroy(gameObject);
    }

    private void ResetColor()
    {
        _renderer.material.color = _initialColor;
    }
}