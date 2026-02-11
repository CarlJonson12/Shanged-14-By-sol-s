namespace Content.Server.Xenoarchaeology.Artifact.XAE.Components;
using Content.Shared.Random;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Audio;


[RegisterComponent, Access(typeof(XAEAddFurryMarkingsSystem))]
public sealed partial class XAEAddFurryMarkingsComponent : Component
{

    [DataField("radius")]
    public float Radius = 15f;

    [DataField]
    public ProtoId<WeightedRandomPrototype> FurryMarkings = string.Empty;

    [DataField]
    public SoundSpecifier ActivationSound = new SoundPathSpecifier("/Audio/Nyanotrasen/Voice/Felinid/cat_meow1.ogg");
}
