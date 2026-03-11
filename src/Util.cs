using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;

namespace Funnies;

public static class Util
{
    /// <summary>
    /// Returns the model path of a player's pawn.
    /// </summary>
    public static string GetPlayerModel(CCSPlayerController player)
    {
        return player.Pawn.Value!.CBodyComponent!.SceneNode!
            .GetSkeletonInstance().ModelState.ModelName;
    }

    /// <summary>
    /// Checks if a player entity is valid and connected.
    /// </summary>
    public static bool IsPlayerValid([NotNullWhen(true)] CCSPlayerController? player) =>
        player != null &&
        player.IsValid &&
        player.PlayerPawn != null &&
        player.PlayerPawn.IsValid &&
        player.Connected == PlayerConnectedState.PlayerConnected &&
        !player.IsHLTV;

    /// <summary>
    /// Returns all valid players (bots and humans).
    /// </summary>
    public static List<CCSPlayerController> GetValidPlayers()
    {
        return Utilities
            .GetPlayers()
            .Where(IsPlayerValid)
            .ToList();
    }

    /// <summary>
    /// Recursively collects all child scene nodes.
    /// Useful for iterating model parts.
    /// </summary>
    public static List<CGameSceneNode> GetChildrenRecursive(CGameSceneNode node)
    {
        List<CGameSceneNode> children = [];

        var currentChild = node.Child;
        while (currentChild != null)
        {
            children.Add(currentChild);
            currentChild = currentChild.NextSibling;
        }

        foreach (var child in children.ToList())
        {
            children.AddRange(GetChildrenRecursive(child));
        }

        return children;
    }
}
