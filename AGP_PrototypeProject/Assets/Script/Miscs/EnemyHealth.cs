﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HealthCare
{
    public class EnemyHealth : Health
    {

        protected override void Initialize()
        {
            base.Initialize();
        }


        protected override void OnDeathBegin()
        {
            if (m_Animator)
            {
                m_Animator.SetBool("Dead", true);
                Destroy(gameObject, 2.0f);
            }
        }

        public override void TakeDamage(float damage, GameObject dmgDealer = null)
        {
            base.TakeDamage(damage, dmgDealer);
        }
    }
}
