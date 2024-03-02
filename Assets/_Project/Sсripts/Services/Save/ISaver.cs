using System;

namespace _Project.Sсripts.Services.Save
{
    public interface ISaver
    {
        void Write(string data);
        void Read(Action<string> action);
    }
}