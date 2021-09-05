using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    
    // offset between camera and target
    public Vector3 _offset;
    
    // This value will change at the runtime depending on target movement. Initialize with zero vector.
    private Vector3 velocity = Vector3.zero;
    
    // change this value to get desired smoothness
    public float SmoothTime = 0.3f;
    
    // Start is called before the first frame update
    void Start()
    {
        _offset = transform.position - _target.position;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        // update position
        //Vector3 targetPosition = _target.position + Offset;
        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, Time.deltaTime);
 
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, _offset.z + _target.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, 0.6f);
    }
}
