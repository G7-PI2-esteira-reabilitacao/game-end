using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static int s_DeadHash     = Animator.StringToHash("Dead");
    static int s_RunStartHash = Animator.StringToHash("runStart");
    static int s_MovingHash   = Animator.StringToHash("Moving");

    private CharacterController _controller;
    private Animator            _animator;
    
    private Vector3             _direction;
    private float               _forwardSpeed = 6f;

    private int   _desiredLane    = 1;
    private float _laneDistance   = 1f;
    public  float laneChangeSpeed = 1.0f;
    
    protected Vector3 m_TargetPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        _controller        = gameObject.AddComponent<CharacterController>();
        _controller.height = 1.0f;
        
        _animator          = gameObject.GetComponent<Animator>();

        StartRunning();
    }
    
    public void StartRunning()
    {
        if (_animator)
        {
            _animator.Play(s_RunStartHash);
            _animator.SetBool(s_MovingHash, true);
        }
    }
    
    public void StopMoving()
    {
        if (_animator)
        {
            _animator.SetBool(s_MovingHash, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _direction.z = _forwardSpeed;

        ProcessInput();
    }

    void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _desiredLane++;
            if (_desiredLane == 2)
                _desiredLane = 1;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _desiredLane--;
            if (_desiredLane == -1)
                _desiredLane = 0;
        }
        //m_TargetPosition = new Vector3((_desiredLane - 1) * _laneDistance, 0, 0);
        var position = transform.position.z               * transform.forward + transform.position.y * transform.up;

        switch (_desiredLane)
        {
            case 0 :
                position += Vector3.left * _laneDistance;
                break;
            case 1 :
                position += Vector3.right * _laneDistance;
                break;
        }
        
        transform.position = Vector3.Lerp(transform.position, position, 80 * Time.deltaTime);
        
        _controller.Move(_direction * Time.deltaTime);
        //Vector3 verticalTargetPosition = m_TargetPosition;
        //transform.localPosition = Vector3.MoveTowards(transform.localPosition, verticalTargetPosition, laneChangeSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        
    }
}