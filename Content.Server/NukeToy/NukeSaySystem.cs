using Content.Server.NukeToy.NukeSay;
using Content.Server.Popups;
using Robust.Shared.Random;
using Content.Shared.Interaction.Events;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Audio;
namespace Content.Server.NukeToy.NukeSay;

public sealed class NukeSaySystem : EntitySystem
{
    // Залупка туда-сюда. Лишь бы работало. Так все делают
    [Dependency] private readonly PopupSystem _gay = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;

    public override void Initialize()
    {
        base.Initialize();
        // Вот эта хуйня - типа в руке мы используем
        SubscribeLocalEvent<NukeSayComponent, UseInHandEvent>(OnUse);
    }

    private void OnUse(Entity<NukeSayComponent> ent, ref UseInHandEvent args)
    {
        if (args.Handled)
            return;

        _audio.PlayPvs(new SoundPathSpecifier("/Audio/Machines/Nuke/nuke_alarm.ogg"), args.User);
        var num = _random.Next(0, 4);
        var text = "Мрррр";
        switch (num)
        {
            case 0:
                text = "UwU";
                break;
            case 1:
                text = "Какой хороший мальчик!";
                break;
            case 2:
                text = "You like kissing boys don't you";
                break;
            case 3:
                text = "ббрббрбрббрб пщщщ";
                break;
        }
        _gay.PopupEntity(text, args.User, args.User);
        args.Handled = true;
    }
}

// Без текста ниже ничего работать не будет !!!
/*
⣿⡟⢸⣿⣿⣿⣄⠹⣷⠰⣤⣌⡙⢿⠏⣠⣿⣿⣿⣿⡇⣸
⣿⡇⣾⣿⣿⣿⣿⡧⠈⣀⣹⣿⣿⣦⣰⣿⣿⣿⣿⣿⡇⣿
⣿⡇⢿⣿⣿⣿⣿⣶⣶⣶⣶⣾⣿⣿⣿⣿⣿⣿⣿⣿⡇⣿
⣿⣇⢸⣿⡿⠿⠿⠿⠿⣿⣿⣿⣿⠿⠟⠛⠛⢻⣿⣿⢁⣿
⡿⠿⠄⠻⡖⢰⡆⠀⠀⢸⣿⣿⡇⠀⠀⢸⡆⢹⠋⠁⠚⣿
⣷⡀⠲⣶⡇⢺⣷⡀⢀⡾⠿⣿⣷⣀⣀⣾⠇⣸⡿⠋⣰⣿
⣿⣿⢁⣦⣀⣡⣿⣿⡿⠿⠛⠻⠟⢻⣿⣥⣀⣽⣷⡌⢻⣿
⣿⣿⣬⣭⣌⡙⠛⠿⣷⣶⣾⣿⣿⣿⠛⢛⣀⣬⣥⣤⣼⣿
⣿⣿⣿⣿⣿⣿⣄⠒⢶⣾⣿⣿⣿⣿⣧⡈⢿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⡏⠐⢻⣿⣿⣿⣿⣿⣿⣧⠘⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⠃⣼⣿⣿⣿⣿⣿⣿⣿⡇⢻⣿⣿⣿⣿
*/
//dotnet build
