using UnityEngine;
using UnityEngine.UI;
using System;

public class PassportField : MonoBehaviour
{
    public enum FieldType
    {
        Photo,
        Name,
        ExpirationDate,
        Signature,
        Nationality,
        PassportNumber
    }

    [Tooltip("The type of field this button represents.")]
    public FieldType fieldType;

    // An action that PassportUI can subscribe to.
    // It passes which field was clicked.
    public static Action<FieldType> OnFieldClicked;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("PassportField requires a Button component.", this);
            return;
        }
        button.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        // When clicked, invoke the static action.
        OnFieldClicked?.Invoke(fieldType);
    }

    private void OnDestroy()
    {
        // Cleanup the listener when the object is destroyed.
        if (button != null)
        {
            button.onClick.RemoveListener(HandleClick);
        }
    }
}
