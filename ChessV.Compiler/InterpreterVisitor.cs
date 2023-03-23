
/***************************************************************************

                                 ChessV

                  COPYRIGHT (C) 2012-2019 BY GREG STRONG

This file is part of ChessV.  ChessV is free software; you can redistribute
it and/or modify it under the terms of the GNU General Public License as 
published by the Free Software Foundation, either version 3 of the License, 
or (at your option) any later version.

ChessV is distributed in the hope that it will be useful, but WITHOUT ANY 
WARRANTY; without even the implied warranty of MERCHANTABILITY or 
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for 
more details; the file 'COPYING' contains the License text, but if for
some reason you need a copy, please visit <http://www.gnu.org/licenses/>.

****************************************************************************/

using System;
using System.Collections.Generic;
using System.Reflection;
using Antlr4.Runtime.Misc;
using System.Linq.Expressions;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

namespace ChessV.Compiler
{
	public class InterpreterVisitor: ChessVCBaseVisitor<object>
	{
		public InterpreterVisitor( Environment environment, ExObject hostObject )
		{
			env = environment;
			obj = hostObject;
		}

		public object Parse( ParserRuleContext context )
		{
			locals = new Dictionary<string, Reference>();
			base.VisitBlock( (ChessVCParser.BlockContext) context );
			locals = null;
			return null;
		}


		#region VisitIfStatement
		public override object VisitIfStatement( [NotNull] ChessVCParser.IfStatementContext context )
		{
			object conditional = deref( Visit( context.expression() ) );
			if( !(conditional is bool) )
				throw new Exception( "if-statement conditional not boolean: " + conditional.ToString() );
			if( (bool) conditional )
				return Visit( context.ifBody( 0 ) );
			else if( context.el != null )
				return Visit( context.ifBody( 1 ) );
			// the if is false and there is no else so return null
			return null;
		}
		#endregion

		#region VisitVariableDeclaration
		public override object VisitVariableDeclaration( [NotNull] ChessVCParser.VariableDeclarationContext context )
		{
			object expr = Visit( context.expression() );
			if( context.t.Text == "var" )
			{
				if( expr is Reference )
					expr = (expr as Reference).GetValue();
				obj.SetCustomProperty( (string) Visit( context.identifier() ), expr );
			}
			else
			{
				if( expr is Reference )
					locals.Add( (string) Visit( context.identifier() ), (Reference) expr );
				else
					locals.Add( (string) Visit( context.identifier() ), new Reference( expr ) );
			}
			return expr;
		}
		#endregion


