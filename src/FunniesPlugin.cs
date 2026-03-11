using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;
using Funnies.Modules;

namespace Funnies;

public class FunniesPlugin : BasePlugin
{
    public override string ModuleName => "Fair Wallhack";
    public override string ModuleVersion => "1.0";

    private bool WallhackEnabled = false;

    public override void Load(bool hotReload)
    {
        Console.WriteLine("Wallhack plugin loaded");

        Globals.Plugin = this;

        RegisterListener<Listeners.CheckTransmit>(OnCheckTransmit);
        RegisterEventHandler<EventRoundStart>(OnRoundStart);

        Wallhack.Setup();
    }

    public override void Unload(bool hotReload)
    {
        if (hotReload)
        {
            Wallhack.Cleanup();
        }
    }

    private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        WallhackEnabled = false;

        // Enable wallhack after 5 seconds
        AddTimer(5.0f, () =>
        {
            WallhackEnabled = true;
            Console.WriteLine("Wallhack enabled for all players");
        });

        return HookResult.Continue;
    }

    public void OnCheckTransmit(CCheckTransmitInfoList infoList)
    {
        if (!WallhackEnabled)
            return;

        foreach ((CCheckTransmitInfo info, CCSPlayerController? player) in infoList)
        {
            if (!Util.IsPlayerValid(player))
                continue;

            Wallhack.OnPlayerTransmit(info, player!);
        }
    }
}
