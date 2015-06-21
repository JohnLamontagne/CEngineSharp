namespace CEngineSharp_Server.Utilities.ServiceLocators
{
    public interface IServiceLocator<T>
    {
        T GetService();

        void SetService(T service);
    }
}