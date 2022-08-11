using EzySlice;
using Platforms;
using UnityEngine;

namespace Gameplay
{
    public class Slicer
    {
        public static GameObject SlicePlatform(GameObject main, LayerMask layerMask)
        {
            BoxCollider _collider = main.GetComponent<BoxCollider>();
            Transform mainTransform = main.transform;
        
            Bounds boxBounds = _collider.bounds;
        
            RaycastHit[] hits = Physics.BoxCastAll(
                mainTransform.position, 
                mainTransform.localScale/2, 
                mainTransform.forward,
                mainTransform.rotation,
                _collider.size.z * mainTransform.localScale.z,
                layerMask);
        
            Vector3 firstPos = boxBounds.center - new Vector3(boxBounds.extents.x,0,0);
            Vector3 secondPos = boxBounds.center + new Vector3(boxBounds.extents.x, 0,0);
        
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider == _collider) continue;
                Collider objectToBeSliced = hit.collider;
                Material materialAfterSlice = objectToBeSliced.GetComponent<MeshRenderer>().material;

                Vector3 usedpos = firstPos;
                bool isRight = false;
                float horizontalDist = objectToBeSliced.transform.position.x - mainTransform.position.x;
            
                if (horizontalDist > 0)
                {
                    isRight = true;
                    usedpos = secondPos;
                }
            
                SlicedHull slicedObject = SliceObject(main, objectToBeSliced.gameObject, usedpos, materialAfterSlice);

                if (slicedObject != null)
                {

                    GameObject upperHullGameobject =
                        slicedObject.CreateUpperHull(objectToBeSliced.gameObject, materialAfterSlice);
                    GameObject lowerHullGameobject =
                        slicedObject.CreateLowerHull(objectToBeSliced.gameObject, materialAfterSlice);

                    upperHullGameobject.transform.position = objectToBeSliced.transform.position;
                    lowerHullGameobject.transform.position = objectToBeSliced.transform.position;
                    
                    if (isRight)
                    {
                        upperHullGameobject.AddComponent<Rigidbody>();
                        upperHullGameobject.AddComponent<SelfDestruct>();
                        MakeItPhysical(lowerHullGameobject);
                        return lowerHullGameobject;
                    }
                    else
                    {
                        lowerHullGameobject.AddComponent<Rigidbody>();
                        lowerHullGameobject.AddComponent<SelfDestruct>();
                        MakeItPhysical(upperHullGameobject);
                        return upperHullGameobject;
                    }
                }
            }

            return null;
        }
    
        private static void MakeItPhysical(GameObject obj)
        {
            //obj.AddComponent<MeshCollider>().convex = true;
            obj.AddComponent<BoxCollider>();
            obj.AddComponent<PlatformMovement>();
        }

        private static SlicedHull SliceObject(GameObject main, GameObject obj, Vector3 pos, Material crossSectionMaterial = null)
        {
            return obj.Slice(pos, main.transform.right, crossSectionMaterial);
        }
    }
}
