// LIGHT TODO: лорные название вербов
// HARD TODO: анимации
// POSSIBLE FUTURE: цвет зависит от фурри, голос от пола
using Content.Server.DoAfter;
using Content.Shared.DoAfter;
using Content.Shared.Verbs;
using Content.Shared.Changed14.Fuckable;
using Robust.Shared.Audio.Systems;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Changed14.Furry;
using Content.Server.Stunnable;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Server.NPC.Systems;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Robust.Shared.Map;
using Robust.Shared.Physics.Components;
using System.Numerics;
using Content.Shared.Stunnable;
using Content.Server.NPC.HTN.PrimitiveTasks.Operators.Specific;
using Content.Shared.DoAfter;
using Content.Server.Administration.Systems;
using Robust.Shared.Audio;

namespace Content.Server.Changed14.Fuckable;

public sealed class FuckableSystem : EntitySystem
{
    [Dependency] private readonly DoAfterSystem _doAfter = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;
    [Dependency] private readonly StunSystem _stun = default!;
    [Dependency] private readonly SharedInteractionSystem _interaction = default!;
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
    [Dependency] private readonly NPCSteeringSystem _steering = default!;
    [Dependency] private readonly SharedStunSystem _stunSystem = default!;
    [Dependency] private readonly RejuvenateSystem _rejuvenate = default!;


    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FuckableComponent, GetVerbsEvent<ActivationVerb>>(OnActivationVerb);
        SubscribeLocalEvent<FuckableComponent, FuckDoAfterEvent>(OnFuckDoAfter);
        // SubscribeLocalEvent<NPCFuckableComponent, ComponentStartup>(OnNPCFuckableStartup);
        // SubscribeLocalEvent<NPCFuckableComponent, ComponentShutdown>(OnNPCFuckableShutdown);
    }

    // public override void Update(float frameTime)
    // {
    //     base.Update(frameTime);
    //     UpdateNPCFuckable(frameTime);
    // }

    private void OnFuckDoAfter(EntityUid uid, FuckableComponent comp, ref FuckDoAfterEvent args)
    {

        if (args.Cancelled)
            return;

        if (!_solutionContainer.TryGetInjectableSolution(uid, out var injectable, out _))
            return;

        _solutionContainer.TryAddReagent(injectable.Value, comp.ReagentId, 5);

        // _rejuvenate.PerformRejuvenate(uid);

        return;
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
            Act = () => HandleFuck(uid, user, comp),
            Text = Loc.GetString("changed-fuck-verb"),
            Message = Loc.GetString("changed-fuck-desc"),
        };

        args.Verbs.Add(verb);
    }

    private void HandleFuck(EntityUid uid, EntityUid user, FuckableComponent comp)
    {
        var doAfterArgs = new DoAfterArgs(EntityManager, user, TimeSpan.FromSeconds(3), new FuckDoAfterEvent(), uid, uid)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            NeedHand = false,
            DistanceThreshold = 0.5f,
            MovementThreshold = 0.15f,
        };

        _doAfter.TryStartDoAfter(doAfterArgs);

        if (_interaction.InRangeUnobstructed(user, uid, 0.5f, Shared.Physics.CollisionGroup.Impassable))
        {
            _audio.PlayPvs(comp.FuckableCumSound, uid);
            _stun.TryKnockdown(uid, TimeSpan.FromSeconds(5), true);
        }
        else
        {
            _popupSystem.PopupEntity(Loc.GetString("changed-fuck-range"), user, user);
        }
    }

    public bool CheckFuckable(Entity<FurryComponent?> furry, EntityUid target, bool manual = false)
    {
        if (!TryComp<MobStateComponent>(target, out var mobState)) return false;

        if (mobState.CurrentState == MobState.Alive && mobState.CurrentState == MobState.Dead) return false;

        return true;
    }
    public bool TryFuck(Entity<FurryComponent?> uid, EntityUid target)
    {

        if (!_interaction.InRangeUnobstructed(uid.Owner, target)) return false;

        if (!TryComp<MobStateComponent>(uid, out var mobState)) return false;

        if (mobState.CurrentState == MobState.Alive && mobState.CurrentState == MobState.Dead) return false;

        HandleNPCFuck(uid, target);

        return true;
    }
    private void HandleNPCFuck(EntityUid uid, EntityUid target)
    {

        var doAfterArgs = new DoAfterArgs(
            EntityManager,
            uid,
            TimeSpan.FromSeconds(3),
            new FuckDoAfterEvent(),
            target,
            target
        )
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            NeedHand = false,
            DistanceThreshold = 0.5f,
            MovementThreshold = 0.15f,
        };

        if (!HasComp<ActiveDoAfterComponent>(uid))
        {
            _doAfter.TryStartDoAfter(doAfterArgs);
            _audio.PlayPvs(new SoundPathSpecifier("/Audio/Voice/Human/malescream_1.ogg"), target);
        }
        return;

        // if (_interaction.InRangeUnobstructed(uid, target, 0.5f, Shared.Physics.CollisionGroup.Impassable))
        // {

        //     _stun.TryKnockdown(target, TimeSpan.FromSeconds(5), true);
        //     _stun.TrySlowdown(uid, TimeSpan.FromSeconds(5), true, 0f, 0f);
        // }
    }
}


    // private void OnNPCFuckableStartup(EntityUid uid, NPCFuckableComponent component, ComponentStartup args)
    // {
    //     component.ActionTimer = 0f;
    // }

    // private void OnNPCFuckableShutdown(EntityUid uid, NPCFuckableComponent component, ComponentShutdown args)
    // {
    //     component.Target = null;
    //     _steering.Unregister(uid);
    // }

    // private void UpdateNPCFuckable(float frameTime)
    // {
    //     var query = EntityQueryEnumerator<NPCFuckableComponent>();

    //     while (query.MoveNext(out var uid, out var npcComp))
    //     {
    //         if (!npcComp.Enabled)
    //             continue;

    //         if (npcComp.Target == null)
    //             continue;

    //         var target = npcComp.Target.Value;

    //         if (!EntityManager.EntityExists(target))
    //         {
    //             npcComp.Target = null;
    //             continue;
    //         }

    //         if (!TryComp<FuckableComponent>(target, out var fuckComp))
    //         {
    //             npcComp.Target = null;
    //             continue;
    //         }

    //         if (!TryComp<MobStateComponent>(target, out var targetMobState))
    //         {
    //             continue;
    //         }

    //         if (targetMobState.CurrentState != MobState.Critical)
    //         {
    //             npcComp.Target = null;
    //             continue;
    //         }

    //         npcComp.ActionTimer += frameTime;

    //         if (npcComp.ActionTimer < npcComp.ActionDelay)
    //             continue;
    //         npcComp.ActionTimer = 0f;

    //         if (!TryGetDistance(uid, target, out var distance))
    //         {
    //             npcComp.Target = null;
    //             continue;
    //         }

    //         if (distance > npcComp.MaxDistance)
    //         {

    //             _steering.Register(uid, new EntityCoordinates(target, Vector2.Zero), null);
    //             continue;
    //         }

    //         HandleNPCFuck(uid, target, npcComp, fuckComp);
    //     }
    // }

    // private bool TryGetDistance(EntityUid from, EntityUid to, out float distance)
    // {
    //     distance = 0f;

    //     var xformQuery = GetEntityQuery<TransformComponent>();

    //     if (!xformQuery.TryGetComponent(from, out var fromXform))
    //         return false;

    //     if (!xformQuery.TryGetComponent(to, out var toXform))
    //         return false;

    //     return fromXform.Coordinates.TryDistance(EntityManager, toXform.Coordinates, out distance);
    // }


