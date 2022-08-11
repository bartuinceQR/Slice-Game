using Platforms;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager Instance;

        private GameState _gameState;

        [SerializeField] private PlayerMove player;
        [SerializeField] private GameObject platformTemplate;
        [SerializeField] private Transform stacksObject;
    
        [SerializeField] private Transform currentRefPlatform;
        
        [SerializeField] private float speed = 3f;
        
        [SerializeField] private LayerMask _layerMask;

        [SerializeField] private AudioClip chainSound;
        [SerializeField] private AudioClip chainBrokenSound;

        private GameObject incomingPlatform;
        private Transform playerTransform;
        private bool isFinishing;
        private bool nextSpawned;
        private bool InputEnabled;
        private int dir = 1;
        
        private int chainTier = 1;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            playerTransform = player.transform;
            chainTier = 0;
            _gameState = GameState.Ongoing;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isFinishing)
            {
                if (playerTransform.position.z > currentRefPlatform.position.z && !nextSpawned)
                {
                    SpawnNextPlatform();
                    dir = -dir;
                    nextSpawned = true;
                }

                if (nextSpawned && InputEnabled && Input.GetKeyDown(KeyCode.Space))
                {
                    CheckPlatform();
                }
            }

        }

        void CheckPlatform()
        {
            Bounds currentCollider = currentRefPlatform.GetComponent<BoxCollider>().bounds;
            Bounds nextCollider = incomingPlatform.GetComponent<BoxCollider>().bounds;
            
            float platformdiff = incomingPlatform.transform.position.x - currentRefPlatform.position.x;
            
            bool isInside = nextCollider.min.x >= currentCollider.min.x
                            && nextCollider.max.x <= currentCollider.max.x;
            bool isClose = Mathf.Abs(platformdiff) <= currentRefPlatform.localScale.x / 20f;
                
            if (isClose || isInside) //nice shot
            {
                if (isClose && !isInside)
                {
                    chainTier++;
                    chainTier = Mathf.Clamp(chainTier, 1, 21);
                    AudioManager.Instance.PlaySoundWithPitch(chainSound, chainTier);
                }

                PlatformsVeryClose();
            }
            else
            {
                if (chainTier > 1)
                {
                    AudioManager.Instance.PlaySound(chainBrokenSound);
                }
                chainTier = 1;
                PlatformsCheckSlice();
            }
            
            
            player.SetTargetX(currentRefPlatform.GetComponent<BoxCollider>().bounds.center.x);
            
        }
        
        void PlatformsVeryClose()
        {
            UpdatePlatformData(incomingPlatform);
            InputEnabled = false;
            nextSpawned = false;
        }

        void PlatformsCheckSlice()
        {
            var newPlatform = Slicer.SlicePlatform(currentRefPlatform.gameObject, _layerMask);
            if (newPlatform != null) //slice it up
            {
                UpdatePlatformData(newPlatform);

                nextSpawned = false;
                Destroy(incomingPlatform);
            }
            else //you're screwed, mon
            {
                incomingPlatform.AddComponent<Rigidbody>();
            }  
            
            InputEnabled = false;
        }

        void UpdatePlatformData(GameObject platform)
        {
            platform.GetComponent<PlatformMovement>().SetSpeed(0f);
            platform.transform.SetParent(stacksObject);
            currentRefPlatform = platform.transform;
            platformTemplate = platform;
            platform.layer = LayerMask.NameToLayer("Stacks");
        }

        void SpawnNextPlatform()
        {
            Transform curTransform = currentRefPlatform.transform;
            var forward = curTransform.forward;
            Vector3 nextPos = curTransform.position
                              - curTransform.right * (curTransform.localScale.x * speed/3 * dir)
                              + (curTransform.localScale.z + platformTemplate.transform.localScale.z)/ 2 * forward ;
            incomingPlatform = Instantiate(platformTemplate, nextPos, Quaternion.identity, stacksObject);
            incomingPlatform.layer = LayerMask.NameToLayer("Platforms");
            
            incomingPlatform.GetComponent<MeshRenderer>().material =
                    MaterialManager.Instance.GetMaterialByTier(chainTier);

            PlatformMovement movement = incomingPlatform.GetComponent<PlatformMovement>();
            movement.SetDir(dir);
            movement.SetSpeed(speed);
            
            
            InputEnabled = true;
        }

        public void SetFinish()
        {
            isFinishing = true;
        }
        
        public void EndGame()
        {
            _gameState = GameState.Over;
            HUDManager.Instance.SetFail();
        }

        public void WinStage()
        {
            _gameState = GameState.Finished;
            player.SetState(PlayerState.Dancing);
            HUDManager.Instance.SetSuccess();
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // TODO : make an actual restart and not a bozo button
        }
        
    
        public enum GameState
        {
            NotStarted = 1,
            Ongoing = 2,
            Finished = 3,
            Over = 4,
        }
    
    }
}
