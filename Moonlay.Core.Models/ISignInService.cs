namespace Moonlay.Core.Models
{
    public interface ISignInService
    {
        string CurrentUser { get; }

        bool Demo { get; }
    }
}
