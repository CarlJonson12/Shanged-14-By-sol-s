// SPDX-FileCopyrightText: 2022 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Kirill <kirill@example.com>
// SPDX-FileCopyrightText: 2025 RedFoxIV <38788538+RedFoxIV@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 ReserveBot <211949879+ReserveBot@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Rouden <149893554+Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 SX-7 <sn1.test.preria.2002@gmail.com>
// SPDX-FileCopyrightText: 2025 Svarshik <96281939+lexaSvarshik@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 2025 nazrin <tikufaev@outlook.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Numerics;
using Content.Shared.Follower.Components;
using Robust.Client.Animations;
using Robust.Client.GameObjects;
using Robust.Shared.Animations;
using Robust.Shared.Random;

namespace Content.Client.Orbit;

public sealed class OrbitVisualsSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _robustRandom = default!;
    [Dependency] private readonly AnimationPlayerSystem _animations = default!;

    // [Dependency] private readonly IGameTiming _timing = default!;
    // [Dependency] private readonly SpriteSystem _sprite = default!;
    private readonly string _orbitAnimationKey = "orbiting"; // Reserve edit: Port WWDP, CustomGhost
    private readonly string _orbitStopKey = "orbiting_stop";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<OrbitVisualsComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<OrbitVisualsComponent, ComponentRemove>(OnComponentRemove);
        SubscribeLocalEvent<OrbitVisualsComponent, AnimationCompletedEvent>(OnAnimationCompleted); // Reserve edit: Port WWDP, CustomGhost
    }

    private void OnComponentInit(EntityUid uid, OrbitVisualsComponent component, ComponentInit args)
    {
        component.OrbitDistance =
            _robustRandom.NextFloat(0.75f * component.OrbitDistance, 1.25f * component.OrbitDistance);

        component.OrbitLength = _robustRandom.NextFloat(0.5f * component.OrbitLength, 1.5f * component.OrbitLength);

        if (TryComp<SpriteComponent>(uid, out var sprite))
        {
            sprite.EnableDirectionOverride = true;
            sprite.DirectionOverride = Direction.South;
        }

        // Reserve edit START: Port WWDP, CustomGhost
        var animationPlayer = EnsureComp<AnimationPlayerComponent>(uid);
        if (_animations.HasRunningAnimation(uid, animationPlayer, _orbitAnimationKey))
            return;

        if (_animations.HasRunningAnimation(uid, animationPlayer, _orbitStopKey))
        {
            _animations.Stop(uid, animationPlayer, _orbitStopKey);
        }

        _animations.Play(uid, animationPlayer, GetOrbitAnimation(component), _orbitAnimationKey);
        // Reserve edit END: Port WWDP, CustomGhost
    }

    private void OnComponentRemove(EntityUid uid, OrbitVisualsComponent component, ComponentRemove args)
    {
        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        sprite.EnableDirectionOverride = false;

        // Reserve edit START: Port WWDP, CustomGhost
        var animationPlayer = EnsureComp<AnimationPlayerComponent>(uid);
        if (_animations.HasRunningAnimation(uid, animationPlayer, _orbitAnimationKey))
        {
            _animations.Stop(uid, animationPlayer, _orbitAnimationKey);
        }

        if (!_animations.HasRunningAnimation(uid, animationPlayer, _orbitStopKey))
        {
            _animations.Play(uid, animationPlayer, GetStopAnimation(component, sprite), _orbitStopKey);
        }
    }

    public override void FrameUpdate(float frameTime)
    {
        base.FrameUpdate(frameTime);

        foreach (var (orbit, sprite) in EntityManager.EntityQuery<OrbitVisualsComponent, SpriteComponent>())
        {
            var angle = new Angle(Math.PI * 2 * orbit.Orbit);
            var vec = angle.RotateVec(new Vector2(orbit.OrbitDistance, 0));

            sprite.Rotation = orbit.KeepUpright ? Angle.Zero : angle;
            sprite.Offset = vec;
        }
    }
    private void OnAnimationCompleted(EntityUid uid, OrbitVisualsComponent component, AnimationCompletedEvent args)
    {
        if (args.Key == _orbitAnimationKey && TryComp(uid, out AnimationPlayerComponent? animationPlayer))
        {
            _animations.Play(uid, animationPlayer, GetOrbitAnimation(component), _orbitAnimationKey);
        }
    }
    // Reserve edit END: Port WWDP, CustomGhost

    // Reserve edit START: Port WWDP, CustomGhost
    private Animation GetOrbitAnimation(OrbitVisualsComponent component)
    {
        var length = component.OrbitLength;

        return new Animation()
        {
            Length = TimeSpan.FromSeconds(length),
            AnimationTracks =
            {
                new AnimationTrackComponentProperty()
                {
                    ComponentType = typeof(OrbitVisualsComponent),
                    Property = nameof(OrbitVisualsComponent.Orbit),
                    KeyFrames =
                    {
                        new AnimationTrackProperty.KeyFrame(0.0f, 0f),
                        new AnimationTrackProperty.KeyFrame(1.0f, length),
                    },
                    InterpolationMode = AnimationInterpolationMode.Linear
                }
            }
        };
    }
    // Reserve edit END: Port WWDP, CustomGhost

    private Animation GetStopAnimation(OrbitVisualsComponent component, SpriteComponent sprite)
    {
        var length = component.OrbitStopLength;

        return new Animation()
        {
            Length = TimeSpan.FromSeconds(length),
            AnimationTracks =
            {
                new AnimationTrackComponentProperty()
                {
                    ComponentType = typeof(SpriteComponent),
                    Property = nameof(SpriteComponent.Offset),
                    KeyFrames =
                    {
                        new AnimationTrackProperty.KeyFrame(sprite.Offset, 0f),
                        new AnimationTrackProperty.KeyFrame(Vector2.Zero, length),
                    },
                    InterpolationMode = AnimationInterpolationMode.Linear
                },
                new AnimationTrackComponentProperty()
                {
                    ComponentType = typeof(SpriteComponent),
                    Property = nameof(SpriteComponent.Rotation),
                    KeyFrames =
                    {
                        new AnimationTrackProperty.KeyFrame(sprite.Rotation.Reduced(), 0f),
                        new AnimationTrackProperty.KeyFrame(Angle.Zero, length),
                    },
                    InterpolationMode = AnimationInterpolationMode.Linear
                }
            }
        };
    }
}
