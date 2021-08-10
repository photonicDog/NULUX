using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace WalkAround {
    public class ObjectController : MonoBehaviour
    {
        public float speed;
        public bool canIdle;
        private Rigidbody _rb;
        public ObjectConfig config;
        public SpriteRenderer _renderer;
        private Vector3 _movement;

        [FormerlySerializedAs("interactBubble")] public GameObject inpectBubble;

        private float idleTimer;

        public Animator talksprite;

        public Vector2 _facing = Vector2.right;

        private float speedMul = 1;

        // Use this for initialization
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            config = GetComponent<ObjectConfig>();
            idleTimer = 0f;
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

            if (_facing.x < -0.1) _renderer.flipX = false;
            else _renderer.flipX = true;

            idleTimer += Time.fixedDeltaTime;

            if (canIdle && idleTimer > 20f) {
                talksprite.SetTrigger("Idle");
                idleTimer = -6f;
            }
        }

        public void ResetIdle() {
            idleTimer = 0f;
        }

        public void InspectBubble(bool enable) {
            inpectBubble.SetActive(enable);
        }
    
        void OnTriggerEnter(Collider collision)
        {
            //Debug.Log($"Look with {collision.gameObject.name}");
            if (collision.gameObject.TryGetComponent(out ObjectConfig conf)) {
                if (conf.IsLookable) {
                    conf.Look(config);
                    if (!conf.IsDoor) InspectBubble(true);
                }
                if (conf.IsInteractable) {
                    WalkaroundManager.Instance.ReadPotentialInteraction(conf);
                }
                if (conf.HasInteractBubble) {
                    conf.interactBubble.gameObject.SetActive(true);
                }
            }
        }

        private void OnTriggerExit(Collider collision) {
            if (collision.gameObject.TryGetComponent(out ObjectConfig conf)) {
                if (conf.IsLookable) {
                    conf.Leave(config);
                    InspectBubble(false);
                }
                if (conf.HasInteractBubble) {
                    conf.interactBubble.gameObject.SetActive(false);
                }
            }
        }

        private void OnCollisionEnter(Collision collision) {
            //Debug.Log($"Collision with {collision.gameObject.name}");
            if (collision.gameObject.TryGetComponent(out ObjectConfig conf)) {
                if (conf.IsDoor) {
                    conf.Door(config);
                }
            }
        }

        public void OnMove(InputAction.CallbackContext context) {
            idleTimer = 0f;
            _movement = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
            if (_movement.magnitude > 0.1f) _facing = _movement.normalized;
            if (config.IsTalkspritable) {
                if (context.started) talksprite.SetTrigger("Walk");
                if (context.canceled) talksprite.SetTrigger("NoWalk");
            }
        }

        public void OnMoveScripted(Vector3 end, float speedMul, System.Action OnCompleted = null, GameObject view = null) {
            this.speedMul = speedMul;
            StartCoroutine(MoveRoutine(end, OnCompleted, view));
        }
    
        private IEnumerator MoveRoutine(Vector3 end, System.Action OnCompleted = null, GameObject view = null) {
            if (config.IsTalkspritable) {
                config.talkspriteController.SetState(true, true);
            }
        
            Debug.Log("Moving to " + end);
        
            while (Mathf.Abs((_rb.position - end).magnitude) > 0.02f) {
                _movement = Vector3.Normalize(end - _rb.position);
                if (Mathf.Abs((_rb.position - end).magnitude) < 0.08f) _rb.MovePosition(end);
                yield return new WaitForEndOfFrame();
            }
        
            if (config.IsTalkspritable) {
                config.talkspriteController.SetState(true, false);
            }

            _movement = Vector3.zero;
        
            view?.SetActive(true);
            OnCompleted?.Invoke();
            speedMul = 1;
        }

    }
}
