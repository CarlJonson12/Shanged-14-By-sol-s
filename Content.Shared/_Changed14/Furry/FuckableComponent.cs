using Robust.Shared.Audio;
using Content.Shared.Chemistry.Reagent;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Changed14.Fuckable;

[RegisterComponent, AutoGenerateComponentState, AutoGenerateComponentPause, NetworkedComponent]
public sealed partial class FuckableComponent : Component
{
    [DataField]
    public SoundSpecifier FuckableCumSound = new SoundPathSpecifier("/Audio/Voice/Human/malescream_1.ogg");

    [DataField]
    public SoundSpecifier FurryCumSound = new SoundPathSpecifier("/Audio/Voice/Vulpkanin/howl.ogg");

    [DataField, AutoNetworkedField]
    public ProtoId<ReagentPrototype> ReagentId = "FurryJuiceWhite";

}
