// TODO: update state for inserting and removing a disk, красивее ui, сообщение об уничтожении диска
using Content.Server._Changed14.ResearchEvac.Components;
using Content.Shared._Changed14.ResearchEvac;
using Content.Shared.UserInterface;
using Robust.Server.GameObjects;
using Content.Server.Research.Systems;
using Content.Server.Chat.Systems;
using Robust.Shared.Containers;
using Content.Server.RoundEnd;


namespace Content.Server._Changed14.ResearchEvac;

public sealed class ResearchEvacSystem : EntitySystem
{
    [Dependency] private readonly UserInterfaceSystem _ui = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] protected readonly SharedContainerSystem Container = default!;
    [Dependency] private readonly RoundEndSystem _roundEnd = default!;


    public override void Initialize()
    {
        SubscribeLocalEvent<ResearchEvacConsoleComponent, ComponentInit>(OnCompInit);
        SubscribeLocalEvent<ResearchEvacConsoleComponent, BoundUIOpenedEvent>(OnGeneratorBUIOpened);
        SubscribeLocalEvent<ResearchEvacConsoleComponent, BeforeActivatableUIOpenEvent>(OnBeforeUiOpen);
        SubscribeLocalEvent<ResearchEvacConsoleComponent, ResearchEvacButtonPressedEvent>(OnCallEvacButtonPressed);
    }

    private void OnCompInit(EntityUid uid,  ResearchEvacConsoleComponent component, ref ComponentInit args)
    {
        UpdateGeneratorUi(uid, component);
    }
    private void OnGeneratorBUIOpened(EntityUid uid, ResearchEvacConsoleComponent component, BoundUIOpenedEvent args)
    {
        UpdateGeneratorUi(uid, component);
    }

    private void OnBeforeUiOpen(EntityUid uid, ResearchEvacConsoleComponent component, BeforeActivatableUIOpenEvent args)
    {
        UpdateGeneratorUi(uid, component);
    }

     private void OnCallEvacButtonPressed(EntityUid uid, ResearchEvacConsoleComponent component, ResearchEvacButtonPressedEvent message)
    {

        UpdateGeneratorUi(uid, component);
        OnGeneratingFinished(uid, component);
    }

    private void UpdateGeneratorUi(EntityUid uid, ResearchEvacConsoleComponent? component = null)
    {

        var hasContainer = (Container.EnsureContainer<ContainerSlot>(uid, "evac_disk"));
        var canCall = hasContainer.ContainedEntity.HasValue;
        var state = new ResearchEvacConsoleBoundUserInterfaceState(canCall);
        _ui.SetUiState(uid, ResearchEvacConsoleUiKey.Key, state);

    }

    private void OnGeneratingFinished(EntityUid uid, ResearchEvacConsoleComponent component)
    {

        var container = Container.GetContainer(uid, "evac_disk");

        Container.CleanContainer(container);

        _chat.DispatchGlobalAnnouncement(Loc.GetString("Результаты исследования латекса были получены, вызван эвакуационный шаттл"), Loc.GetString("Командование"), false, null, colorOverride: Color.Crimson);
        _roundEnd.RequestRoundEnd(null, false);

        UpdateGeneratorUi(uid, component);
    }

}
