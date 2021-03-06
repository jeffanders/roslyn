// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis.CodeGeneration;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.ImplementType;
using Microsoft.CodeAnalysis.LanguageServices;
using Microsoft.CodeAnalysis.Shared.Extensions;
using Microsoft.CodeAnalysis.Shared.Utilities;

namespace Microsoft.CodeAnalysis.ImplementInterface
{
    internal abstract partial class AbstractImplementInterfaceService
    {
        internal partial class ImplementInterfaceCodeAction
        {
            private ISymbol GenerateProperty(
                Compilation compilation,
                IPropertySymbol property,
                Accessibility accessibility,
                DeclarationModifiers modifiers,
                bool generateAbstractly,
                bool useExplicitInterfaceSymbol,
                string memberName,
                ImplementTypePropertyGenerationBehavior propertyGenerationBehavior,
                CancellationToken cancellationToken)
            {
                var factory = this.Document.GetLanguageService<SyntaxGenerator>();
                var attributesToRemove = AttributesToRemove(compilation);

<<<<<<< HEAD
                var getAccessor = GenerateGetAccessor(compilation, property, accessibility, generateAbstractly,
                    useExplicitInterfaceSymbol, attributesToRemove, cancellationToken);

                var setAccessor = GenerateSetAccessor(compilation, property, accessibility,
                    generateAbstractly, useExplicitInterfaceSymbol, attributesToRemove, cancellationToken);
=======
                var getAccessor = GenerateGetAccessor(
                    compilation, property, accessibility, generateAbstractly, useExplicitInterfaceSymbol,
                    propertyGenerationBehavior, attributesToRemove, cancellationToken);

                var setAccessor = GenerateSetAccessor(
                    compilation, property, accessibility, generateAbstractly, useExplicitInterfaceSymbol,
                    propertyGenerationBehavior, attributesToRemove, cancellationToken);
>>>>>>> 865fef487a864b6fe69ab020e32218c87befdd00

                var syntaxFacts = Document.GetLanguageService<ISyntaxFactsService>();
                var parameterNames = NameGenerator.EnsureUniqueness(
                    property.Parameters.Select(p => p.Name).ToList(), isCaseSensitive: syntaxFacts.IsCaseSensitive);

                var updatedProperty = property.RenameParameters(parameterNames);

                updatedProperty = updatedProperty.RemoveAttributeFromParameters(attributesToRemove);

                // TODO(cyrusn): Delegate through throughMember if it's non-null.
                return CodeGenerationSymbolFactory.CreatePropertySymbol(
                    updatedProperty,
                    accessibility: accessibility,
                    modifiers: modifiers,
                    explicitInterfaceSymbol: useExplicitInterfaceSymbol ? property : null,
                    name: memberName,
                    getMethod: getAccessor,
                    setMethod: setAccessor);
            }

            /// <summary>
            /// Lists compiler attributes that we want to remove.
            /// The TupleElementNames attribute is compiler generated (it is used for naming tuple element names).
            /// We never want to place it in source code.
            /// Same thing for the Dynamic attribute.
            /// </summary>
            private INamedTypeSymbol[] AttributesToRemove(Compilation compilation)
            {
                return new[] { compilation.ComAliasNameAttributeType(), compilation.TupleElementNamesAttributeType(),
                    compilation.DynamicAttributeType() };
            }

            private IMethodSymbol GenerateSetAccessor(
                Compilation compilation,
                IPropertySymbol property,
                Accessibility accessibility,
                bool generateAbstractly,
                bool useExplicitInterfaceSymbol,
<<<<<<< HEAD
=======
                ImplementTypePropertyGenerationBehavior propertyGenerationBehavior,
>>>>>>> 865fef487a864b6fe69ab020e32218c87befdd00
                INamedTypeSymbol[] attributesToRemove,
                CancellationToken cancellationToken)
            {
                if (property.SetMethod == null)
<<<<<<< HEAD
=======
                {
                    return null;
                }

                if (property.GetMethod == null)
                {
                    // Can't have an auto-prop with just a setter.
                    propertyGenerationBehavior = ImplementTypePropertyGenerationBehavior.PreferThrowingProperties;
                }

                var setMethod = property.SetMethod.RemoveInaccessibleAttributesAndAttributesOfTypes(
                     this.State.ClassOrStructType,
                     attributesToRemove);

                return CodeGenerationSymbolFactory.CreateAccessorSymbol(
                    setMethod,
                    attributes: default(ImmutableArray<AttributeData>),
                    accessibility: accessibility,
                    explicitInterfaceSymbol: useExplicitInterfaceSymbol ? property.SetMethod : null,
                    statements: GetSetAccessorStatements(
                        compilation, property, generateAbstractly, propertyGenerationBehavior, cancellationToken));
            }

