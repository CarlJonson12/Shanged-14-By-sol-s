using Content.Server.Changed14.FuckMachine.Components;
using Content.Shared.Interaction.Events;
using Content.Shared.Humanoid;
using Content.Shared.Popups;
using Content.Shared.Emag.Systems;
using Content.Shared.Movement.Components;
using Content.Server.NPC.HTN;
using Content.Shared.NPC.Components;
using Content.Shared.NPC.Prototypes;
using Content.Shared.NPC.Systems;
using Content.Server.NPC;
using Content.Server.NPC.HTN;
using Content.Server.NPC.Systems;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Toggleable;
using Content.Shared.Buckle.Components;
using Content.Shared.Damage;
using Content.Shared._Shitmed.Targeting; // Shitmed Change
using Content.Shared._Shitmed.Damage; // Shitmed Change
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Server.Changed14.FuckMachine;
public sealed class FuckMachineSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly EmagSystem _emag = default!;
    [Dependency] private readonly HTNSystem _htn = default!;
    [Dependency] private readonly NPCSystem _npc = default!;
    [Dependency] private readonly ItemToggleSystem _itemToggle = default!;
    [Dependency] private readonly DamageableSystem _damageableSystem = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FuckMachineComponent, GotEmaggedEvent>(OnGotEmagged);
        SubscribeLocalEvent<FuckMachineComponent, StrappedEvent>(OnFuck);
    }

    private void OnGotEmagged(EntityUid uid, FuckMachineComponent component, ref GotEmaggedEvent args)
    {
        if (!HasComp<InputMoverComponent>(uid))
            AddComp<InputMoverComponent>(uid);

        if (!HasComp<MobMoverComponent>(uid))
            AddComp<MobMoverComponent>(uid);

        var htn = EnsureComp<HTNComponent>(uid); //Возвращает указанный компонент сущности, при его отсутствии - добавляет его
        htn.RootTask = new HTNCompoundTask() {Task = "Changed14FuckMachineCompound"};
        htn.Blackboard.SetValue(NPCBlackboard.Owner, uid);
        _npc.WakeNPC(uid, htn);
        _htn.Replan(htn);

        _itemToggle.TryActivate(uid, uid);
    }

    private void OnFuck(EntityUid uid, FuckMachineComponent component, ref StrappedEvent args)
    {
        _itemToggle.TryActivate(uid, uid);
        _damageableSystem.TryChangeDamage(args.Buckle, new DamageSpecifier(_proto.Index<DamageGroupPrototype>("Brute"), 50), true, origin: uid, targetPart: TargetBodyPart.All, splitDamage: SplitDamageBehavior.SplitEnsureAll);
    }
}
