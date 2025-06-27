using UnityEngine;

// Specialized subsystem for Screen UI elements
public class ScreenSubsystem : BaseUISubsystem<BaseScreen>
{
    public ScreenSubsystem(UIManager manager) : base(manager, UIType.Screen) { }
}