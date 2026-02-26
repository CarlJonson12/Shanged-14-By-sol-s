using Robust.Shared.GameObjects;

namespace Content.Shared.Changed14.Fuckable;
[RegisterComponent]
public sealed partial class NPCFuckableComponent : Component
{

    [DataField("target")]
    public EntityUid? Target = null;
    [DataField("enabled")]
    public bool Enabled = true;
    public float ActionTimer = 0f;
    [DataField("actionDelay")]
    public float ActionDelay = 5f;
    [DataField("maxDistance")]
    public float MaxDistance = 1f;
}
