using UnityEngine;

// Specialized subsystem for Notification UI elements
public class NotifySubsystem : BaseUISubsystem<BaseNotify>
{
    public NotifySubsystem(UIManager manager) : base(manager, UIType.Notify) { }
}