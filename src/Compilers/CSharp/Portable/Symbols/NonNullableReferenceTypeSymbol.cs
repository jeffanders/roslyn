using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Utilities;
using System.Diagnostics;
using Microsoft.CodeAnalysis.Symbols;

namespace Microsoft.CodeAnalysis.CSharp.Symbols
{
    internal sealed partial class NonNullableReferenceTypeSymbol : TypeSymbol, INonNullableReferenceTypeSymbol
    {
        private readonly TypeSymbol _underlyingType;

        private NonNullableReferenceTypeSymbol(
            TypeSymbol underlyingType)
        {
            Debug.Assert((object)underlyingType != null);

            _underlyingType = underlyingType;
        }

        internal static NonNullableReferenceTypeSymbol CreateNonNullableReference(
            TypeSymbol underlyingType)
        {

            return new NonNullableReferenceTypeSymbol(underlyingType);
        }

        public TypeSymbol UnderlyingType
        {
            get
            {
                return _underlyingType;
            }
        }

        ITypeSymbol INonNullableReferenceTypeSymbol.UnderlyingType
        {
            get
            {
                return this.UnderlyingType;
            }
        }

        internal override bool Equals(TypeSymbol t2, TypeCompareKind compareKind = TypeCompareKind.ConsiderEverything)
        {
            if (t2.TypeKind != TypeKind.NonNullableReference)
                return false;
            return _underlyingType.Equals(((NonNullableReferenceTypeSymbol)t2).UnderlyingType, compareKind);
        }

        public override Symbol ContainingSymbol
        {
            get
            {
                return null;
            }
        }

        public override Accessibility DeclaredAccessibility
        {
            get
            {
                return Accessibility.NotApplicable;
            }
        }

        public override ImmutableArray<SyntaxReference> DeclaringSyntaxReferences
        {
            get
            {
                return ImmutableArray<SyntaxReference>.Empty;
            }
        }

        public override bool IsAbstract
        {
            get
            {
                return false;
            }
        }

        public override bool IsReferenceType
        {
            get
            {
                return true;
            }
        }

        public override bool IsSealed
        {
            get
            {
                return false;
            }
        }

        public override bool IsStatic
        {
            get
            {
                return false;
            }
        }

        public override bool IsValueType
        {
            get
            {
                return false;
            }
        }

        public override SymbolKind Kind
        {
            get
            {
                return SymbolKind.NonNullableReference;
            }
        }

        public override ImmutableArray<Location> Locations
        {
            get
            {
                return ImmutableArray<Location>.Empty;
            }
        }

        public override TypeKind TypeKind
        {
            get
            {
                return TypeKind.NonNullableReference;
            }
        }

        internal override NamedTypeSymbol BaseTypeNoUseSiteDiagnostics
        {
            get
            {
                return _underlyingType.BaseTypeNoUseSiteDiagnostics;
            }
        }

        internal override bool IsManagedType
        {
            get
            {
                return true;
            }
        }

        internal override ObsoleteAttributeData ObsoleteAttributeData
        {
            get
            {
                return null;
            }
        }

        public override void Accept(CSharpSymbolVisitor visitor)
        {
            visitor.VisitNonNullableReferenceType(this);
        }

        public override void Accept(SymbolVisitor visitor)
        {
            visitor.VisitNonNullableReferenceType(this);
        }

        public override TResult Accept<TResult>(CSharpSymbolVisitor<TResult> visitor)
        {
            return visitor.VisitNonNullableReferenceType(this);
        }

        public override TResult Accept<TResult>(SymbolVisitor<TResult> visitor)
        {
            return visitor.VisitNonNullableReferenceType(this);
        }

        public override ImmutableArray<Symbol> GetMembers()
        {
            return _underlyingType.GetMembers();
        }

        public override ImmutableArray<Symbol> GetMembers(string name)
        {
            return _underlyingType.GetMembers(name);
        }

        public override ImmutableArray<NamedTypeSymbol> GetTypeMembers()
        {
            return _underlyingType.GetTypeMembers();
        }

        public override ImmutableArray<NamedTypeSymbol> GetTypeMembers(string name)
        {
            return _underlyingType.GetTypeMembers(name);
        }

        internal override TResult Accept<TArgument, TResult>(CSharpSymbolVisitor<TArgument, TResult> visitor, TArgument a)
        {
            throw new NotImplementedException();
        }

        internal override bool GetUnificationUseSiteDiagnosticRecursive(ref DiagnosticInfo result, Symbol owner, ref HashSet<TypeSymbol> checkedTypes)
        {
            return _underlyingType.GetUnificationUseSiteDiagnosticRecursive(ref result, owner, ref checkedTypes);
        }

        internal override ImmutableArray<NamedTypeSymbol> InterfacesNoUseSiteDiagnostics(ConsList<Symbol> basesBeingResolved = null)
        {
            return _underlyingType.InterfacesNoUseSiteDiagnostics(basesBeingResolved);
        }
    }
}
