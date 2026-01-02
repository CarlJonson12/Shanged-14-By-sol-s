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
using Content.Server.Body.Systems;
using Content.Server.Chat;
using Content.Server.Chat.Systems;
using Content.Server.Emoting.Systems;
using Content.Shared.Damage;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Popups;
using Content.Shared.Roles;
using Content.Shared.Weapons.Melee.Events;
using Robust.Shared.Prototypes;
using Content.Server.Speech.EntitySystems;



namespace Content.Server.Changed14.Furry;

public sealed class FurrySystem : EntitySystem
{
    [Dependency] private readonly DoAfterSystem _doAfter = default!;
    [Dependency] private readonly TagSystem _tag = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;
    [Dependency] private readonly AutoEmoteSystem _autoEmote = default!;
    [Dependency] private readonly EmoteOnDamageSystem _emoteOnDamage = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly IPrototypeManager _protoManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FurryComponent, EmoteEvent>(OnEmote);
        SubscribeLocalEvent<FurryComponent, ComponentStartup>(OnStartup);
        // SubscribeLocalEvent<FurryComponent, MobStateChangedEvent>(OnMobState);

    }

    private void OnStartup(EntityUid uid, FurryComponent component, ComponentStartup args)
    {
        if (TryComp<HandsComponent>(uid, out var handsComp))
        {
            var handId = $"{uid}-hand-{1}";
            _hands.RemoveHand(uid, "hand");
            // RemComp(uid, handsComp);
        }

        if (!TryComp<MobStateComponent>(uid, out var mob))
            return;

        if (mob.CurrentState == MobState.Alive)
        {
            // Groaning when damaged
            EnsureComp<EmoteOnDamageComponent>(uid);
            _emoteOnDamage.AddEmote(uid, "Scream");

            // Random groaning
            EnsureComp<AutoEmoteComponent>(uid);
            _autoEmote.AddEmote(uid, "ZombieGroan");
        }

    }

    private void OnEmote(EntityUid uid, FurryComponent component, ref EmoteEvent args)
    {
        if (args.Handled)
            return;

        _protoManager.TryIndex(component.EmoteSoundsId, out var sounds);

        args.Handled = _chat.TryPlayEmoteSound(uid, sounds, args.Emote);
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
