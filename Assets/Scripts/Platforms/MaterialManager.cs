using UnityEngine;

namespace Platforms
{
    public class MaterialManager : MonoBehaviour
    {
        public static MaterialManager Instance;
        
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

        public Material GetMaterialByTier(int tier)
        {
            tier = Mathf.Clamp(tier, 1, 21);
            return Resources.Load<Material>("ArtAssets/Materials/Stack_Color " + tier);
        }
    }
}
