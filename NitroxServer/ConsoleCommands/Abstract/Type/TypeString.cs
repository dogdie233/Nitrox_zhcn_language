using NitroxModel.Helper;

namespace NitroxServer.ConsoleCommands.Abstract.Type
{
    public class TypeString : Parameter<string>
    {
        public TypeString(string name, bool isRequired) : base(name, isRequired) { }

        public override bool IsValid(string arg)
        {
            return !string.IsNullOrEmpty(arg);
        }

        public override string Read(string arg)
        {
            Validate.IsTrue(IsValid(arg), "接收到空字符串，而不是有效字符串");
            return arg;
        }
    }
}
