using AegisTasks.Core.TaskPlan;
using System.IO;

public class CopyDirectoryPlanInputParams : TaskPlanInputParamsBase<CopyDirectoryPlanInputParams>
{
    #region PUBLIC PROPERTIES

    public bool CreateDestinationDirectoryIfNotExists { get; set; }
    public bool OverwriteDirectoriesIfExist { get; set; }
    public bool OverwriteFilesIfExist { get; set; }
    public DirectoryInfo DirectoryToCopy { get; set; }
    public DirectoryInfo DestinationDirectory { get; set; }
    public int? CopyDepth { get; set; }

    #endregion

    #region CONSTRUCTORS

    public CopyDirectoryPlanInputParams(
        bool createDestinationDirectoryIfNotExists,
        bool overwriteDirectoriesIfExist,
        bool overwriteFilesIfExist,
        DirectoryInfo directoryToCopy,
        DirectoryInfo destinationDirectory,
        int? copyDepth)
    {
        CreateDestinationDirectoryIfNotExists = createDestinationDirectoryIfNotExists;
        OverwriteDirectoriesIfExist = overwriteDirectoriesIfExist;
        OverwriteFilesIfExist = overwriteFilesIfExist;
        DirectoryToCopy = directoryToCopy;
        DestinationDirectory = destinationDirectory;
        CopyDepth = copyDepth;
    }

    public CopyDirectoryPlanInputParams(
        bool createDestinationDirectoryIfNotExists,
        bool overwriteDirectoriesIfExist,
        bool overwriteFilesIfExist,
        DirectoryInfo directoryToCopy,
        DirectoryInfo destinationDirectory)
        : this(createDestinationDirectoryIfNotExists, overwriteDirectoriesIfExist, overwriteFilesIfExist, directoryToCopy, destinationDirectory, null)
    { }

    #endregion

    public bool IsValid()
    {
        return DirectoryToCopy != null && DestinationDirectory != null && (CopyDepth == null || CopyDepth >= 0);
    }

    public override string ToJson()
    {
        CopyDirectoryPlanInputParamsDTO dto = new CopyDirectoryPlanInputParamsDTO
        {
            CreateDestinationDirectoryIfNotExists = this.CreateDestinationDirectoryIfNotExists,
            OverwriteDirectoriesIfExist = this.OverwriteDirectoriesIfExist,
            OverwriteFilesIfExist = this.OverwriteFilesIfExist,
            DirectoryToCopy = this.DirectoryToCopy?.FullName,
            DestinationDirectory = this.DestinationDirectory?.FullName,
            CopyDepth = this.CopyDepth
        };

        return System.Text.Json.JsonSerializer.Serialize(dto, new System.Text.Json.JsonSerializerOptions { WriteIndented = false });
    }

    public static CopyDirectoryPlanInputParams FromJson(string json)
    {
        CopyDirectoryPlanInputParamsDTO dto = System.Text.Json.JsonSerializer.Deserialize<CopyDirectoryPlanInputParamsDTO>(json);
        return new CopyDirectoryPlanInputParams(
            dto.CreateDestinationDirectoryIfNotExists,
            dto.OverwriteDirectoriesIfExist,
            dto.OverwriteFilesIfExist,
            new DirectoryInfo(dto.DirectoryToCopy),
            new DirectoryInfo(dto.DestinationDirectory),
            dto.CopyDepth
        );
    }

    private class CopyDirectoryPlanInputParamsDTO
    {
        public bool CreateDestinationDirectoryIfNotExists { get; set; }
        public bool OverwriteDirectoriesIfExist { get; set; }
        public bool OverwriteFilesIfExist { get; set; }
        public string DirectoryToCopy { get; set; }
        public string DestinationDirectory { get; set; }
        public int? CopyDepth { get; set; }
    }
}
