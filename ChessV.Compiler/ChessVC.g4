
grammar ChessVC;

unit
	: declaration*
	;

declaration
	: gameDeclaration
	| pieceTypeDeclaration
	;
	
pieceTypeDeclaration
	: 'PieceType' identifier '{' declMember* '}'
	;
	
gameDeclaration
	: 'Game' identifier ':' identifier '{' declMember* '}'
	;
	
declMember
	: constructorAssign
	| functionDefn
	| memberDefn
	;
	
constructorAssign
	: identifier '=' literal ';'
	;

functionDefn
	: identifier block
	;

memberDefn
	: predefinedType identifier ';'
	;
	
lambdaprimary
	: identifier                                                    # lambdaID
	| c='true'                                                      # lambdaConstBool
	| c='false'                                                     # lambdaConstBool
	| INTEGER                                                       # lambdaConstInt
	;
	
lambdaexpr
	: lambdaprimary                                                 # lambdaPri
	| '(' lambdaexpr ')'                                            # lambdaParen
	| identifier '.' identifier                                     # lambdaDot
	| lambdaexpr bop=('+'|'-') lambdaexpr                           # lambdaAdd
	| lambdaexpr bop=('<'|'>'|'<='|'>=') lambdaexpr                 # lambdaCompare
	| lambdaexpr bop=('=='|'!=') lambdaexpr                         # lambdaEqualtiy
	| lambdaexpr '&&' lambdaexpr                                    # lambdaAnd
	| lambdaexpr '||' lambdaexpr                                    # lambdaOr
	;
	
simpleEmbeddedStatement
	: ';'                                                           # theEmptyStatement
	| expression ';'                                                # expressionStatement
	| 'if' '(' expression ')' ifBody (el='else' ifBody)?            # ifStatement
	| 'return' expression ';'                                       # returnStatement
	;

embeddedStatement
	: block
	| simpleEmbeddedStatement
	;
	
statement
	: variableDeclaration ';'
	| embeddedStatement
	;
	
statementList
	: statement+
	;
	
block
	: '{' statementList? '}'
	;
	
ifBody
	: block
	| simpleEmbeddedStatement
	;

variableDeclaration
	: t='var' identifier '=' expression
	| t='local' identifier '=' expression
	;

primaryExpr
	: literal                                                      # literalExp
	| identifier                                                   # simpleNameExp
	| '(' expression ')'                                           # parenthesisExp
	| predefinedType                                               # predefTypeExp
	;

postfixExpr
	: postfixExpr '[' expression ']'                               # indexExp
	| postfixExpr '(' argumentList? ')'                            # fnCallExp
	| postfixExpr '.' identifier                                   # memberAccExp
	| postfixExpr uop=('++' | '--')                                # postfixOpExp
	| primaryExpr                                                  # postfixExprPassthrough
	;
	
unaryExpr
	: postfixExpr
	| uop=('+' | '-' | '!') unaryExpr
	;
	
multiplicativeExpr
	: unaryExpr 
	| multiplicativeExpr bop=('*' | '/' | '%') unaryExpr
	;

additiveExpr
	: multiplicativeExpr
	| additiveExpr bop=('+' | '-') multiplicativeExpr
	;

relationalExpr
	: additiveExpr
	| relationalExpr bop=('<' | '>' | '<=' | '>=') additiveExpr
	;
	
equalityExpr
	: relationalExpr 
	| equalityExpr bop=('==' | '!=') relationalExpr
	;

conditionalAndExpr
	: equalityExpr
	| conditionalAndExpr '&&' equalityExpr
	;

conditionalOrExpr
	: conditionalAndExpr 
	| conditionalOrExpr '||' conditionalAndExpr
	;

conditionalExpr
	: conditionalOrExpr
	| conditionalOrExpr '?' expression ':' expression
	;

assignmentExpr
	: conditionalExpr
	| conditionalExpr bop=('=' | '+=' | '-=') assignmentExpr
	;
	
expression
	: assignmentExpr                                                            # exprPassthrough
	| expression ATTRIBUTE                                                      # exprApplyAttribute
	;

argumentList
	: argument (',' argument)*
	;
	
argument
	: expression
	;
	
predefinedType
	: 'Int'
	| 'IntRange'
	| 'String'
	| 'Bool'
	| 'Choice'
	;
	
identifier
	: IDENTIFIER
	;

literal
	: 'true'                                              # ConstBoolTrue
	| 'false'                                             # ConstBoolFalse
	| INTEGER                                             # ConstInt
	| STRING                                              # ConstStrg
	| CHAR                                                # ConstChar
	| INTEGER '..' INTEGER                                # ConstRange
	| '<' (m1='-')? INTEGER ',' (m2='-')? INTEGER '>'     # ConstDir
	| '{' (expression (',' expression)*)? '}'             # ConstList
	| '{' identifier ':' lambdaexpr '}'                   # ConstLambda
	| 'MirrorSymmetry'                                    # ConstSymmetry
	| 'RotationalSymmetry'                                # ConstSymmetry
	| 'NoSymmetry'                                        # ConstSymmetry
	| 'null'                                              # ConstNull
	;


LINE_COMMENT
	: '//' .*? '\r'? '\n' -> channel(HIDDEN)
	;
	
COMMENT
	: '/*' .*? '*/' -> channel(HIDDEN)
	;

WHITESPACE: [ \t\r\n]+ -> skip ; // skip spaces, tabs, newlines


fragment STR_ESC
	: '\\"' 
	| '\\\\'
	;

ATTRIBUTE
	: '@' [a-zA-Z_][a-zA-Z0-9_]*
	;
	
IDENTIFIER
	: [a-zA-Z_][a-zA-Z0-9_]*
	| '\'' (ID_ESC | .)*? '\''
	;

fragment ID_ESC
	: '\\\''
	| '\\\\'
	;

CHAR
	: '`' . '`'
	;
	
STRING
	: '"' (STR_ESC | .)*? '"'
	;
	
fragment DIGIT
	: [0-9]
	;

INTEGER
	: DIGIT+
	;
