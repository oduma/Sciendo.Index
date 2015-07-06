using Id3;
using Id3.Frames;
using Id3.Id3;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.Tests
{
    public static class Mp3StreamMockUtils
    {
        public static IMp3Stream MockedMp3FileLoader_NoTag(string filePath)
        {
            var mockMp3File = MockRepository.GenerateStub<IMp3Stream>();
            mockMp3File.Stub(m => m.HasTags).Return(false);
            return mockMp3File;
        }
        public static IMp3Stream MockedMp3FileLoader_NoTag1(string filePath)
        {
            var mockMp3File = MockRepository.GenerateStub<IMp3Stream>();
            mockMp3File.Stub(m => m.HasTags).Return(true);
            mockMp3File.Stub(m => m.AvailableTagVersions).Return(null);
            return mockMp3File;
        }
        public static IMp3Stream MockedMp3FileLoader_UnknownTag(string filePath)
        {
            var mockMp3File = MockRepository.GenerateStub<IMp3Stream>();
            mockMp3File.Stub(m => m.HasTags).Return(true);
            mockMp3File.Stub(m => m.AvailableTagVersions).Return(new Version[] { });
            return mockMp3File;
        }
        public static IMp3Stream MockedMp3FileLoader_ErrorTag(string filePath)
        {
            var mockMp3File = MockRepository.GenerateStub<IMp3Stream>();
            mockMp3File.Stub(m => m.HasTags).Return(true);
            mockMp3File.Stub(m => m.AvailableTagVersions).Return(new Version[] { new Version(3,1)});
            mockMp3File.Stub(m => m.GetTag(3, 1)).Throw(new Exception());
            return mockMp3File;
        }
        public static IMp3Stream MockedMp3FileLoader_EmptyTag(string filePath)
        {
            var mockMp3File = MockRepository.GenerateStub<IMp3Stream>();
            mockMp3File.Stub(m => m.HasTags).Return(true);
            mockMp3File.Stub(m => m.AvailableTagVersions).Return(new Version[] { new Version(3, 1) });
            mockMp3File.Stub(m => m.GetTag(3, 1)).Return(null);
            return mockMp3File;
        }
        public static IMp3Stream MockedMp3FileLoader_EverythingButArtist(string filePath)
        {
            var mId3Tag = new Id3Tag();
            mId3Tag.Album.TextValue = "my album";
            mId3Tag.Title.TextValue = "my song";
           
            var mockTitleFrame = MockRepository.GenerateStub<TitleFrame>();
            mockTitleFrame.TextValue = "my song";
            var mockAlbumFrame = MockRepository.GenerateStub<AlbumFrame>();
            mockAlbumFrame.TextValue = "my album";
            var mockId3Tag = MockRepository.GenerateStub<IId3Tag>();
            mockId3Tag.Stub(m => m.Artists).Return(null);
            mockId3Tag.Stub(m => m.Title).Return(mockTitleFrame);
            mockId3Tag.Stub(m => m.Album).Return(mockAlbumFrame);
            var mockMp3File = MockRepository.GenerateStub<IMp3Stream>();
            mockMp3File.Stub(m => m.HasTags).Return(true);
            mockMp3File.Stub(m => m.AvailableTagVersions).Return(new Version[] { new Version(3, 1) });
            mockMp3File.Stub(m => m.GetTag(3, 1)).Return(mId3Tag);

            return mockMp3File;
        }

        public static IMp3Stream MockedMp3FileLoader_EverythingButTitle(string filePath)
        {
            var mId3Tag = new Id3Tag();
            mId3Tag.Artists.TextValue = "my artist";
            mId3Tag.Album.TextValue = "my album";

            var mockArtistsFrame = MockRepository.GenerateStub<ArtistsFrame>();
            mockArtistsFrame.TextValue = "my artist";
            var mockAlbumFrame = MockRepository.GenerateStub<AlbumFrame>();
            mockAlbumFrame.TextValue = "my album";
            var mockId3Tag = MockRepository.GenerateStub<IId3Tag>();
            mockId3Tag.Stub(m => m.Artists).Return(mockArtistsFrame);
            mockId3Tag.Stub(m => m.Title).Return(null);
            mockId3Tag.Stub(m => m.Album).Return(mockAlbumFrame);
            var mockMp3File = MockRepository.GenerateStub<IMp3Stream>();
            mockMp3File.Stub(m => m.HasTags).Return(true);
            mockMp3File.Stub(m => m.AvailableTagVersions).Return(new Version[] { new Version(3, 1) });
            mockMp3File.Stub(m => m.GetTag(3, 1)).Return(mId3Tag);

            return mockMp3File;
        }

        public static IMp3Stream MockedMp3FileLoader_FullTag(string filePath)
        {
            var mId3Tag = new Id3Tag();
            mId3Tag.Artists.TextValue = "my artist";
            mId3Tag.Album.TextValue = "my album";
            mId3Tag.Title.TextValue = "my song";

            var mockTitleFrame = MockRepository.GenerateStub<TitleFrame>();
            mockTitleFrame.TextValue = "my song";
            var mockArtistsFrame = MockRepository.GenerateStub<ArtistsFrame>();
            mockArtistsFrame.TextValue = "my artist";
            var mockAlbumFrame = MockRepository.GenerateStub<AlbumFrame>();
            mockAlbumFrame.TextValue = "my album";
            var mockId3Tag = MockRepository.GenerateStub<IId3Tag>();
            mockId3Tag.Stub(m => m.Artists).Return(mockArtistsFrame);
            mockId3Tag.Stub(m => m.Title).Return(mockTitleFrame);
            mockId3Tag.Stub(m => m.Album).Return(mockAlbumFrame);
            var mockMp3File = MockRepository.GenerateStub<IMp3Stream>();
            mockMp3File.Stub(m => m.HasTags).Return(true);
            mockMp3File.Stub(m => m.AvailableTagVersions).Return(new Version[] { new Version(3, 1) });
            mockMp3File.Stub(m => m.GetTag(3, 1)).Return(mId3Tag);

            return mockMp3File;
        }
        public static IMp3Stream MockedMp3FileLoader_TagOk(string filePath)
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
        public static IMp3Stream MockedMp3FileLoader_NoTitle(string filePath)
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
