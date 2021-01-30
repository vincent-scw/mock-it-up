﻿using log4net;
using MockItUp.Common;
using MockItUp.Common.Contracts;
using System;
using System.Collections.Generic;
using System.IO;

namespace MockItUp.Core
{
    public class SpecLoader : ISpecLoader
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(SpecLoader));

        public SpecDeclaration Load(string content)
        {
            var model = YamlSerializer.Deserialize<SpecDeclaration>(content);
            switch (model.Type.ToLower())
            {
                case "restful":
                    return YamlSerializer.Deserialize<Restful.Models.RestfulSpecDeclaration>(content);
                default:
                    throw new NotSupportedException($"Spec type {model.Type.ToLower()} is not supported.");
            }
        }

        public IList<SpecDeclaration> LoadDirectory(string directoryPath)
        {
            var list = new List<SpecDeclaration>();

            var files = Directory.GetFiles(directoryPath);
            foreach (var file in files)
            {
                if (!".yml|.yaml".Contains(Path.GetExtension(file)))
                    continue;

                var content = File.ReadAllText(file);
                try
                {
                    var spec = Load(content);
                    list.Add(spec);
                }
                catch (Exception ex)
                {
                    _log.Error($"Load spec error: {ex.Message}", ex);
                    throw ex;
                }
            }

            return list;
        }
    }
}