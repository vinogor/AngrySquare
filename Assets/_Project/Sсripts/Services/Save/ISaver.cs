using System.Threading.Tasks;

namespace _Project.Sсripts.Services.Save
{
    public interface ISaver
    {
        void Write(string data);
        Task<string> Read();
    }
}