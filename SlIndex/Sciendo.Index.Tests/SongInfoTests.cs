using System;
using Id3;
using Id3.Frames;
using Id3.Id3;
using NUnit.Framework;
using Rhino.Mocks;
using Sciendo.Music.Real.Procesors.MusicSourced;

namespace Sciendo.Music.Tests
{
    [TestFixture]
    public class SongInfoTests
    {
        [Test]
        public void SongInfoForFileNotTagged()
        {
            var songInfo = new SongInfo(Mp3StreamMockUtils.MockedMp3FileLoader_NoTag("somefile.txt"));
            Assert.IsNotNull(songInfo);
            Assert.IsNullOrEmpty(songInfo.Album);
            Assert.IsNullOrEmpty(songInfo.Title);
            Assert.IsNull(songInfo.Artists);
        }

        [Test]
        public void SongInfoForFileNotTagged_1()
        {
            var songInfo = new SongInfo(Mp3StreamMockUtils.MockedMp3FileLoader_NoTag1("somefile.txt"));
            Assert.IsNotNull(songInfo);
            Assert.IsNullOrEmpty(songInfo.Album);
            Assert.IsNullOrEmpty(songInfo.Title);
            Assert.IsNull(songInfo.Artists);
        }

        [Test]
        public void SongInfoForFileNotTagged_2()
        {
            var songInfo = new SongInfo(Mp3StreamMockUtils.MockedMp3FileLoader_UnknownTag("somefile.txt"));
            Assert.IsNotNull(songInfo);
            Assert.IsNullOrEmpty(songInfo.Album);
            Assert.IsNullOrEmpty(songInfo.Title);
            Assert.IsNull(songInfo.Artists);
        }
        [Test]
        public void SongInfoForFileNotTagged_3()
        {
            var songInfo = new SongInfo(Mp3StreamMockUtils.MockedMp3FileLoader_NoTitle("somefile.txt"));
            Assert.IsNotNull(songInfo);
            Assert.IsNullOrEmpty(songInfo.Album);
            Assert.IsNullOrEmpty(songInfo.Title);
            Assert.IsNotNull(songInfo.Artists);
            Assert.AreEqual("my artist",songInfo.Artists[0]);
        }
        [Test]
        public void SongInfoForOk()
        {
            var songInfo = new SongInfo(Mp3StreamMockUtils.MockedMp3FileLoader_TagOk("somefile.txt"));
            Assert.IsNotNull(songInfo);
            Assert.IsNullOrEmpty(songInfo.Album);
            Assert.IsNotNullOrEmpty(songInfo.Title);
            Assert.IsNotNull(songInfo.Artists);
            Assert.AreEqual("my artist", songInfo.Artists[0]);
            Assert.AreEqual("my song", songInfo.Title);
        }



    }
}
