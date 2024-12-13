// Copyright (c) Meta Platforms, Inc. and affiliates.

using CrypticCabinet.Photon;
using UnityEngine;

namespace CrypticCabinet.GameManagement.Timeline
{
    /// <summary>
    ///     Represents a game phase where a timeline animation is played.
    ///     This will be used by the intro animation (Act1) and outro animation (Act3).
    /// </summary>
    [CreateAssetMenu(fileName = "New CrypticCabinet Timeline Phase", menuName = "CrypticCabinet/Timeline GamePhase")]
    public class TimelineGamePhase : GamePhase
    {
        [SerializeField] private TimelineGameStep m_timelinePrefab;
        [SerializeField] private bool m_triggerNextGamePhaseOnTimelineComplete;
        private TimelineGameStep m_activeTimeline;

        protected override void InitializeInternal()
        {
            base.InitializeInternal();
            if (PhotonConnector.Instance != null && PhotonConnector.Instance.Runner != null)
            {
                m_activeTimeline = PhotonConnector.Instance.Runner.Spawn(m_timelinePrefab);
                m_activeTimeline.TimelineCompleteCallback.AddListener(
                    () =>
                    {
                        if (m_triggerNextGamePhaseOnTimelineComplete)
                        {
                            GameManager.Instance.NextGameplayPhase();
                        }
                    });
            }
            GameManager.Instance.NextGameplayPhase();
        }

        protected override void DeinitializeInternal()
        {
            base.DeinitializeInternal();
            if (PhotonConnector.Instance.Runner != null)
            {
                PhotonConnector.Instance.Runner.Despawn(m_activeTimeline.Object);
            }
            else
            {
                Debug.LogError("Runner was destroyed, unable to despawn object");
            }
        }
    }
}