		#region VisitFnCallExp
		public override object VisitFnCallExp( [NotNull] ChessVCParser.FnCallExpContext context )
		{
			string s = context.GetText();
			object fnobj = Visit( context.postfixExpr() );
			if( fnobj is Reference )
			{
				Reference fnref = (Reference) fnobj;
				if( fnref.IsFunction )
				{
					//	load all the function arguments
					var argListCtx = context.argumentList();
					object[] arglist = argListCtx == null ? new Reference[] { } : (object[]) Visit( context.argumentList() );

					//	case of non-overloaded function
					if( fnref.FunctionMember != null )
					{
						ParameterInfo[] pi = fnref.FunctionMember.GetParameters();
						int calledFunctionArgCount = fnref.FunctionMember.GetParameters().Length;
						if( calledFunctionArgCount != arglist.Length )
						{
							//	we didn't supply all the arguments, but maybe the rest are optional
							bool allRemainingArgsAreOptional = false;
							if( arglist.Length < calledFunctionArgCount )
							{
								allRemainingArgsAreOptional = true;
								for( int x = arglist.Length; x < calledFunctionArgCount; x++ )
									if( !fnref.FunctionMember.GetParameters()[x].IsOptional )
										allRemainingArgsAreOptional = false;
							}
							if( !allRemainingArgsAreOptional )
								throw new Exception( "Incorrect number of arguments specified for function: " + context.postfixExpr().GetText() );
						}
						if( !canMatchSignature( fnref.FunctionMember.GetParameters(), arglist ) )
						{
							//	before we decide we can't call this function, check to see if 
							//	there is only one argument, that argument is a type, that type 
							//	has a constructor taking no arguments, and if, after construction, 
							//	the object is of the required type.  If all this pans out, 
							//	we'll just construct a new object of this type.
							if( arglist.Length == 1 && arglist[0].GetType() is Type &&
								((Type) arglist[0]).IsSubclassOf( fnref.FunctionMember.GetParameters()[0].ParameterType ) )
							{
								Type t = (Type) arglist[0];
								ConstructorInfo ci = t.GetConstructor( new Type[] { } );
								if( ci != null )
								{
									object[] arg = new object[1];
									arg[0] = ci.Invoke( null );
									return fnref.GetValue( arg );
								}
							}
							throw new Exception( "Arguments of incorrect type provided for function: " + context.postfixExpr().GetText() );
						}
						object[] args = new object[calledFunctionArgCount];
						for( int x = 0; x < arglist.Length; x++ )
							args[x] = arglist[x];
						for( int y = arglist.Length; y < calledFunctionArgCount; y++ )
							args[y] = Type.Missing;
						return fnref.GetValue( args );
					}
					else if( fnref.FunctionOverloads != null )
					{
						//	we have overloaded functions - search for one that works
						foreach( MethodInfo mi in fnref.FunctionOverloads )
						{
							if( canMatchSignature( mi.GetParameters(), arglist ) )
							{
								ParameterInfo[] parameters = mi.GetParameters();
								object[] args = new object[parameters.Length];
								for( int x = 0; x < parameters.Length; x++ )
									args[x] = x < arglist.Length ? arglist[x] : Type.Missing;
								return mi.Invoke( fnref.ReferencedObject, args );
							}
						}
						throw new Exception( "No overloaded found with compatible arguments for function: " + context.postfixExpr().GetText() );
					}
				}
				else if( fnref.ReferencedObject.GetType().IsSubclassOf( typeof(Type) ) )
				{
					//	load the constructor arguments
					var argListCtx = context.argumentList();
					object[] arglist = argListCtx == null ? new Reference[] { } : (object[]) Visit( context.argumentList() );
					//	our function call is really calling a constructor
					Type tp = (Type) fnref.ReferencedObject;
					foreach( ConstructorInfo ci in tp.GetConstructors() )
					{
						if( canMatchSignature( ci.GetParameters(), arglist ) )
						{
							ParameterInfo[] parameters = ci.GetParameters();
							object[] args = new object[parameters.Length];
							for( int x = 0; x < parameters.Length; x++ )
								args[x] = x < arglist.Length ? arglist[x] : Type.Missing;
							return ci.Invoke( args );
						}
					}
					throw new Exception( "No constructor found with compatible arguments for type: " + context.postfixExpr().GetText() );

				}
			}
			throw new Exception( "Function call attempted with object that is not a function: " + context.postfixExpr().GetText() );
		}
		#endregion

		#region VisitArgumentList
		public override object VisitArgumentList( [NotNull] ChessVCParser.ArgumentListContext context )
		{
			var argContexts = context.argument();
			object[] argValues = new object[argContexts.Length];
			for( int x = 0; x < argContexts.Length; x++ )
			{
				argValues[x] = Visit( argContexts[x] );
				if( argValues[x] is Reference )
					argValues[x] = ((Reference) argValues[x]).GetValue();
			}
			return argValues;
		}
		#endregion

		#region VisitMemberAccExp
		public override object VisitMemberAccExp( [NotNull] ChessVCParser.MemberAccExpContext context )
		{
			object lhs = Visit( context.postfixExpr() );
			if( lhs is Reference )
				lhs = ((Reference) lhs).GetValue();
			string membername = (string) Visit( context.identifier() );
			return identifierLookup( membername, lhs );
		}
		#endregion

		#region VisitUnaryExpr
		public override object VisitUnaryExpr( [NotNull] ChessVCParser.UnaryExprContext context )
		{
			object o = context.uop;
			return base.VisitUnaryExpr( context );
		}
		#endregion

