﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace Wolf {
    public class WolfMoveComponent : MonoBehaviour {
        [SerializeField]
        private float m_AnimDistFactor = 3;
        [SerializeField]
        public float m_AnimRotationFactor = 15;
        [SerializeField]
        private float m_RotateTowards_Threshold = 0.9f;

        private Animator m_Animator;
        private Vector3 m_TargetPos;
        private Vector3[] m_Path;
        private float m_CurRotRate;
        private bool m_OnLinkTransition;
        private EnumService.OffMeshLinkMoveMethod m_Method = EnumService.OffMeshLinkMoveMethod.Parabola;

        void Start()
        {
            m_Animator = GetComponent<Animator>();
        }

        public IEnumerator Move(Vector3 targetPos, Vector3[] path, NavMeshAgent agent = null)
        {
            //MESH LINKS
            if (agent)
            {
                agent.SetDestination(targetPos);
                agent.speed = 0.01f;
                agent.autoTraverseOffMeshLink = true;
                if (agent.isOnOffMeshLink)
                {

                    //float rateH = Mathf.Lerp(m_Animator.GetFloat("Horizontal"), 0, Time.deltaTime * 50);
                    //float rateV = Mathf.Lerp(m_Animator.GetFloat("Vertical"), 0, Time.deltaTime * 50);
                    m_Animator.SetFloat("Horizontal", 0);
                    m_Animator.SetFloat("Vertical", 0);
                    OffMeshLinkData data = agent.currentOffMeshLinkData;
                    float height = Vector3.Dot(data.endPos - data.startPos, new Vector3(0, 1, 0));
                    height = Math.Abs(height);
                    yield return StartCoroutine(Parabola(agent, height, 1.0f));
                    agent.CompleteOffMeshLink();
                }
                else
                {
                    m_TargetPos = targetPos;
                    m_Path = path;

                    if (path.Length == 0)
                    {
                        Debug.Log("NO PATH");
                        yield break;
                    }
                    else
                    {
                        if (path.Length > 1)
                        {
                            Vector3 nextNode = path[1]; //we only really care the next immediate node, but lets keep passing in the whole array for now cuz maybe we want more from it later

                            Vector3 dir = nextNode - transform.position;
                            dir.Normalize();

                            Vector3 cross = Vector3.zero; //will be the cross product between the wolf's forward and the move direction

                            #region Rotating 
                            if (transform.forward != dir) //only rotate if wolf's forward and dir are different
                            {
                                cross = Vector3.Cross(transform.forward, dir);
                                float angle = Vector3.Angle(transform.forward, dir);
                                float rate = angle / m_AnimRotationFactor;

                                Mathf.Clamp(rate, 0, 3); //max rate is 3 in animator

                                //rotate either clockwise or counter-clockwise
                                if (Vector3.Dot(cross, Vector3.up) > 0)
                                {
                                    rate = Mathf.Lerp(m_CurRotRate, rate, Time.deltaTime * 15);
                                    m_CurRotRate = rate;
                                }
                                else
                                {
                                    rate *= -1;
                                    rate = Mathf.Lerp(m_CurRotRate, rate, Time.deltaTime * 15);
                                    m_CurRotRate = rate;
                                }
                                m_Animator.SetFloat("Horizontal", rate);
                            }
                            else
                            {
                                m_Animator.SetFloat("Horizontal", 0);
                            }

                            //draw these line to debug the rotation if needed
                            //Debug.DrawRay(transform.position, dir * 10, Color.red);
                            //Debug.DrawRay(transform.position, transform.forward * 10, Color.blue);
                            //Debug.DrawRay(transform.position, cross * 10, Color.yellow);
                            #endregion

                            #region Moving
                            float dist = Vector3.Distance(transform.position, m_TargetPos);

                            dist = dist / m_AnimDistFactor;
                            Mathf.Clamp(dist, 0, 3); //max speed is 3 in animator
                            m_Animator.SetFloat("Vertical", dist);
                            #endregion
                        }
                    }
                }
            }
        }

        private IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
        {
            agent.speed = 3.5f;
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 startPos = agent.transform.position;
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
            float normalizedTime = 0.0f;
            m_Animator.SetBool("JumpHolder", true);
            while (normalizedTime < 1.0f)
            {
                float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
                agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
                normalizedTime += Time.deltaTime / duration;
                yield return null;
            }
            agent.speed = 0.01f;
            m_Animator.SetBool("JumpHolder", false);
        }

        public bool RotateTowards(Vector3 targetPos)
        {
            //Vector3 dir = targetPos - transform.position;
            Vector3 dir = targetPos;
            dir.Normalize();

            Vector3 cross = Vector3.zero; //will be the cross product between the wolf's forward and the move direction

            float DotDirs = Vector3.Dot(gameObject.transform.forward, dir);

            if (DotDirs < m_RotateTowards_Threshold) //only rotate if wolf's forward and dir are different
            {
                cross = Vector3.Cross(transform.forward, dir);
                float angle = Vector3.Angle(transform.forward, dir);
                float rate = angle / m_AnimRotationFactor;

                Mathf.Clamp(rate, 0, 3); //max rate is 3 in animator

                //rotate either clockwise or counter-clockwise
                if (Vector3.Dot(cross, Vector3.up) > 0)
                {
                    rate = Mathf.Lerp(m_CurRotRate, rate, Time.deltaTime * 15);
                    m_CurRotRate = rate;
                }
                else
                {
                    rate *= -1;
                    rate = Mathf.Lerp(m_CurRotRate, rate, Time.deltaTime * 15);
                    m_CurRotRate = rate;
                }
                m_Animator.SetFloat("Horizontal", rate);
                return false;
            }

            return true;
        }

        //call this function when we have appropriate place for it
        public void Stop()
        {
            m_Animator.SetFloat("Vertical", 0);
            m_Animator.SetFloat("Horizontal", 0);
        }

        private bool JumpOffMeshLink(Vector3 nextNode)
        {
            NavMeshAgent wolfNavAgent = GetComponent<NavMeshAgent>();
            if (wolfNavAgent.isOnOffMeshLink)
            {
                Debug.Assert(false, "At Off Mesh Link!");
                return false;
            }

            return true;
        }

        //move this to utility later plz?
        bool Equal(Vector3[] a1, Vector3[] a2)
        {
            if (a1.Length != a2.Length)
            {
                return false;
            }
            for (int i=0; i < a1.Length; i++)
            {
                if (a1[i] != a2[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
