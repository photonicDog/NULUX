using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.WalkAround.Objects.Implementations;
using UnityEngine.InputSystem;

public class ObjectController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D _rb2d;
    public ObjectConfig config;
    private Renderer _renderer;
    private Vector2 _movement;

    public Vector2 _facing = Vector2.up;

    // Use this for initialization
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        config = GetComponent<ObjectConfig>();
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (config.IsVisible != _renderer.enabled)
        {
            _renderer.enabled = !_renderer.enabled;
        }

        if (config.IsControllable)
        {
            _rb2d.MovePosition(_rb2d.position + _movement * speed);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Collision with {collision.gameObject.name}");
        ObjectConfig collis;
        if (collision.gameObject.TryGetComponent(out collis)) {
            if (collis.IsDoor) {
                WalkaroundManager.Instance.UseDoor(this.gameObject, collis);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>();
        if (_movement.magnitude > 0.1f) _facing = _movement.normalized;
    }
}
