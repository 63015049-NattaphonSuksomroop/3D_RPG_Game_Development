using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] [Range(0f, 10f)] private float defaulDistance = 6f;
        [SerializeField] [Range(0f, 10f)] private float minimumDistance = 1f;
        [SerializeField] [Range(0f, 10f)] private float maximumDistance = 6f;

        [SerializeField] [Range(0f, 10f)] private float smoothing = 4f;
        [SerializeField] [Range(0f, 10f)] private float zoomSensitivity = 1f;

        private CinemachineFramingTransposer framingTransposer;
        private CinemachineInputProvider inputProvider;

        private float currentTargetDistence;

        private void Awake()
        {
            framingTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
            inputProvider = GetComponent<CinemachineInputProvider>();

            currentTargetDistence = defaulDistance;
        }

        private void Update()
        {
            Zoom();
        }

        private void Zoom()
        {
            float zoomValue = inputProvider.GetAxisValue(2) * zoomSensitivity;

            currentTargetDistence = Mathf.Clamp(currentTargetDistence + zoomValue, minimumDistance, maximumDistance);

            float currentDistence = framingTransposer.m_CameraDistance;
            
            if (currentDistence == currentTargetDistence)
            {
                return;
            }
            float lerpedZoomValue = Mathf.Lerp(currentDistence, currentTargetDistence, smoothing * Time.deltaTime);

            framingTransposer.m_CameraDistance = lerpedZoomValue;

        }
    }