		#region VisitEqualityExpr
		public override object VisitEqualityExpr( [NotNull] ChessVCParser.EqualityExprContext context )
		{
			if( context.bop != null )
			{
				object lhs = deref( Visit( context.equalityExpr() ) );
				object rhs = deref( Visit( context.relationalExpr() ) );
				if( lhs is ChoiceVariable )
					lhs = ((ChoiceVariable) lhs).Value;
				if( lhs is string && rhs is string )
					return context.bop.Text == "==" ? (string) lhs == (string) rhs : (string) lhs != (string) rhs;
				throw new Exception( "Unsupported equality comparison" );
			}
			return Visit( context.relationalExpr() );
		}
		#endregion

		#region VisitAssignmentExpr
		public override object VisitAssignmentExpr( [NotNull] ChessVCParser.AssignmentExprContext context )
		{
			string s = context.GetText();
			var rhsNode = context.assignmentExpr();
			if( rhsNode != null )
			{
				object lhs = Visit( context.conditionalExpr() );
				if( lhs == null || !(lhs is Reference) )
					throw new Exception( "Attempt to assign to object without l-value" );
				object rhs = deref( Visit( rhsNode ) );
				Type assignType = ((Reference) lhs).EffectiveType;
				if( rhs.GetType() != assignType && !rhs.GetType().IsSubclassOf( assignType ) )
				{
					//	First check - if we're assigning a string to a ChoiceVariable, we 
					//	will automatically assign it to it's Value member. This makes cleaner 
					//	code as we don't need to put ".Value" all over the place.
					if( assignType == typeof(ChoiceVariable) && rhs is string )
						return (((ChoiceVariable) (((Reference) lhs).GetValue())).Value = (string) rhs);

					//	We can't directly assign = however, we may be able to construct a new object 
					//	if the type to be assigned contains a constructor with a single argument 
					//	of the type supplied on the right-hand-side.
					var constructors = assignType.GetConstructors();
					foreach( ConstructorInfo ci in constructors )
					{
						var parameters = ci.GetParameters();
						if( parameters.Length == 1 )
						{
							if( rhs.GetType() == parameters[0].ParameterType ||
								parameters[0].ParameterType.IsSubclassOf( assignType ) )
							{
								//	types compatible - we can perform constructon
								((Reference) lhs).SetValue( ci.Invoke( new object[] { rhs } ) );
								return lhs;
							}
						}
					}
					throw new Exception( "Incompatable types for assignment" );
				}
				((Reference) lhs).SetValue( rhs );
				return lhs;
			}
			return base.VisitAssignmentExpr( context );
		}
		#endregion

		#region VisitExprApplyAttribute
		public override object VisitExprApplyAttribute( [NotNull] ChessVCParser.ExprApplyAttributeContext context )
		{
			object o = Visit( context.expression() );
			string attrname = context.ATTRIBUTE().GetText().Substring( 1 );
			Reference attr = globalIdentifierLookup( attrname );
			if( attr == null )
			{
				attrname += "Attribute";
				attr = globalIdentifierLookup( attrname );
				if( attr == null )
					throw new Exception( "Attribute not defined: " + context.ATTRIBUTE().GetText() );
			}
			//	make sure object we found is of correct type
			if( !(attr.ReferencedObject is Type) || !(attr.ReferencedObject as Type).IsSubclassOf( typeof(Attribute) ) )
				throw new Exception( "Specified object is not an attribute: " + attrname );
			//	construct new object of attribute
			Type attrType = attr.ReferencedObject as Type;
			ConstructorInfo ci = attrType.GetConstructor( new Type[] { } );
			Attribute newAttr = (Attribute) ci.Invoke( null );
			if( o is ExObject )
				(o as ExObject).AddAttribute( newAttr );
			else if( o is Reference )
			{
				Reference r = (Reference) o;
				if( r.EffectiveType == typeof(ExObject) || r.EffectiveType.IsSubclassOf( typeof(ExObject) ) )
					((ExObject) r.GetValue()).AddAttribute( newAttr );
				else if( r.EffectiveType == typeof(System.Type) || r.EffectiveType.IsSubclassOf( typeof(System.Type) ) )
				{
					//	if the attribute is applied to a PieceType class (rather than an instance) then 
					//	we will try to construct an instance and apply the attribute to that.
					Type type = (Type) r.GetValue();
					if( type.IsSubclassOf( typeof(PieceType) ) )
					{
						ConstructorInfo ci2 = type.GetConstructor( new Type[] { typeof(string), typeof(string), typeof(int), typeof(int), typeof(string) } );
						if( ci2 != null )
						{
							PieceType pt = (PieceType) ci2.Invoke( new object[] { null, null, 0, 0, null } );
							pt.AddAttribute( newAttr );
							//	if the attribute is a PieceTypePropertyAttribute, we will apply it 
							//	to the move capabilities now.  this is a temporary object and the 
							//	user is exepecting this to take effect (likely to pass this type 
							//	to the AddMoveOf function.)
							if( newAttr is PieceTypePropertyAttribute )
								((PieceTypePropertyAttribute) newAttr).AdjustMovement( pt );
							return pt;
						}
					}
				}
			}
			else
				throw new Exception( "Object of this type cannot be decorated with an attribute" );
			return o;
		}
		#endregion



