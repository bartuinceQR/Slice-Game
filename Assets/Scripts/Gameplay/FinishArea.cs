using Player;
using UnityEngine;

namespace Gameplay
{
    public class FinishArea : MonoBehaviour
    {
        [SerializeField] private Transform emeraldTarget; 
        
        public void SetFinishState()
        {
            GameplayManager.Instance.SetFinish();
        }

        public Transform GetTarget()
        {
            return emeraldTarget;
        }
    }
}
