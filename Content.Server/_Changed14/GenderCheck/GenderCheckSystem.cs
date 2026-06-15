using Content.Server.Changed14.GenderCheck.Components;
using Content.Shared.Interaction.Events;
using Content.Shared.Humanoid;
using Content.Shared.Popups;

namespace Content.Server.Changed14.GenderCheck;
public sealed class GenderCheckSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GenderCheckComponent, UseInHandEvent>(OnUse);
    }

    private void OnUse(Entity<GenderCheckComponent> ent, ref UseInHandEvent args)
    {
        var userSex = CompOrNull<HumanoidAppearanceComponent>(args.User)?.Sex;
        var genderSay = "";
        if (userSex != null)
        {
            switch (userSex)
            {
                case Sex.Male:
                        genderSay = "я мальчик";
                    break;
                case Sex.Female:
                        genderSay = "я девочка";
                    break;
            }
        }
        _popup.PopupEntity(genderSay, args.User, args.User);
    }
}
