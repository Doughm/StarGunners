using System;

public class RandomGen
{
    private ulong[] numberArray = new ulong[624];
    private ulong incrementor;

    public ulong seed { get; private set; }
    public int position { get; private set; }

    public RandomGen()
    {
        Random seedGen = new Random();
        seed = (ulong)seedGen.Next();
        position = 1;
        initialize(seed);
    }

    public RandomGen(ulong startingSeed)
    {
        seed = startingSeed;
        position = 1;
        initialize(startingSeed);
    }

    // initializes the array
    private void initialize(ulong seedPassed)
    {
        numberArray[0] = seedPassed & 0xffffffffUL;
        for (incrementor = 1; incrementor < (ulong)numberArray.Length; incrementor++)
        {
            numberArray[incrementor] = (1812433253UL * (numberArray[incrementor - 1] ^ (numberArray[incrementor - 1] >> 30)) + incrementor);
            numberArray[incrementor] *= 4294967295;
            numberArray[incrementor] &= 0xffffffffUL;
        }
    }

    //checks if the position is out of bounds
    private void checkBounds()
    {
        if (position >= numberArray.Length)
        {
            position = 2;
        }
    }

    //returns a random integer
    public ulong getNext()
    {
        position++;
        checkBounds();
        return numberArray[position - 1];
    }

    //returns a random decimal number
    public double getNextDecimal()
    {
        position++;
        checkBounds();
        return (double)numberArray[position - 1] * (1.0 / 4294967295.0);
    }

    //returns a range between two numbers
    public int getRange(int start, int end)
    {
        position++;
        checkBounds();
        return start + (int)(numberArray[position - 1] % (ulong)(end - start));
    }
}