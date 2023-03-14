using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
// LogManager.GetCurrentClassLogger().Info("");
namespace mbFinEx1s
{
    internal class Program
    {
        static Logger log=LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            log.Info("Start of Programme.");            
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
            string filename = "dictionary.txt";
            LoadDictionaryFromFile(dictionary, filename);
            Console.WriteLine("Добро Пожаловать в наш ' Веселый Словарик '  ");
            Console.WriteLine("\nИдет загрузка, подождите подалуйста.");
            log.Info("Downloading of Programme 3sec.");
            Console.WriteLine("Загрузка...");
            Thread.Sleep(3000);
            log.Info("Programme started.");
            while (true)
            {

                Console.WriteLine("\nМеню Словарика:\n");
                Console.WriteLine("1. Добавить слово в словарик\n");
                Console.WriteLine("2. Поменять слово в словарике\n");
                Console.WriteLine("3. Удалить слово с словарика\n");
                Console.WriteLine("4. Искать слово в словарике (с рус на англ)\n");
                Console.WriteLine("5. Выгрузить словарик в файл\n");
                Console.WriteLine("6. Закрыть словарик\n");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        log.Info("AddWord_SaveDictionaryToFile.choise");
                        AddWord(dictionary);
                        SaveDictionaryToFile(dictionary, filename);
                        
                        break;

                    case 2:
                        log.Info("ReplaceWord_SaveDictionaryToFile.choise");
                        ReplaceWord(dictionary);
                        SaveDictionaryToFile(dictionary, filename);
                        
                        break;

                    case 3:
                        log.Info("RemoveWord_SaveDictionaryToFile.choise");
                        RemoveWord(dictionary);
                        SaveDictionaryToFile(dictionary, filename);
                       
                        break;

                    case 4:
                        log.Info("SearchWord.choise");
                        SearchWord(dictionary);
                        break;

                    case 5:
                        log.Info(" ExportDictionaryToFile.choise");
                        ExportDictionaryToFile(dictionary);
                        break;

                    case 6:
                        log.Info("End of Programme.choise");
                        Environment.Exit(0);                      
                        break;

                    default:
                        log.Warn("Incorrect choise.choise");
                        Console.WriteLine("Неправильный выбор.");

                        break;
                }

