using System.Collections.Generic;
using System.IO;
using NitroxModel.DataStructures.Util;

namespace NitroxModel.Discovery.InstallationFinders
{
    /// <summary>
    ///     Tries to read a local file that contains the installation directory of Subnautica.
    /// </summary>
    public class ConfigFileGameFinder : IFindGameInstallation
    {
        private const string FILENAME = "path.txt";

        public string FindGame(List<string> errors = null)
        {
            if (!File.Exists(FILENAME))
            {
                errors?.Add($@"游戏安装目录配置文件未设置。在目录 '{Directory.GetCurrentDirectory()}' 中创建文件 '{FILENAME}'");
                return null;
            }

            string path = File.ReadAllText(FILENAME).Trim();
            if (string.IsNullOrEmpty(path))
            {
                errors?.Add($@"配置文件 {Path.GetFullPath(FILENAME)} 是空的。请输入深海迷航的安装路径。");
                return null;
            }

            if (!Directory.Exists(Path.Combine(path, "Subnautica_Data", "Managed")))
            {
                errors?.Add($@"游戏安装目录配置文件 {path} 不存在，请输入深海迷航的安装路径");
                return null;
            }

            return path;
        }
    }
}
