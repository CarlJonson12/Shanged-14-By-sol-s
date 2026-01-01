using Content.Server.DoAfter;
using Content.Shared.DoAfter;
using Content.Shared.Verbs;
using Content.Shared.Changed14.Fuckable;
using Content.Shared.Tag;
using Robust.Shared.Audio.Systems;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Changed14.Furry;
using Content.Server.Body.Systems;
using Content.Shared.Administration;
using Content.Shared.Body.Components;
using Content.Shared.Body.Part;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Hands.Components;



namespace Content.Server.Changed14.Furry;

public sealed class FurrySystem : EntitySystem
{
    [Dependency] private readonly DoAfterSystem _doAfter = default!;
    [Dependency] private readonly TagSystem _tag = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FurryComponent, ComponentStartup>(OnStartup);

    }

    private void OnStartup(EntityUid uid, FurryComponent component, ComponentStartup args)
    {
        if (TryComp<HandsComponent>(uid, out var handsComp))
        {
            var handId = $"{uid}-hand-{1}";
            _hands.RemoveHand(uid, "hand");
            // RemComp(uid, handsComp);
        }
    }
}

// эта хуйня не работает
//                          ,-------.                 /
//                        ,'         `.           ,--'
//                      ,'             `.      ,-;--        _.-
//                     /                 \ ---;-'  _.=.---''
//   +-------------+  ;    X        X     ---=-----'' _.-------
//   |    -----    |--|                   \-----=---:i-
//   +XX|'i:''''''''  :                   ;`--._ ''---':----
//   /X+-)             \   \         /   /      ''--._  `-
//  .XXX|)              `.  `.     ,'  ,'             ''---.
//    X\/)                `.  '---'  ,'                     `-
//      \                   `---+---'
//       \                      |
//        \.                    |
//          `-------------------+
