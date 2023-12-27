#include <iostream>

//Wyświetl podgląd binarny dla zmiennej całkowitoliczbowej.
void task1()
{
    int number = 15;

    int numberOfBits = sizeof(number) * 8;

    unsigned int shift = 1 << (numberOfBits - 1);

    std::cout << "Reprezentacja liczby " << number << "w pamięci operacyjnej ";
    while (shift != 0)
    {
        if ((number & shift) == shift)
            std::cout << 1;
        else
            std::cout << 0;
        shift = shift >> 1;
    }
    std::cout << "\n";
}

//Wyświetl podgląd binarny dla zmiennej rzeczywistej.
void task2()
{
    union numberToView
    {
        float numberFloat = -15.5;
        int numberInt;
    };

    numberToView number;

    int numberOfBits = sizeof(number) * 8;

    unsigned int shift = 1 << (numberOfBits - 1);

    std::cout << "Reprezentacja liczby " << number.numberFloat << "w pamięci operacyjnej ";
    while (shift != 0)
    {
        if ((number.numberInt & shift) == shift)
            std::cout << 1;
        else
            std::cout << 0;
        shift = shift >> 1;
    }
    std::cout << "\n";
}

//Wyświetl podgląd binarny dla zmiennej rzeczywistej z rozbiciem na składowe
void task3(float numberParam)
{
    //wzór
    // (-1)^signBit * 2^(exponent - 127) * 1.fraction

    union numberToView
    {
        float numberFloat = 0.1;
        int numberInt;
    };

    numberToView number;
    number.numberFloat = numberParam;

    int numberOfBits = sizeof(number) * 8;

    unsigned int shift = 1 << (numberOfBits - 1);

    std::cout << "Reprezentacja liczby " << number.numberFloat << " w pamięci operacyjnej: \n";
    std::cout << "Bit znaku \t ";
    if ((number.numberInt & shift) == shift)
        std::cout << 1;
    else
        std::cout << 0;
    shift = shift >> 1;
    std::cout << "\n";

    std::cout << "Wykładnik ";
    for (int i = 0; i < 8; i++)
    {
        if ((number.numberInt & shift) == shift)
            std::cout << 1;
        else
            std::cout << 0;
        shift = shift >> 1;
    }
    std::cout << "\n";

    std::cout << "Mantysa ";

    while (shift != 0)
    {
        if ((number.numberInt & shift) == shift)
            std::cout << 1;
        else
            std::cout << 0;
        shift = shift >> 1;
    }
    std::cout << "\n";
}

//Wyświetl podgląd binarny dla zmiennej rzeczywistej z rozbiciem na składowe
void task4(double numberParam)
{
    //wzór
    // (-1)^signBit * 2^(exponent - 127) * 1.fraction

    union numberToView
    {
        double numberFloat = 0.1;
        long long numberInt;
    };

    numberToView number;
    number.numberFloat = numberParam;

    long long numberOfBits = sizeof(number) * 8;

    unsigned long long shift = 1 << (numberOfBits - 1l);

    std::cout << "Reprezentacja liczby " << number.numberFloat << " w pamięci operacyjnej: \n";
    std::cout << "Bit znaku \t ";
    if ((number.numberInt & shift) == shift)
        std::cout << 1;
    else
        std::cout << 0;
    shift = shift >> 1;
    std::cout << "\n";

    std::cout << "Wykładnik ";
    for (int i = 0; i < 11; i++)
    {
        if ((number.numberInt & shift) == shift)
            std::cout << 1;
        else
            std::cout << 0;
        shift = shift >> 1;
    }
    std::cout << "\n";

    std::cout << "Mantysa ";

    while (shift != 0)
    {
        if ((number.numberInt & shift) == shift)
            std::cout << 1;
        else
            std::cout << 0;
        shift = shift >> 1;
    }
    std::cout << "\n";
}

void task5()
{
    if (0.1 + 0.2 == 0.3)
        std::cout << "Liczby są równe\n";
    else
        std::cout << "Liczby są różne\n";

    double x = 0.1 + 0.2;
    double y = 0.3;
    task4(x);
    task4(y);
    if (x == y)
        std::cout << "Liczby są równe\n";
    else
        std::cout << "Liczby są różne\n";
}

int main()
{
    setlocale(LC_CTYPE, "polish");
    //task3(0.1);
    //task3(0.2);
    //task3(0.3);
    //task3(0.1 + 0.2);
    task5();
}