		#region VisitLiteralExp
		public override object VisitLiteralExp( [NotNull] ChessVCParser.LiteralExpContext context )
		{
			return base.VisitLiteralExp( context );
		}
		#endregion

		#region VisitSimpleNameExp
		public override object VisitSimpleNameExp( [NotNull] ChessVCParser.SimpleNameExpContext context )
		{
			string name = (string) Visit( context.identifier() );
			return identifierLookup( name, obj ) ?? globalIdentifierLookup( name );
		}
		#endregion

		#region VisitIdentifier
		public override object VisitIdentifier( [NotNull] ChessVCParser.IdentifierContext context )
		{
			string s = context.IDENTIFIER().GetText();
			if( s.Length >= 2 && s[0] == '\'' )
				s = s.Substring( 1, s.Length - 2 );
			return s;
		}
		#endregion


		// *** LITERALS *** //

		#region ConstBoolFalse
		public override object VisitConstBoolFalse( [NotNull] ChessVCParser.ConstBoolFalseContext context )
		{
			return false;
		}
		#endregion

		#region ConstBoolTrue
		public override object VisitConstBoolTrue( [NotNull] ChessVCParser.ConstBoolTrueContext context )
		{
			return true;
		}
		#endregion

		#region ConstInt
		public override object VisitConstInt( [NotNull] ChessVCParser.ConstIntContext context )
		{
			return Convert.ToInt32( context.INTEGER().GetText() );
		}
		#endregion

