namespace CEngineSharp_World.Entities
{
    public interface IBaseEntity
    {
        string Name { get; set; }

        int TextureNumber { get; set; }

        int Level { get; set; }

        Vector Position { get; set; }

        int GetStat(Stats stat);

        void SetStat(Stats stat, int value);
    }
}