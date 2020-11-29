using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Bullet BulletPrefab;

    public float ReloadTimerStart;

    public int Damage;

    private float _reloadTimer;

    [SerializeField] private Transform _rotateLeftButton;

    [SerializeField] private Transform _rotateRightButton;

    [SerializeField] private Transform _forwardPoint;

    [SerializeField] private float _bulletMultiplier;

    public bool IsActive;
    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _reloadTimer = ReloadTimerStart;
    }

    

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.CanMoveTurrets)
        {
            _rotateLeftButton.gameObject.SetActive(true);
            _rotateRightButton.gameObject.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = MouseHelper.GetMouseCoords(Input.mousePosition);

                if (MouseHelper.ClickedInArea(mousePos, _rotateLeftButton.position, 0.5f))
                {
                    Debug.Log($"mouse clicked rotate left");
                    transform.Rotate(new Vector3(0, 0, 45));
                }

                if (MouseHelper.ClickedInArea(mousePos, _rotateRightButton.position, 0.5f))
                {
                    Debug.Log($"mouse clicked rotate right");
                    transform.Rotate(new Vector3(0, 0, -45));
                }
            }

            return;
        }
        else
        {
            _rotateLeftButton.gameObject.SetActive(false);
            _rotateRightButton.gameObject.SetActive(false);
        }

        if (GameStateManager.IsPlaying)
        {
            _reloadTimer -= Time.deltaTime;
            if (_reloadTimer <= 0)
            {
                Bullet bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
                bullet.ForwardPoint = _forwardPoint;
                bullet.Damage = Mathf.CeilToInt(bullet.Damage * _bulletMultiplier);
                // instantiate bullet and fire in direction facing
                _reloadTimer = ReloadTimerStart;
            }
        }
    }
}