		#region ConstStrg
		public override object VisitConstStrg( [NotNull] ChessVCParser.ConstStrgContext context )
		{
			string s = context.STRING().GetText();
			//	trim the quotation marks from the end
			s = s.Substring( 1, s.Length - 2 );
			//	unescape embedded backslashes
			s = s.Replace( @"\\", @"\" );
			//	unescape embedded quotes
			s = s.Replace( "\\\"", "\"" );
			return s;
		}
		#endregion

		#region ConstChar
		public override object VisitConstChar( [NotNull] ChessVCParser.ConstCharContext context )
		{
			return context.CHAR().GetText()[1];
		}
		#endregion

		#region ConstSymmetry
		public override object VisitConstSymmetry( [NotNull] ChessVCParser.ConstSymmetryContext context )
		{
			string txt = context.GetText();
			if( txt == "MirrorSymmetry" )
				return new MirrorSymmetry();
			else if( txt == "RotationalSymmetry" )
				return new RotationalSymmetry();
			else if( txt == "NoSymmetry" )
				return new NoSymmetry();
			throw new Exception( "!" );
		}
		#endregion

		#region ConstNull
		public override object VisitConstNull( [NotNull] ChessVCParser.ConstNullContext context )
		{
			return null;
		}
		#endregion

		#region VisitConstDir
		public override object VisitConstDir( [NotNull] ChessVCParser.ConstDirContext context )
		{
			int rankOffset = Convert.ToInt32( context.INTEGER( 0 ).GetText() );
			int fileOffset = Convert.ToInt32( context.INTEGER( 1 ).GetText() );
			if( context.m1 != null )
				rankOffset = -rankOffset;
			if( context.m2 != null )
				fileOffset = -fileOffset;
			return new Direction( rankOffset, fileOffset );
		}
		#endregion

		#region VisitConstList
		public override object VisitConstList( [NotNull] ChessVCParser.ConstListContext context )
		{
			var expressions = context.expression();
			if( expressions.Length == 0 )
				return new List<string>();
			object first = Visit( expressions[0] );
			if( first is Reference )
				first = (first as Reference).GetValue();
			if( first is string )
				return buildListOfType<string>( (string) first, expressions );
			if( first is int )
				return buildListOfType<int>( (int) first, expressions );
			if( first is char )
				return buildListOfType<char>( (char) first, expressions );
			if( first is Direction )
				return buildListOfType<Direction>( (Direction) first, expressions );
			if( first is PieceType )
				return buildListOfType<PieceType>( (PieceType) first, expressions );
			throw new Exception( "Unsupported type in literal list" );
		}
		#endregion

		#region VisitConstLambda
		public override object VisitConstLambda( [NotNull] ChessVCParser.ConstLambdaContext context )
		{
			lambdavar = (string) Visit( context.identifier() );
			lambdaParam1 = Expression.Parameter( typeof(Location), lambdavar );
			object o = Visit( context.lambdaexpr() );
			Expression exp = (Expression) o;
			var l = Expression.Lambda<Func<Location, bool>>( exp, new[] { lambdaParam1 } );
			var func = l.Compile();
			return (ConditionalLocationDelegate) (location => func( location ));
		}
		#endregion


		// *** LAMBDA EXPRESSIONS *** //

		#region VisitLambdaDot
		public override object VisitLambdaDot( [NotNull] ChessVCParser.LambdaDotContext context )
		{
			string leftid = (string) Visit( context.identifier( 0 ) );
			string rightid = (string) Visit( context.identifier( 1 ) );
			if( leftid != lambdavar )
				throw new Exception( "Unexpected member access in lambda expression" );
			MemberExpression me = Expression.Property( lambdaParam1, rightid );
			return me;
		}
		#endregion

		#region VisitLambdaID
		public override object VisitLambdaID( [NotNull] ChessVCParser.LambdaIDContext context )
		{
			throw new Exception( "Unexpected ID in lambda expresssion" );
		}
		#endregion

		#region VisitLambdaConstBool
		public override object VisitLambdaConstBool( [NotNull] ChessVCParser.LambdaConstBoolContext context )
		{
			return Expression.Constant( context.c.Text == "true" ? true : false );
		}
		#endregion

		#region VisitLambdaConstInt
		public override object VisitLambdaConstInt( [NotNull] ChessVCParser.LambdaConstIntContext context )
		{
			return Expression.Constant( Convert.ToInt32( context.INTEGER().GetText() ) );
		}
		#endregion

		#region VisitLambdaEqualtiy
		public override object VisitLambdaEqualtiy( [NotNull] ChessVCParser.LambdaEqualtiyContext context )
		{
			if( context.bop.Text == "==" )
				return Expression.Equal( (Expression) Visit( context.lambdaexpr( 0 ) ), (Expression) Visit( context.lambdaexpr( 1 ) ) );
			else
				return Expression.NotEqual( (Expression) Visit( context.lambdaexpr( 0 ) ), (Expression) Visit( context.lambdaexpr( 1 ) ) );
		}
		#endregion

		#region VisitLambdaCompare
		public override object VisitLambdaCompare( [NotNull] ChessVCParser.LambdaCompareContext context )
		{
			if( context.bop.Text == "<" )
				return Expression.LessThan( (Expression) Visit( context.lambdaexpr( 0 ) ), (Expression) Visit( context.lambdaexpr( 1 ) ) );
			else if( context.bop.Text == "<=" )
				return Expression.LessThanOrEqual( (Expression) Visit( context.lambdaexpr( 0 ) ), (Expression) Visit( context.lambdaexpr( 1 ) ) );
			else if( context.bop.Text == ">" )
				return Expression.GreaterThan( (Expression) Visit( context.lambdaexpr( 0 ) ), (Expression) Visit( context.lambdaexpr( 1 ) ) );
			else
				return Expression.GreaterThanOrEqual( (Expression) Visit( context.lambdaexpr( 0 ) ), (Expression) Visit( context.lambdaexpr( 1 ) ) );
		}
		#endregion

		#region VisitLambdaOr
		public override object VisitLambdaOr( [NotNull] ChessVCParser.LambdaOrContext context )
		{
			return Expression.Or( (Expression) Visit( context.lambdaexpr( 0 ) ), (Expression) Visit( context.lambdaexpr( 1 ) ) );
		}
		#endregion

		#region VisitLambdaAnd
		public override object VisitLambdaAnd( [NotNull] ChessVCParser.LambdaAndContext context )
		{
			return Expression.And( (Expression) Visit( context.lambdaexpr( 0 ) ), (Expression) Visit( context.lambdaexpr( 1 ) ) );
		}
		#endregion


		// *** HELPER FUNCTIONS *** //

		#region identifierLookup
		protected Reference identifierLookup( string name, object scope )
		{
			//	determine type of scope object (which may be an actual type)
			bool scopeIsType = scope is Type;
			Type scopeType = scopeIsType ? (Type) scope : scope.GetType();
			//	See if scope object has member with this name - first check static
			MemberInfo[] membersWithName = scopeType.GetMember( name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic );
			if( membersWithName.Length > 1 )
				throw new Exception( "Ambiguous member name: " + name );
			if( membersWithName.Length == 1 )
			{
				MemberInfo mi = membersWithName[0];
				if( mi.MemberType == MemberTypes.Field )
					return new Reference( scope, (FieldInfo) mi, true );
				else if( mi.MemberType == MemberTypes.Property )
					return new Reference( scope, (PropertyInfo) mi, true );
				else if( mi.MemberType == MemberTypes.Method )
					return new Reference( scope, (MethodInfo) mi, true );
			}
			//	See if scope object has member with this name - check instance members
			membersWithName = scopeType.GetMember( name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
			if( membersWithName.Length == 1 )
			{
				MemberInfo mi = membersWithName[0];
				if( mi.MemberType == MemberTypes.Field )
				{
					FieldInfo fi = (FieldInfo) mi;
					//	Don't return members that are PieceTypes unless it is a GameVariable.
					//	Active PieceTypes are identified through a more advanced mechanism.
					if( (fi.FieldType != typeof(PieceType) && !fi.FieldType.IsSubclassOf( typeof(PieceType) )) || 
						fi.GetCustomAttribute( typeof(GameVariableAttribute) ) != null )
						return new Reference( scope, (FieldInfo) mi, false );
				}
				else if( mi.MemberType == MemberTypes.Property )
				{
					PropertyInfo pi = (PropertyInfo) mi;
					//	Don't return members that are PieceTypes unless it is a GameVariable.
					//	Active PieceTypes are identified through a more advanced mechanism.
					if( (pi.PropertyType != typeof(PieceType) && !pi.PropertyType.IsSubclassOf( typeof(PieceType) )) ||
						pi.GetCustomAttribute( typeof(GameVariableAttribute) ) != null )
						return new Reference( scope, (PropertyInfo) mi, false );
				}
				else if( mi.MemberType == MemberTypes.Method )
					return new Reference( scope, (MethodInfo) mi, false );
			}
			else if( membersWithName.Length > 1 )
			{
				MethodInfo[] overloads = new MethodInfo[membersWithName.Length];
				for( int x = 0; x < membersWithName.Length; x++ )
					overloads[x] = (MethodInfo) membersWithName[x];
				return new Reference( scope, overloads );
			}
			//	Next, check the custom properties of the ExObject
			object customProperty = scope is ExObject ? ((ExObject) scope).GetCustomProperty( name ) : null;
			if( customProperty != null )
				return new Reference( customProperty );
			return null;
		}
		#endregion

		#region globalIdentifierLookup
		public Reference globalIdentifierLookup( string name )
		{
			//	first, check the locals
			if( locals.ContainsKey( name ) )
				return locals[name];

			//	check the environment and, recursively, parent environments
			Environment environment = env;
			while( environment != null )
			{
				object o = environment.LookupSymbol( name );
				if( o != null )
					return new Reference( o );
				environment = environment.ParentEnvironment;
			}
			return null;
		}
		#endregion

		#region canMatchSignature
		protected bool canMatchSignature( ParameterInfo[] parameters, object[] arglist )
		{
			int requiredParams = parameters.Length;
			for( int x = requiredParams - 1; x >= 0; x-- )
			{
				if( (parameters[x].Attributes & ParameterAttributes.Optional) != 0 )
					requiredParams--;
				else
					break;
			}
			if( arglist.Length < requiredParams || arglist.Length > parameters.Length )
				return false;
			int nArgOfPieceTypeSwitch = -1;
			for( int x = 0; x < arglist.Length; x++ )
			{
				if( parameters[x].ParameterType != arglist[x].GetType() &&
					!arglist[x].GetType().IsSubclassOf( parameters[x].ParameterType ) )
				{
					Type paramtype = parameters[x].ParameterType;
					Type argtype = arglist[x].GetType();
					if( nArgOfPieceTypeSwitch >= 0 || 
						paramtype != typeof(System.Type) || 
						!argtype.IsSubclassOf( typeof(PieceType) ) )
						return false;
					//	cheesy work-around.  Our argument list doesn't seem to match but 
					//	we will allow the option for one auto-conversion to work around a 
					//	common case.  We often have a local identifier ("King") that refers 
					//	to a PieceType instance, but our function expects a runtime type 
					//	of a PieceType (represented by the global identifier "King", but 
					//	the local identifier hides the global.)  So we will catch that 
					//	case here.  This interpreted language is intended to be easy and 
					//	clean, so we add the occasional work-around at the cost of the 
					//	language being somewhat less predictable.
					PieceType argPieceType = (PieceType) arglist[x];
					Reference r = globalIdentifierLookup( argPieceType.Name );
					if( r.EffectiveType != typeof(System.Type) && 
						!r.EffectiveType.IsSubclassOf( typeof(System.Type) ) )
						return false;
					//	we will allow this one conversion if everything else matches
					nArgOfPieceTypeSwitch = x;
				}
			}
			//	we match, but do we need to perform the piece type conversion?
			if( nArgOfPieceTypeSwitch >= 0 )
				arglist[nArgOfPieceTypeSwitch] = 
					globalIdentifierLookup( ((PieceType) arglist[nArgOfPieceTypeSwitch]).Name ).GetValue();
			return true;
		}
		#endregion

		#region buildListOfType<T>
		protected object buildListOfType<T>( T firstElement, ChessVCParser.ExpressionContext[] expressions )
		{
			List<T> rtn = new List<T>( expressions.Length );
			rtn.Add( (T) firstElement );
			for( int x = 1; x < expressions.Length; x++ )
			{
				object next = Visit( expressions[x] );
				if( next is Reference )
					next = (next as Reference).GetValue();
				if( !(next is T) )
					throw new Exception( "mismatched data types in list" );
				rtn.Add( (T) next );
			}
			return rtn;
		}
		#endregion

		#region deref
		protected object deref( object o )
		{
			return o is Reference ? ((Reference) o).GetValue() : o;
		}
		#endregion



		// *** HELPER DATA *** //

		protected Environment env;
		protected ExObject obj;
		protected string lambdavar;
		protected ParameterExpression lambdaParam1;
		protected Dictionary<string, Reference> locals;
	}
}
