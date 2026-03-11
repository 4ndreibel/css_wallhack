using CounterStrikeSharp.API.Core;
using Funnies.Modules;

namespace Funnies;

public class FunniesPlugin : BasePlugin
{
    public override string ModuleName => "Funny plugin";
    public override string ModuleVersion => "0.0.1";

    public override void Load(bool hotReload)
    {
        Console.WriteLine("Wallhack plugin loaded");

        Globals.Plugin = this;

        RegisterListener<Listeners.CheckTransmit>(OnCheckTransmit);

        Wallhack.Setup();
    }

    public override void Unload(bool hotReload)
    {
        if (hotReload)
        {
            Wallhack.Cleanup();
        }
    }

    public void OnCheckTransmit(CCheckTransmitInfoList infoList)
    {
        foreach ((CCheckTransmitInfo info, CCSPlayerController? player) in infoList)
        {
            if (!Util.IsPlayerValid(player))
                continue;

            Wallhack.OnPlayerTransmit(info, player!);
        }
    }
}
