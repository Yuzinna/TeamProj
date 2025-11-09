using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;

/// <summary>
/// 여권 UI의 모든 동작(애니메이션, 입력, 상태 변경)을 관리하는 메인 스크립트입니다.
/// </summary>
public class PassportUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region UI & Data References
    [Header("UI Elements")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI expirationDateText;
    public Button clickArea;
    #endregion

    #region Animation & Zoom Settings
    [Header("Animation Settings")]
    public Vector2 endPosition; // 여권이 완전히 올라왔을 때의 위치
    public Vector2 startPosition; // 여권이 살짝 보일 때(peek)의 위치
    public float animationSpeed = 10f;
    public float dismissDragThreshold = 50f; // 이 거리 이상 드래그해야 Dismiss 처리

    [Header("Zoom Settings")]
    public float zoomScale = 2.0f;
    public Vector2 zoomPosition; // 확대되었을 때의 위치
    public float zoomSpeed = 10f;
    #endregion

    #region State Variables
    private enum PassportState { Hidden, Peeking, Animating, Visible }
    private PassportState currentState;

    private RectTransform rectTransform;
    private Vector2 dragStartPosition;
    private Vector2 dragStartAnchoredPosition;
    
    private bool isZoomed = false;
    private Vector3 initialScale;

    // 짧은 드래그와 클릭을 구분하기 위한 플래그. OnDrag가 호출되면 true가 됩니다.
    private bool didDrag = false; 
    private PassengerData currentPassengerData;
    #endregion

    #region Unity Lifecycle Methods
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("PassportUI requires a RectTransform component.");
        }

        if (clickArea == null)
        {
            clickArea = GetComponent<Button>();
        }
        initialScale = rectTransform.localScale;

        // Start() 대신 Awake()에서 초기화하는 이유:
        // 오브젝트가 비활성화 상태에서 처음 활성화될 때 Start()가 호출됩니다.
        // StartPeek()에서 오브젝트를 활성화하는데, 이때 Start()가 실행되면 상태가 꼬일 수 있습니다.
        // Awake()는 오브젝트가 생성될 때 한 번만 호출되므로 더 안전합니다.
        currentState = PassportState.Hidden;
        rectTransform.anchoredPosition = startPosition;
        clickArea?.onClick.AddListener(OnClick);
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        // PassportField의 클릭 이벤트를 구독합니다.
        // static 이벤트이므로, 오브젝트가 활성화될 때마다 구독해야 합니다.
        PassportField.OnFieldClicked += InspectField;
    }

    void OnDisable()
    {
        // OnEnable에서 구독한 이벤트를 해제합니다.
        // 해제하지 않으면 메모리 누수가 발생할 수 있습니다.
        PassportField.OnFieldClicked -= InspectField;
    }

    void Start()
    {
        // 모든 초기화 로직은 Awake()로 이동했습니다.
    }
    #endregion

    #region Input Handlers
    /// <summary>
    /// 여권 UI의 메인 클릭 이벤트를 처리합니다.
    /// </summary>
    private void OnClick()
    {
        // 드래그 직후에 발생하는 클릭 이벤트를 무시하기 위한 로직
        if (didDrag)
        {
            didDrag = false;
            return;
        }

        if (currentState == PassportState.Peeking)
        {
            currentState = PassportState.Animating;
            StartCoroutine(AnimateTo(endPosition, PassportState.Visible));
        }
        else if (currentState == PassportState.Visible)
        {
            StartCoroutine(ToggleZoom());
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        didDrag = false; // 드래그 시작 시 플래그 초기화
        if (currentState == PassportState.Visible && !isZoomed)
        {
            StopAllCoroutines();
            dragStartPosition = eventData.position;
            dragStartAnchoredPosition = rectTransform.anchoredPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        didDrag = true; // 드래그가 발생했음을 표시
        if (currentState == PassportState.Visible && !isZoomed)
        {
            Vector2 dragDelta = eventData.position - dragStartPosition;
            rectTransform.anchoredPosition = dragStartAnchoredPosition + dragDelta / rectTransform.lossyScale.x;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentState == PassportState.Visible && !isZoomed)
        {
            float dragDistanceY = eventData.position.y - dragStartPosition.y;
            if (dragDistanceY > dismissDragThreshold)
            {
                Dismiss();
            }
            else
            {
                // 드래그 거리가 짧으면 원래 위치로 복귀
                currentState = PassportState.Animating;
                StartCoroutine(AnimateTo(endPosition, PassportState.Visible));
            }
        }
    }
    #endregion

    #region Core Logic & Animations
    /// <summary>
    /// 여권의 확대/축소 상태를 토글합니다.
    /// </summary>
    private IEnumerator ToggleZoom()
    {
        isZoomed = !isZoomed;
        currentState = PassportState.Animating;

        Vector2 targetPosition = isZoomed ? zoomPosition : endPosition;
        Vector3 targetScale = isZoomed ? new Vector3(zoomScale, zoomScale, initialScale.z) : initialScale;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * zoomSpeed;
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, t);
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, targetScale, t);
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
        rectTransform.localScale = targetScale;

        currentState = PassportState.Visible;
    }
    
    /// <summary>
    /// 여권을 화면에서 치웁니다 (Peeking 상태로 되돌림).
    /// </summary>
    public void Dismiss()
    {
        if (currentState == PassportState.Visible)
        {
            if (isZoomed)
            {
                StartCoroutine(ToggleZoom());
            }
            currentState = PassportState.Animating;
            StartCoroutine(AnimateTo(startPosition, PassportState.Peeking));
        }
    }

    /// <summary>
    /// 지정된 위치로 여권을 부드럽게 이동시키는 코루틴입니다.
    /// </summary>
    private IEnumerator AnimateTo(Vector2 targetPosition, PassportState finalState)
    {
        while (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) > 0.1f)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * animationSpeed);
            yield return null;
        }
        rectTransform.anchoredPosition = targetPosition;
        currentState = finalState;
    }
    #endregion

    #region Inspection
    /// <summary>
    /// PassportField로부터 클릭 이벤트를 받아 특정 항목을 검사합니다.
    /// </summary>
    private void InspectField(PassportField.FieldType fieldType)
    {
        if (!isZoomed)
        {
            // 확대되지 않은 상태에서는 검사 불가
            return;
        }

        Debug.Log($"Inspecting field: {fieldType}");

        // TODO: 실제 위조 여부 판별 로직 구현
        switch (fieldType)
        {
            case PassportField.FieldType.Photo:
                Debug.Log("Checking photo against passenger's appearance...");
                break;
            case PassportField.FieldType.Name:
                Debug.Log($"Checking name: {currentPassengerData?.passengerName}");
                break;
            case PassportField.FieldType.ExpirationDate:
                Debug.Log($"Checking expiration date: {currentPassengerData?.passportExpirationDate}");
                break;
            case PassportField.FieldType.Signature:
                Debug.Log("Checking signature for inconsistencies...");
                break;
            case PassportField.FieldType.Nationality:
                Debug.Log("Checking nationality...");
                break;
            case PassportField.FieldType.PassportNumber:
                Debug.Log("Checking passport number format...");
                break;
        }
    }
    #endregion

    #region Public Interface
    /// <summary>
    /// 외부에서 승객 데이터를 받아 UI를 업데이트합니다.
    /// </summary>
    public void UpdateInfo(PassengerData data)
    {
        if (data == null) return;
        currentPassengerData = data; // 검사를 위해 현재 승객 데이터 저장
        if (nameText != null) nameText.text = data.passengerName;
        if (expirationDateText != null) expirationDateText.text = data.passportExpirationDate.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// 여권이 화면에 살짝 보이도록(Peeking) 시작합니다.
    /// </summary>
    public void StartPeek()
    {
        // 새로운 상호작용이 시작되므로, 이전 상호작용의 상태(didDrag)를 초기화합니다.
        didDrag = false;

        // 만약 이전 여권이 확대된 상태였다면, 원래 크기로 되돌립니다.
        if (isZoomed)
        {
            isZoomed = false;
            rectTransform.localScale = initialScale;
        }
        
        rectTransform.anchoredPosition = startPosition;
        currentState = PassportState.Peeking;
        clickArea.interactable = true;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 여권을 화면에서 완전히 숨깁니다.
    /// </summary>
    public void Hide()
    {
        currentState = PassportState.Hidden;
        gameObject.SetActive(false);
        rectTransform.anchoredPosition = startPosition;
    }
    #endregion
}