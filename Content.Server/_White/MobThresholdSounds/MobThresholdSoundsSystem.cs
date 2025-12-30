// SPDX-FileCopyrightText: 2025 Space Station 14 Contributors
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Damage;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Robust.Shared.Audio.Systems;

namespace Content.Server._White.MobThresholdSounds;

public sealed class MobThresholdSoundsSystem : EntitySystem
{
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly MobThresholdSystem _mobThreshold = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MobThresholdSoundsComponent, DamageChangedEvent>(OnDamageChanged);
        SubscribeLocalEvent<MobThresholdSoundsComponent, MobStateChangedEvent>(OnMobStateChanged);
    }

    private void OnDamageChanged(EntityUid uid, MobThresholdSoundsComponent component, DamageChangedEvent args)
    {
        if (!TryComp<MobThresholdsComponent>(uid, out var thresholds))
            return;

        if (args.Damageable.TotalDamage == FixedPoint2.Zero)
        {
            component.PlayedCritSound = false;
            component.PlayedDeathSound = false;
            return;
        }

        if (!_mobThreshold.TryGetThresholdForState(uid, MobState.Critical, out var critThreshold, thresholds))
            return;

        if (args.Damageable.TotalDamage >= critThreshold && !component.PlayedCritSound && component.CritSound != null)
        {
            _audio.PlayEntity(component.CritSound, uid, uid); // Reserve edit
            component.PlayedCritSound = true;
        }
    }

    private void OnMobStateChanged(EntityUid uid, MobThresholdSoundsComponent component, MobStateChangedEvent args)
    {
        if (args.NewMobState == MobState.Dead && !component.PlayedDeathSound && component.DeathSound != null)
        {
            var sound = component.DeathSound;
            _audio.PlayEntity(sound, uid, uid); // Reserve edit
            component.PlayedDeathSound = true;
        }

        if (args.NewMobState == MobState.Alive)
        {
            component.PlayedCritSound = false;
            component.PlayedDeathSound = false;
        }
    }
}
