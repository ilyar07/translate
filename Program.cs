namespace translate
{
    internal class Program
    {
        const string symbols = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        class MyInt
        {

            public string number { get; set; }

            private int _system;

            public int system
            {
                get => _system;
                set
                {
                    if (value == _system) return;
                    int oldSystem = _system;
                    _system = value;
                    this.number = TranslateSystem(this.number, oldSystem, value);
                }
            }

            public MyInt(string number, int system)
            {
                this.number = number;
                this._system = system;
            }

            public static string TranslateSystem(string number, int sys1, int sys2)
            {
                /* Метод перевода из 1 системы счисления в другую*/

                if (number == "0") { return "0"; }
                int decimalNumber = 0;
                if (sys1 != 10)
                {
                    for (int i = number.Length - 1; i >= 0; --i)
                    {
                        decimalNumber += symbols.IndexOf(char.ToString(number[i]), StringComparison.Ordinal) * Convert.ToInt32(Math.Pow(sys1, number.Length - 1 - i));
                    }
                }
                else
                {
                    decimalNumber = int.Parse(number);
                }

                string result = "";

                if (sys2 != 10)
                {
                    while (decimalNumber > 0)
                    {
                        result = symbols[decimalNumber % sys2] + result;
                        decimalNumber /= sys2;
                    }
                }
                else
                {
                    result = decimalNumber.ToString();
                }
                return result;
            }
            public static MyInt operator +(MyInt int1, MyInt int2)
            {
                string decimalint1 = MyInt.TranslateSystem(int1.number, int1.system, 10);
                string decimalint2 = MyInt.TranslateSystem(int2.number, int2.system, 10);

                string newNumber = (int.Parse(decimalint1) + int.Parse(decimalint2)).ToString();
                newNumber = MyInt.TranslateSystem(newNumber, 10, int1.system);

                return new MyInt(newNumber, int1.system);
            }
            public static MyInt operator -(MyInt int1, MyInt int2)
            {
                string decimalint1 = MyInt.TranslateSystem(int1.number, int1.system, 10);
                string decimalint2 = MyInt.TranslateSystem(int2.number, int2.system, 10);

                string newNumber = (int.Parse(decimalint1) - int.Parse(decimalint2)).ToString();
                if (int.Parse(newNumber) < 0)
                {
                    throw new InvalidOperationException("Результат вычитания отрицательный!");
                }

                newNumber = MyInt.TranslateSystem(newNumber, 10, int1.system);
                return new MyInt(newNumber, int1.system);
            }
            public static MyInt operator *(MyInt int1, MyInt int2)
            {
                string decimalint1 = MyInt.TranslateSystem(int1.number, int1.system, 10);
                string decimalint2 = MyInt.TranslateSystem(int2.number, int2.system, 10);

                string newNumber = (int.Parse(decimalint1) * int.Parse(decimalint2)).ToString();
                newNumber = MyInt.TranslateSystem(newNumber, 10, int1.system);

                return new MyInt(newNumber, int1.system);
            }
            public static MyInt operator /(MyInt int1, MyInt int2)
            {
                string decimalint1 = MyInt.TranslateSystem(int1.number, int1.system, 10);
                string decimalint2 = MyInt.TranslateSystem(int2.number, int2.system, 10);

                if (decimalint2 == "0")
                {
                    throw new DivideByZeroException();
                }

                string newNumber = (int.Parse(decimalint1) / int.Parse(decimalint2)).ToString();
                newNumber = MyInt.TranslateSystem(newNumber, 10, int1.system);
                return new MyInt(newNumber, int1.system);

            }
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Выберите действие");
                Console.WriteLine("0 - выйти");
                Console.WriteLine("1 - перевод");
                Console.WriteLine("2 - сложние");
                Console.WriteLine("3 - вычитание");
                Console.WriteLine("4 - умножение");
                Console.WriteLine("5 - деление");
                string action = Console.ReadLine();

                if (action == "0") { break; }
                if (action == "1")
                {

                    /* Проверка системы счисления 1 */
                    Console.WriteLine("Введите систему счисления ИЗ которой хотите перевести\n");
                    string sys1String = Console.ReadLine();
                    if (!(int.TryParse(sys1String, out int sys1)))
                    {
                        Console.WriteLine("Система счисления должна быть цифрой!");
                        continue;
                    }
                    else if (sys1 <= 1)
                    {
                        Console.WriteLine("Система счисления должна быть больше еденицы!");
                        continue;
                    }
                    else if (sys1 > symbols.Length)
                    {
                        Console.WriteLine($"Слишком большая система счисления! Максимальная - {symbols.Length}");
                        continue;
                    }

                    /* Проверка числа */
                    Console.WriteLine("Введите число");
                    string number = Console.ReadLine();
                    if (number.Length == 0 || number == "0")
                    {
                        Console.WriteLine("Число не должно быть пустым");
                        continue;
                    }
                    bool breakFlag = false;
                    for (int i = 0; i < number.Length; i++)
                    {
                        if (symbols.Substring(0, sys1).IndexOf(number[i], StringComparison.Ordinal) == -1)
                        {
                            breakFlag = true;
                            break;
                        }
                    }

                    if (breakFlag)
                    {
                        Console.WriteLine(
                            $"Для вашей системы счисления используйте символы из набора: '{symbols.Substring(0, sys1)}'!");
                        continue;
                    }

                    /* Проверка системы счисления 2 */
                    Console.WriteLine("Введите систему счисления В Какую хотите перевести\n");
                    string sys2String = Console.ReadLine();
                    if (!(int.TryParse(sys2String, out int sys2)))
                    {
                        Console.WriteLine("Система счисления должна быть цифрой!");
                        continue;
                    }
                    else if (sys2 <= 1)
                    {
                        Console.WriteLine("Система счисления должна быть больше еденицы!");
                        continue;
                    }
                    else if (sys2 > symbols.Length)
                    {
                        Console.WriteLine($"Слишком большая система счисления! Максимальная - {symbols.Length}");
                        continue;
                    }



                    MyInt myint = new MyInt(number, sys1);
                    myint.system = sys2;
                    Console.WriteLine($"Число {number} в системе счисления {sys1} == числу {myint.number} в системе счисления {myint.system}");
                }
                if (action == "2")
                {
                    Console.WriteLine("Для всех действий с числами в ответе будет система счисление первого введенного числа!!!");

                    /* Проверка системы счисления 1 */
                    Console.WriteLine("Введите систему счисления первого числа\n");
                    string sys1String = Console.ReadLine();
                    if (!(int.TryParse(sys1String, out int sys1)))
                    {
                        Console.WriteLine("Система счисления должна быть цифрой!");
                        continue;
                    }
                    else if (sys1 <= 1)
                    {
                        Console.WriteLine("Система счисления должна быть больше еденицы!");
                        continue;
                    }
                    else if (sys1 > symbols.Length)
                    {
                        Console.WriteLine($"Слишком большая система счисления! Максимальная - {symbols.Length}");
                        continue;
                    }

                    /* Проверка числа 1*/
                    Console.WriteLine("Введите первое число");
                    string number1 = Console.ReadLine();
                    if (number1.Length == 0 || number1 == "0")
                    {
                        Console.WriteLine("Число не должно быть пустым");
                        continue;
                    }
                    bool breakFlag1 = false;
                    for (int i = 0; i < number1.Length; i++)
                    {
                        if (symbols.Substring(0, sys1).IndexOf(number1[i], StringComparison.Ordinal) == -1)
                        {
                            breakFlag1 = true;
                            break;
                        }
                    }

                    if (breakFlag1)
                    {
                        Console.WriteLine(
                            $"Для вашей системы счисления используйте символы из набора: '{symbols.Substring(0, sys1)}'!");
                        continue;
                    }

                    /* Проверка системы счисления 2 */
                    Console.WriteLine("Введите систему счисления второго числа\n");
                    string sys2String = Console.ReadLine();
                    if (!(int.TryParse(sys2String, out int sys2)))
                    {
                        Console.WriteLine("Система счисления должна быть цифрой!");
                        continue;
                    }
                    else if (sys2 <= 1)
                    {
                        Console.WriteLine("Система счисления должна быть больше еденицы!");
                        continue;
                    }
                    else if (sys2 > symbols.Length)
                    {
                        Console.WriteLine($"Слишком большая система счисления! Максимальная - {symbols.Length}");
                        continue;
                    }

                    /* Проверка числа 2*/
                    Console.WriteLine("Введите второе число");
                    string number2 = Console.ReadLine();
                    if (number2.Length == 0 || number2 == "0")
                    {
                        Console.WriteLine("Число не должно быть пустым");
                        continue;
                    }
                    bool breakFlag2 = false;
                    for (int i = 0; i < number2.Length; i++)
                    {
                        if (symbols.Substring(0, sys2).IndexOf(number2[i], StringComparison.Ordinal) == -1)
                        {
                            breakFlag2 = true;
                            break;
                        }
                    }

                    if (breakFlag2)
                    {
                        Console.WriteLine(
                            $"Для вашей системы счисления используйте символы из набора: '{symbols.Substring(0, sys2)}'!");
                        continue;
                    }

                    MyInt a = new MyInt(number1, sys1);
                    MyInt b = new MyInt(number2, sys2);
                    MyInt c = a + b;
                    Console.WriteLine($"'{number1}' в системе счисления {sys1} +\n" +
                        $"'{number2}' в системе счисления {sys2} == '{c.number}' в системе счисления {c.system}");
                }
                if (action == "3")
                {
                    Console.WriteLine("Для всех действий с числами в ответе будет система счисление первого введенного числа!!!");

                    /* Проверка системы счисления 1 */
                    Console.WriteLine("Введите систему счисления первого числа\n");
                    string sys1String = Console.ReadLine();
                    if (!(int.TryParse(sys1String, out int sys1)))
                    {
                        Console.WriteLine("Система счисления должна быть цифрой!");
                        continue;
                    }
                    else if (sys1 <= 1)
                    {
                        Console.WriteLine("Система счисления должна быть больше еденицы!");
                        continue;
                    }
                    else if (sys1 > symbols.Length)
                    {
                        Console.WriteLine($"Слишком большая система счисления! Максимальная - {symbols.Length}");
                        continue;
                    }

                    /* Проверка числа 1*/
                    Console.WriteLine("Введите первое число");
                    string number1 = Console.ReadLine();
                    if (number1.Length == 0 || number1 == "0")
                    {
                        Console.WriteLine("Число не должно быть пустым");
                        continue;
                    }
                    bool breakFlag1 = false;
                    for (int i = 0; i < number1.Length; i++)
                    {
                        if (symbols.Substring(0, sys1).IndexOf(number1[i], StringComparison.Ordinal) == -1)
                        {
                            breakFlag1 = true;
                            break;
                        }
                    }

                    if (breakFlag1)
                    {
                        Console.WriteLine(
                            $"Для вашей системы счисления используйте символы из набора: '{symbols.Substring(0, sys1)}'!");
                        continue;
                    }

                    /* Проверка системы счисления 2 */
                    Console.WriteLine("Введите систему счисления второго числа\n");
                    string sys2String = Console.ReadLine();
                    if (!(int.TryParse(sys2String, out int sys2)))
                    {
                        Console.WriteLine("Система счисления должна быть цифрой!");
                        continue;
                    }
                    else if (sys2 <= 1)
                    {
                        Console.WriteLine("Система счисления должна быть больше еденицы!");
                        continue;
                    }
                    else if (sys2 > symbols.Length)
                    {
                        Console.WriteLine($"Слишком большая система счисления! Максимальная - {symbols.Length}");
                        continue;
                    }

                    /* Проверка числа 2*/
                    Console.WriteLine("Введите второе число");
                    string number2 = Console.ReadLine();
                    if (number2.Length == 0 || number2 == "0")
                    {
                        Console.WriteLine("Число не должно быть пустым");
                        continue;
                    }
                    bool breakFlag2 = false;
                    for (int i = 0; i < number2.Length; i++)
                    {
                        if (symbols.Substring(0, sys2).IndexOf(number2[i], StringComparison.Ordinal) == -1)
                        {
                            breakFlag2 = true;
                            break;
                        }
                    }

                    if (breakFlag2)
                    {
                        Console.WriteLine(
                            $"Для вашей системы счисления используйте символы из набора: '{symbols.Substring(0, sys2)}'!");
                        continue;
                    }

                    MyInt a = new MyInt(number1, sys1);
                    MyInt b = new MyInt(number2, sys2);
                    try
                    {
                        MyInt c = a - b;
                        Console.WriteLine($"'{number1}' в системе счисления {sys1} -\n" +
                        $"'{number2}' в системе счисления {sys2} == '{c.number}' в системе счисления {c.system}");
                    }
                    catch
                    {
                        Console.WriteLine("Получается отрицательное число");
                        break;
                    }
                }
                if (action == "4")
                {
                    Console.WriteLine("Для всех действий с числами в ответе будет система счисление первого введенного числа!!!");

                    /* Проверка системы счисления 1 */
                    Console.WriteLine("Введите систему счисления первого числа\n");
                    string sys1String = Console.ReadLine();
                    if (!(int.TryParse(sys1String, out int sys1)))
                    {
                        Console.WriteLine("Система счисления должна быть цифрой!");
                        continue;
                    }
                    else if (sys1 <= 1)
                    {
                        Console.WriteLine("Система счисления должна быть больше еденицы!");
                        continue;
                    }
                    else if (sys1 > symbols.Length)
                    {
                        Console.WriteLine($"Слишком большая система счисления! Максимальная - {symbols.Length}");
                        continue;
                    }

                    /* Проверка числа 1*/
                    Console.WriteLine("Введите первое число");
                    string number1 = Console.ReadLine();
                    if (number1.Length == 0 || number1 == "0")
                    {
                        Console.WriteLine("Число не должно быть пустым");
                        continue;
                    }
                    bool breakFlag1 = false;
                    for (int i = 0; i < number1.Length; i++)
                    {
                        if (symbols.Substring(0, sys1).IndexOf(number1[i], StringComparison.Ordinal) == -1)
                        {
                            breakFlag1 = true;
                            break;
                        }
                    }

                    if (breakFlag1)
                    {
                        Console.WriteLine(
                            $"Для вашей системы счисления используйте символы из набора: '{symbols.Substring(0, sys1)}'!");
                        continue;
                    }

                    /* Проверка системы счисления 2 */
                    Console.WriteLine("Введите систему счисления второго числа\n");
                    string sys2String = Console.ReadLine();
                    if (!(int.TryParse(sys2String, out int sys2)))
                    {
                        Console.WriteLine("Система счисления должна быть цифрой!");
                        continue;
                    }
                    else if (sys2 <= 1)
                    {
                        Console.WriteLine("Система счисления должна быть больше еденицы!");
                        continue;
                    }
                    else if (sys2 > symbols.Length)
                    {
                        Console.WriteLine($"Слишком большая система счисления! Максимальная - {symbols.Length}");
                        continue;
                    }

                    /* Проверка числа 2*/
                    Console.WriteLine("Введите второе число");
                    string number2 = Console.ReadLine();
                    if (number2.Length == 0 || number2 == "0")
                    {
                        Console.WriteLine("Число не должно быть пустым");
                        continue;
                    }
                    bool breakFlag2 = false;
                    for (int i = 0; i < number2.Length; i++)
                    {
                        if (symbols.Substring(0, sys2).IndexOf(number2[i], StringComparison.Ordinal) == -1)
                        {
                            breakFlag2 = true;
                            break;
                        }
                    }

                    if (breakFlag2)
                    {
                        Console.WriteLine(
                            $"Для вашей системы счисления используйте символы из набора: '{symbols.Substring(0, sys2)}'!");
                        continue;
                    }

                    MyInt a = new MyInt(number1, sys1);
                    MyInt b = new MyInt(number2, sys2);
                    MyInt c = a * b;
                    Console.WriteLine($"'{number1}' в системе счисления {sys1} *\n" +
                        $"'{number2}' в системе счисления {sys2} == '{c.number}' в системе счисления {c.system}");
                }
                if (action == "5")
                {
                    Console.WriteLine("Для всех действий с числами в ответе будет система счисление первого введенного числа!!!");

                    /* Проверка системы счисления 1 */
                    Console.WriteLine("Введите систему счисления первого числа\n");
                    string sys1String = Console.ReadLine();
                    if (!(int.TryParse(sys1String, out int sys1)))
                    {
                        Console.WriteLine("Система счисления должна быть цифрой!");
                        continue;
                    }
                    else if (sys1 <= 1)
                    {
                        Console.WriteLine("Система счисления должна быть больше еденицы!");
                        continue;
                    }
                    else if (sys1 > symbols.Length)
                    {
                        Console.WriteLine($"Слишком большая система счисления! Максимальная - {symbols.Length}");
                        continue;
                    }

                    /* Проверка числа 1*/
                    Console.WriteLine("Введите первое число");
                    string number1 = Console.ReadLine();
                    if (number1.Length == 0 || number1 == "0")
                    {
                        Console.WriteLine("Число не должно быть пустым");
                        continue;
                    }
                    bool breakFlag1 = false;
                    for (int i = 0; i < number1.Length; i++)
                    {
                        if (symbols.Substring(0, sys1).IndexOf(number1[i], StringComparison.Ordinal) == -1)
                        {
                            breakFlag1 = true;
                            break;
                        }
                    }

                    if (breakFlag1)
                    {
                        Console.WriteLine(
                            $"Для вашей системы счисления используйте символы из набора: '{symbols.Substring(0, sys1)}'!");
                        continue;
                    }

                    /* Проверка системы счисления 2 */
                    Console.WriteLine("Введите систему счисления второго числа\n");
                    string sys2String = Console.ReadLine();
                    if (!(int.TryParse(sys2String, out int sys2)))
                    {
                        Console.WriteLine("Система счисления должна быть цифрой!");
                        continue;
                    }
                    else if (sys2 <= 1)
                    {
                        Console.WriteLine("Система счисления должна быть больше еденицы!");
                        continue;
                    }
                    else if (sys2 > symbols.Length)
                    {
                        Console.WriteLine($"Слишком большая система счисления! Максимальная - {symbols.Length}");
                        continue;
                    }

                    /* Проверка числа 2*/
                    Console.WriteLine("Введите второе число");
                    string number2 = Console.ReadLine();
                    if (number2.Length == 0 || number2 == "0")
                    {
                        Console.WriteLine("Число не должно быть пустым");
                        continue;
                    }
                    bool breakFlag2 = false;
                    for (int i = 0; i < number2.Length; i++)
                    {
                        if (symbols.Substring(0, sys2).IndexOf(number2[i], StringComparison.Ordinal) == -1)
                        {
                            breakFlag2 = true;
                            break;
                        }
                    }

                    if (breakFlag2)
                    {
                        Console.WriteLine(
                            $"Для вашей системы счисления используйте символы из набора: '{symbols.Substring(0, sys2)}'!");
                        continue;
                    }

                    MyInt a = new MyInt(number1, sys1);
                    MyInt b = new MyInt(number2, sys2);
                    MyInt c = a / b;

                    Console.WriteLine($"'{number1}' в системе счисления {sys1}  /  \n" +
                    $"'{number2}' в системе счисления {sys2} == '{c.number}' в системе счисления {c.system}");

                }
            }
        }
    }
}

