﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inputs;
using Utility;
using System;

public class PlayerControl : MonoBehaviour {

    [SerializeField]
    private UserInput m_UserInput;

    [SerializeField]
    private Transform m_cam;

    private MoveComponent m_moveComp;
    //private InputPacket[] m_inputPackets;

    [System.Serializable]
    public class PCActions
    {
        public float Horizontal;
        public float Vertical;
        public Vector3 Move;
        public Vector3 CamForward;
        public Vector3 CamRight;
        public InputPacket[] InputPackets;

    }

    private PCActions m_PCActions;

    void Start()
    {
        m_moveComp = GetComponent<MoveComponent>();
        m_PCActions = new PCActions();
        m_PCActions.InputPackets = new InputPacket[16];
    }

	void FixedUpdate()
    {
        
        if (m_UserInput)
        {
            m_PCActions.InputPackets = m_UserInput.InputPackets;
            ProcessInput();
        }
    }

    void ProcessInput()
    {
        CheckMovement();    
        CheckActions();
        CheckCommands();
        CheckCamera();
    }

    private void CheckCamera()
    {
        
    }

    private void CheckCommands()
    {
        
    }

    private void CheckActions()
    {
        
    }

    private void CheckMovement()
    {
        m_moveComp.ProcessMovement(m_PCActions);
    }
}
