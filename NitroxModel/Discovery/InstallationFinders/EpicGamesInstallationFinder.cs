using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NitroxModel.Logger;

namespace NitroxModel.Discovery.InstallationFinders
{
    public class EpicGamesInstallationFinder : IFindGameInstallation
    {
        private readonly Regex installLocationRegex = new Regex("\"InstallLocation\"[^\"]*\"(.*)\"");

        public string FindGame(List<string> errors = null)
        {
            string epicGamesManifestsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Epic\EpicGamesLauncher\Data\Manifests");
            if (!Directory.Exists(epicGamesManifestsDir))
            {
                errors?.Add("Epic games 清单文件目录不存在，请检查 Epic Games Store 被正确安装。");
                return null;
            }

            string[] files = Directory.GetFiles(epicGamesManifestsDir, "*.item");
            foreach (string file in files)
            {
                string fileText = File.ReadAllText(file);
                Match match = installLocationRegex.Match(fileText);
                if (match.Success && match.Value.Contains("Subnautica") && !match.Value.Contains("Below"))
                {
                    Log.Debug($"Found Subnautica install path in '{Path.GetFullPath(file)}'. Full pattern match: '{match.Value}'");
                    return match.Groups[1].Value;
                }
            }

            errors?.Add("无法从 Epic Games 安装记录中获取深海迷航安装路径，请检查深海迷航已经从 Epic Games Store 中安装。");
            return null;
        }
    }
}
