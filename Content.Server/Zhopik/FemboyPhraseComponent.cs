using Content.Server.Zhopik.FemboyPhrase.Components;
using Content.Shared.Dataset;
using Robust.Shared.Prototypes;
namespace Content.Server.Zhopik.FemboyPhrase.Components;

[RegisterComponent]
public sealed partial class FemboyPhraseComponent : Component
{
    [DataField]
    public ProtoId<DatasetPrototype> Dataset;
}
