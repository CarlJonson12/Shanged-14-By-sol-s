using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared._Changed14.ResearchEvac;


[Serializable, NetSerializable]
public enum ResearchEvacConsoleUiKey : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class ResearchEvacButtonPressedEvent : BoundUserInterfaceMessage
{

}

[Serializable, NetSerializable]
public sealed class ResearchEvacConsoleBoundUserInterfaceState : BoundUserInterfaceState
{
    public bool CanCall;

    public ResearchEvacConsoleBoundUserInterfaceState(bool canCall)
    {
        CanCall = canCall;
    }
}
