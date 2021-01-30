using System;
using System.Linq;
using NitroxModel.Helper;

namespace NitroxServer.ConsoleCommands.Abstract.Type
{
    public class TypeBoolean : Parameter<bool?>, IParameter<object>
    {
        private static readonly string[] noValues = new string[]
        {
            bool.FalseString,
            "no",
            "off",
            "假",
            "否",
            "关闭",
            "禁用",
            "错"
        };

        private static readonly string[] yesValues = new string[]
        {
            bool.TrueString,
            "yes",
            "on",
            "真",
            "是",
            "打开",
            "启用",
            "对"
        };

        public TypeBoolean(string name, bool isRequired) : base(name, isRequired) { }

        public override bool IsValid(string arg)
        {
            return yesValues.Contains(arg, StringComparer.OrdinalIgnoreCase) || noValues.Contains(arg, StringComparer.OrdinalIgnoreCase);
        }

        public override bool? Read(string arg)
        {
            Validate.IsTrue(IsValid(arg), "接收到无效的布尔值");
            return yesValues.Contains(arg, StringComparer.OrdinalIgnoreCase);
        }

        object IParameter<object>.Read(string arg)
        {
            return Read(arg);
        }
    }
}
