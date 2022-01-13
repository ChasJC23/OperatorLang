using MyLanguage.ExpressionEvaluator_POC.Nodes;
using MyLanguage.ExpressionEvaluator_POC.Operators;

namespace MyLanguage.ExpressionEvaluator_POC
{
    static class Program
    {
        public static void Main()
        {
            OperatorTable standard = new();
            // ORO ORORORORO OR OR O ROR OROR  RO OR O RORORR
            standard.AddAssociativeBinaryCategory<bool>(false);
            standard.AddAssociativeBinaryOperator<bool>("OR", (a, b) => a | b);
            // XOR XOROXR ROXRO XRO ORX RX XRORXOXOR  ORXRX RXO
            standard.AddAssociativeBinaryCategory<bool>(false);
            standard.AddAssociativeBinaryOperator<bool>("XOR", (a, b) => a ^ b);
            // ANDAND N ANDNAD NAND NAD AN DAN DNADNA DNAD NAD
            standard.AddAssociativeBinaryCategory<bool>(false);
            standard.AddAssociativeBinaryOperator<bool>("AND", (a, b) => a & b);
            // Relational operators
            standard.AddImpliedAssociativeBinaryCategory<float, bool>((a, b) => a & b);
            standard.AddImplicativeAssociativeBinaryOperator(">", (float a, float b) => a > b);
            standard.AddImplicativeAssociativeBinaryOperator("<", (float a, float b) => a < b);
            standard.AddImplicativeAssociativeBinaryOperator("=", (float a, float b) => a == b);
            // addition subtraction category
            standard.AddAssociativeBinaryCategory<float>(false);
            standard.AddAssociativeBinaryOperator<float>("+", (a, b) => a + b);
            standard.AddAssociativeBinaryOperator<float>("-", (a, b) => a - b);
            // multiplication division category
            standard.AddAssociativeBinaryCategory<float>(false);
            standard.AddAssociativeBinaryOperator<float>("*", (a, b) => a * b);
            standard.AddAssociativeBinaryOperator<float>("/", (a, b) => a / b);
            standard.SetAssociativeNullBinaryOperator<float>((a, b) => a * b);
            // exponent category
            standard.AddAssociativeBinaryCategory<float>(true);
            standard.AddAssociativeBinaryOperator<float>("^", MathF.Pow);
            // pos neg category
            standard.AddConsistentUnaryCategory<float>(false);
            standard.AddConsistentUnaryOperator<float>("+", (a) => +a);
            standard.AddConsistentUnaryOperator<float>("-", (a) => -a);
            var expression = "'A'";
            var result = Parser.Parse(expression, standard);
            Console.WriteLine(result.Eval(new BaseContext()));
        }
    }
}