using System;
using UnityEngine;

//[ExecuteInEditMode]
public class GridEntity : MonoBehaviour
{
	public event Action<GridEntity> OnMove = delegate {};
	//public Vector3 velocity = new Vector3(0, 0, 0);
    public bool onGrid;
    //Renderer _rend;
    private PlayObject _playObject;
    private void Awake()
    {
        _playObject = GetComponent<PlayObject>();
    }

    void Update() {
        
//        if (onGrid)
//        {
//            Debug.Log(_playObject);
//    
//            if (_playObject != null)
//            {
//                Debug.Log(_playObject);
//                _playObject.On();
//            }
//        }
//        else
//            _playObject.Off();
        
        
        
//            _rend.material.color = Color.gray;
//		//Optimization: Only on *actual* move
//		transform.position += velocity * Time.deltaTime;
//	    OnMove(this);
	}
}
