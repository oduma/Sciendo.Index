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
            var songInfo = new SongInfo(MockedMp3FileLoader_NoTag("somefile.txt"));
            Assert.IsNotNull(songInfo);
            Assert.IsNullOrEmpty(songInfo.Album);
            Assert.IsNullOrEmpty(songInfo.Title);
            Assert.IsNull(songInfo.Artists);
        }

        [Test]
        public void SongInfoForFileNotTagged_1()
        {
            var songInfo = new SongInfo(MockedMp3FileLoader_NoTag1("somefile.txt"));
            Assert.IsNotNull(songInfo);
            Assert.IsNullOrEmpty(songInfo.Album);
            Assert.IsNullOrEmpty(songInfo.Title);
            Assert.IsNull(songInfo.Artists);
        }

        [Test]
        public void SongInfoForFileNotTagged_2()
        {
            var songInfo = new SongInfo(MockedMp3FileLoader_UnknownTag("somefile.txt"));
            Assert.IsNotNull(songInfo);
            Assert.IsNullOrEmpty(songInfo.Album);
            Assert.IsNullOrEmpty(songInfo.Title);
            Assert.IsNull(songInfo.Artists);
        }
        [Test]
        public void SongInfoForFileNotTagged_3()
        {
            var songInfo = new SongInfo(MockedMp3FileLoader_NoTitle("somefile.txt"));
            Assert.IsNotNull(songInfo);
            Assert.IsNullOrEmpty(songInfo.Album);
            Assert.IsNullOrEmpty(songInfo.Title);
            Assert.IsNotNull(songInfo.Artists);
            Assert.AreEqual("my artist",songInfo.Artists[0]);
        }
        [Test]
        public void SongInfoForOk()
        {
            var songInfo = new SongInfo(MockedMp3FileLoader_TagOk("somefile.txt"));
            Assert.IsNotNull(songInfo);
            Assert.IsNullOrEmpty(songInfo.Album);
            Assert.IsNotNullOrEmpty(songInfo.Title);
            Assert.IsNotNull(songInfo.Artists);
            Assert.AreEqual("my artist", songInfo.Artists[0]);
            Assert.AreEqual("my song", songInfo.Title);
        }

        public IMp3Stream MockedMp3FileLoader_NoTag(string filePath)
        {
            var mockMp3File = MockRepository.GenerateStub<IMp3Stream>();
            mockMp3File.Stub(m => m.HasTags).Return(false);
            return mockMp3File;
        }
        public IMp3Stream MockedMp3FileLoader_NoTag1(string filePath)
        {
            var mockMp3File = MockRepository.GenerateStub<IMp3Stream>();
            mockMp3File.Stub(m => m.HasTags).Return(true);
            mockMp3File.Stub(m => m.AvailableTagVersions).Return(null);
            return mockMp3File;
        }
        public IMp3Stream MockedMp3FileLoader_UnknownTag(string filePath)
        {
            var mockMp3File = MockRepository.GenerateStub<IMp3Stream>();
            mockMp3File.Stub(m => m.HasTags).Return(true);
            mockMp3File.Stub(m => m.AvailableTagVersions).Return(new Version[] { });
            return mockMp3File;
        }
        public IMp3Stream MockedMp3FileLoader_TagOk(string filePath)
        {
            var mId3Tag = new Id3Tag();
            mId3Tag.Artists.TextValue = "my artist";
            mId3Tag.Title.TextValue = "my song";

            var mockTitleFrame = MockRepository.GenerateStub<TitleFrame>();
            mockTitleFrame.TextValue = "my song";
            var mockArtistsFrame = MockRepository.GenerateStub<ArtistsFrame>();
            mockArtistsFrame.TextValue = "my artist";
            var mockId3Tag = MockRepository.GenerateStub<IId3Tag>();
            mockId3Tag.Stub(m => m.Artists).Return(mockArtistsFrame);
            mockId3Tag.Stub(m => m.Title).Return(mockTitleFrame);
            var mockMp3File = MockRepository.GenerateStub<IMp3Stream>();
            mockMp3File.Stub(m => m.HasTags).Return(true);
            mockMp3File.Stub(m => m.AvailableTagVersions).Return(new Version[] { new Version(3, 1) });
            mockMp3File.Stub(m => m.GetTag(3, 1)).Return(mId3Tag);

            return mockMp3File;
        }
        public IMp3Stream MockedMp3FileLoader_NoTitle(string filePath)
        {
            var mId3Tag = new Id3Tag();
            mId3Tag.Artists.TextValue = "my artist";

            var mockMp3File = MockRepository.GenerateStub<IMp3Stream>();
            mockMp3File.Stub(m => m.HasTags).Return(true);
            mockMp3File.Stub(m => m.AvailableTagVersions).Return(new Version[] { new Version(3, 1) });
            mockMp3File.Stub(m => m.GetTag(3, 1)).Return(mId3Tag);

            return mockMp3File;
        }


    }
}
