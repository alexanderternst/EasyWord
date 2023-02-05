using System.Collections.Generic;
using System.Linq;
using EasyWord.Models;

namespace EasyWord.ListExtensions;

public static class WordListExtensions
{
    public static bool CheckForAllBucketsAreFive(this List<Word> wordList)
    {
        return wordList.All(a => a.CurrentBucket == Bucket.Five);
    }

    public static Word GetNextNonBucketFiveWord(this List<Word> wordList, ref int position)
    {
        Word word;

        do
        {
            position++;
            if (wordList.Count <= position)
            {
                position = 0;
            }
            word = wordList[position];

        } while (word.CurrentBucket == Bucket.Five);

        return word;
    }
}