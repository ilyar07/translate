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
        }

        static void Main(string[] args)
        {
            while (true)
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
        }
    }
}

