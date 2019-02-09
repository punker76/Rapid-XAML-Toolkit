﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;

namespace RapidXamlToolkit.XamlAnalysis
{
    public class RapidXamlDocument
    {
        public RapidXamlDocument()
        {
            this.SuggestionTags = new List<IRapidXamlTag>();
        }

        public string RawText { get; set; }

        public List<IRapidXamlTag> SuggestionTags { get; set; }

        public static RapidXamlDocument Create(ITextSnapshot snapshot)
        {
            var result = new RapidXamlDocument();

            try
            {
                var text = snapshot.GetText();
                result.RawText = text;

                // TODO: offload the creation of tags to separate classes for handling each XAML element
                // Register handlers and the elements they are looking for


                var processors = new List<(string, XamlElementProcessor)>
                {
                    ("Grid", new GridProcessor()),
                    ("TextBlock", new TextBlockProcessor()),
                };

                XamlElementExtractor.Parse(snapshot, text, processors, result.SuggestionTags);
            }
            catch (Exception e)
            {
                result.SuggestionTags.Add(new UnexpectedErrorTag
                {
                    Span = new Span(0, 0),
                    Message = "Unexpected error occurred while parsing XAML. Please log an issue to https://github.com/Microsoft/Rapid-XAML-Toolkit/issues Reason: " + e,
                });
            }

            return result;
        }
    }
}
