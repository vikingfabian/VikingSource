#define PJ

/// <summary>
/// Entry point for the Content Builder project, 
/// which when executed will build content according to the "Content Collection Strategy" defined in the Builder class.
/// </summary>
/// <remarks>
/// Make sure to validate the directory paths in the "ContentBuilderParams" for your specific project.
/// For more details regarding the Content Builder, see the MonoGame documentation: <tbc.>
/// </remarks>

using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.Content.Pipeline.Builder;

var contentCollectionArgs = new ContentBuilderParams()
{
    Mode = ContentBuilderMode.Builder,
    WorkingDirectory = $"{AppContext.BaseDirectory}../../", // path to where your content folder can be located
    SourceDirectory = "Assets", // Not actually needed as this is the default, but added for reference
    Platform = TargetPlatform.DesktopGL
};
var builder = new Builder();

if (args is not null && args.Length > 0)
{
    builder.Run(args);
}
else
{
    builder.Run(contentCollectionArgs);
}

return builder.FailedToBuild > 0 ? -1 : 0;

public class Builder : ContentBuilder
{
    public override IContentCollection GetContentCollection()
    {
        var contentCollection = new ContentCollection();

        // By default, no content will be imported from the Assets folder using the default importer for their file type.
        // Please define your content collection rules here.

        /* Examples

        // Import all content in the Assets folder using the default importer for their file type.
        contentCollection.Include<WildcardRule>("*");

        // Only copy content from the assets folder rather than build it with the pipeline.
        contentCollection.IncludeCopy<WildcardRule>("*.json");

        // Exclude assets that match the pattern., only required overriding a default import behaviour.
        contentCollection.Exclude<WildcardRule>("Font/*.txt");

        // Include a specific asset with processor parameters.
        contentCollection.Include("Models/character.glb", new FbxImporter(),
            new MeshAnimatedModelProcessor()
            {
                Scale = 100.0f
            }
        );
        */
        /* Examples
        // Import all content in the Assets folder using the default importer for their file type.
        content.Include<WildcardRule>("*");
        // Only copy content from the assets folder rather than build it with the pipeline.
        content.IncludeCopy<WildcardRule>("*.json");
        // Exclude assets that match the pattern., only required overriding a default import behaviour.
        content.Exclude<WildcardRule>("Font/*.txt");
        // Include a specific asset with processor parameters.
        content.Include("Models/character.glb", new FbxImporter(),
            new MeshAnimatedModelProcessor()
            {
                Scale = 100.0f
            }
        );
        */

        //// Import tilemaps with specific importers
        //contentCollection.Include<WildcardRule>("*.tmx", tiledTilemapImporter);
        //contentCollection.Include<WildcardRule>("*.tsx", tilemapTilesetImporter);

        //// Import everything else with default importers
        //contentCollection.Include<WildcardRule>("*");

        //// Exclude non-content files
        //contentCollection.Exclude<WildcardRule>("*.txt");
        //contentCollection.Exclude<WildcardRule>("*.ico");
        //contentCollection.Exclude<WildcardRule>("*.pdn");
        //contentCollection.Exclude<WildcardRule>("*.xml");
        //contentCollection.Exclude<WildcardRule>("*.tiled-project");
        //contentCollection.Exclude<WildcardRule>("*.tiled-session");


        //// Copy specific files that were excluded above
        //contentCollection.IncludeCopy("LICENSE.txt", "../LICENSE.txt");
        //contentCollection.IncludeCopy("Levels/00.txt", "Levels/00.txt");
        //C:\Users\m\Documents\VikingSource2023\Repo\VikingSource\VikingBuilder\Assets\VoxelModel\Character\
        //C:\Users\m\Documents\VikingSource2023\Repo\VikingSource\VikingBuilder\Assets\VoxelModel\LfWars\
        
        contentCollection.Include<WildcardRule>("*");

#if DSS
        contentCollection.Exclude<WildcardRule>("PjContent/**");
#endif
#if PJ
        contentCollection.Exclude<WildcardRule>("DSS/**");
#endif
        contentCollection.Include<WildcardRule>("*.wav", new WavImporter());
        contentCollection.Exclude<WildcardRule>("Shaders/DeferredRenderer/*.*");
        contentCollection.Exclude<WildcardRule>("Shaders/Old/*.*");
        
        contentCollection.IncludeCopy<WildcardRule>("*.vox", null);
        contentCollection.IncludeCopy<WildcardRule>("*.sav", null);
        contentCollection.IncludeCopy<WildcardRule>("*.map", null);
        contentCollection.IncludeCopy<WildcardRule>("*.lvl", null);
        contentCollection.IncludeCopy<WildcardRule>("*.txt", null);
        contentCollection.IncludeCopy<WildcardRule>("*.vdf", null);

        return contentCollection;
    }
}
