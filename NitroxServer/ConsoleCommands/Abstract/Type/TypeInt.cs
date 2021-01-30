using NitroxModel.Helper;

namespace NitroxServer.ConsoleCommands.Abstract.Type
{
    public class TypeInt : Parameter<int?>, IParameter<object>
    {
        public TypeInt(string name, bool isRequired) : base(name, isRequired) { }

        public override bool IsValid(string arg)
        {
            return int.TryParse(arg, out int value);
        }

        public override int? Read(string arg)
        {
            Validate.IsTrue(int.TryParse(arg, out int value), "接收到无效的整数");
            return value;
        }

        object IParameter<object>.Read(string arg)
        {
            return Read(arg);
        }
    }
}
