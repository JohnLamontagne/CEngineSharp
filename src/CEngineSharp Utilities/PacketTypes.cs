namespace CEngineSharp_Utilities
{
    public enum PacketTypes
    {
        LoginPacket = 0,
        RegistrationPacket,
        PlayerDataPacket,
        MapDataPacket,
        ChatMessagePacket,
        PlayerMovementPacket,
        SpawnMapItemPacket,
        MapCheckPacket,
        DespawnMapItemPacket,
        SpawnMapNpcPacket,
        LogoutPacket,
        UpdatePlayerStatsPacket,
        UpdateInventoryPacket,
        PickupItemPacket,
        DropItemPacket,
        AlertMessagePacket
    }
}