using UnityEngine;

// Specialized subsystem for Overlap UI elements
public class OverlapSubsystem : BaseUISubsystem<BaseOverlap>
{
    public OverlapSubsystem(UIManager manager) : base(manager, UIType.Overlap) { }
}