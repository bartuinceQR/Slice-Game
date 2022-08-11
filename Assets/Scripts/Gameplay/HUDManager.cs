using UnityEngine;

namespace Gameplay
{
    public class HUDManager : MonoBehaviour
    {

        public static HUDManager Instance;
        
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

        //why do I even have two of these
        [SerializeField] private GameObject successScreen;
        [SerializeField] private GameObject failScreen;

        public void SetSuccess()
        {
            successScreen.SetActive(true);
        }

        public void SetFail()
        {
            failScreen.SetActive(true);
            
        }
    }
}
