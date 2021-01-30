using NitroxModel.Helper;

namespace NitroxServer.ConsoleCommands.Abstract.Type
{
    public class TypeFloat : Parameter<float?>, IParameter<object>
    {
        public TypeFloat(string name, bool isRequired) : base(name, isRequired) { }

        public override bool IsValid(string arg)
        {
            return float.TryParse(arg, out float value);
        }

        public override float? Read(string arg)
        {
            Validate.IsTrue(float.TryParse(arg, out float value), "接收到无效的数字(十进制)");
            return value;
        }

        object IParameter<object>.Read(string arg)
        {
            return Read(arg);
        }
    }
}
