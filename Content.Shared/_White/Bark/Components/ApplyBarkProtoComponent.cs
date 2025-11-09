// SPDX-FileCopyrightText: 2025 Cinkafox <70429757+Cinkafox@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 ReserveBot <211949879+ReserveBot@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Humanoid;
using Robust.Shared.Prototypes;

namespace Content.Shared._White.Bark.Components;

[RegisterComponent]
public sealed partial class ApplyBarkProtoComponent : Component
{
    public static ProtoId<BarkVoicePrototype> DefaultVoice = SharedHumanoidAppearanceSystem.DefaultBarkVoice;

    [DataField]
    public ProtoId<BarkVoicePrototype> VoiceProto { get; set; } = DefaultVoice;

    [DataField]
    public BarkPercentageApplyData? PercentageApplyData { get; set; }
}
