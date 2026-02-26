// // SPDX-FileCopyrightText: 2025 Changed14
// //
// // SPDX-License-Identifier: AGPL-3.0-or-later

// using System.Threading;
// using System.Threading.Tasks;
// using Content.Shared.Changed14.Fuckable;
// using Content.Shared.Mobs;
// using Content.Shared.Mobs.Components;
// using Content.Shared.NPC;

// namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators;
// public sealed partial class FuckableOperator : HTNOperator
// {
//     [Dependency] private readonly IEntityManager _entManager = default!;
//     [DataField("targetKey")]
//     public string TargetKey = "FuckableTarget";
//     [DataField("range")]
//     public float Range = 10f;

//     public override void Initialize(IEntitySystemManager sysManager)
//     {
//         base.Initialize(sysManager);
//     }

//     public override HTNOperatorStatus Update(NPCBlackboard blackboard, float frameTime)
//     {
//         var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
//         if (!_entManager.TryGetComponent<NPCFuckableComponent>(owner, out var fuckableComp))
//             return HTNOperatorStatus.Failed;
//         var xformQuery = _entManager.GetEntityQuery<TransformComponent>();
//         if (!xformQuery.TryGetComponent(owner, out var ownerXform))
//             return HTNOperatorStatus.Failed;
//         var query = _entManager.GetEntityQuery<FuckableComponent>();
//         var mobStateQuery = _entManager.GetEntityQuery<MobStateComponent>();

//         var closestTarget = EntityUid.Invalid;
//         float closestDistance = float.MaxValue;
//         foreach (var ent in _entManager.GetEntities())
//         {
//             if (ent == owner)
//                 continue;
//             if (!query.HasComponent(ent))
//                 continue;
//             if (!xformQuery.TryGetComponent(ent, out var targetXform))
//                 continue;
//             if (ownerXform.MapID != targetXform.MapID)
//                 continue;
//             if (mobStateQuery.TryGetComponent(ent, out var mobState))
//             {
//                 if (mobState.CurrentState != MobState.Critical)
//                     continue;
//             }
//             if (!ownerXform.Coordinates.TryDistance(_entManager, targetXform.Coordinates, out var distance))
//                 continue;
//             if (distance < closestDistance && distance < Range)
//             {
//                 closestDistance = distance;
//                 closestTarget = ent;
//             }
//         }
//         if (closestTarget == EntityUid.Invalid)
//             return HTNOperatorStatus.Failed;
//         fuckableComp.Target = closestTarget;
//         fuckableComp.Enabled = true;
//         blackboard.SetValue(TargetKey, closestTarget);

//         return HTNOperatorStatus.Finished;
//     }
// }
