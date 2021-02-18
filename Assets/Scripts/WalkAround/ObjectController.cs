using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.WalkAround.Objects.Implementations;
using UnityEngine.InputSystem;

public class ObjectController : MonoBehaviour
{
    public float speed;
    private Rigidbody _rb;
    public ObjectConfig config;
    public Renderer _renderer;
    private Vector3 _movement;

    public Vector2 _facing = Vector2.right;

    private float speedMul = 1;

    // Use this for initialization
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        config = GetComponent<ObjectConfig>();
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
            _rb.velocity = (_movement * speed * speedMul);
        }
    }
    
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log($"Look with {collision.gameObject.name}");
        if (collision.gameObject.TryGetComponent(out ObjectConfig conf)) {
            if (conf.IsLookable) {
                conf.Look(config);
            }
            if (conf.IsInteractable) {
                WalkaroundManager.Instance.ReadPotentialInteraction(conf);
            }
            if (conf.IsDoor) {
                conf.Door(config);
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log($"Collision with {collision.gameObject.name}");
        if (collision.gameObject.TryGetComponent(out ObjectConfig conf)) {

        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movement = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
        if (_movement.magnitude > 0.1f) _facing = _movement.normalized;
    }

    public void OnMoveScripted(Vector3 end, float speedMul) {
        this.speedMul = speedMul;
        StartCoroutine(MoveRoutine(end));
    }
    
    private IEnumerator MoveRoutine(Vector3 end) {
        while (_rb.position != end) {
            _movement = Vector3.Normalize(Vector3.MoveTowards(_rb.position, end, 99999));
            yield return new WaitForEndOfFrame();
        }

        speedMul = 1;
    }

}
