﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [RequireComponent( typeof(Rigidbody), typeof(BoxCollider) )]
    public class ArrowComponent : MonoBehaviour 
    {

        [SerializeField]
        private float LifeSpan;

        private Rigidbody m_Rigidbody;

        // Use this for initialization
        void Start () 
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update () 
        {

        }

        public void Initialize(Vector3 force)
        {
            m_Rigidbody.AddForce(force, ForceMode.Impulse);

            // remove yourself after LifeSpan seconds to keep object count down
            Destroy(gameObject, LifeSpan);
        }

        void OnCollisionEnter(Collision col)
        {
            //        if(col.gameObject.GetComponent(Enemy))
            //        {
            //            //make the enemy take damage
            //            Destroy(gameObject);
            //        }
        }
    }
}

