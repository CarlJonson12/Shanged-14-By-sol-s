using Content.Shared.NPC.Components;
using Content.Shared.Interaction;
using Content.Shared.Mobs.Components;
using Content.Server.Changed14.Fuckable;
using Content.Server.Changed14.Furry;
using Content.Shared.Changed14.Fuckable;
using Content.Shared.Changed14.Furry;
using Content.Server.Changed14.FuckMachine.Components;
using Content.Server.Changed14.FuckMachine;
using Content.Shared.Buckle;
using Content.Shared.Item.ItemToggle;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.Specific;

public sealed partial class MachineFuckOperator : HTNOperator
{
    [Dependency] private readonly IEntityManager _entMan = default!;

    private FuckableSystem _fuckable = default!;
    private FurrySystem _furry = default!;
    private FuckMachineSystem _fuckMachine = default!;
    private SharedBuckleSystem _buckle = default!;
    private ItemToggleSystem _itemToggle = default!;

    /// <summary>
    /// Target entity to fuck.
    /// </summary>
    [DataField("targetKey", required: true)]
    public string TargetKey = string.Empty;

    public override void Initialize(IEntitySystemManager sysManager)
    {
        base.Initialize(sysManager);
        _fuckable = sysManager.GetEntitySystem<FuckableSystem>();
        _furry = sysManager.GetEntitySystem<FurrySystem>();
        _fuckMachine = sysManager.GetEntitySystem<FuckMachineSystem>();
        _buckle = sysManager.GetEntitySystem<SharedBuckleSystem>();
;
    }

    public override void TaskShutdown(NPCBlackboard blackboard, HTNOperatorStatus status)
    {
        base.TaskShutdown(blackboard, status);
        blackboard.Remove<EntityUid>(TargetKey);
    }

    public override HTNOperatorStatus Update(NPCBlackboard blackboard, float frameTime)
    {

        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        if (!blackboard.TryGetValue<EntityUid>(TargetKey, out var target, _entMan) || _entMan.Deleted(target))
            return HTNOperatorStatus.Failed;

        if (!_entMan.TryGetComponent<FuckMachineComponent>(owner, out var botComp))
            return HTNOperatorStatus.Failed;

        if (!_buckle.TryBuckle(target, owner, owner))
            return HTNOperatorStatus.Failed;

        // if (!_itemToggle.TryActivate(owner))
            // return HTNOperatorStatus.Failed;


        return HTNOperatorStatus.Finished;
    }
}
