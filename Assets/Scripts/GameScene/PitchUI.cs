using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PitchUI : MonoBehaviour
{
    [SerializeField] TMP_Text pitchText;
    [SerializeField] RectTransform pitchIndicatorRect;

    [SerializeField] RectTransform canvasRect;

    [SerializeField] PitchTracker pitchTracker;
    [SerializeField] PlayerController playerController;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        pitchText.text = "Pitch: " + pitchTracker.pitchValue.ToString("F0") + " Hz";
        if (pitchTracker.pitchValue != 0)
        {
            Vector3 pitchIndicatorWorldPos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRect, pitchIndicatorRect.position, mainCamera, out pitchIndicatorWorldPos);
            pitchIndicatorWorldPos = new Vector3(pitchIndicatorWorldPos.x, playerController.heightT, pitchIndicatorWorldPos.z);
            pitchIndicatorRect.anchoredPosition = WorldToCanvasRectPoint(pitchIndicatorWorldPos, canvasRect);
            /*
            float pitchIndicatorAnchoredPosX = pitchIndicatorRect.anchoredPosition.x;
            Vector3 pitchIndicatorWorldPos = new Vector3(pitchIndicatorRect.position.x, playerController.heightT, pitchIndicatorRect.position.z);
            // Debug.Log(pitchTrackerWorldPos);
            pitchIndicatorRect.anchoredPosition = mainCamera.WorldToScreenPoint((Vector2)pitchIndicatorWorldPos);
            float playerWorldHeightRange = playerController.maxY - playerController.minY;
            Debug.Log(mainCamera.WorldToScreenPoint(new Vector3(0, playerWorldHeightRange, 0)).y);
            float pitchIndicatorAnchoredPosY = pitchIndicatorRect.anchoredPosition.y * 2 - mainCamera.WorldToScreenPoint(new Vector3(0, playerWorldHeightRange, 0)).y;
            pitchIndicatorRect.anchoredPosition = new Vector2(pitchIndicatorAnchoredPosX, pitchIndicatorAnchoredPosY);
            Debug.Log(pitchIndicatorRect.anchoredPosition);
            */
        }
    }

    public Vector2 WorldToCanvasRectPoint(Vector2 worldPoint, RectTransform canvasRect)
    {
        Vector2 viewportPoint = mainCamera.WorldToViewportPoint(worldPoint);
        float halfCanvasWidth = canvasRect.rect.width / 2;
        float halfCanvasHeight = canvasRect.rect.height / 2;
        float canvasRectPointX = Mathf.Lerp(-halfCanvasWidth, halfCanvasWidth, viewportPoint.x);
        float canvasRectPointY = Mathf.Lerp(-halfCanvasHeight, halfCanvasHeight, viewportPoint.y);
        return new Vector2(canvasRectPointX, canvasRectPointY);
    }
}
