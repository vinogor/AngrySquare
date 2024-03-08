using System;

namespace Services.Save
{
    public interface ISaver
    {
        void Write(string data);
        void Read(Action<string> action);
    }
}