using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Domain.NoSql.Data.Provider;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;

namespace Domain.NoSql.Data.Provider
{
    public class GridFSProvider
    {
        private readonly IGridFSBucket _gridFsBucket;

        public GridFSProvider() : this(DatabaseProvider.GetDatabase())
        {
        }

        public GridFSProvider(string connectionStringName) : this(DatabaseProvider.GetDatabase(connectionStringName))
        {
        }

        public GridFSProvider(IMongoDatabase database)
        {
            _gridFsBucket = new GridFSBucket(database);
        }

        public GridFSProvider(IMongoDatabase database, GridFSBucketOptions gridFsBucketOptions)
        {
            _gridFsBucket = new GridFSBucket(database, gridFsBucketOptions);
        }

        public async void AddFileAsync(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            using (var stream = await _gridFsBucket.OpenUploadStreamAsync(fileName))
            {
                var id = stream.Id;

                await stream.CloseAsync();
            }

        }

        public async void AddFileAsync(string fileName, GridFSUploadOptions gridFsUploadOptions)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            using (var stream = await _gridFsBucket.OpenUploadStreamAsync(fileName, gridFsUploadOptions))
            {
                var id = stream.Id;

                await stream.CloseAsync();
            }

        }

        public ObjectId AddFile(string fileName, Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            return _gridFsBucket.UploadFromStream(fileName, stream);
        }

        public ObjectId AddFile(string fileName, Stream stream, GridFSUploadOptions gridFsUploadOptions)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (gridFsUploadOptions == null) throw new ArgumentNullException(nameof(gridFsUploadOptions));
            if (gridFsUploadOptions.Metadata == null) throw new NullReferenceException("gridFsUploadOptions.Metadata");

            return _gridFsBucket.UploadFromStream(fileName, stream, gridFsUploadOptions);
        }

        public byte[] GetFile(ObjectId id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            return _gridFsBucket.DownloadAsBytes(id);
        }

        public void GetFile(ObjectId id, Stream destination)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            _gridFsBucket.DownloadToStream(id, destination);
        }

        public byte[] GetFile(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            return _gridFsBucket.DownloadAsBytesByName(fileName);
        }

        public void GetFile(string fileName, Stream destination)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            _gridFsBucket.DownloadToStreamByName(fileName, destination);
        }

        public IEnumerable<GridFSFileInfo> Find(FilterDefinition<GridFSFileInfo> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            return _gridFsBucket.Find(filter).ToEnumerable();
        }

        public GridFSFileInfo Find(ObjectId id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Id, id);
            return _gridFsBucket.Find(filter).FirstOrDefault();
        }

        public async void ReNameFile(string fileName, string newFileName)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, fileName);
            var filesCursor = await _gridFsBucket.FindAsync(filter);
            var files = await filesCursor.ToListAsync();

            foreach (var file in files)
            {
                await _gridFsBucket.RenameAsync(file.Id, newFileName);
            }
        }

        public void DeleteById(ObjectId id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            _gridFsBucket.DeleteAsync(id);
        }
    }
}