                Console.WriteLine();
            }
        }



        private static void LoadDictionaryFromFile(Dictionary<string, List<string>> dictionary, string filename)
        {
            LogManager.GetCurrentClassLogger().Info("LoadDictionaryFromFile started");
            if (File.Exists(filename))//проверяет существование файла
            {
                using (StreamReader sr = new StreamReader(filename)) //если файл существует, то он открывается для чтения с помощью объекта StreamReader, созданного в операторе using.
                {
                    while (!sr.EndOfStream) //происходит чтение файла построчно, пока не будет достигнут конец файла
                    {
                        string line = sr.ReadLine();
                        string[] words = line.Split(':');//строка разбивается на две части с помощью метода Split
                        string ruWord = words[0].Trim();//удаляет пробельные символы
                        string[] englishWords = words[1].Split(',');//разделяет слова запятой
                        List<string> englishList = new List<string>();
                        foreach (string englishWord in englishWords)
                        {
                            englishList.Add(englishWord.Trim()); //добавляется в этот список с помощью метода Add
                        }
                        dictionary.Add(ruWord, englishList);
                    }
                }
            }
        }

        private static void AddWord(Dictionary<string, List<string>> dictionary)
        {
            LogManager.GetCurrentClassLogger().Info("AddWord started");
            Console.WriteLine("Введите слово на русском/english:");
            string ruWord = Console.ReadLine().ToLower();
            if (dictionary.ContainsKey(ruWord))//Затем метод проверяет, существует ли уже в словаре ключ
            {
                Console.WriteLine("Это слово уже есть в словарике.");
                return;
            }
            Console.WriteLine("Введите перевод этого слова на русском/english. При наличии нескольких переводов, разделите их запятой ( , ):");
            string[] englishWords = Console.ReadLine().Split(',');//
            List<string> englishList = new List<string>();
            foreach (string englishWord in englishWords)//благодаря foreeach не исп LINQ
            {
                englishList.Add(englishWord.Trim().ToLower());//Каждое английское слово удаляет пробельные символы с помощью метода Trim и приводится к нижнему регистру с помощью метода ToLower.
            }
            dictionary.Add(ruWord, englishList);
            Console.WriteLine("Слово успешно было добавлено в словарик.");//слово добавляется в список с помощью метода Add
        }

        private static void ReplaceWord(Dictionary<string, List<string>> dictionary)
        {
            LogManager.GetCurrentClassLogger().Info("ReplaceWord started");
            Console.Write("Введите слово на русском: ");
            string ruWord = Console.ReadLine().ToLower();

            if (dictionary.ContainsKey(ruWord))
            {
                Console.Write("Введите перевод на английский: ");
                string englishWord = Console.ReadLine().ToLower();//читает строку, введенную пользователем, и сохраняет ее в переменной ruWord, приводя ее к нижнему регистру.


                dictionary[ruWord].Clear();//если слово найдено в словаре, очищает список переводов, связанных с ключом ruWord
                dictionary[ruWord].Add(englishWord);//если слово найдено в словаре, добавляет englishWord в список переводов, связанный с ключом ruWord.
                Console.WriteLine($"Перевод слова '{ruWord}' заменен на '{englishWord}'");
            }
            else
            {
                Console.WriteLine($"Слово '{ruWord}' отсутствует в словарике");
            }
        }

        private static void RemoveWord(Dictionary<string, List<string>> dictionary)//принимает словарь dictionary в качестве аргумента и не возвращает значения (потому что его тип возвращаемого значения равен void)
        {
            LogManager.GetCurrentClassLogger().Info("RemoveeWord started");
            Console.Write("Введите слово на русском: ");
            string ruWord = Console.ReadLine().ToLower();//читаетв веденную  строку  и сохраняет ее в переменной ruWord, приводя ее к нижнему регистру

            if (dictionary.ContainsKey(ruWord))// содержит ли словарь dictionary ключ ruWord.

            {
                dictionary.Remove(ruWord);// если слово есть в словаре удаляет ключ ruWord и связанные с ним значения из словаря dictionary
                Console.WriteLine($"Слово '{ruWord}' успешно удалено из словарика");
            }
            else
            {
                Console.WriteLine($"Слово '{ruWord}' отсутствует в словарике");
            }
        }

        private static void SearchWord(Dictionary<string, List<string>> dict)
        {
            LogManager.GetCurrentClassLogger().Info("SearchWord started");
            Console.WriteLine("Введите слово для поиска:");
            string word = Console.ReadLine();//читает строку, введенную пользователем, и сохраняет ее в переменной word

            if (dict.ContainsKey(word))// проверяет, содержит ли словарь dict ключ word
            {
                Console.WriteLine($"Перевод слова '{word}':");

                List<string> translations = dict[word];//сохраняет список всех переводов слова word из словаря dict в переменной translations

                for (int i = 0; i < translations.Count; i++)//еребирает все переводы слова word из списка translations
                {
                    Console.WriteLine($"{i + 1}. {translations[i]}");//выводит на экран номер перевода и соответствующий перевод из списка translations
                }
            }
            else
            {
                Console.WriteLine($"Слово '{word}' не найдено в словарике.");
            }
        }

        private static void SaveDictionaryToFile(Dictionary<string, List<string>> dictionary, string filename)
        {
            LogManager.GetCurrentClassLogger().Info("SaveDictionaryToFile started");
            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (KeyValuePair<string, List<string>> kvp in dictionary)
                {
                    string ruWord = kvp.Key;
                    List<string> englishWords = kvp.Value;
                    sw.Write(ruWord + " : ");
                    for (int i = 0; i < englishWords.Count; i++)
                    {
                        sw.Write(englishWords[i]);
                        if (i < englishWords.Count - 1)
                        {
                            sw.Write(", ");
                        }
                    }
                    sw.WriteLine();
                }
            }
        }

        private static void ExportDictionaryToFile(Dictionary<string, List<string>> dictionary)
        {
            LogManager.GetCurrentClassLogger().Info("ExportDictionaryToFile started");
            Console.WriteLine("Введите путь к файлу для экспорта словарика:");

            string filePath = Console.ReadLine();

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))//использует using для создания объекта StreamWriter с именем write который будет использоватся для записи в файл путем filePath
                {
                    foreach (KeyValuePair<string, List<string>> pair in dictionary)//перебирает все ключ-значение пары в словаре dictionary
                    {
                        writer.Write(pair.Key + " - ");//записывает на диск ключ словаря, за которым следует тире и пробел
                        foreach (string translation in pair.Value)//перебирает все значения (список переводов) для данного ключа
                        {
                            writer.Write(translation + ", ");//записывает на диск каждый перевод, за которым следует запятая и пробел
                        }

                        writer.WriteLine();
                    }
                    log.Info("Export done");
                    Console.WriteLine($"Словарик успешно экспортирован в файл {filePath}");
                }
            }
            catch (Exception ex)
            {
                log.Warn("Error of export");
                Console.WriteLine($"Ошибка при экспорте словарика: {ex.Message}");
            }
        }
    }
}
