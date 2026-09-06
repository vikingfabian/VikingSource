using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VikingEngine.DataStream;
using VikingEngine.DSSWars;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.SteamWrapping
{
    enum WorkshopUploadState
    {
        None,
        Uploading,
        Completed,
        Failed
    }
    partial class SteamManager
    {
        WorkshopItem workShopItem;
        public WorkshopUploadState workshopUploadState = WorkshopUploadState.None;
        private CallResult<CreateItemResult_t> _createItemResult;
        private CallResult<SubmitItemUpdateResult_t> _submitItemUpdateResult;

        private UGCUpdateHandle_t updateHandle = UGCUpdateHandle_t.Invalid;
        private bool _isUploading = false;

        void initWorkShop()
        {
            _createItemResult = CallResult<CreateItemResult_t>.Create(OnItemCreated);
            _submitItemUpdateResult = CallResult<SubmitItemUpdateResult_t>.Create(OnItemSubmitted);
        }

        public void BeginUpload(WorkshopItem workShopItem)
        { 

            if (workshopUploadState == WorkshopUploadState.Uploading)
            {
                throw new Exception("Workshop upload is already in progress");
            }
            if (Directory.Exists(workShopItem.itempath.CompleteDirectory) == false)
            {
                throw new Exception($"Workshop item path does not exist: {workShopItem.itempath.CompleteDirectory}");
            }

            this.workShopItem = workShopItem;
            
            ulong existingId = 0;

            // 1. Check if we have an existing ID
            string idPath = workShopItem.workshopIdPath().CompletePath(true);

            if (File.Exists(idPath))
            {
                string idText = File.ReadAllText(idPath).Trim();
                ulong.TryParse(idText, out existingId);
            }

            workshopUploadState = WorkshopUploadState.Uploading;

            if (existingId > 0)
            {
                // UPDATE EXISTING ITEM
                Console.WriteLine($"Updating existing Workshop item: {existingId}");
                PublishedFileId_t fileId = new PublishedFileId_t(existingId);

                updateHandle = SteamUGC.StartItemUpdate(applicationSettings.appId, fileId);
                setItemProperties(updateHandle);

                SteamAPICall_t submitHandle = SteamUGC.SubmitItemUpdate(updateHandle, "Updated map version.");
                _submitItemUpdateResult.Set(submitHandle);
            }
            else
            {
                // CREATE NEW ITEM
                Console.WriteLine("No ID found. Creating new Workshop item.");
                SteamAPICall_t handle = SteamUGC.CreateItem(applicationSettings.appId, EWorkshopFileType.k_EWorkshopFileTypeCommunity);
                _createItemResult.Set(handle);
            }
        }

        private void OnItemCreated(CreateItemResult_t param, bool bIOFailure)
        {
            if (bIOFailure || param.m_eResult != EResult.k_EResultOK)
            {
                Console.WriteLine($"Failed to create item. Error: {param.m_eResult}");
                workshopUploadState = WorkshopUploadState.Failed;
                return;
            }

            Console.WriteLine($"Item created with ID: {param.m_nPublishedFileId}");

            // Save the new ID to the map's source folder immediately
            ulong newId = param.m_nPublishedFileId.m_PublishedFileId;
            string idFilePath = workShopItem.workshopIdPath().CompletePath(true);
            File.WriteAllText(idFilePath, newId.ToString());

            Console.WriteLine($"Saved new Workshop ID {newId} to {idFilePath}");

            // 2. Start the update transaction using the newly generated ID
            updateHandle = SteamUGC.StartItemUpdate(applicationSettings.appId, param.m_nPublishedFileId);
            setItemProperties(updateHandle);

            // 3. Submit the transaction to begin the upload
            SteamAPICall_t submitHandle = SteamUGC.SubmitItemUpdate(updateHandle, $"Date: {DateTime.Now.ToString()}, Version: {HudLib.EngineVersionString}");
            _submitItemUpdateResult.Set(submitHandle);
        }

        private void setItemProperties(UGCUpdateHandle_t updateHandle)
        {
            // Set text metadata
            SteamUGC.SetItemTitle(updateHandle, workShopItem.itemName);
            SteamUGC.SetItemDescription(updateHandle, workShopItem.itemDescription);
            SteamUGC.SetItemVisibility(updateHandle, workShopItem.visibility);
            SteamUGC.SetItemContent(updateHandle, workShopItem.itempath.CompleteDirectory);
            SteamUGC.SetItemTags(updateHandle, workShopItem.itemTags);
            // Optional: Add a preview image (must be under 1MB and an absolute file path)
            if (workShopItem.iconpath != null)
            {
                SteamUGC.SetItemPreview(updateHandle, workShopItem.iconpath.Value.CompletePath(true));
            }

            _isUploading = true;
        }

        private void OnItemSubmitted(SubmitItemUpdateResult_t param, bool bIOFailure)
        {
            _isUploading = false;
            updateHandle = UGCUpdateHandle_t.Invalid;

            if (bIOFailure || param.m_eResult != EResult.k_EResultOK)
            {
                Console.WriteLine($"Upload failed. Error: {param.m_eResult}");
                workshopUploadState = WorkshopUploadState.Failed;
                return;
            }

            // The item is uploaded, but might be hidden if the legal agreement isn't signed
            if (param.m_bUserNeedsToAcceptWorkshopLegalAgreement)
            {
                Console.WriteLine("Item uploaded, but user must accept the Steam Workshop Legal Agreement.");
                // Optionally open the Steam overlay directly to the item page:
                // SteamFriends.ActivateGameOverlayToWebPage("steam://url/CommunityFilePage/" + param.m_nPublishedFileId);
            }
            else
            {
                Console.WriteLine("Workshop item uploaded successfully!");
                workshopUploadState = WorkshopUploadState.Completed;
            }
        }

        public bool InWorkshopUploadProgress()
        {
            return _isUploading && updateHandle != UGCUpdateHandle_t.Invalid;
        }

        public void WorkshopUploadToHud(RichBoxContent content)
        {
            // 3. Only poll for progress while an upload is actually running
            if (InWorkshopUploadProgress())
            {
                content.h1("Steam workshop - upload", HudLib.TitleColor_Head);

                ulong bytesProcessed;
                ulong bytesTotal;

                EItemUpdateStatus status = SteamUGC.GetItemUpdateProgress(updateHandle, out bytesProcessed, out bytesTotal);

                // 4. Handle the various upload states to update your game UI
                switch (status)
                {
                    case EItemUpdateStatus.k_EItemUpdateStatusPreparingConfig:
                    case EItemUpdateStatus.k_EItemUpdateStatusPreparingContent:
                        //Console.WriteLine("Preparing files for upload...");
                        content.text(".Preparing files...");
                        break;

                    case EItemUpdateStatus.k_EItemUpdateStatusUploadingContent:
                    case EItemUpdateStatus.k_EItemUpdateStatusUploadingPreviewFile:
                        if (bytesTotal > 0) // Prevent divide-by-zero
                        {
                            float progress = (float)bytesProcessed / bytesTotal * 100f;
                            //Console.WriteLine($"Uploading: {progress:0.0}% ({bytesProcessed}/{bytesTotal} bytes)");
                            content.text($".Uploading: {progress:0.0}% ({bytesProcessed}/{bytesTotal} bytes)");
                        }
                        break;

                    case EItemUpdateStatus.k_EItemUpdateStatusCommittingChanges:
                        content.text("Upload finished. Awaiting Steam backend confirmation...");
                        break;
                }
            }

        }
    }
    struct WorkshopItem
    {
        /// <summary>
        /// tags will place items in a category
        /// </summary>
        public List<string> itemTags = new List<string>();
        public string itemName;
        public string itemDescription;

        /// <summary>
        /// Folder path to the content you want to upload. This should be an absolute path to a folder containing the files for your workshop item.
        /// </summary>
        public FilePath itempath;

        /// <summary>
        /// Image file, max 1MB, to be used as the icon for the workshop item. This should be an absolute path to an image file. Can be inside or 
        /// </summary>
        public FilePath? iconpath;

        public ERemoteStoragePublishedFileVisibility visibility = ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic;

        public WorkshopItem()
        { 
        }

        public FilePath workshopIdPath()
        {
            FilePath path = itempath;
            path.UseTimeMark = false;
            path.FileName = "workshop_id";
            path.FileEnd = ".txt";
            return path;
        }
    }
}
