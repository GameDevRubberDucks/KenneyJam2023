/*
 * PlayerController.cs
 * 
 * Description:
 * - Controls for the player objects
 * 
 * Author(s): 
 * - Kody Wood
*/

using System;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.Events;

using RubberDucks.Utilities;
using RubberDucks.Utilities.Audio;
using RubberDucks.KenneyJam.Level;
using RubberDucks.Utilities.Timing;
using RubberDucks.KenneyJam.Interactions;

namespace RubberDucks.KenneyJam.Player
{
    public class PlayerController : MonoBehaviour
    {
        //--- Events ---//
        [System.Serializable]
        public class EventList
        {
            public UnityEvent OnPlayerScoreChange;
        }
        [Header("Events")]
        public EventList Events = default;

        //--- Properties ---//
        public int PlayerIndex
        {
            get => m_PlayerIndex;
            set => m_PlayerIndex = value;
        }
        public int Score
        {
            get => m_Score;
            set => m_Score = value;
        }
        public float DashThreshold
        {
            get => m_DashThreshold;
        }

        public Color PlayerColour => m_PlayerColours[m_PlayerIndex];

        //--- Public Variables ---//

        //--- Protected Variables ---//

        //--- Private Variables ---//
        [Header("Player Variables")]
        [SerializeField] private int m_PlayerIndex = -1;
        [SerializeField] private int m_Score = 0;
        [SerializeField] private string m_InputAxisX = string.Empty;
        [SerializeField] private string m_InputAxisZ = string.Empty;
        [SerializeField] private string m_TransformInput = string.Empty;

        [Header("Movement Variables")]
        [SerializeField] private float m_Acceleration = 300.0f;
        [SerializeField] private float m_BulldozerSpeedMultiplier = 0.5f;
        [SerializeField] private float m_DashCooldown = 5.0f;
        [SerializeField] private float m_DashSpeed = 300.0f;
        [SerializeField] private float m_DashThreshold = 45.0f;

        [Header("Vehicle Components")]
        [SerializeField] private GameObject m_TruckVisuals = default;
        [SerializeField] private GameObject m_BulldozerVisuals = default;
        [SerializeField] private LevelPathCutter m_ForestCutterComp = default;
        [SerializeField] private CollisionInteractions m_InteractionsComp = default;

        [Header("TireTreads")]
        public TrailRenderer[] m_TireRenderers = default;
        public float m_BulldozerWidth = 1.0f;
        public float m_TruckWidth = 0.5f;

        [Header("Colours")]
        [SerializeField] private Color[] m_PlayerColours = default;
        [SerializeField] private Outline[] m_PlayerOutlines = default;

        [Header("Timers")]
        [SerializeField] private AutomaticTimer m_DashTimer = default;
        [SerializeField] private bool m_CanDash = true;

        private Vector3 m_LastLookDir = Vector3.forward;
        private bool m_IsTruckForm = true;

        [SerializeField] private GameObject arrow;
        private Dictionary<int, GameObject> m_PlayerList = new Dictionary<int, GameObject>();

        //--- Unity Methods ---//
        private void Awake()
        {
            TryTransform(false);
            m_DashTimer.m_Events.OnFinished.AddListener(ToggleDash);
        }

        private void Update()
        {
            float speedMultiplier = (m_IsTruckForm) ? 1.0f : m_BulldozerSpeedMultiplier;
            Vector3 m_velDir = Vector3.Normalize(new Vector3(Input.GetAxis(m_InputAxisX), 0.0f, Input.GetAxis(m_InputAxisZ)));
            if (Input.GetAxisRaw(m_InputAxisX) != 0 || Input.GetAxisRaw(m_InputAxisZ) != 0)
            {
                //AudioManager.Instance.PlayLoopingAudio("Accel",AudioConstant.SFX_Accel,AudioChannel.SFX, 0.2f);
                m_LastLookDir = m_velDir;
            }
            this.GetComponent<Rigidbody>().velocity += m_Acceleration * m_velDir * Time.deltaTime * speedMultiplier;
            UpdateRotation(m_LastLookDir);

            WayFinder();

            if (Input.GetButtonDown(m_TransformInput) && m_CanDash && !m_IsTruckForm)
            {
                m_CanDash = false;
                m_DashTimer.StartTimer(m_DashCooldown);
                this.GetComponent<Rigidbody>().velocity += m_DashSpeed * m_LastLookDir;
                AudioManager.Instance.PlayOneShotAudio(AudioConstant.SFX_Dash);
            }
        }

        //--- Public Methods ---//
        public void InitializePlayer(int playerInd, Dictionary<int, GameObject> playerList)
        {
            m_PlayerIndex = playerInd;
            m_InputAxisX = "Horizontal" + playerInd.ToString();
            m_InputAxisZ = "Vertical" + playerInd.ToString();
            m_TransformInput = "Dash" + playerInd.ToString();
            m_PlayerList = playerList;

            //AudioManager.Instance.PlayLoopingAudio("Engine", AudioConstant.SFX_Engine, AudioChannel.SFX, 0.5f);
            ApplyPlayerColours();
        }

        public void TryTransform(bool toTruck)
        {
            if (m_InteractionsComp.m_IsCarrying)
            {
                toTruck = true;
            }

            m_TruckVisuals.SetActive(toTruck);
            m_BulldozerVisuals.SetActive(!toTruck);

            m_ForestCutterComp.CanCutTrees = !toTruck;

            m_IsTruckForm = toTruck;

            float tireTreadWidth = (toTruck) ? m_TruckWidth : m_BulldozerWidth;
            foreach(var lr in m_TireRenderers)
            {
                lr.startWidth = tireTreadWidth;
            }
        }

        public void ApplyPlayerColours()
        {
            foreach (var arrowSprite in arrow.GetComponentsInChildren<SpriteRenderer>())
            {
                arrowSprite.color = PlayerColour;
            }

            foreach (var outline in m_PlayerOutlines)
            {
                outline.OutlineColor = PlayerColour;
            }
        }

        //--- Protected Methods ---//

        //--- Private Methods ---//
        void UpdateRotation(Vector3 lookAt)
        {
            this.transform.LookAt(this.transform.position + lookAt);
        }

        void WayFinder()
        {
            if (gameObject.GetComponent<CollisionInteractions>().m_IsCarrying)
            {
                GameObject lol = GameObject.FindGameObjectWithTag("Drop");
                if (lol)
                {
                    LookAt2D(lol.transform);
                }
            }
            else
            {
                foreach (var player in m_PlayerList.Values)
                {
                    if (player.GetComponent<CollisionInteractions>().m_IsCarrying)
                    {
                        LookAt2D(player.transform);
                        return;
                    }
                }
                if (GameObject.FindGameObjectWithTag("Collect") != null)
                {
                    LookAt2D(GameObject.FindGameObjectWithTag("Collect").transform);
                }
                else if (GameObject.FindGameObjectWithTag("Pickup") != null)
                {
                    LookAt2D(GameObject.FindGameObjectWithTag("Pickup").transform);
                }
            }
        }

        //get the vector between the you and the target, zero out the y, normalize it and then set it as the farword for the arrow.
        void LookAt2D(Transform target)
        {

            Vector3 vec = target.position - transform.position;
            vec.y = 0.0f;
            vec.Normalize();
            arrow.transform.forward = vec;

        }

        private void ToggleDash()
        {
            m_CanDash = true;
        }

    }
}
