namespace MyLanguage.Content
{
    public class MyVisitor : BaseGrammarBaseVisitor<object>
    {
        private Dictionary<string, object> variables = new Dictionary<string, object>();

        public override object VisitVariableDeclaration(BaseGrammarParser.VariableDeclarationContext context)
        {
            var name = context.IDENTIFIER().GetText();
            var value = Visit(context.expression());
            variables[name] = value;

            return value;
        }

        public override object VisitConstDeclaration(BaseGrammarParser.ConstDeclarationContext context)
        {
            var name = context.IDENTIFIER().GetText();
            var value = Visit(context.expression());
            variables[name] = value;

            return value;
        }

        public override object VisitAssignment(BaseGrammarParser.AssignmentContext context)
        {
            var name = context.IDENTIFIER().GetText();
            var value = Visit(context.expression());
            variables[name] = value;

            return value;
        }

        public override object VisitMulDivExpr(BaseGrammarParser.MulDivExprContext context)
        {
            var left = Convert.ToDouble(Visit(context.left));
            var right = Convert.ToDouble(Visit(context.right));
            var op = context.op.Text;

            return op switch
            {
                "*" => left * right,
                "/" => left / right,
                _ => throw new InvalidOperationException($"Неизвестный оператор {op}")
            };
        }

        public override object VisitPlusMinusExpr(BaseGrammarParser.PlusMinusExprContext context)
        {
            var left = Convert.ToDouble(Visit(context.left));
            var right = Convert.ToDouble(Visit(context.right));
            var op = context.op.Text;

            return op switch
            {
                "+" => left + right,
                "-" => left - right,
                _ => throw new InvalidOperationException($"Неизвестный оператор {op}")
            };
        }

        public override object VisitCompExpr(BaseGrammarParser.CompExprContext context)
        {
            var left = Visit(context.left);
            var right = Visit(context.right);
            var op = context.compOperator().op.Text;

            return op switch
            {
                "<" => (double)left < (double)right,
                "<=" => (double)left <= (double)right,
                "==" => left.Equals(right),
                "!=" => !left.Equals(right),
                ">" => (double)left > (double)right,
                ">=" => (double)left >= (double)right,
                _ => throw new InvalidOperationException($"Неизвестный оператор {op}")
            };
        }

        public override object VisitLogicalExpr(BaseGrammarParser.LogicalExprContext context)
        {
            var left = Convert.ToBoolean(Visit(context.left));
            var right = Convert.ToBoolean(Visit(context.right));
            var op = context.op.Text;

            return op switch
            {
                "AND" => left && right,
                "OR" => left || right,
                _ => throw new InvalidOperationException($"Неизвестный оператор {op}")
            };
        }

        public override object VisitNotExpr(BaseGrammarParser.NotExprContext context)
        {
            var expr = Convert.ToBoolean(Visit(context.expression()));

            return !expr;
        }

        public override object VisitPrintln(BaseGrammarParser.PrintlnContext context)
        {
            var value = Visit(context.expression());
            Console.WriteLine(value);

            return null;
        }

        public override object VisitFunctionCall(BaseGrammarParser.FunctionCallContext context)
        {
            var args = context.expression().Select(Visit).ToList();

            return null;
        }

        public override object VisitIfStatement(BaseGrammarParser.IfStatementContext context)
        {
            var condition = Convert.ToBoolean(Visit(context.expression()));

            if (condition)
            {
                Visit(context.statement());
            }
            else
            {
                foreach (var elifContext in context.elifStatement())
                {
                    var elifCondition = Convert.ToBoolean(Visit(elifContext.expression()));

                    if (elifCondition)
                    {
                        Visit(elifContext.statement());

                        return null;
                    }
                }

                if (context.elseStatement() != null)
                {
                    Visit(context.elseStatement());
                }
            }

            return null;
        }

        public override object VisitElifStatement(BaseGrammarParser.ElifStatementContext context)
        {
            var condition = Convert.ToBoolean(Visit(context.expression()));

            if (condition)
            {
                Visit(context.statement());
            }

            return null;
        }

        public override object VisitWhileStatement(BaseGrammarParser.WhileStatementContext context)
        {
            while (Convert.ToBoolean(Visit(context.expression())))
            {
                Visit(context.statement());
            }

            return null;
        }

        public override object VisitIdExp(BaseGrammarParser.IdExpContext context)
        {
            var name = context.IDENTIFIER().GetText();

            if (variables.ContainsKey(name))
            {
                return variables[name];
            }

            return null;
        }

        public override object VisitStringExpr(BaseGrammarParser.StringExprContext context)
        {
            return context.STRING_LITERAL().GetText().Trim('"');
        }

        public override object VisitNumExpr(BaseGrammarParser.NumExprContext context)
        {
            return Convert.ToDouble(context.NUMBER().GetText());
        }

        public override object VisitBoolExpr(BaseGrammarParser.BoolExprContext context)
        {
            return context.BOOL().GetText() == "True";
        }
    }
}
