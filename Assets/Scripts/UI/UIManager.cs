using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : Singleton<UIManager>
{
    private PlayerManager playerManager;

    [HideInInspector] public List<UIPanel> panels = new();

    private UIPanel currentPanel;
    public UIPanel CurrentPanel { get => currentPanel; }

    private UIPanel previousPanel;
    public UIPanel PreviousPanel { get => previousPanel; }

    public UIPanel openingPanel;

    [Header("Events")]
    public UnityEvent onPanelChanged = new();

    private bool shouldStartListening = false;

    protected override void Awake()
    {
        base.Awake();
        GetComponentsInChildren(true, panels);
    }

    private void Start()
    {
        SwitchPanel(openingPanel);
        openingPanel.onPanelFinishOpen.AddListener(() => shouldStartListening = true);
    }

    private void Update()
    {
        if(shouldStartListening && playerManager.Host.gamepad.buttonNorth.isPressed)
        {
            SwitchPanel(panels[1]);
            shouldStartListening = false;
        }
    }

    public void SwitchPanel(UIPanel uiPanel)
    {
        if (!uiPanel) return;
        if (uiPanel == currentPanel) return;

        if (currentPanel)
        {
            currentPanel.ClosePanel();
            previousPanel = currentPanel;
        }

        currentPanel = uiPanel;
        currentPanel.OpenPanel();

        if(onPanelChanged != null)
        {
            onPanelChanged.Invoke();
        }
    }

    public void SwitchToPreviousPanel()
    {
        if (!previousPanel) return;

        SwitchPanel(previousPanel);
    }
}
