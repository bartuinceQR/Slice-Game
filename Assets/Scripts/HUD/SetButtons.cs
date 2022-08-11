using Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class SetButtons : MonoBehaviour
{
    [SerializeField] private Button _button;
    
    // Start is called before the first frame update
    void Start()
    {
        _button.onClick.AddListener(Restart);
    }

    // Update is called once per frame
    void Restart()
    {
        GameplayManager.Instance.Restart();
    }
}
