using AegisTasks.Core.TaskPlan;
using System;
using System.IO;
using System.Text.Json;

namespace AegisTasks.TasksLibrary.TaskPlan
{
    public class WriteInFilePlanInputParams : TaskPlanInputParamsBase<WriteInFilePlanInputParams>
    {
        #region PUBLIC PROPERTIES

        public FileInfo FilePath { get; set; }
        public object Content { get; set; }
        public bool CreateFileIfNotExists { get; set; }
        public bool CreateDirectoryIfNotExists { get; set; }
        public bool AppendContent { get; set; }

        #endregion

        #region CONSTRUCTORS

        public WriteInFilePlanInputParams(
            FileInfo filePath,
            object content,
            bool createFileIfNotExists,
            bool createDirectoryIfNotExists,
            bool appendContent)
        {
            this.FilePath = filePath;
            this.Content = content;
            this.CreateFileIfNotExists = createFileIfNotExists;
            this.CreateDirectoryIfNotExists = createDirectoryIfNotExists;
            this.AppendContent = appendContent;
        }

        #endregion

        #region METHODS

        public bool IsValid()
        {
            return this.FilePath != null && this.Content != null;
        }

        public object Clone()
        {
            WriteInFilePlanInputParams clone = (WriteInFilePlanInputParams)this.MemberwiseClone();

            if (this.FilePath != null)
            {
                clone.FilePath = new FileInfo(this.FilePath.FullName);
            }

            if (this.Content is ICloneable cloneable)
            {
                clone.Content = cloneable.Clone();
            }
            else
            {
                clone.Content = this.Content;
            }

            return clone;
        }

        public override string ToJson()
        {
            // Serializamos Content como string JSON para preservar su tipo
            string contentJson = this.Content != null ? JsonSerializer.Serialize(this.Content) : null;

            WriteInFilePlanInputParamsDTO dto = new WriteInFilePlanInputParamsDTO
            {
                FilePath = this.FilePath?.FullName,
                ContentJson = contentJson,
                CreateFileIfNotExists = this.CreateFileIfNotExists,
                CreateDirectoryIfNotExists = this.CreateDirectoryIfNotExists,
                AppendContent = this.AppendContent
            };

            return JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = false });
        }

        public static WriteInFilePlanInputParams FromJson(string json)
        {
            WriteInFilePlanInputParamsDTO dto = JsonSerializer.Deserialize<WriteInFilePlanInputParamsDTO>(json);

            object content = null;
            if (!string.IsNullOrEmpty(dto.ContentJson))
            {
                // Deserializamos Content a object
                content = JsonSerializer.Deserialize<object>(dto.ContentJson);
            }

            return new WriteInFilePlanInputParams(
                new FileInfo(dto.FilePath),
                content,
                dto.CreateFileIfNotExists,
                dto.CreateDirectoryIfNotExists,
                dto.AppendContent
            );
        }

        #endregion

        #region INTERNAL DTO

        private class WriteInFilePlanInputParamsDTO
        {
            public string FilePath { get; set; }
            public string ContentJson { get; set; }
            public bool CreateFileIfNotExists { get; set; }
            public bool CreateDirectoryIfNotExists { get; set; }
            public bool AppendContent { get; set; }
        }

        #endregion
    }
}
