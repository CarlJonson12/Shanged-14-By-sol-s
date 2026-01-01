// LIGHT TODO: сделать ренж ивента минимальным, выебать только при крите,
//стан крит на 5+сек жертве, сделать интервал между криками партнеров, лорные название вербов, не показывать верб при >ренж
// HARD TODO: анимации
// POSSIBLE FUTURE: цвет зависит от фурри, голос от пола
using Content.Server.DoAfter;
using Content.Shared.DoAfter;
using Content.Shared.Verbs;
using Content.Shared.Changed14.Fuckable;
using Content.Shared.Tag;
using Robust.Shared.Audio.Systems;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Changed14.Furry;

namespace Content.Server.Changed14.Fuckable;

public sealed class FuckableSystem : EntitySystem
{
    [Dependency] private readonly DoAfterSystem _doAfter = default!;
    [Dependency] private readonly TagSystem _tag = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FuckableComponent, GetVerbsEvent<ActivationVerb>>(OnActivationVerb);
        SubscribeLocalEvent<FuckableComponent, FuckDoAfterEvent>(OnFuckDoAfter);
    }

    private void OnFuckDoAfter(EntityUid uid, FuckableComponent comp, ref FuckDoAfterEvent args)
    {

        if (args.Cancelled)
            return;

        _audio.PlayPvs(comp.FurryCumSound, args.User);
        _audio.PlayPvs(comp.FuckableCumSound, args.User);

        if (!_solutionContainer.TryGetInjectableSolution(uid, out var injectable, out _))
            return;
        _solutionContainer.TryAddReagent(injectable.Value, comp.ReagentId, 5);

    }

    private void OnActivationVerb(EntityUid uid, FuckableComponent comp, ref GetVerbsEvent<ActivationVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        if (!HasComp<FurryComponent>(args.User))
            return;

        var user = args.User;

        var verb = new ActivationVerb()
        {
            Act = () => HandleFuck(uid, user),
            Text = Loc.GetString("changed-fuck-verb"),
            Message = Loc.GetString("changed-fuck-desc"),
        };

        args.Verbs.Add(verb);
    }

    private void HandleFuck(EntityUid uid, EntityUid user)
    {
        var doAfterArgs = new DoAfterArgs(EntityManager, user, TimeSpan.FromSeconds(3), new FuckDoAfterEvent(), uid, uid)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            NeedHand = false,
            DistanceThreshold = 0.5f,
        };

        _doAfter.TryStartDoAfter(doAfterArgs);

    }


}
