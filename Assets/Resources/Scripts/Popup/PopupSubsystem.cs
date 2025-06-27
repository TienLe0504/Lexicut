using UnityEngine;

// Specialized subsystem for Popup UI elements
public class PopupSubsystem : BaseUISubsystem<BasePopup>
{
    public PopupSubsystem(UIManager manager) : base(manager, UIType.Popup) { }
}