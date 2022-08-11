using Platforms;
using UnityEngine;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager Instance;

        private GameState _gameState;

        [SerializeField] private GameObject player;
        [SerializeField] private GameObject platformTemplate;
        [SerializeField] private Transform stacksObject;
    
        [SerializeField] private Transform currentRefPlatform;
        
        [SerializeField]
        private float speed = 3f;
        
        [SerializeField]
        private LayerMask _layerMask;

        private GameObject incomingPlatform;
        private Transform playerTransform;
        private bool nextSpawned;
        private bool InputEnabled;
        private int dir = 1;

    

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
            _gameState = GameState.Ongoing;
        }

        // Update is called once per frame
        void Update()
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

        void CheckPlatform()
        {
            float platformdiff = incomingPlatform.transform.position.x - currentRefPlatform.position.x;

            Bounds currentCollider = currentRefPlatform.GetComponent<BoxCollider>().bounds;
            Bounds nextCollider = incomingPlatform.GetComponent<BoxCollider>().bounds;
            bool isInside = nextCollider.min.x >= currentCollider.min.x
                            && nextCollider.max.x <= currentCollider.max.x;
                
            if (Mathf.Abs(platformdiff) <= currentRefPlatform.localScale.x / 20f || isInside) //nice shot
            {
                PlatformsVeryClose();
            }
            else
            {
                PlatformsCheckSlice();
            }
        }
        
        void PlatformsVeryClose()
        {
            incomingPlatform.GetComponent<PlatformMovement>().SetSpeed(0f);
            currentRefPlatform = incomingPlatform.transform;
            platformTemplate = incomingPlatform;
            incomingPlatform.layer = LayerMask.NameToLayer("Stacks");
            InputEnabled = false;
            nextSpawned = false;
        }

        void PlatformsCheckSlice()
        {
            var newPlatform = Slicer.SlicePlatform(currentRefPlatform.gameObject, _layerMask);
            if (newPlatform != null) //slice it up
            {
                newPlatform.transform.SetParent(stacksObject);
                currentRefPlatform = newPlatform.transform;
                platformTemplate = newPlatform;
                newPlatform.layer = LayerMask.NameToLayer("Stacks");
                nextSpawned = false;
                Destroy(incomingPlatform);
            }
            else //you're screwed, mon
            {
                Debug.Log("CRAP");
                incomingPlatform.AddComponent<Rigidbody>();
            }  
            
            InputEnabled = false;
        }

        public void SpawnNextPlatform()
        {
            Transform curTransform = currentRefPlatform.transform;
            var forward = curTransform.forward;
            Vector3 nextPos = curTransform.position
                              - curTransform.right * (curTransform.localScale.x * speed/3 * dir)
                              + curTransform.localScale.z * forward / 2
                              + platformTemplate.transform.localScale.z * forward / 2;
            incomingPlatform = Instantiate(platformTemplate, nextPos, Quaternion.identity, stacksObject);
            incomingPlatform.layer = LayerMask.NameToLayer("Platforms");
            Debug.Log(nextPos);
            PlatformMovement movement = incomingPlatform.GetComponent<PlatformMovement>();
            movement.SetDir(dir);
            movement.SetSpeed(speed);
            InputEnabled = true;
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
