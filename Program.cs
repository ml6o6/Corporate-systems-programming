using System.Globalization;

namespace CalcApp
{
    internal class Program
    {
        static double memory = 0.0;
        static double ReadNum(string input)
        {
            while (true)
            {
                Console.Write(input);
                string? s = Console.ReadLine();
                if (s == null) continue;

                s = s.Trim();

                // замена запятой на точку
                s = s.Replace(',', '.');
                if (double.TryParse(
                        s,
                        NumberStyles.Float,CultureInfo.InvariantCulture,
                        out double value))
                {
                    return value;
                }

                Console.WriteLine("Не удалось распознать число. Попробуйте ещё раз (пример: 12.34 или 12,34).");
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Классический калькулятор C#");
            Console.WriteLine("Введите знак операции, затем первое и второе число(для бинарных операций)");
            Console.WriteLine("Доступные операции:");
            Console.WriteLine("  Бинарные: +, -, *,  /,  %");
            Console.WriteLine("  Унарные:  1/x   x^2   sqrt(x)");
            Console.WriteLine("  Память:   M+    M-    MR");
            Console.WriteLine("  Команды: clear, exit");
            Console.WriteLine();

            while (true)
            {
                Console.Write("Введите знак операции (+, -, *, /, %, 1/x, x^2, sqrt, M+, M-, MR, clear, exit): ");
                string? op = Console.ReadLine();
                if (op == null) continue;
                op = op.Trim().ToLowerInvariant(); // удаляет пробелы и приводит к нижнему регистру

                // Команды
                if (op == "exit")
                {
                    Console.WriteLine("Выход.");
                    return;
                }
                if (op == "clear")
                {
                    Console.Clear();
                    continue;
                }

                try
                {
                    switch (op)
                    {
                        // Память
                        case "m+":
                            {
                                double x = ReadNum("Введите число для добавления в память (M+): ");
                                memory += x;
                                Console.WriteLine($"Память: {memory}");
                                break;
                            }
                        case "m-":
                            {
                                double x = ReadNum("Введите число для вычитания из памяти (M-): ");
                                memory -= x;
                                Console.WriteLine($"Память: {memory}");
                                break;
                            }
                        case "mr":
                            {
                                Console.WriteLine($"Память (MR): {memory}");
                                break;
                            }

                        // Унарные операции
                        case "1/x":
                            {
                                double x = ReadNum("Введите x: ");
                                if (x == 0.0)
                                    throw new DivideByZeroException("Деление на ноль в 1/x.");
                                double result = 1.0 / x;
                                Console.WriteLine($"1/{x} = {result}");
                                break;
                            }
                        case "x^2":
                            {
                                double x = ReadNum("Введите x: ");
                                double result = x * x;
                                Console.WriteLine($"{x}^2 = {result}");
                                break;
                            }
                        case "sqrt":
                            {
                                double x = ReadNum("Введите x: ");
                                if (x < 0.0)
                                    throw new ArgumentOutOfRangeException(nameof(x), "Нельзя извлечь квадратный корень из отрицательного числа.");
                                double result = Math.Sqrt(x);
                                Console.WriteLine($"sqrt({x}) = {result}");
                                break;
                            }

                        // Бинарные операции
                        case "+":
                        case "-":
                        case "*":
                        case "/":
                        case "%":
                            {
                                double a = ReadNum("Введите первое число: ");
                                double b = ReadNum("Введите второе число: ");
                                double result = op switch
                                {
                                    "+" => a + b,
                                    "-" => a - b,
                                    "*" => a * b,
                                    "/" => (b == 0.0) ? throw new DivideByZeroException("Деление на ноль.") : a / b,
                                    "%" => (b == 0.0) ? throw new DivideByZeroException("Остаток (%): деление на ноль.") : a % b, // остаток от деления (для double)
                                    _ => throw new InvalidOperationException("Неизвестная операция.")
                                };
                                Console.WriteLine($"{a} {op} {b} = {result}");
                                break;
                            }

                        default:
                            Console.WriteLine("Неизвестная команда");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }

                Console.WriteLine();
            }
        }
    }
}
