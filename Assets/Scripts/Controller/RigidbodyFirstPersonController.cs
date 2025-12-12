using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(Rigidbody))]
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class RigidbodyFirstPersonController : MonoBehaviour
    {
        // Basic movement, camera, and audio handling for a rigidbody-driven FPS controller
        [Header("Player")]
        public float MoveSpeed = 4.0f;
        public float SprintSpeed = 6.0f;
        public float RotationSpeed = 1.0f;
        public float JumpForce = 5.0f;

        [Header("Player Grounded")]
        public bool Grounded = true;
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.5f;
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        public GameObject CinemachineCameraTarget;
        public float TopClamp = 90.0f;
        public float BottomClamp = -90.0f;

         [Header("Audio")]
        public AudioSource audioSource;
        public AudioClip jumpSound;
        public AudioClip footstepSound;
        public float footstepCooldown = 0.4f;

        private float _cinemachineTargetPitch;
        private float _rotationVelocity;
        private bool _isWalking = false;

        private StarterAssetsInputs _input;
        private Rigidbody _rb;
#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool IsCurrentDeviceMouse =>
#if ENABLE_INPUT_SYSTEM
            _playerInput.currentControlScheme == "KeyboardMouse";
#else
            false;
#endif

        private void Awake()
        {
            // Locate the main camera used for rotation targets
            if (_mainCamera == null)
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        private void Start()
        {
            // Initialize components and lock rigidbody rotation
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#endif

            
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
                if (audioSource == null)
                    audioSource = GetComponentInChildren<AudioSource>();
            }

            
            if (audioSource != null)
            {
                // Prevent sounds from playing automatically at startup
                audioSource.playOnAwake = false;
                audioSource.loop = false;
            }
        }

        private void Update()
        {
            GroundedCheck();
            Move();
            Jump();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void GroundedCheck()
        {
            // Use a simple sphere cast to determine if the player is grounded
            Vector3 spherePosition = new Vector3(
                transform.position.x,
                transform.position.y + GroundedOffset,
                transform.position.z);

            Grounded = Physics.CheckSphere(
                spherePosition,
                GroundedRadius,
                GroundLayers,
                QueryTriggerInteraction.Ignore);
        }

        private void Move()
        {
            // Determine movement speed based on sprint input
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
            if (_input.move == Vector2.zero)
                targetSpeed = 0.0f;
                

            Vector3 inputDir = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
           
           bool isMoving = inputDir.sqrMagnitude >= 0.01f && targetSpeed > 0f;
              if (!isMoving)
            {
                
                if (_isWalking)
                {
                    StopFootstepSound();
                    _isWalking = false;
                }
                return;
            }

            Vector3 moveDir = transform.right * inputDir.x + transform.forward * inputDir.z;

            Vector3 move = moveDir * (targetSpeed * Time.deltaTime);
            _rb.MovePosition(_rb.position + move);

             if (Grounded && targetSpeed > 0f)
              {
                if (!_isWalking)
                {
                    PlayFootstepLoopSound();
                    _isWalking = true;
                }
            }
            else
            {
                if (_isWalking)
                {
                    StopFootstepSound();
                    _isWalking = false;
                }
            }
        }
            private void PlayFootstepLoopSound()
            {
                // Loop footsteps only while walking on the ground
                if (audioSource != null && footstepSound != null)
                {
                     if (!audioSource.isPlaying)
                {
                    audioSource.clip = footstepSound;
                    audioSource.loop = true;
                    audioSource.Play();
                }
                }
            }
            private void StopFootstepSound()
            {
                if (audioSource != null && audioSource.isPlaying)
                {
                    audioSource.Stop();
                    audioSource.loop = false;
                    audioSource.clip = null;
                }
            }

        private void Jump()
        {
            // Apply jump impulse only when grounded
            if (_input.jump && Grounded)
            {
                _input.jump = false;

                Vector3 v = _rb.linearVelocity;
                v.y = 0;
                _rb.linearVelocity = v;

                _rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                PlayJumpSound();
            }
            else if (_input.jump)
            {
                _input.jump = false;
            }
        }


        private void PlayJumpSound()
        {
            if (audioSource != null && jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
        }


        private void CameraRotation()
        {
            // Rotate camera pitch and player yaw based on look input
            if (_input.look.sqrMagnitude >= _threshold)
            {
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
                _rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

                _cinemachineTargetPitch =
                    ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                CinemachineCameraTarget.transform.localRotation =
                    Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

                transform.Rotate(Vector3.up * _rotationVelocity);
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color c = Grounded
                ? new Color(0.0f, 1.0f, 0.0f, 0.35f)
                : new Color(1.0f, 0.0f, 0.0f, 0.35f);

            Gizmos.color = c;
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y + GroundedOffset, transform.position.z),
                GroundedRadius);
        }
    }
}
