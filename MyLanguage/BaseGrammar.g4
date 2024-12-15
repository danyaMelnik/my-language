grammar BaseGrammar;

// начальное правило грамматики
program: (statement)* EOF;

// список возможных операторов
statement: variableDeclaration
	 | constDeclaration
         | assignment
         | functionDefinition
         | functionCall
         | println
         | return
         | ifStatement
         | blockStatement
	 | whileStatement
         ;

// список возможных выражений
expression: '(' expression ')'                                      #parenthesisExpr
          | left=expression op=(ASTERISK | SLASH) right=expression  #mulDivExpr
          | left=expression op=(PLUS | MINUS) right=expression      #plusMinusExpr
          | left=expression compOperator right=expression           #compExpr
          | IDENTIFIER                                              #idExp
          | NUMBER                                                  #numExpr
          | DOUBLE_NUMBER                                           #doubleExpr
          | STRING_LITERAL                                          #stringExpr
          | functionCall                                            #funcCallExpr
	      | BOOL						                            #boolExpr
          ;

// описания отдельных выражений и операторов
variableDeclaration: 'var' IDENTIFIER '=' expression ;

constDeclaration: 'const' IDENTIFIER '=' expression ;

assignment: IDENTIFIER '=' expression ;

compOperator: op=(LESS | LESS_OR_EQUAL | EQUAL | NOT_EQUAL | GREATER | GREATER_OR_EQUAL) ;

println: 'println' expression ;

return: 'return' expression ;

blockStatement: '{' (statement)* '}' ;

functionCall: IDENTIFIER '(' (expression (',' expression)*)? ')' ;

functionDefinition: 'fun' name=IDENTIFIER '(' (IDENTIFIER (',' IDENTIFIER)*)? ')' '{' (statement)* '}' ;

ifStatement: 'if' '(' expression ')' statement (elifStatement)*  elseStatement? ;

elifStatement: 'elif' '(' expression ')' statement;

elseStatement: 'else' statement ;

whileStatement: 'while' '(' expression ')' statement ;

// список токенов
IDENTIFIER          : [a-zA-Z_] [a-zA-Z_0-9]* ;
NUMBER              : [0-9]+ ;
DOUBLE_NUMBER       : NUMBER '.' NUMBER ;
STRING_LITERAL      : '"' (~["])* '"' ;
BOOL		    : 'True'|'False';
ASTERISK            : '*' ;
SLASH               : '/' ;
PLUS                : '+' ;
MINUS               : '-' ;

ASSIGN              : '=' ;
EQUAL               : '==' ;
NOT_EQUAL           : '!=' ;
LESS                : '<' ;
LESS_OR_EQUAL       : '<=' ;
GREATER             : '>' ;
GREATER_OR_EQUAL    : '>=' ;

SPACE               : [ \r\n\t]+ -> skip;
LINE_COMMENT        : '//' ~[\n\r]* -> skip;
MULTI_LINE_COMMENT  : '/*' .*? '*/' -> skip;