﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using RapidXamlToolkit.ErrorList;

namespace RapidXamlToolkit.XamlAnalysis
{
    public abstract class RapidXamlViewTag : IRapidXamlViewTag
    {
        public ActionTypes ActionType { get; protected set; }

        public Span Span { get; set; }

        public string ToolTip { get; set; }

        public string Message { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        public ITextSnapshot Snapshot { get; set; }

        public string ErrorCode { get; set; }

        public bool IsFatal { get; protected set; }

        public ITagSpan<IErrorTag> AsErrorTag()
        {
            var span = new SnapshotSpan(this.Snapshot, this.Span);
            return new TagSpan<IErrorTag>(span, new RapidXamlWarningTag(this.ToolTip));
        }

        public ErrorRow AsErrorRow()
        {
            // TODO: add a property for Extended message, rather than just using the action type
            return new ErrorRow
            {
                ExtendedMessage = this.ActionType.ToString(),
                Span = new SnapshotSpan(this.Snapshot, this.Span),
                Message = this.Message,
                ErrorCode = this.ErrorCode,
                IsFatal = this.IsFatal,
            };
        }
    }
}
