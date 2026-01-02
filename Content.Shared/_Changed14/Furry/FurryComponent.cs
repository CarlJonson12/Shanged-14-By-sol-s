using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Content.Shared.Chat.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;


namespace Content.Shared.Changed14.Furry;

[RegisterComponent, NetworkedComponent]
public sealed partial class FurryComponent : Component
{
    [DataField("emoteId")]
    public ProtoId<EmoteSoundsPrototype>? EmoteSoundsId = "Furry";

}
