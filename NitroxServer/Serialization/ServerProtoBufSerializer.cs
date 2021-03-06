﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NitroxModel.Logger;
using ProtoBufNet;
using ProtoBufNet.Meta;

namespace NitroxServer.Serialization
{
    public class ServerProtoBufSerializer : IServerSerializer
    {
        private static readonly object[] emptyArray = { };
        protected RuntimeTypeModel Model { get; } = TypeModel.Create();

        public ServerProtoBufSerializer(params string[] assemblies)
        {
            foreach (string assembly in assemblies)
            {
                RegisterAssemblyClasses(assembly);
            }
        }

        public void Serialize(Stream stream, object o)
        {
            Model.SerializeWithLengthPrefix(stream, o, o.GetType(), PrefixStyle.Base128, 0);
        }

        public void Serialize(string filePath, object o)
        {
            using (Stream stream = File.OpenWrite(filePath))
            {
                Serialize(stream, o);
            }
        }

        public T Deserialize<T>(Stream stream)
        {
            T t = (T)Activator.CreateInstance(typeof(T));
            Model.DeserializeWithLengthPrefix(stream, t, typeof(T), PrefixStyle.Base128, 0);
            return t;
        }

        public T Deserialize<T>(string filePath)
        {
            using (Stream stream = File.OpenRead(filePath))
            {
                return Deserialize<T>(stream);
            }
        }

        public void Deserialize(Stream stream, object o, Type t)
        {
            Model.DeserializeWithLengthPrefix(stream, o, t, PrefixStyle.Base128, 0);
        }

        private void RegisterAssemblyClasses(string assemblyName)
        {
            List<string> ignoredTypeErrors = new List<string>();

            foreach (Type type in Assembly.Load(assemblyName).GetTypes())
            {
                try
                {
                    bool hasNitroxProtoBuf = type.GetCustomAttributes(typeof(ProtoContractAttribute), false).Length > 0;

                    if (hasNitroxProtoBuf)
                    {
                        // As of the latest protobuf update they will automatically register detected attributes.
                        Model.Add(type, true);
                    }
                    else if (HasUweProtoContract(type))
                    {
                        Model.Add(type, true);

                        ManuallyRegisterUweProtoMembers(type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance), type);
                    }
                }
                catch (Exception ex)
                {
                    if (type.FullName?.Contains("Oculus.Platform.Models") ?? false)
                    {
                        ignoredTypeErrors.Add(type.ToString());
                    }
                    else
                    {
                        Log.Error(ex, $"[服务器协议缓冲区序列化程序(ServerProtoBufSerializer)] 在注册类型 {type} 时从 {assemblyName} 抛出了一个错误");
                    }
                }
            }

            if (ignoredTypeErrors.Count > 0)
            {
                Log.Debug($"[服务器协议缓冲区序列化程序(ServerProtoBufSerializer)] 抛出了一个错误在注册 {string.Join(", ", ignoredTypeErrors)} 从 {assemblyName} 时。但可能是由与的Newtonsoft.Json文件Oculus和Nitrox的版本不同造成的，可以忽略。");
            }
        }

        private bool HasUweProtoContract(Type type)
        {
            foreach (object o in type.GetCustomAttributes(false))
            {
                if (o.GetType().ToString().Contains("ProtoContractAttribute"))
                {
                    return true;
                }
            }

            return false;
        }

        private void ManuallyRegisterUweProtoMembers(MemberInfo[] info, Type type)
        {
            foreach (MemberInfo property in info)
            {
                if (!(property.DeclaringType != type))
                {
                    foreach (object customAttribute in property.GetCustomAttributes(false))
                    {
                        Type attributeType = customAttribute.GetType();
                        if (attributeType.ToString().Contains("ProtoMemberAttribute"))
                        {
                            int tag = (int)attributeType.GetProperty("Tag", BindingFlags.Public | BindingFlags.Instance).GetValue(customAttribute, emptyArray);
                            Model[type].Add(tag, property.Name);
                        }
                    }
                }
            }
        }
    }
}
