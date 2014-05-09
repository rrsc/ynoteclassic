﻿using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FastColoredTextBoxNS;
using SS.Ynote.Classic.Features.Syntax;

internal static class FileTypes
{
    internal static IDictionary<IEnumerable<string>, Language> FileTypesDictionary { get; private set; }

    internal static void BuildDictionary()
    {
        FileTypesDictionary = new Dictionary<IEnumerable<string>, Language>();
        using (var reader = XmlReader.Create(SettingsBase.SettingsDir + "Extensions.xml"))
        {
            while (reader.Read())
            {
                if (reader.IsStartElement() && reader.Name == "Key")
                    FileTypesDictionary.Add(reader["Extensions"].Split('|'), reader["Language"].ToEnum<Language>());
            }
        }
    }
    internal static SyntaxDesc GetLanguage(IDictionary<IEnumerable<string>, Language> dic, string extension)
    {
        var desc = new SyntaxDesc();
        Language lang;
        foreach (var key in dic.Keys)
        {
            if (key.Contains(extension))
            {
                dic.TryGetValue(key, out lang);
                desc.Language = lang;
            }
            else
            {
                foreach (
                    var syntax in
                        SyntaxHighlighter.LoadedSyntaxes.Where(syntax => syntax.Extensions.Contains(extension)))
                    desc.SyntaxBase = syntax;
            }
        }
        return desc;
    }
}

internal class SyntaxDesc
{
    /// <summary>
    ///     if IsBase = false Value of Language
    /// </summary>
    internal Language Language;

    /// <summary>
    ///     Syntax Base
    /// </summary>
    internal SyntaxBase SyntaxBase;

    /// <summary>
    ///     Is a Syntax Base
    /// </summary>
    internal bool IsBase
    {
        get { return SyntaxBase != null; }
    }
}