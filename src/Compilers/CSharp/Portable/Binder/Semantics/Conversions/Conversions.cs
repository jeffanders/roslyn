// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.CSharp
{
    internal sealed class Conversions : ConversionsBase
    {
        private readonly Binder _binder;

        public Conversions(Binder binder)
            : this(binder, currentRecursionDepth: 0)
        {
<<<<<<< HEAD
            Debug.Assert(source != null);
            Debug.Assert((object)destination != null);

            if (destination.Kind == SymbolKind.NonNullableReference)
                destination = ((NonNullableReferenceTypeSymbol)destination).UnderlyingType;

            Conversion implicitConversion = ClassifyImplicitConversionFromExpression(source, destination, ref useSiteDiagnostics);
            if (implicitConversion.Exists && !implicitConversion.IsUserDefined && !implicitConversion.IsDynamic)
            {
                return implicitConversion;
            }

            Conversion explicitConversion = ClassifyExplicitOnlyConversionFromExpression(source, source.Type, destination, ref useSiteDiagnostics);
            if (explicitConversion.Exists)
            {
                return explicitConversion;
            }

            // It is possible for a user-defined conversion to be unambiguous when considered as
            // an implicit conversion and ambiguous when considered as an explicit conversion.
            // The native compiler does not check to see if a cast could be successfully bound as
            // an unambiguous user-defined implicit conversion; it goes right to the ambiguous
            // user-defined explicit conversion and produces an error. This means that in
            // C# 5 it is possible to have:
            //
            // Y y = new Y();
            // Z z1 = y;
            // 
            // succeed but
            //
            // Z z2 = (Z)y;
            //
            // fail.
            //
            // However, there is another interesting wrinkle. It is possible for both
            // an implicit user-defined conversion and an explicit user-defined conversion
            // to exist and be unambiguous. For example, if there is an implicit conversion
            // double-->C and an explicit conversion from int-->C, and the user casts a short
            // to C, then both the implicit and explicit conversions are applicable and
            // unambiguous. The native compiler in this case prefers the explicit conversion,
            // and for backwards compatibility, we match it.

            return implicitConversion.Exists ? implicitConversion : Conversion.NoConversion;
=======
>>>>>>> 865fef487a864b6fe69ab020e32218c87befdd00
        }

        private Conversions(Binder binder, int currentRecursionDepth)
            : base(binder.Compilation.Assembly.CorLibrary, currentRecursionDepth)
        {
<<<<<<< HEAD
            Debug.Assert(sourceExpression != null || (object)source != null);
            Debug.Assert(sourceExpression == null || (object)sourceExpression.Type == (object)source);
            Debug.Assert((object)destination != null);

            if (HasImplicitDynamicConversionFromExpression(source, destination))
            {
                return Conversion.ImplicitDynamic;
            }

            // The following conversions only exist for certain form of expressions, 
            // if we have no expression none if them is applicable.
            if (sourceExpression == null)
            {
                return Conversion.NoConversion;
            }

            if (HasImplicitEnumerationConversion(sourceExpression, destination))
            {
                return Conversion.ImplicitEnumeration;
            }

            var kind = ClassifyImplicitConstantExpressionConversion(sourceExpression, destination);
            if (kind != ConversionKind.NoConversion)
            {
                return new Conversion(kind);
            }

            if (destination.Kind == SymbolKind.NonNullableReference && !sourceExpression.EffectiveNullability.IsWeakerThan(EffectiveNullability.NonNullable))
            {
                Conversion conversion = ClassifyImplicitConversionFromExpression(sourceExpression, source, ((NonNullableReferenceTypeSymbol)destination).UnderlyingType, ref useSiteDiagnostics);
                if (conversion != Conversion.NoConversion)
                {
                    return conversion;
                }
            }

            switch (sourceExpression.Kind)
            {
                case BoundKind.Literal:
                    kind = ClassifyNullLiteralConversion(sourceExpression, destination);
                    if (kind != ConversionKind.NoConversion)
                    {
                        return new Conversion(kind);
                    }
                    break;

                case BoundKind.UnboundLambda:
                    if (HasAnonymousFunctionConversion(sourceExpression, destination))
                    {
                        return Conversion.AnonymousFunction;
                    }
                    break;

                case BoundKind.MethodGroup:
                    Conversion methodGroupConversion = GetMethodGroupConversion((BoundMethodGroup)sourceExpression, destination, ref useSiteDiagnostics);
                    if (methodGroupConversion.Exists)
                    {
                        return methodGroupConversion;
                    }
                    break;

                case BoundKind.InterpolatedString:
                    Conversion interpolatedStringConversion = GetInterpolatedStringConversion((BoundInterpolatedString)sourceExpression, destination, ref useSiteDiagnostics);
                    if (interpolatedStringConversion.Exists)
                    {
                        return interpolatedStringConversion;
                    }
                    break;
            }

            return Conversion.NoConversion;
=======
            _binder = binder;
>>>>>>> 865fef487a864b6fe69ab020e32218c87befdd00
        }

        protected override ConversionsBase CreateInstance(int currentRecursionDepth)
        {
            return new Conversions(_binder, currentRecursionDepth);
        }

        private CSharpCompilation Compilation { get { return _binder.Compilation; } }

        public override Conversion GetMethodGroupConversion(BoundMethodGroup source, TypeSymbol destination, ref HashSet<DiagnosticInfo> useSiteDiagnostics)
        {
<<<<<<< HEAD
            Debug.Assert(sourceExpression != null || (object)source != null);
            Debug.Assert(sourceExpression == null || (object)sourceExpression.Type == (object)source);
            Debug.Assert((object)destination != null);

            //PERF: identity conversion is by far the most common implicit conversion, check for that first
            if ((object)source != null && HasIdentityConversion(source, destination))
            {
                // Will this too negatively impact performance? If so, should consider actually making T!? a separate TypeSymbol and default(T!?) returns type T not T!?
                if (sourceExpression == null || sourceExpression.Kind != BoundKind.DefaultOperator || !destination.IsReferenceType || destination.GetEffectiveNullability() == EffectiveNullability.Nullable)
                    return Conversion.Identity;
                else
                    return Conversion.NoConversion;
            }

            Conversion conversion = ClassifyImplicitBuiltInConversionFromExpression(sourceExpression, source, destination, ref useSiteDiagnostics);
            if (conversion.Exists)
=======
            // Must be a bona fide delegate type, not an expression tree type.
            if (!destination.IsDelegateType())
>>>>>>> 865fef487a864b6fe69ab020e32218c87befdd00
            {
                return Conversion.NoConversion;
            }

            var methodSymbol = GetDelegateInvokeMethodIfAvailable(destination);
            if ((object)methodSymbol == null)
            {
                return Conversion.NoConversion;
            }

            var resolution = ResolveDelegateMethodGroup(_binder, source, methodSymbol, ref useSiteDiagnostics);
            var conversion = (resolution.IsEmpty || resolution.HasAnyErrors) ?
                Conversion.NoConversion :
                ToConversion(resolution.OverloadResolutionResult, resolution.MethodGroup, (NamedTypeSymbol)destination);
            resolution.Free();
            return conversion;
        }

        protected override Conversion GetImplicitTupleLiteralConversion(BoundTupleLiteral source, TypeSymbol destination, ref HashSet<DiagnosticInfo> useSiteDiagnostics)
        {
            var arguments = source.Arguments;

            // check if the type is actually compatible type for a tuple of given cardinality
            if (!destination.IsTupleOrCompatibleWithTupleOfCardinality(arguments.Length))
            {
<<<<<<< HEAD
                return ConversionKind.NoConversion;
            }

            // SPEC: An implicit conversion exists from the null literal to any nullable type. 
            if (destination.IsNullableType())
            {
                // The spec defines a "null literal conversion" specifically as a conversion from
                // null to nullable type.
                return ConversionKind.NullLiteral;
            }

            // SPEC: An implicit conversion exists from the null literal to any reference type. 
            // SPEC: An implicit conversion exists from the null literal to type parameter T, 
            // SPEC: provided T is known to be a reference type. [...] The conversion [is] classified 
            // SPEC: as implicit reference conversion. 

            if (destination.IsReferenceType)
            {
                if (destination.GetEffectiveNullability() != EffectiveNullability.Nullable)
                    return ConversionKind.NoConversion;
                return ConversionKind.ImplicitReference;
            }

            // SPEC: The set of implicit conversions is extended to include...
            // SPEC: ... from the null literal to any pointer type.

            if (destination is PointerTypeSymbol)
            {
                return ConversionKind.NullToPointer;
=======
                return Conversion.NoConversion;
>>>>>>> 865fef487a864b6fe69ab020e32218c87befdd00
            }

            ImmutableArray<TypeSymbol> targetElementTypes = destination.GetElementTypesOfTupleOrCompatible();
            Debug.Assert(arguments.Length == targetElementTypes.Length);

            // check arguments against flattened list of target element types 
            var argumentConversions = ArrayBuilder<Conversion>.GetInstance(arguments.Length);
            for (int i = 0; i < arguments.Length; i++)
            {
                var argument = arguments[i];
                var result = ClassifyImplicitConversionFromExpression(argument, targetElementTypes[i], ref useSiteDiagnostics);
                if (!result.Exists)
                {
                    argumentConversions.Free();
                    return Conversion.NoConversion;
                }

                argumentConversions.Add(result);
            }

            return new Conversion(ConversionKind.ImplicitTupleLiteral, argumentConversions.ToImmutableAndFree());
        }

        protected override Conversion GetExplicitTupleLiteralConversion(BoundTupleLiteral source, TypeSymbol destination, ref HashSet<DiagnosticInfo> useSiteDiagnostics, bool forCast)
        {
            var arguments = source.Arguments;

            // check if the type is actually compatible type for a tuple of given cardinality
            if (!destination.IsTupleOrCompatibleWithTupleOfCardinality(arguments.Length))
            {
                return Conversion.NoConversion;
            }

            ImmutableArray<TypeSymbol> targetElementTypes = destination.GetElementTypesOfTupleOrCompatible();
            Debug.Assert(arguments.Length == targetElementTypes.Length);

            // check arguments against flattened list of target element types 
            var argumentConversions = ArrayBuilder<Conversion>.GetInstance(arguments.Length);
            for (int i = 0; i < arguments.Length; i++)
            {
                var result = ClassifyConversionFromExpression(arguments[i], targetElementTypes[i], ref useSiteDiagnostics, forCast);
                if (!result.Exists)
                {
                    argumentConversions.Free();
                    return Conversion.NoConversion;
                }

                argumentConversions.Add(result);
            }

<<<<<<< HEAD
            if (source.Kind != BoundKind.UnboundLambda)
            {
                return false;
            }

            return IsAnonymousFunctionCompatibleWithType((UnboundLambda)source, destination) == LambdaConversionResult.Success;
        }

        internal Conversion ClassifyImplicitUserDefinedConversionForSwitchGoverningType(TypeSymbol sourceType, out TypeSymbol switchGoverningType, ref HashSet<DiagnosticInfo> useSiteDiagnostics)
        {
            // SPEC:    The governing type of a switch statement is established by the switch expression.
            // SPEC:    1) If the type of the switch expression is sbyte, byte, short, ushort, int, uint,
            // SPEC:       long, ulong, bool, char, string, or an enum-type, or if it is the nullable type
            // SPEC:       corresponding to one of these types, then that is the governing type of the switch statement. 
            // SPEC:    2) Otherwise, exactly one user-defined implicit conversion (§6.4) must exist from the
            // SPEC:       type of the switch expression to one of the following possible governing types:
            // SPEC:       sbyte, byte, short, ushort, int, uint, long, ulong, char, string, or, a nullable type
            // SPEC:       corresponding to one of those types

            // NOTE:    We should be called only if (1) is false for source type.
            Debug.Assert((object)sourceType != null);
            Debug.Assert(!sourceType.IsValidSwitchGoverningType());

            UserDefinedConversionResult result = AnalyzeImplicitUserDefinedConversionForSwitchGoverningType(sourceType, ref useSiteDiagnostics);

            if (result.Kind == UserDefinedConversionResultKind.Valid)
            {
                UserDefinedConversionAnalysis analysis = result.Results[result.Best];

                switchGoverningType = analysis.ToType;
                Debug.Assert(switchGoverningType.IsValidSwitchGoverningType(isTargetTypeOfUserDefinedOp: true));
            }
            else
            {
                switchGoverningType = null;
            }

            return new Conversion(result, isImplicit: true);
        }

        internal Conversion GetCallerLineNumberConversion(TypeSymbol destination, ref HashSet<DiagnosticInfo> useSiteDiagnostics)
        {
            var greenNode = new Syntax.InternalSyntax.LiteralExpressionSyntax(SyntaxKind.NumericLiteralExpression, new Syntax.InternalSyntax.SyntaxToken(SyntaxKind.NumericLiteralToken));
            var syntaxNode = new LiteralExpressionSyntax(greenNode, null, 0);

            TypeSymbol expectedAttributeType = corLibrary.GetSpecialType(SpecialType.System_Int32);
            BoundLiteral intMaxValueLiteral = new BoundLiteral(syntaxNode, ConstantValue.Create(int.MaxValue), expectedAttributeType);
            return ClassifyStandardImplicitConversion(intMaxValueLiteral, expectedAttributeType, destination, ref useSiteDiagnostics);
        }

        internal bool HasCallerLineNumberConversion(TypeSymbol destination, ref HashSet<DiagnosticInfo> useSiteDiagnostics)
        {
            return GetCallerLineNumberConversion(destination, ref useSiteDiagnostics).Exists;
        }

        internal bool HasCallerInfoStringConversion(TypeSymbol destination, ref HashSet<DiagnosticInfo> useSiteDiagnostics)
        {
            TypeSymbol expectedAttributeType = corLibrary.GetSpecialType(SpecialType.System_String);
            Conversion conversion = ClassifyStandardImplicitConversion(expectedAttributeType, destination, ref useSiteDiagnostics);
            return conversion.Exists;
        }
    }

    internal sealed class Conversions : ConversionsBase
    {
        private readonly Binder _binder;

        public Conversions(Binder binder)
            : this(binder, currentRecursionDepth: 0)
        {
        }

        private Conversions(Binder binder, int currentRecursionDepth)
            : base(binder.Compilation.Assembly.CorLibrary, currentRecursionDepth)
        {
            _binder = binder;
        }

        protected override ConversionsBase CreateInstance(int currentRecursionDepth)
        {
            return new Conversions(_binder, currentRecursionDepth);
        }

        private CSharpCompilation Compilation { get { return _binder.Compilation; } }

        public override Conversion GetMethodGroupConversion(BoundMethodGroup source, TypeSymbol destination, ref HashSet<DiagnosticInfo> useSiteDiagnostics)
        {
            // Must be a bona fide delegate type, not an expression tree type.
            if (!destination.IsDelegateType())
            {
                return Conversion.NoConversion;
            }

            if (destination.Kind == SymbolKind.NonNullableReference)
            {
                destination = ((NonNullableReferenceTypeSymbol)destination).UnderlyingType;
            }

            var methodSymbol = GetDelegateInvokeMethodIfAvailable(destination);
            if ((object)methodSymbol == null)
            {
                return Conversion.NoConversion;
            }

            var resolution = ResolveDelegateMethodGroup(_binder, source, methodSymbol, ref useSiteDiagnostics);
            var conversion = (resolution.IsEmpty || resolution.HasAnyErrors) ?
                Conversion.NoConversion :
                ToConversion(resolution.OverloadResolutionResult, resolution.MethodGroup, (NamedTypeSymbol)destination);
            resolution.Free();
            return conversion;
=======
            return new Conversion(ConversionKind.ExplicitTupleLiteral, argumentConversions.ToImmutableAndFree());
>>>>>>> 865fef487a864b6fe69ab020e32218c87befdd00
        }

        protected override Conversion GetInterpolatedStringConversion(BoundInterpolatedString source, TypeSymbol destination, ref HashSet<DiagnosticInfo> useSiteDiagnostics)
        {
            // An interpolated string expression may be converted to the types
            // System.IFormattable and System.FormattableString
            return (destination == Compilation.GetWellKnownType(WellKnownType.System_IFormattable) ||
                    destination == Compilation.GetWellKnownType(WellKnownType.System_FormattableString))
                ? Conversion.InterpolatedString : Conversion.NoConversion;
        }

        /// <summary>
        /// Resolve method group based on the optional delegate invoke method.
        /// If the invoke method is null, ignore arguments in resolution.
        /// </summary>
        private static MethodGroupResolution ResolveDelegateMethodGroup(Binder binder, BoundMethodGroup source, MethodSymbol delegateInvokeMethodOpt, ref HashSet<DiagnosticInfo> useSiteDiagnostics)
        {
            if ((object)delegateInvokeMethodOpt != null)
            {
                var analyzedArguments = new AnalyzedArguments();
                GetDelegateArguments(source.Syntax, analyzedArguments, delegateInvokeMethodOpt.Parameters, binder.Compilation);
                var resolution = binder.ResolveMethodGroup(source, analyzedArguments, isMethodGroupConversion: true, inferWithDynamic: true, useSiteDiagnostics: ref useSiteDiagnostics);
                return resolution;
            }
            else
            {
                return binder.ResolveMethodGroup(source, null, isMethodGroupConversion: true, useSiteDiagnostics: ref useSiteDiagnostics);
            }
        }

        /// <summary>
        /// Return the Invoke method symbol if the type is a delegate
        /// type and the Invoke method is available, otherwise null.
        /// </summary>
        private static MethodSymbol GetDelegateInvokeMethodIfAvailable(TypeSymbol type)
        {
            var delegateType = type.GetDelegateType();
            if ((object)delegateType == null)
            {
                return null;
            }

            MethodSymbol methodSymbol = delegateType.DelegateInvokeMethod;
            if ((object)methodSymbol == null || methodSymbol.HasUseSiteError)
            {
                return null;
            }

            return methodSymbol;
        }

        public static bool ReportDelegateMethodGroupDiagnostics(Binder binder, BoundMethodGroup expr, TypeSymbol targetType, DiagnosticBag diagnostics)
        {
            var invokeMethodOpt = GetDelegateInvokeMethodIfAvailable(targetType);
            HashSet<DiagnosticInfo> useSiteDiagnostics = null;
            var resolution = ResolveDelegateMethodGroup(binder, expr, invokeMethodOpt, ref useSiteDiagnostics);
            diagnostics.Add(expr.Syntax, useSiteDiagnostics);

            bool hasErrors = resolution.HasAnyErrors;

            diagnostics.AddRange(resolution.Diagnostics);

            // SPEC VIOLATION: Unfortunately, we cannot exactly implement the specification for
            // the scenario in which an extension method that extends a value type is converted
            // to a delegate. The code we generate that captures a delegate to a static method
            // that is "partially evaluated" with the bound-to-the-delegate first argument
            // requires that the first argument be of reference type.
            //
            // SPEC VIOLATION: Similarly, we cannot capture a method of Nullable<T>, because
            // boxing a Nullable<T> gives a T, not a boxed Nullable<T>.
            //
            // We give special error messages in these situations.

            if (resolution.MethodGroup != null)
            {
                var result = resolution.OverloadResolutionResult;
                if (result != null)
                {
                    if (result.Succeeded)
                    {
                        var method = result.BestResult.Member;
                        Debug.Assert((object)method != null);
                        if (resolution.MethodGroup.IsExtensionMethodGroup)
                        {
                            Debug.Assert(method.IsExtensionMethod);

                            var thisParameter = method.Parameters[0];
                            if (!thisParameter.Type.IsReferenceType)
                            {
                                // Extension method '{0}' defined on value type '{1}' cannot be used to create delegates
                                diagnostics.Add(
                                    ErrorCode.ERR_ValueTypeExtDelegate,
                                    expr.Syntax.Location,
                                    method,
                                    thisParameter.Type);
                                hasErrors = true;
                            }
                        }
                        else if (method.OriginalDefinition.ContainingType.SpecialType == SpecialType.System_Nullable_T && !method.IsOverride)
                        {
                            // CS1728: Cannot bind delegate to '{0}' because it is a member of 'System.Nullable<T>'
                            diagnostics.Add(
                                ErrorCode.ERR_DelegateOnNullable,
                                expr.Syntax.Location,
                                method);
                            hasErrors = true;
                        }
                    }
                    else if (!hasErrors &&
                            !resolution.IsEmpty &&
                            resolution.ResultKind == LookupResultKind.Viable)
                    {
                        var overloadDiagnostics = DiagnosticBag.GetInstance();

                        result.ReportDiagnostics(binder, expr.Syntax.Location, overloadDiagnostics,
                            expr.Name,
                            resolution.MethodGroup.Receiver, resolution.AnalyzedArguments, resolution.MethodGroup.Methods.ToImmutable(),
                            typeContainingConstructor: null, delegateTypeBeingInvoked: null, isMethodGroupConversion: true);

                        if (!overloadDiagnostics.IsEmptyWithoutResolution)
                        {
                            hasErrors = overloadDiagnostics.HasAnyErrors();
                            diagnostics.AddRange(overloadDiagnostics);
                        }

                        overloadDiagnostics.Free();
                    }
                }
            }

            resolution.Free();
            return hasErrors;
        }

        public Conversion MethodGroupConversion(SyntaxNode syntax, MethodGroup methodGroup, NamedTypeSymbol delegateType, ref HashSet<DiagnosticInfo> useSiteDiagnostics)
        {
            var analyzedArguments = AnalyzedArguments.GetInstance();
            var result = OverloadResolutionResult<MethodSymbol>.GetInstance();

            Debug.Assert((object)delegateType.DelegateInvokeMethod != null && !delegateType.DelegateInvokeMethod.HasUseSiteError,
                         "This method should only be called for valid delegate types");
            GetDelegateArguments(syntax, analyzedArguments, delegateType.DelegateInvokeMethod.Parameters, Compilation);
            _binder.OverloadResolution.MethodInvocationOverloadResolution(
                methodGroup.Methods, methodGroup.TypeArguments, analyzedArguments, result, ref useSiteDiagnostics, isMethodGroupConversion: true);
            var conversion = ToConversion(result, methodGroup, delegateType);

            analyzedArguments.Free();
            result.Free();
            return conversion;
        }

        public static void GetDelegateArguments(SyntaxNode syntax, AnalyzedArguments analyzedArguments, ImmutableArray<ParameterSymbol> delegateParameters, CSharpCompilation compilation)
        {
            foreach (var p in delegateParameters)
            {
                ParameterSymbol parameter = p;

                // In ExpressionBinder::BindGrpConversion, the native compiler substitutes object in place of dynamic.  This is
                // necessary because conversions from expressions of type dynamic always succeed, whereas conversions from the
                // type generally fail (modulo identity conversions).  This is not reflected in the C# 4 spec, but will be
                // incorporated going forward.  See DevDiv #742345 for additional details.
                // NOTE: Dev11 does a deep substitution (e.g. C<C<C<dynamic>>> -> C<C<C<object>>>), but that seems redundant.
                if (parameter.Type.IsDynamic())
                {
                    // If we don't have System.Object, then we'll get an error type, which will cause overload resolution to fail, 
                    // which will cause some error to be reported.  That's sufficient (i.e. no need to specifically report its absence here).
                    parameter = new SignatureOnlyParameterSymbol(
                        compilation.GetSpecialType(SpecialType.System_Object), parameter.CustomModifiers, parameter.RefCustomModifiers, parameter.IsParams, parameter.RefKind);
                }

                analyzedArguments.Arguments.Add(new BoundParameter(syntax, parameter) { WasCompilerGenerated = true });
                analyzedArguments.RefKinds.Add(parameter.RefKind);
            }
        }

        private static Conversion ToConversion(OverloadResolutionResult<MethodSymbol> result, MethodGroup methodGroup, NamedTypeSymbol delegateType)
        {
            // 6.6 An implicit conversion (6.1) exists from a method group (7.1) to a compatible
            // delegate type. Given a delegate type D and an expression E that is classified as
            // a method group, an implicit conversion exists from E to D if E contains at least
            // one method that is applicable in its normal form (7.5.3.1) to an argument list
            // constructed by use of the parameter types and modifiers of D...

            // SPEC VIOLATION: Unfortunately, we cannot exactly implement the specification for
            // the scenario in which an extension method that extends a value type is converted
            // to a delegate. The code we generate that captures a delegate to a static method
            // that is "partially evaluated" with the bound-to-the-delegate first argument
            // requires that the first argument be of reference type.

            // SPEC VIOLATION: Similarly, we cannot capture a method of Nullable<T>, because
            // boxing a Nullable<T> gives a T, not a boxed Nullable<T>. (We can capture methods
            // of object on a nullable receiver, but not GetValueOrDefault.)

            if (!result.Succeeded)
            {
                return Conversion.NoConversion;
            }

            MethodSymbol method = result.BestResult.Member;

            if (methodGroup.IsExtensionMethodGroup && !method.Parameters[0].Type.IsReferenceType)
            {
                return Conversion.NoConversion;
            }

            if (method.OriginalDefinition.ContainingType.SpecialType == SpecialType.System_Nullable_T &&
                !method.IsOverride)
            {
                return Conversion.NoConversion;
            }

            // NOTE: Section 6.6 will be slightly updated:
            //
            //   - The candidate methods considered are only those methods that are applicable in their
            //     normal form (§7.5.3.1), and do not omit any optional parameters. Thus, candidate methods
            //     are ignored if they are applicable only in their expanded form, or if one or more of their
            //     optional parameters do not have a corresponding parameter in the targeted delegate type.
            //   
            // Therefore, we shouldn't get here unless the parameter count matches.

            // NOTE: Delegate type compatibility is important, but is not part of the existence check.

            Debug.Assert(method.ParameterCount == delegateType.DelegateInvokeMethod.ParameterCount + (methodGroup.IsExtensionMethodGroup ? 1 : 0));

            return new Conversion(ConversionKind.MethodGroup, method, methodGroup.IsExtensionMethodGroup);
        }
    }
}
