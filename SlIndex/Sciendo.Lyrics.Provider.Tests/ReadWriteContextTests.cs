﻿using System;
using System.IO;
using System.Net;
using Id3;
using Id3.Frames;
using Id3.Id3;
using NUnit.Framework;
using Rhino.Mocks;
using Sciendo.Common.Serialization;
using Sciendo.Lyrics.Common;
using Sciendo.Lyrics.Provider.Service;

namespace Sciendo.Lyrics.Provider.Tests
{
    [TestFixture]
    public class ReadWriteContextTests
    {
        [SetUp]
        public void SetUp()
        {
            if(File.Exists("target\\dir1\\dir2\\existingfile1.lrc"))
                File.Delete("target\\dir1\\dir2\\existingfile1.lrc");
            if(Directory.Exists("target\\dir1\\dir2"))
                Directory.Delete("target\\dir1\\dir2");
            if (!Directory.Exists("target"))
                Directory.CreateDirectory("target");
        }
        [Test]
        public void ProcessFile_FileNotExists()
        {
            Assert.AreEqual(Status.Error, new ReadWriteContext { ReadLocation = "non existent file", Status = Status.None }.ProcessFile(null).Status);
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
            mockMp3File.Stub(m => m.AvailableTagVersions).Return(new Version[] {});
            return mockMp3File;
        }
        public IMp3Stream MockedMp3FileLoader_TagOk(string filePath)
        {
            var mId3Tag = new Id3Tag();
            mId3Tag.Artists.TextValue = "my artist";
            mId3Tag.Title.TextValue = "my song";

            var mockTitleFrame = MockRepository.GenerateStub<TitleFrame>();
            mockTitleFrame.TextValue = "my song";
            var mockArtistsFrame= MockRepository.GenerateStub<ArtistsFrame>();
            mockArtistsFrame.TextValue="my artist";
            var mockId3Tag=MockRepository.GenerateStub<IId3Tag>();
            mockId3Tag.Stub(m=>m.Artists).Return(mockArtistsFrame);
            mockId3Tag.Stub(m => m.Title).Return(mockTitleFrame);
            var mockMp3File = MockRepository.GenerateStub<IMp3Stream>();
            mockMp3File.Stub(m => m.HasTags).Return(true);
            mockMp3File.Stub(m => m.AvailableTagVersions).Return(new Version[] { new Version(3,1)});
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

        [Test]
        public void ProcessFile_FileNotTagged()
        {
            
            Assert.AreEqual(Status.FileNotTagged, new ReadWriteContext { ReadLocation = "existingfile.txt", Status = Status.None }.ProcessFile(MockedMp3FileLoader_NoTag).Status);
        }
        [Test]
        public void ProcessFile_FileNotTagged_1()
        {

            Assert.AreEqual(Status.FileNotTagged, new ReadWriteContext { ReadLocation = "existingfile.txt", Status = Status.None }.ProcessFile(MockedMp3FileLoader_NoTag1).Status);
        }
        [Test]
        public void ProcessFile_FileNotTagged_2()
        {
            Assert.AreEqual(Status.FileNotTagged, new ReadWriteContext { ReadLocation = "existingfile.txt", Status = Status.None }.ProcessFile(MockedMp3FileLoader_UnknownTag).Status);
        }
        [Test]
        public void ProcessFile_FileNotTagged_3()
        {
            Assert.AreEqual(Status.FileNotTagged, new ReadWriteContext { ReadLocation = "existingfile.txt", Status = Status.None }.ProcessFile(MockedMp3FileLoader_NoTitle).Status);
        }
        [Test]
        public void ProcessFile_Ok()
        {
            var reqdWriteContext=new ReadWriteContext { ReadLocation = "existingfile.txt", Status = Status.None }.ProcessFile(MockedMp3FileLoader_TagOk);
            Assert.AreEqual(Status.ArtistSongRetrievedFromFile, reqdWriteContext.Status);
            Assert.AreEqual("my_artist", reqdWriteContext.artist);
            Assert.AreEqual("my_song", reqdWriteContext.song);
            Assert.AreEqual("existingfile.txt", reqdWriteContext.ReadLocation);
            Assert.IsNotNull(reqdWriteContext.Url);
            Assert.AreEqual(new Uri("http://lyrics.wikia.com/api.php?func=getSong&artist=my_artist&song=my_song&fmt=xml"), reqdWriteContext.Url);
        }

        [Test]
        public void ProcessFile_AlreadyProcessed()
        {
            var reqdWriteContext = new ReadWriteContext { ReadLocation = "existingfile.txt", Status = Status.ArtistSongRetrievedFromFile }.ProcessFile(MockedMp3FileLoader_TagOk);
            Assert.AreEqual(Status.ArtistSongRetrievedFromFile, reqdWriteContext.Status);
        }

        [Test]
        public void ProcessFile_LyricsDownloadedOk()
        {
            var reqdWriteContext = new ReadWriteContext { ReadLocation = "existingfile.txt", Status = Status.LyricsDownloadedOk }.ProcessFile(MockedMp3FileLoader_TagOk);
            Assert.AreEqual(Status.LyricsDownloadedOk, reqdWriteContext.Status);
        }
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        public void TakeFromWeb_ArtistSongNotReadyOrAlreadyDownloaded(int expected)
        {
            var expectedStatus = (Status) expected;
            Assert.AreEqual(expectedStatus, new ReadWriteContext { ReadLocation = "existingfile.txt", Status = expectedStatus }.TakeFromWeb(MockedGoodClient(),"","", new LyricsDeserializer()).Status);
        }

        [TestCase(4)]
        [TestCase(5)]
        public void TakeFromWeb_Ok(int inStatus)
        {
            var inStatusStatus = (Status) inStatus;
            var readWriteContext =
                new ReadWriteContext { ReadLocation = "source\\dir1\\dir2\\existingfile1.txt", Status = inStatusStatus }.TakeFromWeb(
                    MockedGoodClient(),"source","target", MockedLyricsOk());
            Assert.AreEqual(Status.LyricsDownloadedOk, readWriteContext.Status);
            Assert.True(File.Exists("target\\dir1\\dir2\\existingfile1.lrc"));
            using (StreamReader sr = File.OpenText("target\\dir1\\dir2\\existingfile1.lrc"))
            {
                Assert.AreEqual("my download", sr.ReadToEnd());
            }
        }

        [TestCase(4)]
        [TestCase(5)]
        public void TakeFromWeb_LyricsNotFound(int inStatus)
        {
            var inStatusStatus = (Status)inStatus;
            var readWriteContext =
                new ReadWriteContext { ReadLocation = "source\\dir1\\dir2\\existingfile1.txt", Status = inStatusStatus }.TakeFromWeb(
                    MockedGoodClient(), "source", "target", MockedLyricsNotFound());
            Assert.AreEqual(Status.LyricsNotFound, readWriteContext.Status);
            Assert.False(File.Exists("target\\dir1\\dir2\\existingfile1.lrc"));
        }

        private ILyricsDeserializer MockedLyricsNotFound()
        {
            var mockedWebClient = MockRepository.GenerateStub<ILyricsDeserializer>();
            mockedWebClient.Stub(m => m.Deserialize<LyricsResult>("my download")).Throw(new PreSerializationCheckException());
            return mockedWebClient;
        }

        private ILyricsDeserializer MockedLyricsOk()
        {
            var mockedWebClient = MockRepository.GenerateStub<ILyricsDeserializer>();
            mockedWebClient.Stub(m => m.Deserialize<LyricsResult>("my download")).Return(new LyricsResult());
            return mockedWebClient;
        }

        private WebDownloaderBase MockedGoodClient()
        {
            var mockedWebClient = MockRepository.GenerateStub<WebDownloaderBase>();
            mockedWebClient.Stub(m => m.TryQuery<string>("http://lyrics.wikia.com/api.php?func=getSong&artist=&song=&fmt=xml")).Return("my download");
            return mockedWebClient;
        }

        [TestCase(4)]
        [TestCase(5)]
        public void TakeFromWeb_NoDwonload(int inStatus)
        {
            var inStatusStatus = (Status) inStatus;
            var readWriteContext =
                new ReadWriteContext {ReadLocation = "existingfile.txt", Status = inStatusStatus}.TakeFromWeb(
                    MockedNoGoodClient(),"source","target", new LyricsDeserializer());
            Assert.AreEqual(Status.LyricsUrlUnreachable, readWriteContext.Status);
        }

        private WebDownloaderBase MockedNoGoodClient()
        {
            var mockedWebClient = MockRepository.GenerateStub<WebDownloaderBase>();
            mockedWebClient.Stub(m => m.TryQuery<string>("http://lyrics.wikia.com/api.php?func=getSong&artist=&song=&fmt=xml")).Throw(new Exception());
            return mockedWebClient;
        }

    }
}
