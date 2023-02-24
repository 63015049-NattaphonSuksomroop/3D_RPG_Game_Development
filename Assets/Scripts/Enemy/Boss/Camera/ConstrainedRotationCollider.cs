
using UnityEngine;
using Cinemachine.Utility;

namespace Cinemachine

    [AddComponentMenu("")] // Hide in menu
    [SaveDuringPlay]
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class ConstrainedRotationCollider : CinemachineExtension
    {
        [Header("Obstacle Detection")]
        [Tooltip("Objects on these layers will be detected")]
        public LayerMask m_CollideAgainst = 1;


        [Tooltip("Objects on these layers will never obstruct view of the target")]
        public LayerMask m_TransparentLayers = 0;

        [TagField]
        [Tooltip("Obstacles with this tag will be ignored.  It is a good idea to set this field to the target's tag")]
        public string m_IgnoreTag = string.Empty;

        [Tooltip("Camera will try to maintain this distance from any obstacle.  Try to keep this value small.  "
            + "Increase it if you are seeing inside obstacles due to a large FOV on the camera.")]
        public float m_CameraRadius = 0.7f;

        [Range(0, 10)]
        [Tooltip("How gradually the camera returns to its normal position after having been corrected.  "
            + "Higher numbers will move the camera more gradually back to normal.")]
        public float m_Damping = 0.6f;

        public float m_MinimumTimeout = 0.5f;

        private Vector3 debugHitLocation;

        class VcamExtraState
        {
            public float previousTime;
            public float previousDistance;
            public float obstructionTime;
            public bool isObstructed = false;
            public Vector3 lastDisplacement = Vector3.zero;
        }

        public override float GetMaxDampTime()
        {
            return m_Damping;
        }

        private Vector3 GetTargetDisplacement(Vector3 lookAtPos, Vector3 pos, float cameraRadius)
        {
            var dir = pos - lookAtPos;
            var maxDist = dir.magnitude;
            if (RuntimeUtility.SphereCastIgnoreTag(lookAtPos, cameraRadius, dir.normalized, out RaycastHit hit, maxDist, m_CollideAgainst & ~m_TransparentLayers, m_IgnoreTag))
            {
                return hit.point - pos;
            }

            return Vector3.zero;
        }

        private bool IsViewObstructed(Vector3 lookAtPos, Vector3 pos, float cameraRadius)
        {
            var dir = pos - lookAtPos;
            var maxDist = dir.magnitude - cameraRadius;
            var result = Physics.CapsuleCast(lookAtPos, lookAtPos, cameraRadius, dir.normalized, out RaycastHit hit, maxDist, m_CollideAgainst & ~m_TransparentLayers);
            debugHitLocation = result ? hit.point : Vector3.zero;
            return result;
        }

        void OnDrawGizmosSelected()
        {
            if (!debugHitLocation.AlmostZero())
            {
                Gizmos.color = new Color(1, 1, 0, 0.75F);
                Gizmos.DrawSphere(debugHitLocation, m_CameraRadius);
            }
        }

        private float GetTargetDistance(Vector3 displacement, Vector3 pos, Vector3 lookAtPos, float cameraRadius)
        {
            var target = pos + displacement;
            var dir = target - pos;
            var dist = dir.magnitude;
            var lookAtDist = (lookAtPos - pos).magnitude;
            var maxDist = lookAtDist;
            return dist > maxDist ? maxDist : dist;
        }

        private float SmoothDistance(float targetDistance, float previousDistance, float elapsedTime, float damping)
        {
            if (damping < Epsilon)
            {
                return targetDistance;
            }

            if (elapsedTime <= Epsilon)
            {
                return 0.0f;
            }

            return previousDistance + (targetDistance - previousDistance) * elapsedTime / damping;
        }

        private Vector3 GetNextDisplacement(Vector3 displacement, Vector3 pos, float targetDistance)
        {
            var target = pos + displacement;
            var dir = target - pos;
            if (targetDistance > Epsilon)
            {
                displacement = dir.normalized * targetDistance;
            }

            return displacement;
        }

        /// <summary>Callback to preform the zoom adjustment</summary>
        /// <param name="vcam">The virtual camera being processed</param>
        /// <param name="stage">The current pipeline stage</param>
        /// <param name="state">The current virtual camera state</param>
        /// <param name="deltaTime">The current applicable deltaTime</param>
        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            
            if (stage == CinemachineCore.Stage.Body)
            {
                var extra = GetExtraState<VcamExtraState>(vcam);
                var now = CinemachineCore.CurrentTime;
                var lookAtPos = state.ReferenceLookAt;
                var pos = state.CorrectedPosition;
                var isObstructed = IsViewObstructed(lookAtPos, pos, m_CameraRadius);
                if (!extra.isObstructed && isObstructed)
                {
                    extra.obstructionTime = now;
                    extra.previousDistance = 0.0f;
                    extra.previousTime = 0.0f;
                }

                if (extra.isObstructed && !isObstructed)
                {
                    extra.obstructionTime = now;
                    extra.previousTime = 0.0f;
                }

                extra.isObstructed = isObstructed;
                if (!isObstructed && now - extra.obstructionTime > m_MinimumTimeout)
                {
                    if (extra.previousTime > Epsilon)
                    {
                        var targetDistance = (pos - lookAtPos).magnitude;
                        var smoothDistance = SmoothDistance(targetDistance, extra.previousDistance, now - extra.previousTime, m_Damping);
                        extra.previousDistance = smoothDistance;
                        state.PositionCorrection = GetNextDisplacement(Vector3.zero, pos, smoothDistance);
                    }

                    extra.previousTime = now;
                }

                if (isObstructed && now - extra.obstructionTime > m_MinimumTimeout)
                {
                    if (extra.previousTime > Epsilon)
                    {
                        var displacement = GetTargetDisplacement(lookAtPos, pos, m_CameraRadius);
                        var targetDistance = GetTargetDistance(displacement, pos, lookAtPos, m_CameraRadius);
                        var smoothDistance = SmoothDistance(targetDistance, extra.previousDistance, now - extra.previousTime, m_Damping);
                        extra.previousDistance = smoothDistance;
                        state.PositionCorrection = GetNextDisplacement(displacement, pos, smoothDistance);
                    }

                    extra.previousTime = now;
                }
            }
        }
    }
}