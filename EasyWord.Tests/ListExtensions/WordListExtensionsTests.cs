using EasyWord.ListExtensions;
using EasyWord.Models;
using FluentAssertions;
using NUnit.Framework;


namespace EasyWord.Tests.ListExtensions;

public class WordListExtensionsTests
{
    [Test]
    public void CheckForAllBucketsAreFive_WennAlleWoerterBucket5Sind_GibtTrueZurueck()
    {
        var testee = new List<Word>()
        {
            new Word()
            {
                CurrentBucket = Bucket.Five,
            },
            new Word()
            {
                CurrentBucket = Bucket.Five,
            },
            new Word()
            {
                CurrentBucket = Bucket.Five,
            },
        };

        var result = testee.CheckForAllBucketsAreFive();

        result.Should().BeTrue();
    }


    [Test]
    public void CheckForAllBucketsAreFive_WennNichtAlleWoerterBucket5Sind_GibtFalseZurueck()
    {
        var testee = new List<Word>()
        {
            new Word()
            {
                CurrentBucket = Bucket.Five,
            },
            new Word()
            {
                CurrentBucket = Bucket.Three,
            },
            new Word()
            {
                CurrentBucket = Bucket.Five,
            },
        };

        var result = testee.CheckForAllBucketsAreFive();

        result.Should().BeFalse();
    }

    [Test]
    public void GetNextNonBucketFiveWord_WortMitBucket5Vorhanden_UeberspringtAlleWoerterMitBucket5()
    {

        var position = 0;
        var testee = new List<Word>()
        {
            new Word()
            {
                CurrentBucket = Bucket.Five,
            },
            new Word()
            {
                CurrentBucket = Bucket.Five,
            },
            new Word()
            {
                CurrentBucket = Bucket.Five,
            },

            new Word()
            {
                CurrentBucket = Bucket.Five,
            },
            new Word()
            {
                EnglishWord = "Hello",
                GermanWord = "Hallo",
                CurrentBucket = Bucket.Three,
            },
            new Word()
            {
                CurrentBucket = Bucket.Five,
            },
        };

        var result = testee.GetNextNonBucketFiveWord(ref position);

        result.EnglishWord.Should().Be("Hello");
        result.GermanWord.Should().Be("Hallo");

        position.Should().Be(4);
    }
    
    [Test]
    public void GetNextNonBucketFiveWord_NaechstesWortIstHinten_SetztPositionZurueck()
    {

        var position = 2;
        var testee = new List<Word>()
        {
            new Word()
            {
                CurrentBucket = Bucket.Five,
            },
            new Word()
            {
                GermanWord = "Hallo",
                EnglishWord = "Hello",
                CurrentBucket = Bucket.One,
            },
            new Word()
            {
                CurrentBucket = Bucket.Five,
            },

            new Word()
            {
                CurrentBucket = Bucket.Five,
            },
            new Word()
            {
                CurrentBucket = Bucket.Five,
            },
            new Word()
            {
                CurrentBucket = Bucket.Five,
            },
        };

        var result = testee.GetNextNonBucketFiveWord(ref position);

        result.EnglishWord.Should().Be("Hello");
        result.GermanWord.Should().Be("Hallo");

        position.Should().Be(1);
    }
}