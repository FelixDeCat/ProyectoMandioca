using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AStarNodeMakerTool : MonoBehaviour
{
    public bool execute;
    [SerializeField] Transform startPos = null;
    Vector3 posZ;
    Vector3 posX;
    [SerializeField] int _amountOfNodesInX = 20;
    [SerializeField] int _amountOfNodesInZ = 20;
    [SerializeField] float maxHeight = 10;
    [SerializeField] float minHeight = -5;
    [SerializeField] LayerMask _mask = 0;
    [SerializeField] float _range = 166;
    [SerializeField] GameObject _node = null;
    [SerializeField] float sensivility = 10;
    [SerializeField] GameObject parentOfNodes = null;


    void Update()
    {
        if (execute)
        {

            var childs = parentOfNodes.GetComponentsInChildren<Transform>();
            
            if (childs.Length!=0)
            {
                for (int i = 0; i < childs.Length; i++)
                {
                    if (childs[i] != parentOfNodes.transform)
                        DestroyImmediate(childs[i].gameObject);
                }
            }
           
          
            posZ = startPos.position;
            posX = startPos.position;
            for (int i = 0; i < _amountOfNodesInZ; i++)
            {
                for (int x = 0; x < _amountOfNodesInX; x++)
                {
                    RaycastHit hit;
                    if(Physics.Raycast(posX,Vector3.down,out hit, _range, _mask))
                    {
                        if (hit.collider.gameObject.layer == 21)
                        {
                            Vector3 posOfCollision = hit.point;
                            if (posOfCollision.y < maxHeight && posOfCollision.y > minHeight)
                            {
                                var currentnode = Instantiate(_node);
                                currentnode.transform.position = hit.point;
                                currentnode.transform.parent = parentOfNodes.transform;
                            }
                                
                          
                        }
                    }
                    posX.x+= sensivility;
                }
                posZ.z+= sensivility;
                posX.x = posZ.x;
                posX.z = posZ.z;

            }
            execute=false;
        }
    }
}
