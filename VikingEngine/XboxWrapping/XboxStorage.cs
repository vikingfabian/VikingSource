#if XBOX
using Microsoft.Xbox.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;
using Windows.Gaming.XboxLive.Storage;
using Windows.Storage.Streams;

namespace VikingEngine.XboxWrapping
{
    class XboxStorage : AbsPlatformStorage
    {
        string c_saveContainerDisplayName;
        string c_saveContainerName;

        public XboxStorage()
        {
            c_saveContainerDisplayName = FilePath.StorageFolderName + " Save";
            c_saveContainerName = FilePath.StorageFolderName + "Container";
        }

        public override void write(FilePath file, MemoryStream data, Action onComplete)
        {
            if (!Ref.xbox.gamer.signedIn)
            {
                onComplete();
                return;
            }

            var task = Task.Factory.StartNew(async () =>
            {
                GameSaveContainer gameSaveContainer = await OpenContainter();

                IBuffer dataBuffer = dataStreamToIBuffer(data);

                var blobsToWrite = new Dictionary<string, IBuffer> { { file.NameAndEnd(), dataBuffer } };

                GameSaveOperationResult gameSaveOperationResult = await gameSaveContainer.SubmitUpdatesAsync(
                    blobsToWrite, null, c_saveContainerDisplayName);

                Debug.Log("SAVE SUCCESS");
                onComplete();
            });

            try
            {
                task.Wait();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public override void read(FilePath file, MemoryStream data, Action onComplete)
        {       
            var task = Task.Factory.StartNew(async () =>
            {
                if (file.Storage)
                {
                    GameSaveContainer gameSaveContainer = await OpenContainter();

                    string[] blobsToRead = new string[] { file.NameAndEnd() };

                    GameSaveBlobGetResult result = await gameSaveContainer.GetAsync(blobsToRead);

                    if (result.Status == GameSaveErrorStatus.Ok)
                    {
                        //prepare a buffer to receive blob
                        IBuffer loadedBuffer;

                        //retrieve the named blob from the GetAsync result, place it in loaded buffer.
                        result.Value.TryGetValue(blobsToRead[0], out loadedBuffer);

                        
                        if (loadedBuffer == null || loadedBuffer.Length == 0)
                        {
                            throw new Exception("Blob is empty: " + blobsToRead[0]);
                        }
                        IBufferToDataSteam(loadedBuffer, data);

                        Debug.Log("LOAD SUCCESS");
                    }
                    else if (result.Status == GameSaveErrorStatus.BlobNotFound)
                    {
                        Debug.Log("There is no save file: " + blobsToRead[0]);
                    }
                    else
                    {
                        Debug.Log("GameSave Error Status: " + result.Status);
                    }
                }
                else
                {
                    string path = "ms-appx:///Content" + FilePath.Dir + file.CompleteLocalPath(false);

                    var storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(
                        new Uri(path));
                        //"//PjContent/MiniGolf/lines.lvl"));

                    IBuffer loadedBuffer = await Windows.Storage.FileIO.ReadBufferAsync(storageFile);
                    IBufferToDataSteam(loadedBuffer, data);

                    Debug.Log("----LOAD CONTENT COMPLETE");
                }

                onComplete();
            });

            try
            {
                task.Wait();
            }
            catch (Exception e)
            {
                data = null;
                Debug.LogError(e.Message);
            }
        }

        async Task<GameSaveContainer> OpenContainter()
        {
            //TODO user.WindowsSystemUser

            var users = await Windows.System.User.FindAllAsync();
            Windows.System.User mainUser = users[0];

            var configId = XboxLiveAppConfiguration.SingletonInstance.ServiceConfigurationId;
            GameSaveProviderGetResult gameSaveTask = await GameSaveProvider.GetForUserAsync(mainUser, configId);

            GameSaveProvider gameSaveProvider;
            if (gameSaveTask.Status == GameSaveErrorStatus.Ok)
            {
                gameSaveProvider = gameSaveTask.Value;
            }
            else
            {
                Debug.LogError("Game Save Provider Initialization failed");
                return null;
            }

            GameSaveContainer gameSaveContainer = gameSaveProvider.CreateContainer(c_saveContainerName);

            return gameSaveContainer;
        }

        IBuffer dataStreamToIBuffer(MemoryStream data)
        {
            DataWriter writer = new DataWriter();
            writer.WriteBytes(data.ToArray());
            IBuffer dataBuffer = writer.DetachBuffer();
            Debug.Log("dataBuffer.Length" + dataBuffer.Length.ToString());
            return dataBuffer;
        }

        void IBufferToDataSteam(IBuffer dataBuffer, MemoryStream data)
        {
            DataReader reader = DataReader.FromBuffer(dataBuffer);
            byte[] byteArray = new byte[reader.UnconsumedBufferLength];
            reader.ReadBytes(byteArray);

            data.Write(byteArray, 0, byteArray.Length);//= new System.IO.MemoryStream(byteArray);
            data.Position = 0;            
        }
    }
}
#endif