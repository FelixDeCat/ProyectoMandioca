using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AStarNodeMakerTool : MonoBehaviour
{
    public bool execute;
    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;
    Vector3 posZ;
    Vector3 posX;
    [SerializeField] int _amountOfNodesInX;
    [SerializeField] int _amountOfNodesInZ;
    [SerializeField] float maxHeight;
    [SerializeField] float minHeight;
    [SerializeField] LayerMask _mask;
    [SerializeField] float _range;
    [SerializeField] GameObject _node;
    [SerializeField] float sensivility;
    [SerializeField] GameObject parentOfNodes;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
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
