// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading;
using Microsoft.CodeAnalysis.FindUsages;
using Microsoft.CodeAnalysis.Options;

namespace Microsoft.CodeAnalysis.Navigation
{
    internal class DefaultSymbolNavigationService : ISymbolNavigationService
    {
        public bool TryNavigateToSymbol(ISymbol symbol, Project project, OptionSet options = null, CancellationToken cancellationToken = default(CancellationToken))
            => false;

        public bool TrySymbolNavigationNotify(ISymbol symbol, Solution solution, CancellationToken cancellationToken)
<<<<<<< HEAD
        {
            return false;
        }

        public bool WouldNavigateToSymbol(
            ISymbol symbol, Solution solution, CancellationToken cancellationToken,
=======
            => false;

        public bool WouldNavigateToSymbol(
            DefinitionItem definitionItem, Solution solution, CancellationToken cancellationToken,
>>>>>>> 865fef487a864b6fe69ab020e32218c87befdd00
            out string filePath, out int lineNumber, out int charOffset)
        {
            filePath = null;
            lineNumber = 0;
            charOffset = 0;

            return false;
        }
    }
}