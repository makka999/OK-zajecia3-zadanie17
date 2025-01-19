using System;
using System.Collections.Generic;

class Program
{
    // Słownik zapamiętujący wyniki,
    // aby nie liczyć tych samych stanów wielokrotnie
    static Dictionary<(int pos, int prevDigit, bool leading, bool tight), long> dictionaryNumberTest;

    static void Main(string[] args)
    {
        long a = 122;
        long b = 4566;

        // Wywołujemy funkcję zliczającą liczby w zakresie [a, b]
        long result = CountValidNumbers(a, b);

        // Wypisujemy wynik na ekran
        Console.WriteLine(result);
    }

    // Zwraca liczbę liczb w [a, b], gdzie żadne dwie sąsiadujące cyfry nie są takie same
    static long CountValidNumbers(long a, long b)
    {
        // Jeśli a > b, nic nie liczymy
        if (a > b) return 0;

        // Liczba w [a, b] = Liczba w [0, b] - Liczba w [0, a-1]
        return CountUpTo(b) - CountUpTo(a - 1);
    }

    // Liczy liczby w [0, x], które spełniają warunek
    static long CountUpTo(long x)
    {
        // Jeśli x < 0, nie ma żadnych liczb w tym przedziale
        if (x < 0) return 0;

        // Rozbijamy x na cyfry, np. 4566 -> [4, 5, 6, 6]
        List<int> digits = GetDigits(x);

        // Czyścimy słownik przed każdym nowym liczeniem
        dictionaryNumberTest = new Dictionary<(int, int, bool, bool), long>();

        // Rozpoczynamy rekurencję od pozycji 0, poprzednia cyfra = -1, leading=true, tight=true
        return CountDP(0, -1, true, true, digits);
    }

    // Główna funkcja rekurencyjna, zlicza liczbę poprawnych liczb
    static long CountDP(int pos, int prevDigit, bool leading, bool tight, List<int> digits)
    {
        // Jeśli przeszliśmy przez wszystkie cyfry, mamy 1 poprawną liczbę
        if (pos == digits.Count) return 1;

        // Sprawdzamy w słowniku, czy ten stan (pos, prevDigit, leading, tight) jest już policzony
        var key = (pos, prevDigit, leading, tight);
        if (dictionaryNumberTest.ContainsKey(key))
            return dictionaryNumberTest[key];

        // Ustalamy maksymalną cyfrę, którą możemy wybrać na tej pozycji
        int limit = tight ? digits[pos] : 9;

        long ways = 0;

        // Sprawdzamy wszystkie cyfry od 0 do limit
        for (int digit = 0; digit <= limit; digit++)
        {
            bool canUse = true;
            int nextPrevDigit = prevDigit;

            // Obsługa wiodących zer
            if (leading && digit == 0)
            {
                nextPrevDigit = -1;
            }
            else
            {
                // Gdy nie jesteśmy w zerach wiodących, sprawdzamy warunek różności
                if (!leading && digit == prevDigit) canUse = false;
                nextPrevDigit = digit;
            }

            // Jeśli możemy użyć cyfry, wywołujemy rekurencję dla następnej pozycji
            if (canUse)
            {
                bool nextLeading = leading && (digit == 0);
                bool nextTight = tight && (digit == limit);
                ways += CountDP(pos + 1, nextPrevDigit, nextLeading, nextTight, digits);
            }
        }

        // Zapisujemy obliczony wynik w słowniku
        dictionaryNumberTest[key] = ways;
        return ways;
    }

    // Zamienia liczbę x na listę cyfr (od najbardziej znaczącej do najmniej)
    static List<int> GetDigits(long x)
    {
        // Specjalny przypadek: x = 0
        if (x == 0) return new List<int> { 0 };

        var result = new List<int>();
        while (x > 0)
        {
            // Dodajemy cyfrę na początek listy
            result.Insert(0, (int)(x % 10));
            x /= 10;
        }
        return result;
    }
}
