using Content.Server.Zhopik.FemboyPhrase.Components;
using Content.Shared.Interaction.Events;
using Content.Server.Popups;
using Robust.Shared.Prototypes;
using Content.Shared.Dataset;
using Robust.Shared.Random;

namespace Content.Server.Zhopik.FemboyPhrase;

public sealed class FemboyPhraseSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly PopupSystem _femboy = default!;
    [Dependency] private readonly IRobustRandom _robustRandom = default!;
    public override void Initialize()
    {
        SubscribeLocalEvent<FemboyPhraseComponent, UseInHandEvent>(OnUse);
    }
    private void OnUse(Entity<FemboyPhraseComponent> ent, ref UseInHandEvent args)
    {
        var dataset = _proto.Index<DatasetPrototype>(ent.Comp.Dataset);
        var phrase = _robustRandom.Pick(dataset.Values);
        _femboy.PopupEntity(Loc.GetString(phrase), args.User, args.User);
    }
}

/*⠀⠀⠀⠀⠀⠀⠀⢠⢄⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⢸⠀⠈⠙⠲⢤⣠⣶⣿⣶⡄⠀⣀⣄⡀⢀⣠⠤⠔⠒⠉⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠉⠉⠉⠒⢦⡈⣇⠀⠀⠀⠀⠙⢿⣿⣿⣿⣿⣿⣿⣿⣁⡀⠀⠀⠀⠀⠀⢸⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⡇⠀⢀⣀⣀⡬⠤⢼⣆⠀⠀⠀⠀⠀⠻⡝⠛⠛⠿⣿⣿⣿⣿⠄⠀⠀⠀⠀⡞⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⡇⠀⢻⡀⠀⠀⠀⠀⠈⠓⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠛⠟⠀⠀⠀⠀⢰⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⡇⠀⠀⠳⢤⣀⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡎⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⢹⠀⠀⠀⠀⠀⠁⣀⣤⡄⠀⠀⠀⠀⠀⠀⠤⠤⢤⣀⠀⠀⠀⠀⠀⠀⡸⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠈⣆⠀⠀⠀⢀⡞⠁⣠⣦⡀⠀⠀⠀⠀⢀⣴⣦⠀⠈⢧⠀⠀⠀⠀⣰⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⢀⣸⣦⠀⠀⣸⠀⠀⣿⣿⡇⠀⠀⠀⠀⢸⣿⣿⠆⠀⢸⡄⠀⠀⡴⠧⡦⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠘⣆⠀⣠⠤⡠⠄⠀⠿⠿⠁⠠⠄⠀⠀⠘⠿⠟⠀⠤⠞⠢⠔⠂⢀⡴⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠈⢢⡄⠀⠀⠀⠀⠀⠀⠲⠴⠲⠤⠚⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⣆⠀⠀⠀⠀⠀⡀⠀⠀⠀⠀⠀⠀
⠀⠀⠰⣃⣀⣀⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⡠⠦⠤⠴⠚⠀⠀⠀⠀⠀⡏⠳⣄⠀⠀⠀⠀
⠀⠀⠀⠀⠀⢀⣠⣭⣙⠒⢲⣦⣤⠀⠀⠀⠀⢀⣴⡞⣏⣁⣤⣤⣤⡀⠀⠀⠀⠀⠀⢀⡇⠀⠀⠳⣄⠀⠀
⠀⠀⠀⠀⢠⣿⣿⣿⣿⣿⡏⢸⣿⣿⣿⣿⣿⣿⡿⢱⣿⣿⣿⣿⣿⣿⡄⠀⠀⡷⣄⣸⠀⠀⠀⠀⠘⣆⠀
⠀⠀⠀⣰⣿⣿⣿⣿⣿⣿⠇⢸⡛⠋⠀⠛⠛⡏⠀⣾⣿⣿⣿⣿⣿⣿⣧⠀⢠⠏⠀⠁⠀⠀⠀⠀⠀⠘⡆
⠀⠀⠀⣿⣿⣿⣿⣿⣿⣿⠀⠀⢇⣀⢀⣀⡼⠁⢰⣿⣿⣿⣿⣿⣿⣿⡿⠀⡜⠀⠀⠀⠀⠀⠀⠀⠀⠀⢳
⠀⠀⠀⠘⠻⣿⡿⠿⠿⡟⣇⠀⠀⠀⠀⠀⠀⠀⡸⠿⣿⣿⣿⣿⣿⠟⢁⠞⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸
⠀⠀⠀⠀⠘⠣⡖⠚⠚⠉⠹⣄⠀⠀⠀⠀⠀⢀⡟⠢⠴⣉⣩⣉⣧⠔⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸
⠀⠀⠀⠀⠀⠀⢳⠀⠀⠀⠀⢿⣉⠉⠓⠚⣉⡎⠀⠀⠀⠀⢰⣯⣁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡞
⠀⠀⠀⠀⠀⠀⠘⡆⠀⠀⠀⠈⣏⠉⠉⢉⠎⠀⠀⠀⠀⢀⡿⣿⣿⣷⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣰⠁
⠀⠀⠀⠀⠀⣠⡞⢹⡀⠀⠀⠀⠸⡄⢀⡞⠀⠀⠀⠀⠀⡞⢀⣿⣿⣿⣿⣷⡄⠀⠀⠀⠀⠀⠀⠀⡴⠃⠀
⠀⠀⠀⢠⣾⣿⣷⡄⠳⡄⠀⠀⠀⢳⡜⠀⠀⠀⠀⢀⡜⠀⢻⣿⣿⣿⣿⣿⣿⣦⠀⠀⠀⣀⡤⠚⠀⠀⠀
⠀⠀⣰⣿⣿⣿⣿⠀⠀⠙⣄⠀⠀⢸⠁⠀⠀⠀⢠⠞⠀⠀⢀⣿⣿⣿⣿⣿⣿⣿⣷⣙⣯⠀⠀⠀⠀⠀⠀
⠀⣰⣿⣿⣿⣿⣿⣦⣄⣤⠈⠙⠒⠚⠦⠤⠤⠖⠁⢀⣤⣤⣾⣿⣿⣿⣿⣿⣿⣿⡟⢳⠀⠀⠀⠀⠀⠀⠀
⢰⠋⠿⣿⣿⣿⣿⣿⣿⣿⣦⡀⠀⠀⣠⣤⡀⠀⣠⣾⣿⣿⣿⣿⣿⣿⣿⡿⠟⡵⠒⠋⠀⠀⠀⠀⠀⠀⠀
⠀⠙⠒⢺⠙⠻⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠿⠛⣭⣠⠴⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠉⠉⠳⣄⣉⣽⡛⠛⣻⡛⠛⠛⠛⠛⠛⣛⠉⢉⡵⠤⡤⠚⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⢸⠀⠈⠉⠀⠙⠒⡏⠙⠒⠋⠉⠉⠉⠀⢸⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⢸⣷⣶⣶⣶⣶⣶⣶⣤⣤⣤⣤⣤⣤⣤⡏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠘⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⢻⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢿⣿⣿⣿⣿⣿⣿⣿⣿⠏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢿⣿⣿⣿⣿⣿⣿⡟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⣿⣿⣿⣿⡿⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠹⣿⣿⠿⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
*/
