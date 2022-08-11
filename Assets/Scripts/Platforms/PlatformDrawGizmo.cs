using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDrawGizmo : MonoBehaviour
{
    private BoxCollider _collider;
    
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    private void OnDrawGizmos()
    {
        if (!_collider) return;
        Bounds boxBounds = _collider.bounds;
        ExtDebug.DrawBoxCastBox( transform.position, 
            transform.localScale/2, 
            transform.rotation,
            transform.forward,
            _collider.size.z * transform.localScale.z,
            Color.red);
        
        Debug.DrawLine(boxBounds.center, boxBounds.center - new Vector3(boxBounds.extents.x,0,0), Color.blue);
        Debug.DrawLine(boxBounds.center, boxBounds.center + new Vector3(boxBounds.extents.x,0,0), Color.yellow);
    }
}
