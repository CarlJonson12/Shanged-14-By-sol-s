using Content.Shared.Roles;
using Robust.Shared.Prototypes;
using Content.Shared.Humanoid;

namespace Content.Server.Jobs;

public sealed partial class FurryMarkingsSpecial : JobSpecial
{

    [DataField]
    public Color SkinColor = Color.Black;


    public override void AfterEquip(EntityUid mob)
    {
        var entMan = IoCManager.Resolve<IEntityManager>();
        var appearanceSystem = entMan.System<SharedHumanoidAppearanceSystem>();

        appearanceSystem.AddMarking(mob, "VulpTailLong");
        appearanceSystem.AddMarking(mob, "VulpEar");
        appearanceSystem.SetSkinColor(mob, SkinColor);


    }
}
