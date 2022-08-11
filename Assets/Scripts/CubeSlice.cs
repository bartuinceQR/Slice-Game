using EzySlice;
using UnityEngine;

public class CubeSlice : MonoBehaviour
{
    private BoxCollider _collider;
    
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("press");
            Slice();
        }
    }

    void Slice()
    {
        
        Bounds boxBounds = _collider.bounds;
        
        RaycastHit[] hits = Physics.BoxCastAll(
            transform.position, 
            transform.localScale/2, 
            transform.forward,
            transform.rotation,
            _collider.size.z * transform.localScale.z);
        
        
        Vector3 firstPos = boxBounds.center - new Vector3(boxBounds.extents.x,0,0);
        Vector3 secondPos = boxBounds.center + new Vector3(boxBounds.extents.x, 0,0);

        int i = 0;
        foreach (RaycastHit hit in hits)
        {
            Debug.Log("ooo" + i);
            i++;
            if (hit.collider == _collider) continue;
            Collider objectToBeSliced = hit.collider;
            Material materialAfterSlice = objectToBeSliced.GetComponent<MeshRenderer>().material;
            
            
            
            Vector3 usedpos = firstPos;
            bool isRight = false;
            float horizontalDist = objectToBeSliced.transform.position.x - transform.position.x;
            
            if (horizontalDist > 0)
            {
                isRight = true;
                usedpos = secondPos;
            }
            
            SlicedHull slicedObject = SliceObject(objectToBeSliced.gameObject, usedpos, materialAfterSlice);

            if (slicedObject != null)
            {

                GameObject upperHullGameobject =
                    slicedObject.CreateUpperHull(objectToBeSliced.gameObject, materialAfterSlice);
                GameObject lowerHullGameobject =
                    slicedObject.CreateLowerHull(objectToBeSliced.gameObject, materialAfterSlice);

                upperHullGameobject.transform.position = objectToBeSliced.transform.position;
                lowerHullGameobject.transform.position = objectToBeSliced.transform.position;

                MakeItPhysical(upperHullGameobject);
                MakeItPhysical(lowerHullGameobject);
                if (isRight)
                    upperHullGameobject.AddComponent<Rigidbody>();
                else
                    lowerHullGameobject.AddComponent<Rigidbody>();

                Destroy(objectToBeSliced.gameObject);
            }
        }
    }
    
    private void MakeItPhysical(GameObject obj)
    {
        obj.AddComponent<MeshCollider>().convex = true;
        //obj.AddComponent<BoxCollider>();
    }

    private SlicedHull SliceObject(GameObject obj, Vector3 pos, Material crossSectionMaterial = null)
    {
        return obj.Slice(pos, transform.right, crossSectionMaterial);
    }

    private void OnDrawGizmos()
    {
        if (!_collider) return;
        Bounds boxBounds = _collider.bounds;
        Debug.Log(_collider.bounds.size.z);
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
