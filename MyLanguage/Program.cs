using Antlr4.Runtime;
using MyLanguage;
using MyLanguage.Content;

var fileName = "D:\\Programs\\Unik\\language\\MyLanguage\\Content\\test.ss";
var fileContent = File.ReadAllText(fileName);

AntlrInputStream inputStream = new AntlrInputStream(fileContent);
var speakLexer = new BaseGrammarLexer(inputStream);
CommonTokenStream commonTokenStream = new CommonTokenStream(speakLexer);
var speakParser = new BaseGrammarParser(commonTokenStream);
var programContext = speakParser.program();
var visitor = new MyVisitor();

visitor.Visit(programContext);

