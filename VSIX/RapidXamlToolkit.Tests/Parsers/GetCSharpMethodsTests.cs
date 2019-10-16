﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidXamlToolkit.Options;
using RapidXamlToolkit.Parsers;

namespace RapidXamlToolkit.Tests.Parsers
{

    [TestClass]
    public class GetCSharpMethodsTests : CSharpTestsBase
    {
        [TestMethod]
        public void GetAllPropertyOptions()
        {
            var profile = TestProfile.CreateEmpty();
            profile.ClassGrouping = "StackPanel";
            profile.FallbackOutput = "<TextBlock Text=\"FALLBACK_$name$\" />";
            profile.Mappings.Add(new Mapping
            {
                Type = "method()",
                NameContains = "",
                Output = "<TextBlock Text=\"NOPARAMS_$method$\" />",
                IfReadOnly = false,
            });
            profile.Mappings.Add(new Mapping
            {
                Type = "method(T)",
                NameContains = "",
                Output = "<TextBlock Text=\"ONEPARAM_$method$_$arg1$\" />",
                IfReadOnly = false,
            });
            profile.Mappings.Add(new Mapping
            {
                Type = "method(T,T)",
                NameContains = "",
                Output = "<TextBlock Text=\"TWOPARAMS_$method$_$arg1$_$arg2$\" />",
                IfReadOnly = false,
            });

            var code = @"
namespace tests
{
    class Class1☆
    {
        public void OnPhotoTaken(CameraControlEventArgs args) { }

        public void ZoomIn() => _zoomService?.ZoomIn();

        public void Undo() {  }

        public async void SwitchTheme(ElementTheme theme) { }

        public async void Redo() {  }

        public void MethodName(string name, int amount) { }
    }
}";

            var expectedOutput = "<StackPanel>"
         + Environment.NewLine + "    <TextBlock Text=\"ONEPARAM_OnPhotoTaken_args\" />"
         + Environment.NewLine + "    <TextBlock Text=\"NOPARAMS_ZoomIn\" />"
         + Environment.NewLine + "    <TextBlock Text=\"NOPARAMS_Undo\" />"
         + Environment.NewLine + "    <TextBlock Text=\"ONEPARAM_SwitchTheme_theme\" />"
         + Environment.NewLine + "    <TextBlock Text=\"NOPARAMS_Redo\" />"
         + Environment.NewLine + "    <TextBlock Text=\"TWOPARAMS_MethodName_name_amount\" />"
         + Environment.NewLine + "</StackPanel>";


            var expected = new ParserOutput
            {
                Name = "Class1",
                Output = expectedOutput,
                OutputType = ParserOutputType.Class,
            };

            this.PositionAtStarShouldProduceExpected(code, expected);
        }
    }
}
