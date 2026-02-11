using Content.Server.Xenoarchaeology.Artifact.XAE.Components;
using Content.Shared.Humanoid;
using Content.Shared.Xenoarchaeology.Artifact;
using Content.Shared.Xenoarchaeology.Artifact.XAE;
using Content.Shared.Random;
using Content.Shared.Random.Helpers;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Audio.Systems;


namespace Content.Server.Xenoarchaeology.Artifact.XAE;


public sealed class XAEAddFurryMarkingsSystem : BaseXAESystem<XAEAddFurryMarkingsComponent>
{

    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly SharedHumanoidAppearanceSystem _appearance = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;

    private readonly HashSet<Entity<HumanoidAppearanceComponent>> _humanoidEntities = new();

    protected override void OnActivated(Entity<XAEAddFurryMarkingsComponent> ent, ref XenoArtifactNodeActivatedEvent args)
    {
        _humanoidEntities.Clear();
        _lookup.GetEntitiesInRange(args.Coordinates, ent.Comp.Radius, _humanoidEntities);
        var markings = _proto.Index<WeightedRandomPrototype>(ent.Comp.FurryMarkings);
        _audio.PlayPvs(ent.Comp.ActivationSound, ent);

        foreach (var humanoid in _humanoidEntities)
        {
            var pickedMarking = markings.Pick(_random);
            _appearance.AddMarking(humanoid, pickedMarking);
        }
    }
}
