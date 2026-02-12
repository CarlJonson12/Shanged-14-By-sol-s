using Content.Shared._Changed14.ResearchEvac;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Content.Client._Changed14.ResearchEvac;

namespace Content.Client._Changed14.ResearchEvac;

[UsedImplicitly]
public sealed class ResearchEvacConsoleBoundUserInterface : BoundUserInterface
{
    private ResearchEvacWindow? _window;

    public ResearchEvacConsoleBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }


    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindow<ResearchEvacWindow>();

        _window.OnCallEvacButtonPressed += () => SendMessage(new ResearchEvacButtonPressedEvent());

    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not ResearchEvacConsoleBoundUserInterfaceState msg)
            return;
        _window?.Update(msg);
    }

}
