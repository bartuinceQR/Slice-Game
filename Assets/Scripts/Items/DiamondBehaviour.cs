using Gameplay;
using UnityEngine;

namespace Items
{
    public class DiamondBehaviour : MonoBehaviour
    {
        private ParticleSystem _system;
        [SerializeField] private AudioClip winSound;

        // Start is called before the first frame update
        void Start()
        {
            _system = GetComponentInChildren<ParticleSystem>();
        }
        
        public void GetCollected()
        {
            _system.transform.parent = null;
            _system.Stop();

            ParticleSystem.EmitParams newParams = new ParticleSystem.EmitParams();
            newParams.startLifetime = 3f;
            
            _system.Emit(newParams, 20);

            GameplayManager.Instance.WinStage();
            AudioManager.Instance.PlaySound(winSound);
            
            Destroy(_system.gameObject, 3f);
            Destroy(gameObject);
        }
    }
}
