using Content.Server.Zhopik.FemboyPhrase.Components;
using Content.Shared.Interaction.Events;
using Content.Server.Popups;
namespace Content.Server.Zhopik.FemboyPhrase;

public sealed class FemboyPhraseSystem : EntitySystem
{
    [Dependency] private readonly PopupSystem _femboy = default!;
    public override void Initialize()
    {
        SubscribeLocalEvent<FemboyPhraseComponent, UseInHandEvent>(OnUse);

    }
    private void OnUse(Entity<FemboyPhraseComponent> ent, ref UseInHandEvent args)
    {
        _femboy.PopupEntity(Loc.GetString("я люблю мальчиков"), args.User, args.User);
    }
}