            private IMethodSymbol GenerateGetAccessor(
                Compilation compilation,
                IPropertySymbol property,
                Accessibility accessibility,
                bool generateAbstractly,
                bool useExplicitInterfaceSymbol,
                ImplementTypePropertyGenerationBehavior propertyGenerationBehavior,
                INamedTypeSymbol[] attributesToRemove,
                CancellationToken cancellationToken)
            {
                if (property.GetMethod == null)
>>>>>>> 865fef487a864b6fe69ab020e32218c87befdd00
                {
                    return null;
                }

<<<<<<< HEAD
                var setMethod = property.SetMethod.RemoveInaccessibleAttributesAndAttributesOfTypes(
                     this.State.ClassOrStructType,
                     attributesToRemove);

                return CodeGenerationSymbolFactory.CreateAccessorSymbol(
                    setMethod,
                    attributes: default(ImmutableArray<AttributeData>),
                    accessibility: accessibility,
                    explicitInterfaceSymbol: useExplicitInterfaceSymbol ? property.SetMethod : null,
                    statements: GetSetAccessorStatements(compilation, property, generateAbstractly, cancellationToken));
            }

            private IMethodSymbol GenerateGetAccessor(
                Compilation compilation,
                IPropertySymbol property,
                Accessibility accessibility,
                bool generateAbstractly,
                bool useExplicitInterfaceSymbol,
                INamedTypeSymbol[] attributesToRemove,
                CancellationToken cancellationToken)
            {
                if (property.GetMethod == null)
                {
                    return null;
                }

=======
>>>>>>> 865fef487a864b6fe69ab020e32218c87befdd00
                var getMethod = property.GetMethod.RemoveInaccessibleAttributesAndAttributesOfTypes(
                     this.State.ClassOrStructType,
                     attributesToRemove);

                return CodeGenerationSymbolFactory.CreateAccessorSymbol(
                    getMethod,
                    attributes: default(ImmutableArray<AttributeData>),
                    accessibility: accessibility,
                    explicitInterfaceSymbol: useExplicitInterfaceSymbol ? property.GetMethod : null,
<<<<<<< HEAD
                    statements: GetGetAccessorStatements(compilation, property, generateAbstractly, cancellationToken));
=======
                    statements: GetGetAccessorStatements(
                        compilation, property, generateAbstractly, propertyGenerationBehavior, cancellationToken));
>>>>>>> 865fef487a864b6fe69ab020e32218c87befdd00
            }

            private ImmutableArray<SyntaxNode> GetSetAccessorStatements(
                Compilation compilation,
                IPropertySymbol property,
                bool generateAbstractly,
<<<<<<< HEAD
=======
                ImplementTypePropertyGenerationBehavior propertyGenerationBehavior,
>>>>>>> 865fef487a864b6fe69ab020e32218c87befdd00
                CancellationToken cancellationToken)
            {
                if (generateAbstractly)
                {
                    return default(ImmutableArray<SyntaxNode>);
                }

                var factory = this.Document.GetLanguageService<SyntaxGenerator>();
                if (ThroughMember != null)
                {
                    var throughExpression = CreateThroughExpression(factory);
                    SyntaxNode expression;

                    if (property.IsIndexer)
                    {
                        expression = throughExpression;
                    }
                    else
                    {
                        expression = factory.MemberAccessExpression(
                                                throughExpression, factory.IdentifierName(property.Name));
                    }

                    if (property.Parameters.Length > 0)
                    {
                        var arguments = factory.CreateArguments(property.Parameters.As<IParameterSymbol>());
                        expression = factory.ElementAccessExpression(expression, arguments);
                    }

                    expression = factory.AssignmentStatement(expression, factory.IdentifierName("value"));

                    return ImmutableArray.Create(factory.ExpressionStatement(expression));
                }

                return propertyGenerationBehavior == ImplementTypePropertyGenerationBehavior.PreferAutoProperties
                    ? default(ImmutableArray<SyntaxNode>)
                    : factory.CreateThrowNotImplementedStatementBlock(compilation);
            }

            private ImmutableArray<SyntaxNode> GetGetAccessorStatements(
                Compilation compilation,
                IPropertySymbol property,
                bool generateAbstractly,
                ImplementTypePropertyGenerationBehavior propertyGenerationBehavior,
                CancellationToken cancellationToken)
            {
                if (generateAbstractly)
                {
                    return default(ImmutableArray<SyntaxNode>);
                }

                var factory = this.Document.GetLanguageService<SyntaxGenerator>();
                if (ThroughMember != null)
                {
                    var throughExpression = CreateThroughExpression(factory);
                    SyntaxNode expression;

                    if (property.IsIndexer)
                    {
                        expression = throughExpression;
                    }
                    else
                    {
                        expression = factory.MemberAccessExpression(
                                                throughExpression, factory.IdentifierName(property.Name));
                    }

                    if (property.Parameters.Length > 0)
                    {
                        var arguments = factory.CreateArguments(property.Parameters.As<IParameterSymbol>());
                        expression = factory.ElementAccessExpression(expression, arguments);
                    }

                    return ImmutableArray.Create(factory.ReturnStatement(expression));
                }

                return propertyGenerationBehavior == ImplementTypePropertyGenerationBehavior.PreferAutoProperties
                    ? default(ImmutableArray<SyntaxNode>)
                    : factory.CreateThrowNotImplementedStatementBlock(compilation);
            }
        }
    }
}