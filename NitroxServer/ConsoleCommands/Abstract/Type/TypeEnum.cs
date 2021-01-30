using System;
using NitroxModel.Helper;

namespace NitroxServer.ConsoleCommands.Abstract.Type
{
    public class TypeEnum<T> : Parameter<object> where T : struct, Enum
    {
        public TypeEnum(string name, bool required) : base(name, required)
        {
            Validate.IsTrue(typeof(T).IsEnum, $"类型 {typeof(T).FullName} 不是一个枚举值");
        }

        public override bool IsValid(string arg)
        {
            return Enum.TryParse(arg, true, out T result);
        }

        public override object Read(string arg)
        {
            Validate.IsTrue(Enum.TryParse(arg, true, out T value), "接收到位置的值");
            return value;
        }
    }
}
