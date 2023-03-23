
lexer grammar ChessVCLexer;

LINE_COMMENT
	: '//' .*? '\r'? '\n' -> channel(HIDDEN)
	;
	
COMMENT
	: '/*' .*? '*/' -> channel(HIDDEN)
	;

WHITESPACE: [ \t\r\n]+ -> skip ; // skip spaces, tabs, newlines

NULL:        'null' ;
TRUE:        'true' ;
FALSE:       'false' ;
INT_T:       'Integer' ;
INTRANGE_T:  'IntRange' ;
BOOL_T:      'Bool' ;
STRING_T:    'String' ;
CHOICE_T:    'Choice' ;
PIECETYPE_T: 'PieceType' ;
GAME_T:      'Game' ;
IF:          'if' ;
ELSE:        'else' ;
RETURN:      'return' ;
VAR:         'var' ;

MIRROR_SYMMETRY:     'MirrorSymmetry';
ROTATIONAL_SYMMETRY: 'RotationalSymmetry';
NO_SYMMETRY:         'NoSymmetry';

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

SYMMETRY
	: MIRROR_SYMMETRY
	| ROTATIONAL_SYMMETRY
	| NO_SYMMETRY
	;
	


