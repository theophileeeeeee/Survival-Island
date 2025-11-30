using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{
    [RequireComponent(typeof(Text))]
    public class FPSCounter : MonoBehaviour
    {
        const float fpsMeasurePeriod = 0.5f;
        private int m_FpsAccumulator = 0;
        private float m_TimeAccumulator = 0f;
        private float m_FpsNextPeriod = 0;
        private int m_CurrentFps;
        const string display = "{0} FPS";
        public Text m_Text;

        private void Start()
        {
            m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
            m_Text = GetComponent<Text>();
            if (m_Text == null)
            {
                Debug.LogError("FPSCounter: Aucun composant Text trouvé sur ce GameObject !");
                enabled = false;
            }
        }

        private void Update()
        {
            m_FpsAccumulator++;
            m_TimeAccumulator += Time.unscaledDeltaTime;
            if (Time.realtimeSinceStartup > m_FpsNextPeriod)
            {
                m_CurrentFps = (int)(m_FpsAccumulator / m_TimeAccumulator);
                m_FpsAccumulator = 0;
                m_TimeAccumulator = 0f;
                m_FpsNextPeriod += fpsMeasurePeriod;
                m_Text.text = string.Format(display, m_CurrentFps);
            }
        }
    }
}