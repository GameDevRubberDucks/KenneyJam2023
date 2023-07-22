/*
 * PlayerController.cs
 * 
 * Description:
 * - Controls for the player objects
 * 
 * Author(s): 
 * - Kody Wood
*/

using DG.Tweening;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using RubberDucks.KenneyJam.Interactions;

namespace RubberDucks.KenneyJam.Player
{
    public class PlayerController : MonoBehaviour
    {
        //--- Events ---//
        [System.Serializable]
        public class EventList
        {
        }
        [Header("Events")]
        public EventList Events = default;

        //--- Properties ---//
        //public Int32 PlayerIndex
        //{
        //    get => m_PlayerIndex; 
        //    set => m_PlayerIndex = value;
        //}
        //--- Public Variables ---//

        //--- Protected Variables ---//

        //--- Private Variables ---//
        [SerializeField] private Int32 m_PlayerIndex = -1;
        [SerializeField] private string m_InputAxisX = string.Empty;
        [SerializeField] private string m_InputAxisZ = string.Empty;
        [Header("Movement Variables")]
        [SerializeField] private float m_Acceleration = 300.0f;
        private Vector3 m_LastLookDir = Vector3.forward;

        private GameObject teasure;

        //--- Unity Methods ---//


        private void Update()
        {
            //Vector3 m_velDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            //if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            //{
            //    m_LastLookDir = m_velDir;
            //}
            Vector3 m_velDir = new Vector3(Input.GetAxis(m_InputAxisX), 0.0f, Input.GetAxis(m_InputAxisZ));
            if (Input.GetAxisRaw(m_InputAxisX) != 0 || Input.GetAxisRaw(m_InputAxisZ) != 0)
            {
                m_LastLookDir = m_velDir;
            }
            this.GetComponent<Rigidbody>().velocity += m_Acceleration * m_velDir * Time.deltaTime;
            UpdateRotation(m_LastLookDir);
        }

        //--- Public Methods ---//
        public void InitializePlayer(Int32 playerInd)
        {
            m_PlayerIndex = playerInd;
            m_InputAxisX = "Horizontal" + playerInd.ToString();
            m_InputAxisZ = "Vertical" + playerInd.ToString();
        }
        //--- Protected Methods ---//

        //--- Private Methods ---//
        void UpdateRotation(Vector3 lookAt)
        {
            this.transform.LookAt(this.transform.position + lookAt);
        }

        void WayFinder()
        {

            teasure = GameObject.FindGameObjectWithTag("Pickup");
        }
    }
}
