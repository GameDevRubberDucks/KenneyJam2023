using RubberDucks.Utilities;
using RubberDucks.Utilities.Extensions;
using RubberDucks.Utilities.Timing;
using System.Collections;
using UnityEngine;

namespace RubberDucks.KenneyJam.Player
{
    public class PlayerInvincibility : MonoBehaviour
    {
        //--- Events ---//
        [System.Serializable]
        public class EventList
        {
        }
        [Header("Events")]
        public EventList Events = default;

        //--- Properties ---//

        //--- Public Variables ---//

        //--- Protected Variables ---//

        //--- Private Variables ---//
        [Header("Timing")]
        [SerializeField] private AutomaticTimer m_UsageTimer = default;
        
        [Header("Rendering")]
        [SerializeField] private MeshRenderer[] m_VehicleRenderers = default;
        [SerializeField][Range(0.0f, 1.0f)] private float m_Transparency = 0.5f;

        [Header("Collision")]
        [SerializeField] private GameObject m_ColliderObject = default;
        [SerializeField] private string m_NormalLayer = "Player";
        [SerializeField] private string m_InvincibleLayer = "Invincible";

        //--- Unity Methods ---//
        private void Awake()
        {
            m_UsageTimer.m_Events.OnFinished.AddListener(StopInvincibility);
        }

        //--- Public Methods ---//
        [EditorFunctionCall("TriggerInvincibility")]
        public void TriggerInvincibility()
        {
            Debug.Log("Starting invincibility!");

            m_UsageTimer.StartTimer();

            ToggleInvincibility(true);
        }

        public void StopInvincibility()
        {
            Debug.Log("Stopping invincibility!");

            ToggleInvincibility(false);
        }

        //--- Protected Methods ---//

        //--- Private Methods ---//
        private void ToggleInvincibility(bool isInvincible)
        {
            m_ColliderObject.layer = LayerMask.NameToLayer(isInvincible ? m_InvincibleLayer : m_NormalLayer);

            foreach(var renderer in m_VehicleRenderers)
            {
                foreach(var mat in renderer.materials)
                {
                    mat.color = mat.color.CopyWithNewAlpha(isInvincible ? m_Transparency : 1.0f);
                }
            }
        }
    }
}