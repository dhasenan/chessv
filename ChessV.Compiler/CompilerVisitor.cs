
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
using Antlr4.Runtime.Misc;

namespace ChessV.Compiler
{
	public class CompilerVisitor: ChessVCBaseVisitor<object>
	{
		public CompilerVisitor( Compiler compiler )
		{
			this.compiler = compiler;
		}

		public override object VisitPieceTypeDeclaration( [NotNull] ChessVCParser.PieceTypeDeclarationContext context )
		{
			string pieceName = getObjectName( context.identifier().GetText() );
			partial = new PartialDefinition( pieceName );
			var returnval = base.VisitPieceTypeDeclaration( context );
			compiler.AddPieceType( partial );
			return returnval;
		}

		public override object VisitGameDeclaration( [NotNull] ChessVCParser.GameDeclarationContext context )
		{
			string gameName = getObjectName( context.identifier( 0 ).GetText() );
			string baseGameName = getObjectName( context.identifier( 1 ).GetText() );
			partial = new PartialDefinition( gameName, baseGameName );
			var returnval = base.VisitGameDeclaration( context );
			compiler.AddGame( partial );
			return returnval;
		}

		public override object VisitMemberDefn( ChessVCParser.MemberDefnContext context )
		{
			string typename = context.predefinedType().GetText();
			string variableName = context.identifier().GetText();
			Type variableType = null;
			switch( typename )
			{
				case "Choice":
					variableType = typeof(ChoiceVariable);
					break;

				case "IntRange":
					variableType = typeof(IntRangeVariable);
					break;

				case "String":
					variableType = typeof(string);
					break;

				case "Bool":
					variableType = typeof( Boolean );
					break;

				case "PieceType":
					variableType = typeof(PieceType);
					break;
			}
			if( variableType == null )
				throw new Exception( "Game variable declared of unsupported type: " + typename );
			partial.AddMemberVariableDeclaration( variableName, variableType );
			return base.VisitMemberDefn( context );
		}

		public override object VisitFunctionDefn( ChessVCParser.FunctionDefnContext context )
		{
			string functionName = getObjectName( context.identifier().GetText() );
			if( partial != null )
				partial.AddFunctionDeclaration( functionName, context.block() );
			return base.VisitFunctionDefn( context );
		}

		public override object VisitConstructorAssign( [NotNull] ChessVCParser.ConstructorAssignContext context )
		{
			string memberName = getObjectName( context.identifier().GetText() );
			object value = Visit( context.literal() );
			partial.AddVariableAssignment( memberName, value );
			return base.VisitConstructorAssign( context );
		}

		public override object VisitConstBoolTrue( [NotNull] ChessVCParser.ConstBoolTrueContext context )
		{
			return true;
		}

		public override object VisitConstBoolFalse( [NotNull] ChessVCParser.ConstBoolFalseContext context )
		{
			return false;
		}

		public override object VisitConstChar( [NotNull] ChessVCParser.ConstCharContext context )
		{
			return context.CHAR().GetText().Substring( 1, 1 );
		}

		public override object VisitConstNull( [NotNull] ChessVCParser.ConstNullContext context )
		{
			return null;
		}

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

		public override object VisitConstInt( [NotNull] ChessVCParser.ConstIntContext context )
		{
			return Convert.ToInt32( context.INTEGER().GetText() );
		}

		public override object VisitConstSymmetry( [NotNull] ChessVCParser.ConstSymmetryContext context )
		{
			if( context.GetText() == "MirrorSymmetry" )
				return new MirrorSymmetry();
			else if( context.GetText() == "RotationalSymmetry" )
				return new RotationalSymmetry();
			else if( context.GetText() == "NoSymmetry" )
				return new NoSymmetry();
			return null;
		}

		protected string getObjectName( string name )
		{
			if( name.Length >= 2 && name[0] == '\'' )
				name = name.Substring( 1, name.Length - 2 );
			return name;
		}


		protected PartialDefinition partial;
		protected Compiler compiler;
	}
}
