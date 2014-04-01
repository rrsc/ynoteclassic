﻿// =================================
//
// Ynote Package
// Copyright (C) 2014 Samarjeet Singh
//
//==================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SS.Ynote.Classic.UI;

namespace SS.Ynote.Classic.Features.Packages
{
    public class YnotePackage
    {
        /// <summary>
        ///     Manifest File
        /// </summary>
        public string ManifestFile { private get; set; }

        public static IDictionary<string, string> GenerateDictionary(string manifest)
        {
            IDictionary<string,string> dic = new Dictionary<string, string>();
            var lines = File.ReadAllLines(manifest);
            foreach (var command in lines.Select(line => Parse(line.Replace("$ynotedir", Application.StartupPath))))
                dic.Add(command.Key, command.Value);
            //foreach (var line in lines)
            //{
            //    var command = Parse(line.Replace("$ynotedir", Application.StartupPath));
            //    dic.Add(command.Key, command.Value);
            //}
            return dic;
        }
        /// <summary>
        /// Parses a Ynote Package index.manifest
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        static SCommand Parse(string command)
        {
            try
            {
                var cmd = new SCommand();
                int l = command.IndexOf(":");
                if (l > 0)
                    cmd.Key = command.Substring(0, l);
                var result = command.Substring(command.LastIndexOf(':') + 1);
                cmd.Value = result;
                return cmd;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Parse Error : " + ex.Message, "Error");
                return null;
            }
        }
    }
}