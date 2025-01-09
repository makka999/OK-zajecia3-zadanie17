
// Zadanie 17

// Ile jest liczb całkowitych pomiędzy 𝑎 i 𝑏 (0 ≤ 𝑎 ≤ 𝑏 ≤ 1018), w których żadne
// dwie sąsiadujące cyfry nie są takie same? Jeśli na przykład 𝑎 = 123, 𝑏 = 321, to
// prawidłową odpowiedzią będzie 171.

// Mateusz Kłaptocz 

using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        long a = 123;
        long b = 321;

        // Licz liczby w przedziale [a, b], w których żadne dwie sąsiadujące cyfry nie są takie same
        Console.WriteLine(CountValidNumbers(a, b)); // Wynik: 171
    }

    static long CountValidNumbers(long a, long b)
    {
        // Liczymy liczby w zakresie [0, b], a następnie odejmujemy liczby w zakresie [0, a-1]
        return CountNumbers(b) - CountNumbers(a - 1);
    }

    static long CountNumbers(long num)
    {
        var digits = GetDigits(num);
        // Rozpoczynamy obliczenia od pozycji 0, poprzednia cyfra -1 (brak), i z ograniczeniem tight=true
        return Dp(0, -1, true, digits);
    }

    static long Dp(int pos, int prev, bool tight, List<int> digits)
    {
        // Jeśli osiągnęliśmy koniec liczby (wszystkie cyfry wybrano), to mamy 1 poprawną liczbę
        if (pos == digits.Count)
            return 1;

        long result = 0;

        // Ustal maksymalną cyfrę dla bieżącej pozycji
        // Jeśli `tight` jest true, ograniczamy się do cyfry z `digits[pos]`, w przeciwnym razie limit to 9
        int limit = tight ? digits[pos] : 9;

        // Iterujemy po wszystkich możliwych cyfrach od 0 do limit
        for (int d = 0; d <= limit; d++)
        {
            // Sąsiadujące cyfry muszą być różne, więc pomijamy przypadek, gdy `d == prev`
            if (d != prev)
            {
                // Przechodzimy do następnej pozycji z nowym stanem
                // `tight && (d == limit)` oznacza, że ograniczenie na górną granicę pozostaje aktywne
                result += Dp(pos + 1, d, tight && (d == limit), digits);
            }
        }

        return result;
    }

    static List<int> GetDigits(long num)
    {
        var digits = new List<int>();

        // Rozbijamy liczbę na cyfry od końca i dodajemy je na początek listy
        while (num > 0)
        {
            digits.Insert(0, (int)(num % 10)); // Dodaj cyfrę na początek listy
            num /= 10; // Usuwamy ostatnią cyfrę z liczby
        }

        return digits;
    }
}
