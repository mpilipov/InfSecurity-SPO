using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ib_Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            /* 1. СЧИТЫВАНИЕ ГЛАВЫ*/
            string textChapter = File.ReadAllText("C:\\Users\\misha\\source\\repos\\ib_Lab1\\ib_Lab1\\bin\\vm_part.txt", Encoding.Default);
            List<char> list = new List<char>();
            Console.WriteLine("I. Вывод считанного из файла текста:");
            for (int i = 0; i < textChapter.Length; i++)
            {
                    list.Add(textChapter.ToLower()[i]);                    
                    Console.Write(list[list.Count - 1]);
            }
            /* СЧИТЫВАНИЕ РОМАНА*/
            string textRoman = File.ReadAllText("C:\\Users\\misha\\source\\repos\\ib_Lab1\\ib_Lab1\\bin\\vm_all.txt", Encoding.Default);
            textRoman = textRoman.ToLower();
            List<char> list2 = new List<char>();
            for (int i = 0; i < textRoman.Length; i++)
               // if (textRoman[i] > 1000 && textRoman[i] < 2000)
               // {                   
                    list2.Add(textRoman[i]);                                      
                    //заполнение list-a текстом романа
              //  }

            Console.WriteLine("II. Вывод зашифрованного шифром Цезаря текста:");
            /*ШИФР 1 ГЛАВЫ*/
            int shift = 3;  //величина сдвига при шифровании Цезаря
            string alphabet = "абвгдеёжзийклмнопрстуфхцчшщьыъэюя";
            string Ces_code = string.Empty;
            foreach (var s in list)
            {
                if (alphabet.Contains(s))
                {
                    int pos = alphabet.IndexOf(s);
                    int a = pos + shift;

                    if (a < 33)
                        Ces_code += alphabet[a];
                    else
                        Ces_code += alphabet[a - 33];
                    
                }
                else
                    Ces_code += s;
                Console.Write(Ces_code[Ces_code.Length - 1]);
            }            
            /*Шифр 1 главы*/
           
            /* 2. ПОДСЧЁТ ЧАСТОТЫ БУКВ ВСЕГО РОМАНА*/
            Dictionary<char, int> d = new Dictionary<char, int>(); //количество каждой буквы в романе
            Dictionary<char, double> ch = new Dictionary<char, double>(); //частота каждой буквы в романе
            foreach (var a in alphabet)
            {
                d.Add(a, 0);
            }
            Console.WriteLine("///");
            foreach (var s in list2)
            {
                if (alphabet.Contains(s))
                    d[s] += 1;
            }
            for (int i = 0; i < d.Count; i++)
            {
                ch.Add(alphabet[i], d[alphabet[i]]*(1.0) / list2.Count);
            }

            Console.WriteLine("III. Вывод букв, отсортированных по частоте встречаемости во всём романе");
            ch = ch.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            //сортировка и вывод элементов ch в порядке убывания
            foreach (var item in ch)
                Console.WriteLine(item.Key + "==" + item.Value);


            /*ПОДСЧЁТ ЧАСТОТЫ БУКВ ГЛАВЫ*/
            Dictionary<char, int> d2 = new Dictionary<char, int>(); //количество каждой буквы в главе
            Dictionary<char, double> ch2 = new Dictionary<char, double>(); //частота каждой буквы в главе
            foreach (var a in alphabet)
            {
                d2.Add(a, 0);
            }
            
            foreach (var s in Ces_code)
            {
                if (alphabet.Contains(s))
                    d[s] += 1;
            }
            for (int i = 0; i < d2.Count; i++)
            {
                ch2.Add(alphabet[i], d2[alphabet[i]] * (1.0) / Ces_code.Length);
            }

            //Сортировка по убыванию встречаемости букв в зашифрованном тексте
            ch2 = ch2.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            List<char> new_list = new List<char>();
            Console.WriteLine("IV. Вывод букв, отсортированных по частоте встречаемости в зашифрованном тексте:");
            foreach (var item in ch2)
            {
                Console.WriteLine(item.Key + "==" + item.Value);
                new_list.Add(item.Key);
            }

            Dictionary<string, string> mapping_table = new Dictionary<string,string>();
            //создание таблицы соответствия для расшифровки зашифрованного текста по словарю из всего текста
            int i1 = 0;
            foreach (var item in ch)
            {
                mapping_table.Add(Convert.ToString(new_list[i1]), Convert.ToString(item.Key));
                i1++;
            }
            Console.WriteLine("V. Вывод словаря соответствия, по которому будет проводиться восстановление зашифрованного текста");
            foreach (var item in mapping_table)
            {
                Console.WriteLine(item.Key + " " + item.Value);
            }

            string CS_code_unlock=string.Empty;
            // 4. РАСШИФРОВКА ПО ТАБЛИЦЕ СООТВЕТСТВИЯ
            Console.WriteLine("VI. Вывод расшифрованного текста с помощью таблицы соответствия:");
            for (int i = 0; i<Ces_code.Length;i++)
            {
                if (alphabet.Contains(Ces_code[i]))
                    CS_code_unlock += mapping_table[Convert.ToString(Ces_code[i])];
                else
                    CS_code_unlock += Ces_code[i];
                Console.Write(CS_code_unlock[CS_code_unlock.Length - 1]);
            }
            File.WriteAllText("C:\\Users\\misha\\source\\repos\\ib_Lab1\\ib_Lab1\\bin\\vm_wr.txt", CS_code_unlock);
            // 5. ПОДСЧЕТ ЧИСЛА БИГРАММ ВО ВСЕМ РОМАНЕ (LIST2)
            List<string>bigrams = new List<string>();
            for (int m = 0; m < 33; m++)
            {
                for (int n = 0; n < 33; n++)
                {
                    bigrams.Add(System.String.Concat(alphabet[m], alphabet[n]));
                }
            }
            Dictionary<string, int> bigrams_roman = new Dictionary<string, int>(); //количество каждой биграммы в романе
            Dictionary<string, double> ch_bigrams_roman = new Dictionary<string, double>(); //частота каждой биграммы в романе
            foreach (var b in bigrams)
            {
                bigrams_roman.Add(b, 0);
            }
            Console.WriteLine("///");
            for (int i = 0; i < list2.Count-1; i++)
            {
                if(alphabet.Contains(list2[i]) && alphabet.Contains(list2[i+1]))
                    bigrams_roman[string.Concat(list2[i],list2[i+1])] += 1 ;
            }
            for (int i = 0; i < bigrams_roman.Count; i++)
            {
                ch_bigrams_roman.Add(bigrams[i], bigrams_roman[bigrams[i]] * (1.0) / (list2.Count / 2));
            }

            Console.WriteLine("VII. Вывод биграмм, отсортированных по частоте встречаемости во всём романе");
            ch_bigrams_roman = ch_bigrams_roman.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            foreach (var item in ch_bigrams_roman)
            {
                Console.WriteLine(item.Key + "==" + item.Value);
                //Сортировка по убыванию встречаемости букв во всём романе
            }


            // 6. ПОДСЧЕТ ЧИСЛА БИГРАММ ВО ЗАШИФРОВАННОМ ТЕКСТЕ (LIST)
            Dictionary<string, int> bigrams_ces_chapter = new Dictionary<string, int>(); //количество каждой биграммы в романе
            Dictionary<string, double> ch_bigrams_ces_chapter = new Dictionary<string, double>(); //частота каждой биграммы в романе
            foreach (var b in bigrams)
            {
                bigrams_ces_chapter.Add(b, 0);
            }
            Console.WriteLine("///");
            for (int i = 0; i < Ces_code.Length - 1; i++)
            {
                if(alphabet.Contains(Ces_code[i]) && alphabet.Contains(Ces_code[i+1]))
                bigrams_ces_chapter[string.Concat(Ces_code[i], Ces_code[i + 1])] += 1;
            }

            for (int i = 0; i < bigrams_ces_chapter.Count; i++)
            {
                ch_bigrams_ces_chapter.Add(bigrams[i], bigrams_ces_chapter[bigrams[i]] * (1.0) / (Ces_code.Length / 2));
            }
            Console.WriteLine("VIII. Вывод биграмм, отсортированных по частоте встречаемости в зашифрованной главе");
            ch_bigrams_ces_chapter = ch_bigrams_ces_chapter.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            foreach (var item in ch_bigrams_ces_chapter)
            {
                Console.WriteLine(item.Key + "==" + item.Value);
                //Сортировка по убыванию встречаемости букв в зашифрованной главе
            }
            foreach (var item in mapping_table)
            {
                Console.WriteLine(item.Key + " " + item.Value);
            }

            //ОБНОВЛЕНИЕ ТАБЛИЦЫ СООТВЕТСТВИЯ ПО БИГРАММАМ
            Dictionary<string,string> upd_mapping_table = new Dictionary<string,string>();
            Dictionary<string, string> upd_mapping_table2 = new Dictionary<string, string>();
            foreach (var item in mapping_table)
            {
                upd_mapping_table.Add(item.Value, item.Key);
                upd_mapping_table2.Add(item.Value, item.Key);
            }
            
            Console.WriteLine("REVERSE");
            foreach (var item in upd_mapping_table)
                Console.WriteLine(item.Key + " " + item.Value);
            Console.Write("end");
            foreach (var m_t in upd_mapping_table2)
            {
                int s = 0;
                foreach(var ch_big_roman in ch_bigrams_roman)
                {
                    s++;
                    if (m_t.Value==ch_big_roman.Key[0].ToString())
                    {
                        //если в mapping_table найдено значение как ключ в ch_bigrams_roman
                        //проходим по ch_bigrams_ces_chapter, согласно данным там, меняем значение key в mapping_table
                        string r = ch_big_roman.Key[0].ToString();
                        int i = 0;
                        foreach (var Bigrams_chapter in ch_bigrams_ces_chapter)
                        {
                            i++;
                            if (i == s && (m_t.Value != Bigrams_chapter.Key[0].ToString()))
                            {
                                upd_mapping_table.Remove(m_t.Key);
                                upd_mapping_table.Add(m_t.Key, Bigrams_chapter.Key[0].ToString());
                            }
                            else if (i == s && (m_t.Value != Bigrams_chapter.Key[1].ToString()))
                            {
                                upd_mapping_table.Remove(m_t.Key);
                                upd_mapping_table.Add(m_t.Key, Bigrams_chapter.Key[1].ToString());
                            }
                            
                        }
                    }
                    else if(m_t.Value == ch_big_roman.Key[1].ToString())
                    {
                        string r = ch_big_roman.Key[1].ToString();
                        int i = 0;
                        foreach (var Bigrams_chapter in ch_bigrams_ces_chapter)
                        {
                            i++;
                            if (i == s && (m_t.Value != Bigrams_chapter.Key[0].ToString()))
                            {
                                upd_mapping_table.Remove(m_t.Key);
                                upd_mapping_table.Add(m_t.Key, Bigrams_chapter.Key[0].ToString());
                            }
                            else if (i == s && (m_t.Value != Bigrams_chapter.Key[1].ToString()))
                            {
                                upd_mapping_table.Remove(m_t.Key);
                                upd_mapping_table.Add(m_t.Key, Bigrams_chapter.Key[1].ToString());
                            }
                            if (i == 5)
                                break;
                        }
                    }
                    if (s == 5)
                        break;
                }
            }
            foreach (var item in upd_mapping_table)
            {
                Console.WriteLine(item.Key + " " + item.Value);
            }
            //расшифровка с помощью upd_mapping_table
            Console.WriteLine("VI. Вывод расшифрованного текста с помощью таблицы соответствия:");
            for (int i = 0; i < Ces_code.Length; i++)
            {
                if (alphabet.Contains(Ces_code[i]))
                    CS_code_unlock += upd_mapping_table[Convert.ToString(Ces_code[i])];
                else
                    CS_code_unlock += Ces_code[i];
                Console.Write(CS_code_unlock[CS_code_unlock.Length - 1]);
            }
            Console.Write("END");
            File.WriteAllText("C:\\Users\\misha\\source\\repos\\ib_Lab1\\ib_Lab1\\bin\\vm_wr2.txt", CS_code_unlock);
            Console.ReadKey();
        }
    }
}
