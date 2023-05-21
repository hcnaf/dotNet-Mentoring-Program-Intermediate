using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFtsRequestTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;
        private int RecursiveAndCounter = 0;

        public ExpressionToFtsRequestTranslator()
        {
            _resultStringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);

            return _resultStringBuilder.ToString();
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }

            if (node.Method.DeclaringType == typeof(string))
            {
                (string opening, string closing) = node.Method.Name switch
                {
                    "Equals" => ("(", ")"),
                    "Contains" => ("(*", "*)"),
                    "StartsWith" => ("(", "*)"),
                    "EndsWith" => ("(*", ")"),
                    _ => (null, null),
                };

                if (opening != null && closing != null)
                {
                    Visit(node.Object);
                    _resultStringBuilder.Append(opening);
                    Visit(node.Arguments[0]);
                    _resultStringBuilder.Append(closing);
                    return node;
                }
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    (Expression member, Expression constatnt) = (node.Left.NodeType, node.Right.NodeType) switch
                    {
                        (ExpressionType.MemberAccess, ExpressionType.Constant) => (node.Left, node.Right),
                        (ExpressionType.Constant, ExpressionType.MemberAccess) => (node.Right, node.Left),
                        _ => throw new NotSupportedException()
                    };

                    Visit(member);
                    _resultStringBuilder.Append("(");
                    Visit(constatnt);
                    _resultStringBuilder.Append(")");
                    break;

                case ExpressionType.AndAlso:
                    if (RecursiveAndCounter == 0)
                    {
                        _resultStringBuilder.Append("\"statements\": [ { \"query\":\"");
                    }

                    RecursiveAndCounter += 1;
                    Visit(node.Left);
                    _resultStringBuilder.Append("\" }, { \"query\":\"");
                    Visit(node.Right);

                    RecursiveAndCounter -= 1;
                    if (RecursiveAndCounter == 0)
                    {
                        _resultStringBuilder.Append("\" } ]");
                    }
                    break;

                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append(node.Member.Name).Append(":");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _resultStringBuilder.Append(node.Value);

            return node;
        }

        #endregion
    }
}
