using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MenuEntry
{
    public Button button;
    public RectTransform cursorPosition;
}

public class MenuNavigationController : MonoBehaviour
{
    [Header("Menu Entries")]
    [SerializeField]
    private MenuEntry[] entries;

    [SerializeField]
    private RectTransform navigationIcon;

    [Header("Navigation")]
    [SerializeField]
    private int defaultButtonIndex;

    [SerializeField]
    private string navigationAxis = "NavV2";

    [SerializeField]
    private string confirmButton = "Submit";

    [SerializeField]
    private float inputDeadZone = 0.5f;

    private int selectedIndex;
    private bool navigationInputReleased = true;

    private void OnEnable()
    {
        if (entries == null || entries.Length == 0)
        {
            Debug.LogWarning(
                $"{name} has no menu entries assigned.",
                this
            );

            return;
        }

        selectedIndex = Mathf.Clamp(
            defaultButtonIndex,
            0,
            entries.Length - 1
        );

        navigationInputReleased = true;
        UpdateSelection(false);
    }

    private void Update()
    {
        if (entries == null || entries.Length == 0)
            return;

        HandleNavigation();
        HandleConfirmation();
    }

    private void HandleNavigation()
    {
        float input = Input.GetAxisRaw(navigationAxis);

        if (Mathf.Abs(input) < inputDeadZone)
        {
            navigationInputReleased = true;
            return;
        }

        if (!navigationInputReleased)
            return;

        navigationInputReleased = false;

        if (input > 0f)
            MoveSelection(-1);
        else
            MoveSelection(1);
    }

    private void MoveSelection(int direction)
    {
        int previousIndex = selectedIndex;

        selectedIndex += direction;

        if (selectedIndex < 0)
            selectedIndex = entries.Length - 1;
        else if (selectedIndex >= entries.Length)
            selectedIndex = 0;

        if (selectedIndex == previousIndex)
            return;

        UpdateSelection(true);
    }

    private void UpdateSelection(bool playSound)
    {
        MenuEntry selectedEntry = entries[selectedIndex];

        if (
            selectedEntry == null ||
            selectedEntry.button == null ||
            selectedEntry.cursorPosition == null
        )
        {
            return;
        }

        navigationIcon.position =
            selectedEntry.cursorPosition.position;

        selectedEntry.button.Select();

        if (
            playSound &&
            UIAudioManager.Instance != null
        )
        {
            UIAudioManager.Instance.PlayNavigationChime();
        }
    }

    private void HandleConfirmation()
    {
        if (!Input.GetButtonDown(confirmButton))
            return;

        MenuEntry selectedEntry = entries[selectedIndex];

        if (
            selectedEntry == null ||
            selectedEntry.button == null ||
            !selectedEntry.button.interactable
        )
        {
            return;
        }

        if (UIAudioManager.Instance != null)
            UIAudioManager.Instance.PlayConfirmChime();

        selectedEntry.button.onClick.Invoke();
    }
}